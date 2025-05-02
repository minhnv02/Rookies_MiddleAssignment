import React, { useContext, useEffect, useState } from "react";
import { AuthContext } from "../../contexts/AuthContext";
import axiosInstance from "../../utils/axiosInstance";
import { Card, Spin, message } from "antd";

const Profile = () => {
  const { auth } = useContext(AuthContext);
  const [userData, setUserData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [dateForm, setDateForm] = useState(null);

  useEffect(() => {
    const fetchUser = async () => {
      setLoading(true);
      try {
        const response = await axiosInstance.get(`/users/${auth.userId}`);
        if (response.data.success) {
          setUserData(response.data.data);
          const date = new Date(response.data.data.createdAt);
          setDateForm(date.toDateString());
        } else {
          message.error(response.data.message);
        }
      } catch (error) {
        message.error("Failed to fetch user");
      }
      setLoading(false);
    };

    fetchUser();
  }, [auth.userId]);

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100 p-4">
      {loading ? (
        <div className="flex justify-center items-center">
          <Spin size="large" />
        </div>
      ) : (
        <Card title="Profile" className="w-full max-w-md p-4">
          <div className="flex justify-center mb-4">
            <img
              src="https://cdn-icons-png.flaticon.com/512/3135/3135715.png"
              alt="Profile"
              className="w-24 h-24 rounded-full"
            />
          </div>
          <p className="mb-2">
            <strong>Email:</strong> {userData.email}
          </p>
          <p className="mb-2">
            <strong>Name:</strong> {userData.name}
          </p>
          <p className="mb-2">
            <strong>Created At:</strong> {dateForm}
          </p>
          <p className="mb-2">
            <strong>Status:</strong> {userData.status}
          </p>
        </Card>
      )}
    </div>
  );
};

export default Profile;
