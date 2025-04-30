import React from "react";
import CartPage from "../pages/User/Cart/CartPage";
import RequireUser from "../components/RequireUser";
import BorrowingPage from "../pages/User/Borrowing/BorrowingPage";
import BorrowingDetailPage from "../pages/User/Borrowing/BorrowingDetailPage";
import Profile from "../pages/User/Profile";
import NotFound from "../components/NotFound";

const UserRouter = [
  {
    path: "/cart",
    element: (
      <RequireUser>
        <CartPage />
      </RequireUser>
    ),
  },
  {
    path: "/borrowing",
    element: (
      <RequireUser>
        <BorrowingPage />
      </RequireUser>
    ),
  },
  {
    path: "/borrowing/:id",
    element: (
      <RequireUser>
        <BorrowingDetailPage />
      </RequireUser>
    ),
  },
  {
    path: "/profile",
    element: (
      <RequireUser>
        <Profile />
      </RequireUser>
    ),
  },
  // { path: "*", element: <NotFound /> },
];

export default UserRouter;
