import React, { useState } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Form, Input, Button } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';
import SetModalData from '../../helpers/SetModalData';
import ModalWithMessage from '../common/ModalWithMessage';
import GetModalPresentation from '../../helpers/GetModalPresentation';

const ForgotPassword = () => {
    const modalOk = () => {
        setModal(oldModal => ({ ...oldModal, ...{ visible: false } }));
    };

    const modalCancel = () => {
        setModal(oldModal => ({ ...oldModal, ...{ visible: false } }));
    };
    const [modal, setModal] = useState(GetModalPresentation(modalOk, modalCancel));

    const setCatch = (error) => {
        const modalData = SetModalData(error);
        setModal(oldModal => ({
            ...oldModal, ...{
                message: modalData.message,
                errors: modalData.errors
            }
        }));
    };

    const setFinally = () => {
        setModal(oldModal => ({
            ...oldModal,
            ...{
                visible: true
            }
        }));
    };

    const handleSubmit = async (values) => {
        const signal = axios.CancelToken.source();
        try {
            const userData = {
                email: values.email,
            };

            const response = await MakeRequestAsync("account/forgotPassword", userData, "post", signal.token);
            const data = response.data;
            const message = data.message;

            setModal(oldModal => ({
                ...oldModal, ...{
                    message: message
                }
            }));

        } catch (error) {
            setCatch(error);
        } finally {
            setFinally();
        }
    };

    const [form] = Form.useForm();

    const modalWindow = ModalWithMessage(modal);

    return (
        <>
            {modal.visible === true && modalWindow}
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
        </>
    )
}

export default ForgotPassword;