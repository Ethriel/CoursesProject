import React, { Component } from 'react';
import { Redirect, withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { Spin, Space, Button } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import moment from 'moment';
import CourseContainer from './CourseContainer';
import axios from 'axios';
import { USER, ADMIN } from '../common/roles';
import { forbidden, courses } from '../../Routes/RoutersDirections';
import NotificationError from '../common/notifications/notification-error';
import NotificationWarning from '../common/notifications/notification-warning';
import { SET_EMAIL_CONFIRMED } from '../../reducers/reducersActions';
import NotificationOk from '../common/notifications/notification-ok';

class CourseDetailsComponent extends Component {
    constructor(props) {
        super(props)
        this.state = {
            courseData: {
                id: props.match.params.id,
                course: null,
                isPresent: false
            },
            isLoading: true,
            plug: { title: "", cover: "", description: "" },
            selectedDate: "",
            redirect: false,
            redirectDelete: false
        }
    }

    getCourse = async (token) => {
        try {
            const response = await MakeRequestAsync(`courses/get/${this.state.courseData.id}`, { msg: "hello" }, "get", token);
            const course = response.data.data;
            this.setState(old => ({
                courseData: {
                    ...old.courseData,
                    course: course
                }
            }));
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    checkCourse = async (token) => {
        const userId = this.props.currentUser.id;
        const courseId = this.state.courseData.id;
        try {
            const response = await MakeRequestAsync(`courses/check/${userId}/${courseId}`, { msg: "hello" }, "get", token);
            const data = response.data.data;
            const isPresent = data.isPresent;
            const studyDate = data.studyDate;
            this.setState(old => ({
                courseData: {
                    ...old.courseData,
                    isPresent: isPresent,
                    studyDate: data.studyDate
                }
            }));
            if (isPresent === true) {
                NotificationWarning(`You already have this course in your list.`, `Start at ${studyDate}`);
            }

        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    setEmailConfirmed = confirmed => {
        localStorage.setItem("current_user_email_confirmed", confirmed);
        this.props.onEmailConfirmedChanged(confirmed);
    };

    checkEmailConfirmed = async (token) => {
        const email = this.props.currentUser.email;
        try {
            await MakeRequestAsync("account/checkEmailConfirmed", { email: email }, "post", token);

            this.setEmailConfirmed(true);
        } catch (error) {
            const errorResponse = error.response;
            if (errorResponse) {
                const message = errorResponse.data.message;
                if (errorResponse.status === 400 && message.includes("Email is not confirmed")) {
                    this.setEmailConfirmed(false);
                    NotificationWarning("But you still can browse courses", "Your email is not confirmed");
                } else {
                    this.setCatch(error);
                }
            }
        }
    };

    componentDidMount = async () => {
        const role = this.props.currentUser.role;
        if (role !== USER && role !== ADMIN) {
            this.props.history.push(forbidden);
        }
        else {
            const signal = axios.CancelToken.source();
            await this.checkCourse(signal.token);
            await this.getCourse(signal.token);
            await this.checkEmailConfirmed(signal.token);
        }
    };

    handleConfirm = async () => {
        this.setState({ isLoading: true });
        const userId = this.props.currentUser.id;
        const courseId = this.state.courseData.id;
        const date = this.state.selectedDate;
        const data = {
            systemUserId: userId,
            trainingCourseId: courseId,
            studyDate: date
        };
        try {
            await MakeRequestAsync("UserCourses/add", data, "post");
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    }

    handleDateChange = (value, dateString) => {
        this.setState({ selectedDate: dateString });
    };

    updateClick = () => {
        this.setState({ redirect: true });
    }

    updateRedirect = () => {
        return (
            <Redirect to={`/update-course/${this.state.courseData.id}`} push={true} />
        );
    }

    deleteClick = async () => {
        const id = this.state.courseData.id;
        const cancel = axios.CancelToken.source();
        try {
            const response = await MakeRequestAsync(`Courses/delete/${id}`, "hello", "post", cancel.token);
            const message = response.data.message;
            NotificationOk(message);
            this.setState({ redirectDelete: true });
        } catch (error) {
            cancel.cancel("CANCEL BECAUSE OF ERROR");
            this.setCatch(error);
        }
    }

    deleteRedirect = () => {
        return (
            <Redirect to={courses} push={true} />
        );
    }

    disabledDate = current => {
        const start = moment();
        return current < start;
    };

    setCatch = (error) => {
        NotificationError(error);
    };

    setFinally = () => {
        this.setState({ isLoading: false });
    };

    render() {
        const { isLoading, courseData, selectedDate, plug, redirect, redirectDelete } = this.state;
        const role = this.props.currentUser.role;
        const isAdmin = role === ADMIN;
        const isDateSelected = selectedDate !== "" && selectedDate !== null;
        const course = courseData.course === null ? plug : courseData.course;
        const isPresent = courseData.isPresent;
        const spinner = <Space size="middle"> <Spin tip="Getting course data..." size="large" /></Space>;

        const updateBtn =
            <Button
                className="course-btn"
                type="primary"
                size="medium"
                onClick={this.updateClick}>Edit course</Button>;

        const deleteBtn =
            <Button
                className="course-btn"
                type="primary"
                size="medium"
                danger={true}
                onClick={this.deleteClick}>Delete course</Button>;
        return (
            <>
                {isLoading === true && spinner}
                {
                    isLoading === false &&
                    <CourseContainer
                        isPresent={isPresent}
                        currentUser={this.props.currentUser}
                        course={course}
                        isDateSelected={isDateSelected}
                        handleConfirm={this.handleConfirm}
                        handleDateChange={this.handleDateChange}
                        disabledDate={this.disabledDate}
                        userId={this.props.currentUser.id}
                    />

                }
                {isAdmin && updateBtn}
                {isAdmin && deleteBtn}
                {redirect && this.updateRedirect()}
                {redirectDelete && this.deleteRedirect()}
            </>
        )
    };
};

export default withRouter(connect(
    state => ({
        currentUser: state.userReducer
    }),
    dispatch => ({
        onEmailConfirmedChanged: (emailConfirmed) => {
            dispatch({ type: SET_EMAIL_CONFIRMED, payload: emailConfirmed })
        }
    })
)(CourseDetailsComponent));