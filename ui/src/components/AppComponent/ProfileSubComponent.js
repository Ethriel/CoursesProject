import React from 'react';
import H from '../common/HAntD';
import Container from '../common/ContainerComponent';

const ProfileSubComponent = props => {
    const text = props.text;
    const h = <H level={4} myText={text} />;
    const classes = ["display-flex", "center-flex", "center-a-div", "width-100"];
    const container = 
    <Container classes={classes}>
        {h}
        </Container>;

    return container;
}

export default ProfileSubComponent;