import React from "react";
import { Fragment } from "react";
import { Button, Card, Form, Input } from "antd";
import { ILogin } from "../../../Interface/ILogin";
const Login = ({}) => {
  const onSubmitForm = (values: ILogin) => {};

  return (
    <Fragment>
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          minHeight: "70vh",
        }}
      >
        <Card
          title="Đăng nhập"
          style={{
            width: 800,
            boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
          }}
          headStyle={{ backgroundColor: "#333399", color: "#fff" }}
        >
          <div style={{ display: "flex" }}>
            <div style={{ width: "45%", alignContent: "center" }}>
              <Form layout="vertical" onFinish={onSubmitForm}>
                <Form.Item
                  name="UserName"
                  rules={[
                    {
                      required: true,
                      message: "Hãy nhập tên đăng nhập!",
                    },
                  ]}
                  style={{ marginBottom: 40 }}
                >
                  <Input size="large" placeholder="Tên đăng nhập" style={{ borderColor: "#333399" }} />
                </Form.Item>
                <Form.Item
                  name="Password"
                  style={{ marginBottom: 40 }}
                  rules={[
                    {
                      required: true,
                      message: "Hãy nhập mật khẩu!",
                    },
                  ]}
                >
                  <Input.Password size="large" placeholder="Mật khẩu" style={{ borderColor: "#333399" }} />
                </Form.Item>
                <Button size="large" type="primary" htmlType="submit" style={{ width: "100%", backgroundColor: "#333399" }}>
                  Đăng nhập
                </Button>
              </Form>
            </div>
            <div style={{ width: "55%" }}>
              <img src={require("../../../assets/Images/login.jpg")} width={400} height={350} />
            </div>
          </div>
        </Card>
      </div>
    </Fragment>
  );
};

export default Login;
