import React from 'react';
import 'antd/dist/antd.css';
import { Form, Input, Button } from 'antd';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import axios from 'axios';
import NotificationError from '../../common/notifications/notification-error';
import NotificationOk from '../../common/notifications/notification-ok';

const ConfirmEmailForm = () => {

    const setCatch = (error) => {
        NotificationError(error);
    };

    const handleSubmit = async (values) => {
        const signal = axios.CancelToken.source();
        try {
            const userData = {
                email: values.email,
            };

            const response = await MakeRequestAsync("account/confirmEmailRequest", userData, "post", signal.token);
            const data = response.data;
            const message = data.message;

            NotificationOk(message, true);
        } catch (error) {
            setCatch(error);
        }
    };

    const [form] = Form.useForm();

    return (
        <Form
            form={form}
            name="confirm_email_request"
            className="center-a-div max-width-300"
            onFinish={handleSubmit}>
            <Form.Item
                name="email"
                className="ant-input-my-sign-up"
                rules={[
                    {
                        type: 'email',
                        message: 'The input is not a valid email!',
                    },
                    {
                        required: true,
                        message: 'Please input your email!',
                    },
                ]}>
                <Input placeholder="Enter email" className="ant-input-my-sign-up" />
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

export default ConfirmEmailForm;