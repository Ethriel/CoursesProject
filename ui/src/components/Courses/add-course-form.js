import React from 'react';
import { Button, DatePicker, Input, Form, Spin, Space } from 'antd';
import 'antd/dist/antd.css';
import ContainerComponent from '../common/ContainerComponent';
import moment from 'moment';
const AddCourseForm = ({ onFinish, loading, reset, course, ...props }) => {
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

    if(!course){
        course.title = course.description = course.cover = "";
        course.startDate = moment();
    }

    const [form] = Form.useForm();

    if (reset) {
        form.resetFields();
    }

    const spinner = <Space size="middle"> <Spin tip="Processing..." size="large" /></Space>;

    return (
        <ContainerComponent classes={["width-50 center-a-div"]}>
            {loading && spinner}
            {!loading &&
                <Form
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
                        <Input value={course.title}/>
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
                        <Input value={course.description}/>
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
                            value={course.startDate}
                        />
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

export default AddCourseForm;