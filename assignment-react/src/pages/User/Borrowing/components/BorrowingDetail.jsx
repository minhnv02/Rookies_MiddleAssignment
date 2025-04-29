import React, { useEffect, useState } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
import { Button, Spin, Table, message, Select } from "antd";

const BorrowingDetail = () => {
  const { id } = useParams();
  const [borrowingData, setBorrowingData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [borrowingDetailData, setBorrowingDetailData] = useState([]);
  const [isApprover, setIsApprover] = useState(false);
  const [isAddNew, setIsAddNew] = useState(false);
  const [bookDataNotInBorrowing, setBookDataNotInBorrowing] = useState([]);
  const [selectedBooks, setSelectedBooks] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    axiosInstance
      .get(`/borrowings/${id}`)
      .then((res) => {
        if (res.data.success) {
          setBorrowingData(res.data.data);
          setIsApprover(res.data.data.status === "Approved");
        } else {
          message.error(res.data.message);
        }
        setLoading(false);
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message);
        } else {
          message.error(err.message);
        }
        setLoading(false);
      });
  }, [id]);

  useEffect(() => {
    axiosInstance
      .get(`/books/not-borrowed/${id}`)
      .then((res) => {
        if (res.data.success) {
          setBookDataNotInBorrowing(res.data.data);
        } else {
          message.error(res.data.message);
        }
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message);
        } else {
          message.error(err.message);
        }
      });
  }, [id, isAddNew, borrowingDetailData]);

  useEffect(() => {
    axiosInstance
      .get(`/borrowing-details/borrowing/${id}`)
      .then((res) => {
        if (res.data.success) {
          setBorrowingDetailData(res.data.data);
        } else {
          message.error(res.data.message);
        }
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message);
        } else {
          message.error(err.message);
        }
      });
  }, [id, isAddNew]);

  const handleDelete = (bookId) => {
    if (window.confirm("Are you sure you want to delete this borrowing?")) {
      axiosInstance
        .delete(`/borrowing-details/${id}/${bookId}`)
        .then((res) => {
          if (res.data.success) {
            message.success("Borrowing has been deleted");
            setBorrowingDetailData(
              borrowingDetailData.filter((detail) => detail.bookId !== bookId)
            );
          } else {
            message.error(res.data.message);
          }
        })
        .catch((err) => {
          if (err.response && err.response.status === 409) {
            message.error(err.response.data.message);
          } else {
            message.error(err.message);
          }
        });
    }
  };

  const handleExtend = (bookId) => {
    axiosInstance
      .put(`/borrowing-details/update-status-extend/${id}/${bookId}`, {
        statusExtend: "Pending",
      })
      .then((res) => {
        if (res.data.success) {
          message.success("Borrowing has been extended");
          setBorrowingDetailData(
            borrowingDetailData.map((detail) =>
              detail.bookId === bookId
                ? { ...detail, statusExtend: "Pending" }
                : detail
            )
          );
        } else {
          message.error(res.data.message);
        }
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message);
        } else {
          message.error(err.message);
        }
      });
  };

  const handleBookSelection = (value) => {
    setSelectedBooks(value);
    if (selectedBooks.length + borrowingDetailData.length > 5) {
      message.error("You can only borrow up to 5 books");
    }
  };

  const handleSave = () => {
    const borrowingDetailRequests = selectedBooks.map((bookId) => ({
      BorrowingId: id,
      BookId: bookId,
    }));

    axiosInstance
      .post("/borrowing-details", borrowingDetailRequests)
      .then((res) => {
        if (res.data.success) {
          message.success("Borrowing detail(s) added successfully");
          setIsAddNew(!isAddNew); // Toggle to refresh data
          setSelectedBooks([]);
        } else {
          message.error(res.data.message);
        }
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message);
        } else {
          message.error(err.message);
        }
      });
  };

  const columns = [
    { title: "Book Name", dataIndex: "bookName", key: "bookName" },
    {
      title: "Image",
      dataIndex: "imageUrl",
      key: "imageUrl",
      render: (text) => (
        <img src={text} alt="Book" style={{ width: "100px", height: "60px" }} />
      ),
    },
    {
      title: "Created At",
      dataIndex: "createdAt",
      key: "createdAt",
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
      title: "Day must return",
      dataIndex: "returnedAt",
      key: "returnedAt",
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
        return text === "Pending" ? (
          <div className="text-yellow-500">{text}</div>
        ) : text === "Borrowing" ? (
          <div className="text-blue-500">{text}</div>
        ) : text === "Returned" ? (
          <div className="text-green-500">{text}</div>
        ) : (
          <div className="text-red-500">{text}</div>
        );
      },
    },
    {
      title: "Status Extend",
      dataIndex: "statusExtend",
      key: "statusExtend",
      render: (text) => {
        return text === "Pending" ? (
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
          {record.status === "Pending" &&
            borrowingData.status === "Pending" &&
            borrowingDetailData.length > 1 && (
              <Button
                onClick={() => handleDelete(record.bookId)}
                className="bg-red-500 text-white"
              >
                Delete
              </Button>
            )}
          {record.status === "Borrowing" &&
            isApprover &&
            !record.statusExtend && (
              <Button
                onClick={() => handleExtend(record.bookId)}
                className="bg-blue-500 text-white"
              >
                Extend
              </Button>
            )}
        </div>
      ),
    },
  ];

  return (
    <div className="p-4">
      <h1 className="text-2xl">Borrowing Detail</h1>
      {loading ? (
        <Spin size="large" className="flex justify-center items-center" />
      ) : (
        <>
          {borrowingData.status !== "Pending" &&
            borrowingData.status !== "Rejected" && (
              <h2 className="text-xl mt-4 mb-2">
                <strong>Requestor handler:</strong> {borrowingData.approverName}
              </h2>
            )}
          <h2 className="text-xl mt-4 mb-2">
            <strong>Status of borrowing: </strong>
            {borrowingData.status === "Pending" ? (
              <div className="text-yellow-500 inline">
                {borrowingData.status}
              </div>
            ) : borrowingData.status === "Approved" ? (
              <div className="text-green-500 inline">
                {borrowingData.status}
              </div>
            ) : (
              <div className="text-red-500 inline">{borrowingData.status}</div>
            )}
          </h2>
          <Table
            columns={columns}
            dataSource={borrowingDetailData}
            rowKey="id"
            scroll={{ y: 300 }}
            pagination={false}
          />
          {borrowingData.status === "Pending" &&
            borrowingDetailData.length < 5 && (
              <div className="mt-4">
                <Select
                  mode="multiple"
                  style={{ width: "100%" }}
                  placeholder="Select books to add"
                  value={selectedBooks}
                  onChange={handleBookSelection}
                >
                  {bookDataNotInBorrowing.map((book) => (
                    <Select.Option key={book.id} value={book.id}>
                      {book.name}
                    </Select.Option>
                  ))}
                </Select>
                <Button
                  className="bg-blue-500 text-white mt-2"
                  onClick={handleSave}
                  disabled={
                    selectedBooks.length === 0 ||
                    selectedBooks.length + borrowingDetailData.length > 5
                  }
                >
                  Save
                </Button>
              </div>
            )}
          <Button
            className="bg-blue-500 text-white mt-4"
            onClick={() => navigate(-1)}
          >
            Back
          </Button>
        </>
      )}
    </div>
  );
};

export default BorrowingDetail;
