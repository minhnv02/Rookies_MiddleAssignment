import React from "react";
import LoginForm from "./components/LoginForm";

const Login = () => {
  return (
    <div className="flex justify-center items-center min-h-screen">
      <div className="p-4 border rounded-lg shadow-lg w-full max-w-md">
        <LoginForm />
      </div>
    </div>
  );
};

export default Login;
