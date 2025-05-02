import React from "react";
import { Layout, Row, Col, Typography, Space } from "antd";
import {
  FacebookOutlined,
  TwitterOutlined,
  InstagramOutlined,
  LinkedinOutlined,
  GithubOutlined,
} from "@ant-design/icons";

const { Footer } = Layout;
const { Title, Paragraph, Text, Link } = Typography;

const AppFooter = () => {
  const currentYear = new Date().getFullYear();

  return (
    <Footer style={{ backgroundColor: "#f0f2f5", padding: "48px 24px", fontSize: "0.9rem" }}>
      <Row gutter={[32, 32]} justify="space-evenly">
        {/* About */}
        <Col xs={24} md={8}>
          <Title level={5} style={{ color: "#1677ff" }}>
            Library Management System
          </Title>
          <Paragraph type="secondary">
            Your complete solution for managing library resources, borrowing books, and discovering new reading materials.
          </Paragraph>
        </Col>

        {/* Quick Links */}
        <Col xs={24} md={4}>
          <Title level={5} style={{ color: "#1677ff" }}>
            Quick Links
          </Title>
          <Space direction="vertical">
            {["Home", "About Us", "Services", "Contact"].map((item, index) => (
              <Link key={index} href="#" style={{ color: "rgba(0,0,0,0.65)" }}>
                {item}
              </Link>
            ))}
          </Space>
        </Col>

        {/* Contact */}
        <Col xs={24} md={8}>
          <Title level={5} style={{ color: "#1677ff" }}>
            Contact Us
          </Title>
          <Paragraph type="secondary">123 Library Street, Book City</Paragraph>
          <Paragraph type="secondary">Email: info@library.com</Paragraph>
          <Paragraph type="secondary">Phone: (123) 456-7890</Paragraph>

          <Space>
            <Link href="#"><FacebookOutlined style={{ fontSize: "18px", color: "#1677ff" }} /></Link>
            <Link href="#"><TwitterOutlined style={{ fontSize: "18px", color: "#1677ff" }} /></Link>
            <Link href="#"><InstagramOutlined style={{ fontSize: "18px", color: "#1677ff" }} /></Link>
            <Link href="#"><LinkedinOutlined style={{ fontSize: "18px", color: "#1677ff" }} /></Link>
            <Link href="#"><GithubOutlined style={{ fontSize: "18px", color: "#1677ff" }} /></Link>
          </Space>
        </Col>
      </Row>

      {/* Divider */}
      <div style={{ borderTop: "1px solid #e8e8e8", margin: "32px 0" }} />

      {/* Bottom Section */}
      <Row justify="space-between" align="middle">
        <Col xs={24} md="auto" style={{ textAlign: "center" }}>
          <Text type="secondary">© {currentYear} Library Management System. All rights reserved.</Text>
        </Col>
        <Col xs={24} md="auto" style={{ textAlign: "center", marginTop: 8 }}>
          <Space>
            <Link href="#" style={{ color: "rgba(0,0,0,0.65)" }}>Privacy Policy</Link>
            <Link href="#" style={{ color: "rgba(0,0,0,0.65)" }}>Terms of Service</Link>
          </Space>
        </Col>
      </Row>
    </Footer>
  );
};

export default AppFooter;
