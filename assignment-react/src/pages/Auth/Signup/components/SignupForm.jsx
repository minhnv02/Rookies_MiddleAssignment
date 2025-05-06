import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
import { Form, Input, Button, Typography, message, Card, Spin } from "antd";
import { EyeInvisibleOutlined, EyeTwoTone } from "@ant-design/icons";

const { Title, Text, Link } = Typography;

const SignupForm = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const response = await axiosInstance.post("/auths/register", {
        ...values,
        roleId: 2,
      });
      const email = response.data.data.email;
      await axiosInstance.get(`/auths/request-active-account/${email}`);
      message.success(response.data.message);
      navigate("/login");
    } catch (error) {
      if (error.response && error.response.status === 409) {
        alert(error.response.data);
      } else {
        message.error("Registration failed");
      }
    }
    setLoading(false);
  };

  return (
    <div style={{ maxWidth: 400, margin: "40px auto" }}>
      <Title level={2} style={{ textAlign: "center", color: "#1677ff" }}>
        Library Management System
      </Title>

      <Card bordered style={{ borderRadius: 8 }}>
        <Title level={4} style={{ textAlign: "center", marginBottom: 24 }}>
          Sign Up
        </Title>

        <Form layout="vertical" onFinish={onFinish}>
          <Form.Item
            name="email"
            label="Email"
            rules={[
              { required: true, message: "Please input your email!" },
              { type: "email", message: "Invalid email format" },
            ]}
          >
            <Input placeholder="Enter your email" />
          </Form.Item>

          <Form.Item
            name="name"
            label="Name"
            rules={[{ required: true, message: "Please input your name!" }]}
          >
            <Input placeholder="Enter your name" />
          </Form.Item>

          <Form.Item
            name="password"
            label="Password"
            rules={[
              { required: true, message: "Please input your password!" },
              {
                pattern:
                  /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/,
                message:
                  "Password must be at least 8 characters with uppercase, lowercase, number and special character",
              },
            ]}
            hasFeedback
          >
            <Input.Password
              placeholder="Enter your password"
              iconRender={(visible) =>
                visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />
              }
            />
          </Form.Item>

          <Form.Item
            name="confirmPassword"
            label="Confirm Password"
            dependencies={["password"]}
            hasFeedback
            rules={[
              { required: true, message: "Please confirm your password!" },
              ({ getFieldValue }) => ({
                validator(_, value) {
                  if (!value || getFieldValue("password") === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject(
                    new Error("Passwords do not match!")
                  );
                },
              }),
            ]}
          >
            <Input.Password
              placeholder="Confirm your password"
              iconRender={(visible) =>
                visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />
              }
            />
          </Form.Item>

          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              block
              style={{ height: 40, fontSize: 16 }}
              disabled={loading}
            >
              {loading ? <Spin size="small" /> : "Sign Up"}
            </Button>
          </Form.Item>

          <Text style={{ display: "block", textAlign: "center" }}>
            Already have an account?{" "}
            <Link href="/login" style={{ color: "#1677ff" }}>
              Login now
            </Link>
          </Text>
        </Form>
      </Card>
    </div>
  );
};

export default SignupForm;
