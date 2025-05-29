import React, { useState } from 'react';
import { Button, Form, Input, Typography } from 'antd';
import './index.css';
import CustomInput from '../../../Components/CustomInput';
import CustomPasswordInput from '../../../Components/CustomPasswordInput';
import CustomButton from '../../../Components/CustomButton';

const { Title, Text } = Typography;

interface FormValues {
  userName: string;
  password: string;
}

interface ErrorState {
  [key: string]: string | undefined;
}

const LoginForm: React.FC = () => {
  const [form] = Form.useForm<FormValues>();

  const [errors, setErrors] = useState<ErrorState>({});

  const onFinish = (values: any) => {
    setErrors({});
    console.log('Success:', values);
    // Gọi API login ở đây
  };

  const onFinishFailed = ({ errorFields }: { errorFields: any[] }) => {
    // Lưu lỗi vào state
    const newErrors: ErrorState = {};
    errorFields.forEach((field) => {
      newErrors[field.name[0]] = field.errors[0];
    });
    setErrors(newErrors);
  };

  return (
    <div className="login-container">
      <div className='logo-container'>
        <div className="logo">
        </div>
        <div style={{ color: 'white', fontSize: 24, fontWeight: 'bold', marginLeft: 10 }}>
          KenVerse
        </div>
      </div>

      <div className="login-box">
        <Title level={3} style={{ color: 'white', textAlign: 'center' }}>
          Welcome back!
        </Title>

        <Form
          name="login_demo_discord"
          layout="vertical"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
          autoComplete="off"
          requiredMark={false}
        >
          {/* Input giả để đánh lừa Chrome autofill */}
          <input type="text" name="fakeuser" autoComplete="username" style={{ display: 'none' }} />
          <input type="password" name="fakepassword" autoComplete="current-password" style={{ display: 'none' }} />

          <Form.Item
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: 600 }}>EMAIL</span>{' '}<span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.userName != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> - Login or password is invalid</span>
                )}
              </span>
            }
            name="userName"
            rules={[{ required: true, message: '' }]}
          >
            <CustomInput
              customStyle={{
                backgroundColor: "#28282d",
                color: '#d0d1d3',
                borderColor: '#40444b',
              }}
              size="large"
            />
          </Form.Item>

          <Form.Item
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: 600 }}>PASSWORD</span>{' '}<span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.password != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> - Login or password is invalid</span>
                )}
              </span>
            }
            name="password"
            rules={[{ required: true, message: '' }]}
            help={null}
            style={{ textAlign: "left" }}
          >
            <CustomPasswordInput
              size="large"
              autoComplete="new-password"
              customStyle={{
                backgroundColor: "#28282d",
                color: '#d0d1d3',
                borderColor: '#40444b',
              }}
              visibilityToggle={false}
            />
          </Form.Item>

          <Form.Item>
            <CustomButton type="primary" htmlType="submit" block size="large" style={{ backgroundColor: '#5865f2' }}>
              Login
            </CustomButton>
          </Form.Item>
        </Form>

        <div style={{ textAlign: 'left', marginTop: 16 }}>
          <Text type="secondary" style={{ color: "#b5bac1" }}>
            Need an account?<a href="/Register" style={{ color: "#5865f2" }}> Register</a>
          </Text>
        </div>
      </div>
    </div>
  );
};

export default LoginForm;
