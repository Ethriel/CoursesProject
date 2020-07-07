import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import ButtonFaceBook from '../MainPage/ButtonFacebook';
import { Form, Input, Button, Checkbox } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';

const NormalLoginForm = (props) => {
    const confirHandler = props.myConfirHandler;
    const forgetRef = props.myForgetRef;

    return (
        <Form
            name="normal_login"
            className="login-form"
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
                <Form.Item name="remember" valuePropName="checked" noStyle>
                    <Checkbox>Remember me</Checkbox>
                </Form.Item>

                <a className="login-form-forgot" href={forgetRef}>
                    Forgot password
          </a>
            </Form.Item>

            <Form.Item>
                <Button type="primary" htmlType="submit" size="large">
                    Log in
          </Button>
          <ButtonFaceBook clickHandler={props.facebookHandler} />
            </Form.Item>
        </Form>
    );
};

export default NormalLoginForm;