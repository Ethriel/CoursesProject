import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import '../../css/styles.css';
import {
  Form,
  Input,
  Checkbox,
  Button,
  DatePicker
} from 'antd';
import ButtonFaceBook from './ButtonFacebook';




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
      {...formItemLayout}
      form={form}
      style={{ width: '50%', maxWidth: 600 }}
      name="register"
      onFinish={props.onFinish}
      size="small"
      initialValues={{
        residence: ['zhejiang', 'hangzhou', 'xihu'],
        prefix: '86',
      }}
      scrollToFirstError
    >
      <Form.Item
        name={['user', 'name']}
        label="First name"
        rules={
          [
            {
              required: true,
              message: "First name is required"
            }
          ]
        }>
        <Input />
      </Form.Item>
      <Form.Item
        name={['user', 'lastname']}
        label="Last name"
        rules={
          [
            {
              required: true,
              message: "Last name is required"
            }
          ]
        }>
        <Input />
      </Form.Item>
      <Form.Item label="Birth date">
        <DatePicker format='DD/MM/YYYY'/>
      </Form.Item>
      <Form.Item
        name="email"
        label="E-mail"
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
        <Input />
      </Form.Item>

      <Form.Item
        name="password"
        label="Password"
        rules={[
          {
            required: true,
            message: 'Please input your password!',
          },
        ]}
        hasFeedback
      >
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
        ]}
      >
        <Input.Password />
      </Form.Item>

      <Form.Item
        name="agreement"
        valuePropName="checked"
        rules={[
          {
            validator: (_, value) =>
              value ? Promise.resolve() : Promise.reject('Accept an agreement, please'),
          },
        ]}
        {...tailFormItemLayout}
      >
        <Checkbox>
          I have read the <a href="">agreement</a>
        </Checkbox>
      </Form.Item>
      <Form.Item {...tailFormItemLayout}>
        <Button type="primary" htmlType="submit" size="large">
          Register
        </Button>
        <ButtonFaceBook clickHandler={props.facebook} />
      </Form.Item>
    </Form>
  );
};

export default RegistrationForm;