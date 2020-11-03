import React from 'react';
import { Button, Divider } from 'antd';
import Container from '../common/ContainerComponent';
import EditableInfo from './EditableUserInfoComponent';
import StaticInfo from './StaticUserInfoComponent';
import H from '../common/HAntD';

const UserInfoContainer = ({ imageUploader, ...props }) => {
    const user = props.user;
    const classes = ["display-flex", "col-flex", "width-90", "min-width-150", "max-width-300", "align-center", "center-a-div"];
    const onValueChange = props.onValueChange;
    const submitClick = props.submitClick;
    return (
        <Container classes={classes}>
            <H level={4} myText="User profile" />
            <EditableInfo label="First name:" text={user.firstName} onChange={(str) => onValueChange("firstName", str)} />
            <EditableInfo label="Last name:" text={user.lastName} onChange={(str) => onValueChange("lastName", str)} />
            <EditableInfo label="Email:" text={user.email} onChange={(str) => onValueChange("email", str)} />
            <StaticInfo label="Age:" text={user.age} />
            <StaticInfo label="Birth date:" text={user.birthDate} />
            <Button
                type="primary"
                size="default"
                onClick={submitClick}
                style={{ width: 150 }}>
                Submit
                </Button>
            <Divider />
            {imageUploader && imageUploader}
        </Container>
    )
}

export default UserInfoContainer