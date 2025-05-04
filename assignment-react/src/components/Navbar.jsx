import React, { useContext } from "react";
import { Link, useNavigate } from "react-router-dom";
import { AuthContext } from "../contexts/AuthContext";
import { Layout, Menu, Button, Typography, Space } from "antd";
import DropDownProfile from "./DropDownProfile";
import {
  HomeOutlined,
  ShoppingCartOutlined,
  LoginOutlined,
  UserAddOutlined,
  LogoutOutlined
} from "@ant-design/icons";

const { Header } = Layout;
const { Title } = Typography;

const Navbar = () => {
  const { auth, isAuthen, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/home");
  };

  const userItems = [
    { label: "Home", path: "/home", icon: <HomeOutlined /> },
    { label: "Top 10 New", path: "/books/top10news" },
    { label: "Borrowing", path: "/borrowing" },
  ];

  const guestItems = [
    { label: "Home", path: "/home", icon: <HomeOutlined /> },
    { label: "Top 10 New", path: "/books/top10news" },
  ];

  const adminItems = [
    { label: "Book", path: "/admin/book" },
    { label: "Category", path: "/admin/category" },
    { label: "Borrowing", path: "/admin/borrowing" },
    { label: "Borrowing Extend", path: "/admin/borrowing-extend" },
  ];

  const renderMenuItems = (items) =>
    items.map((item, index) => ({
      key: index,
      icon: item.icon || null,
      label: <Link to={item.path}>{item.label}</Link>,
    }));

  const menuItems =
    auth.role === "user"
      ? renderMenuItems(userItems)
      : auth.role === "admin"
      ? renderMenuItems(adminItems)
      : renderMenuItems(guestItems);

  return (
    <Header style={{ display: "flex", alignItems: "center", justifyContent: "space-between" }}>
      <Link to="/" style={{ display: "flex", alignItems: "center" }}>
        <img src="/logo.png" alt="logo" style={{ width: 50, height: 50 }} />
        <Title level={4} style={{ margin: "0 0 0 16px", color: "#fff" }}>
          Library Management System
        </Title>
      </Link>
      <Menu
        theme="dark"
        mode="horizontal"
        items={menuItems}
        style={{ flex: 1, justifyContent: "center" }}
      />
      <Space>
        {isAuthen && auth.role === "user" && (
          <Link to="/cart">
            <Button icon={<ShoppingCartOutlined />} type="text" style={{ color: "#fff" }} />
          </Link>
        )}
        {auth.role === "user" ? (
          <>
            <DropDownProfile/>
          </>
        ) : auth.role === "admin" ? (
          <Button 
            icon={<LogoutOutlined />} 
            type="text" 
            style={{ color: "#fff" }} 
            onClick={handleLogout}
          >
            Logout
          </Button>
        ) : (
          <>
            <Link to="/login">
              <Button icon={<LoginOutlined />} type="text" style={{ color: "#fff" }}>
                Login
              </Button>
            </Link>
            <Link to="/sign-up">
              <Button icon={<UserAddOutlined />} type="primary">
                Sign Up
              </Button>
            </Link>
          </>
        )}
      </Space>
    </Header>
  );
};

export default Navbar;