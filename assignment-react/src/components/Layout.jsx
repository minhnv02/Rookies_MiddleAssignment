import React from "react";
import Navbar from "./Navbar";
import AdminRouter from "../router/AdminRouter";
import UserRouter from "../router/UserRouter";
import AppRouter from "../router/AppRouter";
import { Outlet } from "react-router-dom";
import Footer from "./Footer";

const Layout = () => {
  return (
    <div>
      <Navbar />
      <Outlet />
      <Footer />
    </div>
  );
};

export default Layout;
