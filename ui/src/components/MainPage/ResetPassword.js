import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Form, Input, Button } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';
import Notification from '../common/Notification';
import { withRouter } from "react-router";

const queryString = require('query-string');

const ResetPassword = (props) => {
    const handleSubmit = async (values) => {
        const search = props.location.search;
        const parsed = queryString.parse(search);
        const parsedToken = parsed.token;
        const parsedEmail = parsed.email;

        const resetPasswordData = {
            email: parsedEmail,
            token: parsedToken,
            password: values.password
        };

        const signal = axios.CancelToken.source();

        try {
            const response = await MakeRequestAsync("account/resetPassword", resetPasswordData, "post", signal.token);
            const data = response.data;

            Notification(undefined, undefined, data.message, true);
        } catch (error) {
            Notification(error);
        }
    };
    const [form] = Form.useForm();

    return (
        <Form
            form={form}
            name="reset_password"
            className="center-a-div max-width-300"
            onFinish={handleSubmit}>
            <Form.Item
                name="password"
                className="ant-input-my-sign-up"
                rules={[
                    {
                        required: true,
                        message: 'Please input your new password!',
                    }
                ]}
                hasFeedback>
                <Input.Password placeholder="New password" className="ant-input-my-sign-up" />
            </Form.Item>
            <Form.Item
                name="confirm"
                className="ant-input-my-sign-up"
                dependencies={['password']}
                hasFeedback
                rules={[
                    {
                        required: true,
                        message: 'Please confirm your new password!',
                    },
                    ({ getFieldValue }) => ({
                        validator(rule, value) {
                            if (!value || getFieldValue('password') === value) {
                                return Promise.resolve();
                            }

                            return Promise.reject('The two passwords that you entered do not match!');
                        },
                    }),
                ]}>
                <Input.Password placeholder="Confirm new password" className="ant-input-my-sign-up" />
            </Form.Item>
            <Form.Item>
                <Button type="primary" htmlType="submit" size="large"
                    className="ant-btn-primary-my"
                    style={{ width: '100%' }}>
                    Submit
                    </Button>
            </Form.Item>
        </Form>
    )
}

export default withRouter(ResetPassword);