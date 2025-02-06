import React, { Fragment } from "react";
import { Layout, Form, Input, Button, Typography } from "antd";
import { GoogleOutlined, FacebookOutlined } from "@ant-design/icons";
import "./index.css";
import { Link } from "react-router-dom";

const { Content } = Layout;
const { Title } = Typography;

const Login: React.FC = () => {
  const onFinish = (values: any) => {
    console.log("Success:", values);
  };

  const onFinishFailed = (errorInfo: any) => {
    console.log("Failed:", errorInfo);
  };

  return (
    <Fragment>
      <div className="login-layout">
        <Content className="login-right">
          <div className="login-form-container">
            <div className="login-left"></div>
            <div>
              <div style={{ padding: 40 }}>
                <Title level={2} style={{ color: "#FC9A8F", fontWeight: "bold" }}>
                  KenDZz
                </Title>
                <Form layout="vertical" name="login" initialValues={{ remember: true }} onFinish={onFinish} onFinishFailed={onFinishFailed}>
                  <Form.Item name="email" rules={[{ required: true, message: "Please input your email!" }]} label="Email">
                    <Input size="large" style={{ borderWidth: 1, borderColor: "black" }} />
                  </Form.Item>

                  <Form.Item style={{ marginBottom: 10 }} name="password" rules={[{ required: true, message: "Please input your password!" }]} label="Password">
                    <Input.Password size="large" style={{ borderWidth: 1, borderColor: "black" }} />
                  </Form.Item>
                  <div style={{ textAlign: "right" }}>
                    <Link to="/register">Forgot Password!</Link>
                  </div>

                  <Form.Item>
                    <Button size="large" type="primary" htmlType="submit" block style={{ fontFamily: "Inter, sans-serif", backgroundColor: "#FC9A8F" }}>
                      Sign In
                    </Button>
                  </Form.Item>
                </Form>
                <Link to="/tv/signup" style={{ color: "#FC9A8F" }}>
                  Don't have an account? Sign up
                </Link>
                <div style={{ display: "flex", justifyContent: "space-between" }}>
                  <Button
                    size="large"
                    type="default"
                    icon={<GoogleOutlined style={{ color: "#fff" }} />}
                    block
                    style={{ width: "47%", backgroundColor: "#f44235", color: "#fff", border: "none", boxShadow: "none" }}
                  >
                    Login with Google
                  </Button>
                  <Button
                    size="large"
                    type="default"
                    icon={<FacebookOutlined style={{ color: "#fff" }} />}
                    block
                    style={{ width: "47%", backgroundColor: "#0866ff", color: "#fff", border: "none", boxShadow: "none" }}
                  >
                    Login with Facebook
                  </Button>
                </div>
              </div>
            </div>
          </div>
        </Content>
      </div>
    </Fragment>
  );
};

export default Login;
