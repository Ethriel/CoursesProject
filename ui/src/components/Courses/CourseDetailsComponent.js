import React, { Component } from 'react';
import { Typography, DatePicker } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import Container from '../common/ContainerComponent';
import H from '../common/HAntD';
import ButtonComponent from '../common/ButtonComponent';
import moment from 'moment';

const { Paragraph } = Typography;

class CourseDetailsComponent extends Component {
    constructor(props) {
        super(props)
        this.state = {
            id: props.match.params.id,
            course: null,
            isLoading: true,
            plug: { title: "", cover: "", description: "" },
            selectedDate: ""
        }
        this.handleConfirm = this.handleConfirm.bind(this);
    }

    async componentDidMount() {
        const response = await MakeRequestAsync(`https://localhost:44382/courses/get/${this.state.id}`, { msg: "hello" }, "get");
        const course = response.data;
        this.setState({
            course: course,
            isLoading: false
        });
    }

    async handleConfirm() {
        const userId = localStorage.getItem("current_user_id");
        const courseId = this.state.id;
        const date = this.state.selectedDate;
        const data = {
            systemUserId: userId,
            trainingCourseId: courseId,
            studyDate: date
        };
        try {
            const response = await MakeRequestAsync("https://localhost:44382/UserCourses/add", data, "post");
        } catch (error) {
            console.log(error);
        }
    }

    handleDateChange = (value, dateString) => {
        this.setState({
            selectedDate: dateString
        });
    }

    disabledDate = current => {
        const start = moment();
        return current < start;
    }

    render() {
        const classNameContainer = ["display-flex", "col-flex", "center-flex", "width-75", "center-a-div"];
        const classNameConfirm = ["display-flex", "width-50", "space-between-flex", "center-a-div"];
        const { title, cover, description } = this.state.isLoading ? this.state.plug : this.state.course;
        const isDateSelected = this.state.selectedDate !== "" && this.state.selectedDate !== null;
        return (
            <>
                {
                    !this.state.isLoading &&
                    <Container classes={classNameContainer}>
                        <H myText={title} level={3} />
                        <img
                            style={{ margin: "0 auto" }}
                            alt="No"
                            src={`https://localhost:44382/${cover}`} />
                        <Paragraph>
                            {description}
                        </Paragraph>
                        <Container classes={classNameConfirm}>
                            <DatePicker
                                format='DD/MM/YYYY'
                                onChange={this.handleDateChange}
                                disabledDate={this.disabledDate} />
                            {
                                isDateSelected &&
                                <ButtonComponent size="medium" myHandler={this.handleConfirm} myText="Select course" />
                            }
                        </Container>
                    </Container>
                }
            </>

        );
    };
};

export default CourseDetailsComponent;