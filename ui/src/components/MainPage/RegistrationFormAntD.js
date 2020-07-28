import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import '../../css/styles.css';
import {
  Form,
  Input,
  Button,
  DatePicker
} from 'antd';
import '../../css/styles.css';



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

const RegistrationForm = (props) => {
  const [form] = Form.useForm();

  return (
    <Form
      form={form}
      name="register"
      onFinish={props.onFinish}
      size="small"

      scrollToFirstError
    >
      <Form.Item
        name={['user', 'name']}
        // label="First name"
        className="ant-input-my-sign-up"
        rules={
          [
            {
              required: true,
              message: "First name is required"
            }
          ]
        }>
        <Input placeholder="First name"
        className="ant-input-my-sign-up"
        />
      </Form.Item>
      <Form.Item
        name={['user', 'lastname']}
        // label="Last name"
        className="ant-input-my-sign-up"
        rules={
          [
            {
              required: true,
              message: "Last name is required"
            }
          ]
        }>
        <Input placeholder="Last name"
        className="ant-input-my-sign-up"
        />
      </Form.Item>

      <Form.Item
        name="email"
        // label="E-mail"
        className="ant-input-my-sign-up"
        rules={[
          {
            type: 'email',
            message: 'The input is not valid E-mail!',
          },
          {
            required: true,
            message: 'Please input your E-mail!',
          },
        ]}
      >
        <Input placeholder="Email"
        className="ant-input-my-sign-up"
        />
      </Form.Item>

      <Form.Item
        name="password"
        className="ant-input-my-sign-up"
        // label="Password"
        rules={[
          {
            required: true,
            message: 'Please input your password!',
          },
        ]}
        hasFeedback
      >
        <Input.Password placeholder="Password"
        className="ant-input-my-sign-up"
        />
      </Form.Item>

      <Form.Item
        name="confirm"
        className="ant-input-my-sign-up"
        // label="Confirm Password"
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
        ]}
      >
        <Input.Password placeholder="Confirm password"
        className="ant-input-my-sign-up"
        />
      </Form.Item>
      <Form.Item
        name="birthdate"
        // label="Birth date"

        rules={
          [
            {
              type: 'object',
              required: true,
              message: "Select date of birth, please"
            }
          ]
        }>
        <DatePicker
          format='DD/MM/YYYY'
          className="ant-input-my-sign-up"
          placeholder="Select birth date"
        />
      </Form.Item>

      <Form.Item {...tailFormItemLayout}>
        <Button
          type="primary"
          htmlType="submit"
          size="large"
          className="ant-btn-primary-my"
          style={{ width: '100%' }}
        >
          Sign up
        </Button>
      </Form.Item>
    </Form>
  );
};

export default RegistrationForm;