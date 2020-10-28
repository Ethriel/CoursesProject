import React from 'react';
import Container from '../common/ContainerComponent';
import H from '../common/HAntD';
import ButtonComponent from '../common/ButtonComponent';
import { Typography, DatePicker, Button } from 'antd';
import { connect } from 'react-redux';

const { Paragraph } = Typography;

const CourseContainer = props => {
    const course = props.course;
    const isDateSelected = props.isDateSelected;
    const handleDateChange = props.handleDateChange;
    const disabledDate = props.disabledDate;
    const handleConfirm = props.handleConfirm;
    const handleUnsubscribe = props.handleUnsubscribe;
    let disableButton = !isDateSelected;
    const isEmailConfirmed = props.currentUser.emailConfirmed;
    if (!isEmailConfirmed) {
        disableButton = true;
    }

    const classNameContainer =
        [
            "display-flex", "col-flex", "center-flex",
            "width-75", "center-a-div"
        ];
    const classNameConfirm = ["display-flex", "width-50", "space-between-flex", "center-a-div"];
    const src = course.cover.contains('http') ? `https://localhost:44382/${course.cover}` : course.cover;
    return (
        <Container classes={classNameContainer}>
            <H myText={course.title} level={4} />
            <img
                style={{ margin: "0 auto" }}
                alt="No"
                src={src} />
            <Paragraph className="course-details-margin">
                {course.description}
            </Paragraph>
            <Container classes={classNameConfirm}>
                <DatePicker
                    format='DD/MM/YYYY'
                    onChange={handleDateChange}
                    disabledDate={disabledDate}
                    disabled={props.isPresent === true || isEmailConfirmed === false} />
                <ButtonComponent
                    size="medium"
                    myHandler={handleConfirm}
                    myText="Select course"
                    disabled={disableButton}
                />
                <Button
                    type="primary"
                    size="medium"
                    danger={true}
                    onClick={handleUnsubscribe}
                    disabled={!(props.isPresent === true && isEmailConfirmed === true)}
                >
                    Unsubscribe
                </Button>
            </Container>
        </Container>
    )
}

export default connect(
    state => ({
        currentUser: state.userReducer
    })
)(CourseContainer);