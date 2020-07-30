import React from 'react';
import 'antd/dist/antd.css';
import { Form, Input, Button } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { withRouter } from "react-router";
import { forgotPassword } from '../../../Routes/RoutersDirections';
import ButtonFaceBook from '../Facebook/ButtonFacebook';

const NormalLoginForm = (props) => {
    const confirHandler = props.myConfirHandler;
    const facebookResponse = props.facebookResponse;

    const forgotClick = (event) => {
        props.history.push(forgotPassword);
    }
    return (
        <Form
            name="normal_login"
            onFinish={confirHandler}>

            <Form.Item
                name="username"
                rules={[
                    {
                        required: true,
                        message: 'Please input your Username!',
                    },
                ]}>
                <Input
                    className="ant-input-my"
                    prefix={<UserOutlined className="site-form-item-icon" />}
                    placeholder="Username" />
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
                    className="ant-input-my"
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
                <ButtonFaceBook facebookResponse={facebookResponse} />
            </Form.Item>

            <Form.Item>
                <a className="login-form-forgot ant-login-forgot-my" onClick={forgotClick} href={forgotPassword}>
                    Forgot password
                </a>
            </Form.Item>
        </Form>
    );
};

export default withRouter(NormalLoginForm);