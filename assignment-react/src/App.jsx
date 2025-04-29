import "./App.css";

import React from "react";
import { Link, Route, BrowserRouter as Router, Routes } from "react-router-dom";

import { AuthProvider } from "./contexts/AuthContext";

import Layout from "./components/Layout";
import NotFound from "./components/NotFound";
import AppRouter from "./router/AppRouter";

const App = () => {
  return (
    <AuthProvider>
      <Router>
        <AppRouter />
      </Router>
    </AuthProvider>
  );
};

export default App;
