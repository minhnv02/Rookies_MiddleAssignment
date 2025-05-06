import React, { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
import { AuthContext } from "../../../../contexts/AuthContext";
import {
  Form,
  Input,
  Button,
  Typography,
  Card,
  Space,
  message,
} from "antd";
import {
  EyeInvisibleOutlined,
  EyeTwoTone,
} from "@ant-design/icons";

const { Title, Text, Link } = Typography;

const LoginForm = () => {
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const response = await axiosInstance.post("/auths/login", values);
      login(response.data.data);
      if (response.data.data.role === "admin") {
        navigate("/admin");
      } else navigate("/home");
    } catch (error) {
      if (error.response && error.response.status === 409) {
        alert(error.response.data);
      } else {
        message.error("Login failed");
      }
    }
    setLoading(false);
  };

  return (
    <div style={{ maxWidth: 400, margin: "40px auto" }}>
      <Title level={2} style={{ textAlign: "center", color: "#1677ff" }}>
        Library Management System
      </Title>

      <Card bordered style={{ borderRadius: 8, boxShadow: "0 2px 8px rgba(0,0,0,0.1)" }}>
        <Title level={4} style={{ textAlign: "center" }}>
          Login
        </Title>
        <Text type="secondary" style={{ display: "block", textAlign: "center", marginBottom: 24 }}>
          Sign in to manage the book lending system
        </Text>

        <Form layout="vertical" onFinish={onFinish}>
          <Form.Item
            name="email"
            label="Email"
            rules={[
              { required: true, message: "Please input your email!" },
              { type: "email", message: "Invalid email address!" },
            ]}
          >
            <Input placeholder="Enter your email" />
          </Form.Item>

          <Form.Item
            name="password"
            label="Password"
            rules={[{ required: true, message: "Please input your password!" }]}
          >
            <Input.Password
              placeholder="Enter your password"
              iconRender={(visible) =>
                visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />
              }
            />
          </Form.Item>

          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              loading={loading}
              block
              style={{ padding: "6px 0", fontSize: "16px" }}
            >
              Login
            </Button>
          </Form.Item>
        </Form>

        <Text type="secondary" style={{ display: "block", textAlign: "center" }}>
          Don't have an account?{" "}
          <Link href="/sign-up">
            Signup now
          </Link>
        </Text>
      </Card>
    </div>
  );
};

export default LoginForm;
