import { Navigate } from "react-router-dom";
import { AuthContext } from "../contexts/AuthContext";
import { useContext } from "react";

const RequireAdmin = (props) => {
  debugger
  const { children } = props;
  const { auth } = useContext(AuthContext);
  return auth?.role === `admin` ? children : <Navigate to="/unauthorized" />;
};

export default RequireAdmin;
