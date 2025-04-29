import React from "react";
import { useRoutes } from "react-router-dom";
import RequireAdmin from "../components/RequireAdmin";
import CreateBookPage from "../pages/Admin/BookManagement/CreateBookPage";
import DetailBookPage from "../pages/Admin/BookManagement/DetailBookPage";
import UpdateBookPage from "../pages/Admin/BookManagement/UpdateBookPage";
import BookManagement from "../pages/Admin/BookManagement/BookManagement";
import CategoryManagement from "../pages/Admin/Category/CategoryManagement";
import DetailCategoryPage from "../pages/Admin/Category/DetailCategoryPage";
import BorrowingManagementPage from "../pages/Admin/BorrowingManagement/BorrowingManagementPage";
import BorrowingDetailManagementPage from "../pages/Admin/BorrowingManagement/BorrowingDetailManagementPage";
import BorrowingExtendPage from "../pages/Admin/BorrowingManagement/BorrowingExtendPage";
import NotFound from "../components/NotFound";

const AdminRouter = [
  {
    path: `/admin`,
    element: <BookManagement />,
  },
  {
    path: `/admin/books/create`,
    element: <CreateBookPage />,
  },
  {
    path: `/admin/books/:id`,
    element: <DetailBookPage />,
  },
  {
    path: `/admin/books/edit/:id`,
    element: <UpdateBookPage />,
  },
  {
    path: `/admin/book`,
    element: <BookManagement />,
  },
  {
    path: `/admin/category`,
    element: <CategoryManagement />,
  },
  {
    path: `/admin/category/:id`,
    element: <DetailCategoryPage />,
  },
  {
    path: `/admin/borrowing`,
    element: <BorrowingManagementPage />,
  },
  {
    path: `/admin/borrowing/:id`,
    element: <BorrowingDetailManagementPage />,
  },
  {
    path: `/admin/borrowing-extend`,
    element: <BorrowingExtendPage />,
  },
  // { path: "*", element: <NotFound /> },
];

export default AdminRouter;
