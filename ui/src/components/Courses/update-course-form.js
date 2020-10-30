import React from 'react';
import { Button, DatePicker, Input, Form, Spin, Space } from 'antd';
import 'antd/dist/antd.css';
import ContainerComponent from '../common/ContainerComponent';

const UpdateCourseForm = ({ onFinish, loading, course, ...props }) => {
    const layout = {
        labelCol: {
            span: 8,
        },
        wrapperCol: {
            span: 16,
        },
    };
    const tailLayout = {
        wrapperCol: {
            offset: 8,
            span: 16,
        },
    };

    const [form] = Form.useForm();

    const initialValues = {
        title: course.title,
        description: course.description,
        cover: course.cover
    }

    const spinner = <Space size="middle"> <Spin tip="Processing..." size="large" /></Space>;

    return (
        <ContainerComponent classes={["width-50 center-a-div"]}>
            {loading && spinner}
            {!loading &&
                <Form
                initialValues={initialValues}
                    size="middle"
                    {...layout}
                    form={form}
                    name="add-course-form"
                    onFinish={onFinish}>
                    <Form.Item
                        name="title"
                        label="Title"
                        rules={[
                            {
                                required: true,
                                message: "Input title, please"
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        name="description"
                        label="Description"
                        rules={[
                            {
                                required: true,
                                message: "Input description, please"
                            },
                        ]}
                    >
                        <Input.TextArea autoSize={true} />
                    </Form.Item>

                    <Form.Item
                        name="cover"
                        label="Cover"
                        rules={[
                            {
                                required: true,
                                message: "Input cover URL, please"
                            },
                        ]}
                    >
                        <Input/>
                    </Form.Item>

                    <Form.Item {...tailLayout}>
                        <Button type="primary" htmlType="submit">
                            Submit
          </Button>
                    </Form.Item>
                </Form>
            }

        </ContainerComponent>
    )
};

export default UpdateCourseForm;