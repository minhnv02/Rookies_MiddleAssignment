import React from "react";
import { Outlet, useRoutes } from "react-router-dom";
import HomePage from "../pages/User/HomePage/HomePage";
import BookDetailPage from "../pages/User/Book/BookDetailPage";
import Login from "../pages/Auth/Login/Login";
import Signup from "../pages/Auth/Signup/Signup";
import UnAuthor from "../components/UnAuthor";
import NotFound from "../components/NotFound";
import RequireAdmin from "../components/RequireAdmin";
import AdminRouter from "./AdminRouter";
import RequireUser from "../components/RequireUser";
import CartPage from "../pages/User/Cart/CartPage";
import BorrowingPage from "../pages/User/Borrowing/BorrowingPage";
import BorrowingDetailPage from "../pages/User/Borrowing/BorrowingDetailPage";
import Profile from "../pages/User/Profile";
import Layout from "../components/Layout";
import UserRouter from "./UserRouter";
import CommonRouter from "./CommonRouter";

const AppRouter = () => {
  const elements = useRoutes([
    {
      path: "/",
      element: <Layout />,
      children: [
        ...UserRouter,
        ...CommonRouter,
        {
          path: "/admin",
          element: (
            <RequireAdmin>
              <Outlet />
            </RequireAdmin>
          ),
          children: AdminRouter,
        },
      ],
    },
  ]);
  return elements;
};

export default AppRouter;
