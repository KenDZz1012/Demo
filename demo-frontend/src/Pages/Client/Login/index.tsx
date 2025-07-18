import React, { useEffect, useState } from 'react';
import { Button, Form, Input, message, Typography } from 'antd';
import './index.css';
import CustomInput from '../../../Components/CustomInput';
import CustomPasswordInput from '../../../Components/CustomPasswordInput';
import CustomButton from '../../../Components/CustomButton';
import { useLogin } from '../../../Connections/AppBackend/Auth/Login';
import { useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { RootState } from '../../../app/store';

const { Title, Text } = Typography;

interface FormValues {
  userName: string;
  password: string;
}

interface ErrorState {
  [key: string]: string | undefined;
}

const LoginForm: React.FC = () => {
  const navigate = useNavigate();
  const [messageApi, contextHolder] = message.useMessage();
  const [form] = Form.useForm<FormValues>();
  const [errors, setErrors] = useState<ErrorState>({});
  const loginMutation = useLogin();
  const isLoggedIn = useSelector((state: RootState) => state.auth.isLoggedIn);



  const onFinish = async (values: FormValues) => {
    setErrors({});
    const input = {
      userName: values.userName,
      password: values.password,
    };
    try {
      await loginMutation.mutateAsync(input).then(res => {
        console.log(res)
      });
    }
    catch (error: any) {
      const newErrors: ErrorState = {
        userName: "Invalid email or password",
        password: "Invalid email or password",
      };
      setErrors(newErrors);
    }
  };

  const onFinishFailed = ({ errorFields }: { errorFields: any[] }) => {
    const newErrors: ErrorState = {};
    errorFields.forEach((field) => {
      newErrors[field.name[0]] = "Required";
    });
    setErrors(newErrors);
  };

  useEffect(() => {
    if (loginMutation.isSuccess) {
      navigate('/server/@me');
    }
  }, [loginMutation.isSuccess, navigate]);

  // useEffect(() => {
  //   if (isLoggedIn) {
  //     navigate('/app', { replace: true });
  //   }
  // }, [isLoggedIn, navigate]);

  return (
    <div className="login-container">
      {contextHolder}
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
          <input type="text" name="fakeuser" autoComplete="username" style={{ display: 'none' }} />
          <input type="password" name="fakepassword" autoComplete="current-password" style={{ display: 'none' }} />

          <Form.Item
            validateStatus={errors.userName ? 'error' : ''}
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: "bold" }}>EMAIL</span>{' '}<span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.userName != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> - {errors.userName}</span>
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
            validateStatus={errors.password ? 'error' : ''}
            label={
              <span>
                <span style={{ fontSize: 12, fontWeight: "bold" }}>PASSWORD</span>{' '}<span style={{ color: 'rgb(245, 121, 118)' }}>*</span>
                {errors.password != null && (
                  <span style={{ color: 'rgb(245, 121, 118)', fontStyle: "italic", fontSize: 12 }}> - {errors.password}</span>
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
            <CustomButton type="primary" htmlType="submit" block size="large" style={{ backgroundColor: '#5865f2' }} loading={loginMutation.isPending} >
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
