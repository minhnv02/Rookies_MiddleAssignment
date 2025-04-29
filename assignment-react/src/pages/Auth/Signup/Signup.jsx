import React from "react";
import SignupForm from "./components/SignupForm";

const Signup = () => {
  return (
    <div>
      <div className="flex justify-center items-center min-h-screen">
        <div className="p-4 border rounded-lg shadow-lg w-full max-w-md">
          <SignupForm />
        </div>
      </div>
    </div>
  );
};

export default Signup;
