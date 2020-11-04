import React, { useState } from 'react';
import Container from '../common/ContainerComponent';
import H from '../common/HAntD';
import ButtonComponent from '../common/ButtonComponent';
import { Typography, DatePicker, Button } from 'antd';
import { connect } from 'react-redux';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import NotificationError from '../common/notifications/notification-error';
import './course.css';

const { Paragraph } = Typography;

const CourseContainer = ({ userId, isDateSelected, handleDateChange, disabledDate, course, ...props }) => {
    const handleConfirm = props.handleConfirm;
    let disableButton = !isDateSelected;
    const isEmailConfirmed = props.currentUser.emailConfirmed;

    if (!isEmailConfirmed) {
        disableButton = true;
    }

    const [disableUnsub, setDisableUnsub] = useState(!(props.isPresent === true && isEmailConfirmed === true));

    const classNameContainer =
        [
            "display-flex", "col-flex", "center-flex",
            "width-75", "center-a-div"
        ];
    const classNameConfirm = ["display-flex", "width-50", "space-between-flex", "center-a-div"];
    // const src = course.cover.includes('http') ? course.cover : `https://localhost:44382/${course.cover}`;
    const src = course.cover;

    const setCatch = (error) => {
        NotificationError(error);
    };

    const handleUnsubscribe = async () => {
        const data = {
            courseId: course.id,
            userId: userId
        };

        try {
            await MakeRequestAsync("UserCourses/unsubscribe", data, "post");
            setDisableUnsub(false);
        } catch (error) {
            setCatch(error);
        }
    }

    return (
        <Container classes={classNameContainer}>
            <H myText={course.title} level={4} />
            <img
                style={{ margin: "0 auto" }}
                alt="No"
                className="course-details-img"
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
                    disabled={disableUnsub}
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