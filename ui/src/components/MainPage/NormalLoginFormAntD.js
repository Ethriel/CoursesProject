import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Form, Input, Button } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { withRouter } from "react-router";

const NormalLoginForm = (props) => {
    const confirHandler = props.myConfirHandler;
    const forgetRef = props.myForgetRef;

    const forgotClick = (event) => {
        props.history.push("/forgotPassword");
    }

    return (
        <Form
            name="normal_login"
            className="ant-login-form-my"
            initialValues={{
                remember: true,
            }}
            onFinish={confirHandler}>

            <Form.Item
                name="username"
                rules={[
                    {
                        required: true,
                        message: 'Please input your Username!',
                    },
                ]}>
                <Input prefix={<UserOutlined className="site-form-item-icon" />} placeholder="Username" />
            </Form.Item>

            <Form.Item
                name="password"
                rules={[
                    {
                        required: true,
                        message: 'Please input your Password!',
                    },
                ]}>
                <Input
                    prefix={<LockOutlined className="site-form-item-icon" />}
                    type="password"
                    placeholder="Password"
                />
            </Form.Item>

            <Form.Item>
                <Button type="primary"
                    htmlType="submit"
                    style={{ width: '100%' }}
                    size="large"
                    className="ant-btn-primary-my">
                    Sign in
                </Button>
            </Form.Item>

            <Form.Item>
                <a className="login-form-forgot" onClick={forgotClick} href={forgetRef}>
                    Forgot password
                </a>
            </Form.Item>
        </Form>
    );
};

export default withRouter(NormalLoginForm);