import React, { useState } from 'react';
import { Menu } from 'antd';
import { UserAddOutlined, LoginOutlined } from '@ant-design/icons'
import SignIn from './LoginComponent';
import SignUp from './RegistrationComponent';
import Container from '../common/ContainerComponent';

const SwitchUserActions = (props) => {
    const [current, setCurrent] = useState("signin");
    const handleClick = event => {
        setCurrent(event.key);
    };

    const switchActions =
        <Menu mode="horizontal"
            onClick={handleClick}
            selectedKeys={[current]}
            className="sign-in-up-my"
        >
            <Menu.Item
                key="signin"
                icon={<LoginOutlined />}>
                Sign in
            </Menu.Item>

            <Menu.Item
                key="signup"
                icon={<UserAddOutlined />}>
                Sign up
            </Menu.Item>
        </Menu>;

    const containerClasses =
        [
            "display-flex", "col-flex", "center-a-div",
            "max-width-300", "align-center",
            "width-100", "margin-inside"
        ];

    return (
        <Container classes={containerClasses}>
            {switchActions}
            {current === "signin" && <SignIn />}
            {current === "signup" && <SignUp />}
        </Container>
    );
};

export default SwitchUserActions;