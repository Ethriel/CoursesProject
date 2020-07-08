import React, { Component } from 'react';
import { Typography, DatePicker } from 'antd';
import MakeRequest from '../../helpers/MakeRequest';
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
            plug: { a: "", b: "", c: "" },
            selectedDate: ""
        }
    }

    async componentDidMount() {
        const gottedCourse = await MakeRequest(`https://localhost:44382/courses/get/${this.state.id}`, { msg: "hello" }, "get");
        this.setState({
            course: gottedCourse,
            isLoading: false
        });
    }

    handleConfirm(){

    }
    handleDateChange(value, dateString){
        console.log("DATE STRING = ", dateString);
    }

    render() {
        const classNameContainer = ["display-flex", "col-flex", "center-flex", "width-75", "height-100", "center-a-div"];
        const { title, cover, description } = this.state.isLoading ? this.state.plug : this.state.course;
        const classNameConfirm = ["display-flex", "width-50", "space-between-flex", "center-a-div"];
        return (
            <>
                {
                    !this.state.isLoading &&
                    <Container classes={classNameContainer}>
                        <H myText={title} level={2} />
                        <img 
                            style={{ margin: "0 auto" }}
                            alt="No"
                            src={`https://localhost:44382/${cover}`} />
                        <Paragraph>
                            {description}
                        </Paragraph>
                        <Container classes={classNameConfirm}>
                            <DatePicker format='DD.MM.YYYY' onChange={this.handleDateChange}/>
                            <ButtonComponent size="medium" onClick={this.handleConfirm} myText="Select course"/>
                        </Container>
                    </Container>
                }
            </>

        );
    };
};

export default CourseDetailsComponent;