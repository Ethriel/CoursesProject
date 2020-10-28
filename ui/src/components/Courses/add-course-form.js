import React from 'react';
import { Button, DatePicker, Input, Form } from 'antd';
import 'antd/dist/antd.css';

const AddCourseForm = ({ onFinish, ...props }) => {
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

    return (
        <Form
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
                <Input />
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
                <Input />
            </Form.Item>

            <Form.Item
                name="startDate"
                label="Start date"
                rules={
                    [
                        {
                            type: 'object',
                            required: true,
                            message: "Select date, please"
                        }
                    ]
                }>
                <DatePicker
                    format='DD/MM/YYYY'
                />
            </Form.Item>
            <Form.Item {...tailLayout}>
                <Button type="primary" htmlType="submit">
                    Submit
          </Button>
            </Form.Item>
        </Form>
    )
};

export default AddCourseForm;