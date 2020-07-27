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

    const formItemLayout = {
        labelCol: {
            xs: {
                span: 24,
            },
            sm: {
                span: 8,
            },
        },
        wrapperCol: {
            xs: {
                span: 24,
            },
            sm: {
                span: 16,
            },
        },
    };

    const tailFormItemLayout = {
        wrapperCol: {
            xs: {
                span: 24,
                offset: 0,
            },
            sm: {
                span: 16,
                offset: 8,
            },
        },
    };

    const [form] = Form.useForm();

    const modalWindow = ModalWithMessage(modal);

    return (
        <>
            {modal.visible === true && modalWindow}
            <Form
                form={form}
                {...formItemLayout}
                name="forgot_password"
                className="my-sign-up-form center-a-div"
                onFinish={handleSubmit}>
                <Form.Item
                    name="email"
                    label="Enter email"
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
                    <Input />
                </Form.Item>

                <Form.Item {...tailFormItemLayout}>
                    <Button type="primary" htmlType="submit" size="large">
                        Submit
                    </Button>
                </Form.Item>
            </Form>
        </>
    )
}

export default ForgotPassword;