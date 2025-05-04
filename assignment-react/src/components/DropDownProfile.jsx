import { Dropdown, Space } from "antd";
import React, { useContext } from "react";
import { CgProfile } from "react-icons/cg";
import { Link } from "react-router-dom";
import { AuthContext } from "../contexts/AuthContext";
import { RiAdminFill } from "react-icons/ri";

const DropDownProfile = () => {
  const { auth, logout } = useContext(AuthContext);
  const role = auth.role;

  const userItems = [
    {
      label: (
        <Link to="/profile" className=""> 
          Profile
        </Link>
      ),
      key: 0,
    },
    {
      label: (
        <Link to="/borrowing" className="">
          History borrowing book
        </Link>
      ),
      key: 1,
    },
    {
      type: "divider",
    },
    {
      label: (
        <Link to="/home" onClick={logout} className="">
          Logout
        </Link>
      ),
      key: 2,
    },
  ];

  const adminItems = [
    {
      label: (
        <Link to="/home" onClick={logout} className="">
          Logout
        </Link>
      ),
      key: 0,
    },
  ];

  return (
    <Dropdown menu={{ items: role === "admin" ? adminItems : userItems }}>
      <Space>
        {role === "admin" ? <RiAdminFill /> : <CgProfile />}
        {role === "admin" ? "admin" : "user"}
      </Space>
    </Dropdown>
  );
};

export default DropDownProfile;
