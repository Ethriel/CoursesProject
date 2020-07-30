import React from 'react';
import Container from '../common/ContainerComponent';
import H from '../common/HAntD';
import ButtonComponent from '../common/ButtonComponent';
import { Typography, DatePicker } from 'antd';

const { Paragraph } = Typography;

const CourseContainer = props => {
    const course = props.course;
    const isDateSelected = props.isDateSelected;
    const handleDateChange = props.handleDateChange;
    const disabledDate = props.disabledDate;
    const handleConfirm = props.handleConfirm;
    const disableButton = props.isPresent === true ? true : !isDateSelected;

    const classNameContainer =
        [
            "display-flex", "col-flex", "center-flex",
            "width-75", "center-a-div"
        ];
    const classNameConfirm = ["display-flex", "width-50", "space-between-flex", "center-a-div"];

    return (
        <Container classes={classNameContainer}>
            <H myText={course.title} level={4} />
            <img
                style={{ margin: "0 auto" }}
                alt="No"
                src={`https://localhost:44382/${course.cover}`} />
            <Paragraph className="course-details-margin">
                {course.description}
            </Paragraph>
            <Container classes={classNameConfirm}>
                <DatePicker
                    format='DD/MM/YYYY'
                    onChange={handleDateChange}
                    disabledDate={disabledDate} />
                <ButtonComponent
                    size="medium"
                    myHandler={handleConfirm}
                    myText="Select course"
                    disabled={disableButton}
                />

            </Container>
        </Container>
    )
}

export default CourseContainer;