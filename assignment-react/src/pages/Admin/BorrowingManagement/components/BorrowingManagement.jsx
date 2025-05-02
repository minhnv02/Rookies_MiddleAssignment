import React, { useContext, useEffect, useState } from "react";
import axiosInstance from "../../../../utils/axiosInstance";
import { Button, Select, Spin, Table, message } from "antd";
import { Link } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";

const BorrowingManagement = () => {
  const [borrowings, setBorrowings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filterStatus, setFilterStatus] = useState("All");
  const { auth } = useContext(AuthContext);

  useEffect(() => {
    setLoading(true);
    const fetchData = async () => {
      try {
        const response = await axiosInstance.get(`/borrowings`);
        if (response.data.success) {
          setBorrowings(response.data.data);
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
  }, []);

  const handleFilterChange = (value) => {
    setFilterStatus(value);
  };

  const filteredData =
    filterStatus === "All"
      ? borrowings
      : borrowings.filter((item) => item.status === filterStatus);

  const handleApprove = (id) => async () => {
    try {
      const response = await axiosInstance.put(`/borrowings/${id}`, {
        status: "Approved",
        approverId: auth.userId,
      });
      if (response.data.success) {
        message.success("Borrowing approved successfully");
        setBorrowings((prev) =>
          prev.map((borrowing) =>
            borrowing.id === id
              ? { ...borrowing, status: "Approved" }
              : borrowing
          )
        );
      } else {
        message.error(response.data.message);
      }
    } catch (err) {
      message.error(err.message);
    }
  };

  const handleReject = (id) => async () => {
    try {
      const response = await axiosInstance.put(`/borrowings/${id}`, {
        status: "Rejected",
        approverId: auth.userId,
      });
      if (response.data.success) {
        message.success("Borrowing rejected successfully");
        setBorrowings((prev) =>
          prev.map((borrowing) =>
            borrowing.id === id
              ? { ...borrowing, status: "Rejected" }
              : borrowing
          )
        );
      } else {
        message.error(response.data.message);
      }
    } catch (err) {
      message.error(err.message);
    }
  };

  const columns = [
    {
      title: "ID",
      dataIndex: "id",
      key: "id",
      sorter: {
        compare: (a, b) => a.id - b.id,
        multiple: 1,
      },
    },
    {
      title: "Requestor",
      dataIndex: "requestorName",
      key: "requestorName",
      sorter: {
        compare: (a, b) => a.requestorName.localeCompare(b.requestorName),
        multiple: 2,
      },
    },
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
        multiple: 3,
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
        return text === "Waiting" ? (
          <div className="text-yellow-500">{text}</div>
        ) : text === "Approved" ? (
          <div className="text-green-500">{text}</div>
        ) : (
          <div className="text-red-500">{text}</div>
        );
      },
    },
    {
      title: "Action",
      dataIndex: "action",
      render: (text, record) => (
        <div>
          {record.status === "Waiting" ? (
            <>
              <Button
                className="bg-green-400 text-white"
                onClick={handleApprove(record.id)}
              >
                Approve
              </Button>
              <Button
                className="bg-red-500 text-white"
                onClick={handleReject(record.id)}
              >
                Reject
              </Button>
              <Button className="bg-blue-500 text-white">
                <Link to={`/admin/borrowing/${record.id}`}>Detail</Link>
              </Button>
            </>
          ) : (
            <Button className="bg-blue-400 text-white">
              <Link to={`/admin/borrowing/${record.id}`}>Detail</Link>
            </Button>
          )}
        </div>
      ),
    },
  ];

  return (
    <div>
      {loading ? (
        <Spin size="large" className="flex justify-center items-center" />
      ) : (
        <>
          <h1 className="text-2xl p-4">Borrowing Management</h1>
          <div className="flex mb-4">
            <Select
              defaultValue="All"
              onChange={handleFilterChange}
              style={{ width: 150 }}
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
            scroll={{ y: 500 }}
          />
        </>
      )}
    </div>
  );
};

export default BorrowingManagement;
