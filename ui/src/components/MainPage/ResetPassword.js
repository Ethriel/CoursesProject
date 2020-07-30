import React, { useState } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Form, Input, Button } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';
import SetModalData from '../../helpers/SetModalData';
import ModalWithMessage from '../common/ModalWithMessage';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import { withRouter } from "react-router";

const queryString = require('query-string');

const ResetPassword = (props) => {
    const closeModal = () => {
        setModal(oldModal => ({ ...oldModal, ...{ visible: false } }));
        if (allGood === true) {
            props.history.push("/");
        }
    }
    const modalOk = () => {
        closeModal();
    };

    const modalCancel = () => {
        closeModal();
    };

    const [modal, setModal] = useState(GetModalPresentation(modalOk, modalCancel));
    const [allGood, setAllGood] = useState(false);

    const setCatch = (error) => {
        const modalData = SetModalData(error);
        setModal(oldModal => ({
            ...oldModal,
            ...{
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
        const search = props.location.search;
        const parsed = queryString.parse(search);
        console.log(parsed);
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
            setModal(oldModal => ({
                ...oldModal,
                ...{
                    message: data.message
                }
            }));
            setAllGood(true);

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
                    name="password"
                    label="New password"
                    rules={[
                        {
                            required: true,
                            message: 'Please input your password!',
                        }
                    ]}
                    hasFeedback>
                    <Input.Password />
                </Form.Item>
                <Form.Item
                    name="confirm"
                    label="Confirm Password"
                    dependencies={['password']}
                    hasFeedback
                    rules={[
                        {
                            required: true,
                            message: 'Please confirm your password!',
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
                    <Input.Password />
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

export default withRouter(ResetPassword);