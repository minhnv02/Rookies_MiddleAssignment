// Borrowing.jsx
import React, { useContext, useEffect, useState } from "react";
import axiosInstance from "../../../../utils/axiosInstance";
import { AuthContext } from "../../../../contexts/AuthContext";
import { Button, Select, Spin, Table, message } from "antd";
import { Link } from "react-router-dom";

const Borrowing = () => {
  const [borrowingData, setBorrowingData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filterStatus, setFilterStatus] = useState("All"); // Initial filter state
  const { auth } = useContext(AuthContext);

  useEffect(() => {
    setLoading(true);
    const fetchData = async () => {
      try {
        const response = await axiosInstance.get(
          `/borrowings/requestor/${auth.userId}`
        );
        if (response.data.success) {
          setBorrowingData(response.data.data);
        } else {
          message.error(response.data.message);
        }
      } catch (err) {
        if (err.response?.status === 404) {
          message.warning("No borrowing data");
        } else {
          message.error(err.message);
        }
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [auth.userId]);

  const handleFilterChange = (value) => {
    setFilterStatus(value);
  };
  
  const filteredData =
    filterStatus === "All"
      ? borrowingData
      : borrowingData.filter((item) => item.status === filterStatus);

  const handleCancel = (id) => {
    if (window.confirm("Are you sure you want to cancel this borrowing?")) {
      axiosInstance
        .delete(`/borrowings/${id}`)
        .then((res) => {
          if (res.data.success) {
            alert("Borrowing has been cancelled");
            setBorrowingData((prev) => prev.filter((item) => item.id !== id));
          } else {
            message.error(res.data.message);
          }
        })
        .catch((err) => {
          message.error(err.message);
        });
    }
  };

  const columns = [
    {
      title: "Created At",
      dataIndex: "createdAt",
      key: "createdAt",
      sorter: {
        compare: (a, b) => {
          const dateA = new Date(a.createdAt);
          const dateB = new Date(b.createdAt);
          return dateA.getTime() - dateB.getTime();
        },
      },
      render: (text) => {
        const date = new Date(text);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        const hour = String(date.getHours()).padStart(2, "0");
        const minute = String(date.getMinutes()).padStart(2, "0");

        return `${day}/${month}/${year} ${hour}:${minute}`;
      },
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      render: (text) => {
        const statusStyles = {
          Waiting: "text-yellow-500 font-medium",
          Pending: "text-yellow-500 font-medium", // Added Pending status
          Approved: "text-green-500 font-medium",
          Rejected: "text-red-500 font-medium"
        };
        
        const statusClass = statusStyles[text] || "";
        return <div className={statusClass}>{text}</div>;
      },
    },
    {
      title: "Action",
      dataIndex: "action",
      render: (text, record) => (
        <div className="flex gap-2">
          {(record.status === "Waiting" || record.status === "Pending") ? (
            <>
              <Button className="bg-green-500 text-white hover:bg-green-600 hover:text-white border-0 rounded">
                <Link to={`/borrowing/${record.id}`}>Detail</Link>
              </Button>
              <Button className="bg-blue-500 text-white hover:bg-blue-600 hover:text-white border-0 rounded">
                <Link to={`/borrowing/${record.id}`}>Edit</Link>
              </Button>
              <Button
                onClick={() => handleCancel(record.id)}
                className="bg-red-500 text-white hover:bg-red-600 hover:text-white border-0 rounded"
              >
                Cancel
              </Button>
            </>
          ) : (
            <Button className="bg-green-500 text-white hover:bg-green-600 hover:text-white border-0 rounded">
              <Link to={`/borrowing/${record.id}`}>Detail</Link>
            </Button>
          )}
        </div>
      ),
    },
  ];

  return (
    <div className="bg-white p-6 rounded-lg shadow-md">
      {loading ? (
        <div className="flex justify-center items-center h-60">
          <Spin size="large" />
        </div>
      ) : (
        <>
          <h1 className="text-2xl font-semibold text-gray-800 mb-6">History Borrowing</h1>
          <div className="mb-6">
            <Select
              defaultValue="All"
              onChange={handleFilterChange}
              style={{ width: 180 }}
              className="border rounded"
            >
              <Select.Option value="All">All</Select.Option>
              <Select.Option value="Waiting">Waiting</Select.Option>
              <Select.Option value="Approved">Approved</Select.Option>
              <Select.Option value="Rejected">Rejected</Select.Option>
            </Select>
          </div>
          <Table
            columns={columns}
            dataSource={filteredData}
            rowKey="id"
            scroll={{ y: 500 }}
            className="border rounded-lg"
            pagination={{
              pageSize: 10,
              position: ["bottomCenter"],
              showSizeChanger: false,
            }}
          />
        </>
      )}
    </div>
  );
};

export default Borrowing;

