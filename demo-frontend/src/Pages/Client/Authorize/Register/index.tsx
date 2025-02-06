import { Form, Input, Button, Layout, Typography } from "antd";
import "./index.css";
import React, { Fragment } from "react";
import { Link } from "react-router-dom";
const { Content } = Layout;
const { Title } = Typography;

const Register: React.FC = () => {
  const [form] = Form.useForm();

  const onFinish = (values: any) => {
    console.log("Success:", values);
  };

  const onFinishFailed = (errorInfo: any) => {
    console.log("Failed:", errorInfo);
  };
  return (
    <Fragment>
      <div className="register-layout">
        <Content className="register-right">
          <div className="register-form-container">
            <div style={{ padding: 40, width: "100%" }}>
              <Title level={2} style={{ color: "#FC9A8F", fontWeight: "bold" }}>
                REGISTER
              </Title>
              <Form layout="vertical" name="register" initialValues={{ remember: true }} onFinish={onFinish} onFinishFailed={onFinishFailed}>
                <Form.Item label="User name" name="username" rules={[{ required: true, message: "Please input your username!" }]}>
                  <Input size="large" style={{ borderWidth: 1, borderColor: "black" }} />
                </Form.Item>
                <Form.Item label="Email" name="email" rules={[{ required: true, message: "Please input your email!" }]}>
                  <Input size="large" style={{ borderWidth: 1, borderColor: "black" }} />
                </Form.Item>
                <Form.Item label="Password" name="password" rules={[{ required: true, message: "Please input your password!" }]}>
                  <Input.Password size="large" style={{ borderWidth: 1, borderColor: "black" }} />
                </Form.Item>
                <Form.Item
                  label="Confirm Password"
                  name="confirmPassword"
                  dependencies={["password"]}
                  hasFeedback
                  rules={[
                    { required: true, message: "Please confirm your password!" },
                    ({ getFieldValue }) => ({
                      validator(_, value) {
                        if (!value || getFieldValue("password") === value) {
                          return Promise.resolve();
                        }
                        return Promise.reject(new Error("The two passwords that you entered do not match!"));
                      },
                    }),
                  ]}
                >
                  <Input.Password size="large" style={{ borderWidth: 1, borderColor: "black" }} />
                </Form.Item>
                <Form.Item>
                  <Button size="large" type="primary" htmlType="submit" block style={{ fontFamily: "IBM Plex Serif", backgroundColor: "#FC9A8F" }}>
                    Sign Up
                  </Button>
                </Form.Item>
              </Form>
              <Link to="/tv/signin" style={{ color: "#FC9A8F" }}>
                Already have account? Let's Sign in
              </Link>
            </div>
          </div>
        </Content>
      </div>
    </Fragment>
  );
};

export default Register;
