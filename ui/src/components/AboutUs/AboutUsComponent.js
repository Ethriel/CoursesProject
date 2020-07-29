import React from 'react';
import { Typography } from 'antd';
import { NavLink } from 'react-router-dom';
import { main } from '../../Routes/RoutersDirections';
import Container from '../common/ContainerComponent';

const { Title, Paragraph } = Typography;
const AboutUsComponent = () => {
    const signIn = <NavLink to={main}>sign in</NavLink>;
    const signUp = <NavLink to={main}>sign up</NavLink>;
    const classes = [
        "display-flex", "col-flex", "align-center",
        "justify-center", "center-a-div", "height-80"
    ];
    return (
        <Container classes={classes}>
            <Title level={2}>Forge your future with us!</Title>
            <Paragraph>Training courses on various topics. Pick and learn</Paragraph>
            <Paragraph>All you need is just to {signIn} or {signUp}!</Paragraph>
        </Container>
    );
};
export default AboutUsComponent;