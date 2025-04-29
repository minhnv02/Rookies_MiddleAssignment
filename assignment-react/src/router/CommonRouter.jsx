import React from "react";
import HomePage from "../pages/User/HomePage/HomePage";
import BookDetailPage from "../pages/User/Book/BookDetailPage";
import Login from "../pages/Auth/Login/Login";
import Signup from "../pages/Auth/Signup/Signup";
import UnAuthor from "../components/UnAuthor";
import NotFound from "../components/NotFound";

const CommonRouter = [
  { path: "/", element: <HomePage endpoint={"/books/get-books"} /> },
  { path: "/home", element: <HomePage endpoint={"/books/get-books"} /> },
  { path: "/books/:bookId", element: <BookDetailPage /> },
  { path: "/login", element: <Login /> },
  { path: "/sign-up", element: <Signup /> },
  {
    path: "/books/top10news",
    element: <HomePage endpoint="/books/top-news" />,
  },
  {
    path: "/books/top10loved",
    element: <HomePage endpoint="/books/top-loved" />,
  },
  {
    path: "unauthorized",
    element: <UnAuthor />,
  },
  { path: "*", element: <NotFound /> },
];

export default CommonRouter;
