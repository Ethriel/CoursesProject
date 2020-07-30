import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { Spin, Space } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import moment from 'moment';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import ModalWithMessage from '../common/ModalWithMessage';
import CourseContainer from './CourseContainer';
import axios from 'axios';
import SetModalData from '../../helpers/SetModalData';
import { USER, ADMIN } from '../common/roles';
import { forbidden } from '../../Routes/RoutersDirections';

class CourseDetailsComponent extends Component {
    constructor(props) {
        super(props)
        this.state = {
            course: {
                id: props.match.params.id,
                course: null,
                isPresent: false
            },
            isLoading: true,
            plug: { title: "", cover: "", description: "" },
            selectedDate: "",
            modal: GetModalPresentation(this.modalOk, this.modalCancel)
        }
    }

    getCourse = async (token) => {
        try {
            const response = await MakeRequestAsync(`courses/get/${this.state.course.id}`, { msg: "hello" }, "get", token);
            const course = response.data.data;
            this.setState(old => ({
                course: {
                    ...old.course,
                    course: course
                }
            }));
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }

    }
    checkCourse = async (token) => {
        const userId = localStorage.getItem("current_user_id");
        const courseId = this.state.course.id;
        try {
            const response = await MakeRequestAsync(`courses/check/${userId}/${courseId}`, { msg: "hello" }, "get", token);
            const data = response.data.data;
            const isPresent = data.isPresent;
            const studyDate = data.studyDate;
            this.setState(old => ({
                course: {
                    ...old.course,
                    isPresent: isPresent,
                    studyDate: data.studyDate
                }
            }));
            if (isPresent === true) {
                this.setState(old => ({
                    modal: {
                        ...old.modal,
                        message: `You already have this course in your list. Start at ${studyDate}`,
                        visible: true
                    }
                }));
            }

        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }

    }
    componentDidMount = async () => {
        const role = this.props.store.userRoleReducer.role;
        if (role !== USER && role !== ADMIN) {
            this.props.history.push(forbidden);
        }
        else {
            const signal = axios.CancelToken.source();
            await this.checkCourse(signal.token);
            await this.getCourse(signal.token);
        }
    }

    handleConfirm = async () => {
        this.setState({ isLoading: true });
        const userId = localStorage.getItem("current_user_id");
        const courseId = this.state.course.id;
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
        this.setState({
            selectedDate: dateString
        });
    };

    disabledDate = current => {
        const start = moment();
        return current < start;
    };

    setModal = () =>{
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    };
    
    modalOk = (e) => {
        this.setModal();
    };

    modalCancel = (e) => {
        this.setModal();
    };

    setCatch = (error) => {
        const modalData = SetModalData(error);
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                message: modalData.message,
                errors: modalData.errors,
                visible: true
            }
        }));
    };

    setFinally = () => {
        this.setState({ isLoading: false });
    }

    render() {
        const { isLoading, course, selectedDate, modal, plug } = this.state;
        const isDateSelected = selectedDate !== "" && selectedDate !== null;
        const courseData = course.course === null ? plug : course.course;
        const isPresent = course.isPresent;
        const spinner = <Space size="middle"> <Spin tip="Getting course data..." size="large" /></Space>;
        const modalWindow = ModalWithMessage(modal);
        return (
            <>
                {isLoading === true && spinner}
                {modal.visible === true && modalWindow}
                {
                    isLoading === false &&
                    <CourseContainer
                        isPresent={isPresent}
                        course={courseData}
                        isDateSelected={isDateSelected}
                        handleDateChange={this.handleDateChange}
                        disabledDate={this.disabledDate}
                        handleConfirm={this.handleConfirm}
                    />
                }
            </>
        )
    };
};

export default withRouter(connect(
    state => ({
        store: state
    })
)(CourseDetailsComponent));