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
          const data = res.data.data;
          
          // Normalize the status to handle both "Waiting" and "Pending"
          if (data.status === "Pending") {
            data.status = "Waiting"; // Convert Pending to Waiting for consistency
          }
          
          setBorrowingData(data);
          setIsApprover(data.status === "Approved");
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
          // Normalize status values in the data - important fix!
          const normalizedData = res.data.data.map(item => {
            // Convert "Pending" to "Waiting" for consistency
            if (item.status === "Pending") {
              return { ...item, status: "Waiting" };
            }
            return item;
          });
          
          setBorrowingDetailData(normalizedData);
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
            alert("Borrowing has been deleted");
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
        statusExtend: "Waiting",
      })
      .then((res) => {
        if (res.data.success) {
          alert("Borrowing has been extended");
          setBorrowingDetailData(
            borrowingDetailData.map((detail) =>
              detail.bookId === bookId
                ? { ...detail, statusExtend: "Waiting" }
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
    { 
      title: "Book Name", 
      dataIndex: "bookName", 
      key: "bookName",
      className: "font-semibold",
    },
    {
      title: "Image",
      dataIndex: "imageUrl",
      key: "imageUrl",
      render: (text) => (
        <img 
          src={text} 
          alt="Book" 
          style={{ width: "100px", height: "70px", objectFit: "cover", borderRadius: "4px" }} 
        />
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
        if (text === "Waiting") {
          return <div style={{ color: "#faad14", fontWeight: "bold" }}>{text}</div>;
        } else if (text === "Pending") {
          return <div style={{ color: "#faad14", fontWeight: "bold" }}>Waiting</div>;
        } else if (text === "Borrowing") {
          return <div style={{ color: "#1890ff", fontWeight: "bold" }}>{text}</div>;
        } else if (text === "Returned") {
          return <div style={{ color: "#52c41a", fontWeight: "bold" }}>{text}</div>;
        } else {
          return <div style={{ color: "#ff4d4f", fontWeight: "bold" }}>{text}</div>;
        }
      },
    },
    {
      title: "Status Extend",
      dataIndex: "statusExtend",
      key: "statusExtend",
      render: (text) => {
        if (!text) return <div>-</div>;
        
        if (text === "Waiting") {
          return <div style={{ color: "#faad14", fontWeight: "bold" }}>{text}</div>;
        } else if (text === "Pending") {
          return <div style={{ color: "#faad14", fontWeight: "bold" }}>Waiting</div>;
        } else if (text === "Approved") {
          return <div style={{ color: "#52c41a", fontWeight: "bold" }}>{text}</div>;
        } else {
          return <div style={{ color: "#ff4d4f", fontWeight: "bold" }}>{text}</div>;
        }
      },
    },
    {
      title: "Action",
      dataIndex: "action",
      key: "action",
      render: (text, record) => {
        // Always show Delete button if only one condition is true: record status is Waiting
        if (record.status === "Waiting" && borrowingDetailData.length > 1) {
          return (
            <Button
              onClick={() => handleDelete(record.bookId)}
              style={{ 
                backgroundColor: "#ff4d4f", 
                color: "white", 
                border: "none",
                borderRadius: "4px",
                marginRight: "8px"
              }}
            >
              Delete
            </Button>
          );
        }
        
        // For Borrowing status with approver permissions
        if (record.status === "Borrowing" && isApprover && !record.statusExtend) {
          return (
            <Button
              onClick={() => handleExtend(record.bookId)}
              style={{ 
                backgroundColor: "#1890ff", 
                color: "white", 
                border: "none",
                borderRadius: "4px" 
              }}
            >
              Extend
            </Button>
          );
        }
        
        return null;
      },
    },
  ];

  return (
    <div style={{ background: "white", padding: "24px", borderRadius: "8px", boxShadow: "0 2px 8px rgba(0,0,0,0.1)" }}>
      <h1 style={{ fontSize: "24px", fontWeight: "600", marginBottom: "16px" }}>Borrowing Detail</h1>
      {loading ? (
        <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "240px" }}>
          <Spin size="large" />
        </div>
      ) : (
        <>
          {borrowingData && borrowingData.status !== "Waiting" &&
            borrowingData.status !== "Rejected" && (
              <h2 style={{ fontSize: "16px", marginBottom: "16px" }}>
                <span style={{ fontWeight: "600" }}>Requestor handler:</span> {borrowingData.approverName}
              </h2>
            )}
          <h2 style={{ fontSize: "16px", marginBottom: "24px" }}>
            <span style={{ fontWeight: "600" }}>Status of borrowing: </span>
            {borrowingData && (
              borrowingData.status === "Waiting" ? (
                <span style={{ color: "#faad14", fontWeight: "bold" }}>{borrowingData.status}</span>
              ) : borrowingData.status === "Approved" ? (
                <span style={{ color: "#52c41a", fontWeight: "bold" }}>{borrowingData.status}</span>
              ) : (
                <span style={{ color: "#ff4d4f", fontWeight: "bold" }}>{borrowingData.status}</span>
              )
            )}
          </h2>
          <div style={{ marginBottom: "24px" }}>
            <Table
              columns={columns}
              dataSource={borrowingDetailData}
              rowKey={(record) => `${record.bookId}-${record.id}`}
              pagination={false}
              style={{ border: "1px solid #f0f0f0", borderRadius: "8px" }}
            />
          </div>
          
          {borrowingData && (borrowingData.status === "Waiting" || borrowingData.status === "Pending") &&
            borrowingDetailData.length < 5 && (
              <div style={{ 
                marginTop: "24px", 
                marginBottom: "24px", 
                padding: "16px", 
                background: "#f9f9f9", 
                borderRadius: "8px",
                border: "1px solid #f0f0f0"
              }}>
                <h3 style={{ fontSize: "18px", fontWeight: "500", marginBottom: "12px" }}>Add More Books</h3>
                <Select
                  mode="multiple"
                  style={{ width: "100%", marginBottom: "12px" }}
                  placeholder="Select books to add"
                  value={selectedBooks}
                  onChange={handleBookSelection}
                  maxTagCount={3}
                >
                  {bookDataNotInBorrowing.map((book) => (
                    <Select.Option key={book.id} value={book.id}>
                      {book.name}
                    </Select.Option>
                  ))}
                </Select>
                <Button
                  style={{ 
                    backgroundColor: "#1890ff", 
                    color: "white", 
                    border: "none",
                    borderRadius: "4px"
                  }}
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
            style={{ 
              backgroundColor: "#1890ff", 
              color: "white", 
              border: "none",
              borderRadius: "4px"
            }}
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