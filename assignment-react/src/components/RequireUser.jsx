import React, { useContext } from "react";
import { AuthContext } from "../contexts/AuthContext";
import { Navigate } from "react-router-dom";

const RequireUser = (props) => {
  const { children } = props;
  const { auth } = useContext(AuthContext);
  return auth?.role === `user` ? children : <Navigate to="/unauthorized" />;
};

export default RequireUser;
