import React from 'react';
import H from '../common/HAntD';
import Container from '../common/ContainerComponent';
import { Typography } from 'antd';
const { Paragraph } = Typography;
const ProfileSubComponent = props => {
    const text = props.text;
    const p = <Paragraph >{text}</Paragraph>
    const h = <H level={4} myText={text} />;
    const classes = ["display-flex", "center-flex", "center-a-div", "width-100"];
    const container =
        <Container classes={classes}>
            {p}
        </Container>;

    return p;
}

export default ProfileSubComponent;