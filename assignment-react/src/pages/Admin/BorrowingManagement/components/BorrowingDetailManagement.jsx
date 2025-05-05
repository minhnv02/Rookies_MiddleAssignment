"use client"

import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import axiosInstance from "../../../../utils/axiosInstance"
import { Button, Popconfirm, Spin, Table, message } from "antd"
import { MdCancel } from "react-icons/md"
import { FaCheck } from "react-icons/fa"

const BorrowingDetailManagement = () => {
  const { id } = useParams()
  const [borrowingData, setBorrowingData] = useState(null)
  const [loading, setLoading] = useState(true)
  const [borrowingDetailData, setBorrowingDetailData] = useState([])
  const navigate = useNavigate()

  useEffect(() => {
    setLoading(true)
    axiosInstance
      .get(`/borrowings/${id}`)
      .then((res) => {
        if (res.data.success) {
          setBorrowingData(res.data.data)
        } else {
          message.error(res.data.message)
        }
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message)
        } else {
          message.error(err.message)
        }
      })
    axiosInstance
      .get(`/borrowing-details/borrowing/${id}`)
      .then((res) => {
        if (res.data.success) {
          setBorrowingDetailData(res.data.data)
        } else {
          message.error(res.data.message)
        }
        setLoading(false)
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message)
        } else {
          message.error(err.message)
        }
        setLoading(false)
      })
  }, [id])

  const handleReturned = (bookId) => {
    axiosInstance
      .put(`/borrowing-details/update-status/${id}/${bookId}`, {
        status: "Returned",
      })
      .then((res) => {
        if (res.data.success) {
          alert("Book has been marked as returned")
          setBorrowingDetailData((prev) =>
            prev.map((item) => (item.bookId === bookId ? { ...item, status: "Returned" } : item)),
          )
        } else {
          message.error(res.data.message)
        }
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message)
        } else {
          message.error(err.message)
        }
      })
  }

  const handleExtension = (bookId, statusExtend) => {
    let returnedAt = new Date()
    const borrowingDetail = borrowingDetailData.find((item) => item.bookId === bookId)
    if (statusExtend === "Approved") {
      returnedAt = new Date(
        new Date(borrowingDetail.returnedAt).getTime() + borrowingDetail.daysForBorrow * 24 * 60 * 60 * 1000,
      )
    }

    axiosInstance
      .put(`/borrowing-details/handle-extension/${id}/${bookId}`, {
        statusExtend,
        returnedAt: statusExtend === "Approved" ? returnedAt : null,
      })
      .then((res) => {
        if (res.data.success) {
          alert("Extension status updated successfully")
          setBorrowingDetailData((prev) =>
            prev.map((item) => (item.bookId === bookId ? { ...item, statusExtend, returnedAt } : item)),
          )
        } else {
          message.error(res.data.message)
        }
      })
      .catch((err) => {
        if (err.response && err.response.status === 409) {
          message.error(err.response.data.message)
        } else {
          message.error(err.message)
        }
      })
  }

  const columns = [
    { title: "Book Name", dataIndex: "bookName", key: "bookName" },
    {
      title: "Image",
      dataIndex: "imageUrl",
      key: "imageUrl",
      render: (text) => <img src={text || "/placeholder.svg"} alt="Book" style={{ width: "100px", height: "60px" }} />,
    },
    {
      title: "Created At",
      dataIndex: "createdAt",
      key: "createdAt",
      render: (text) => {
        const date = new Date(text)
        const year = date.getFullYear()
        const month = String(date.getMonth() + 1).padStart(2, "0")
        const day = String(date.getDate()).padStart(2, "0")
        const hour = String(date.getHours()).padStart(2, "0")
        const minute = String(date.getMinutes()).padStart(2, "0")

        return `${day}/${month}/${year} ${hour}:${minute}`
      },
    },
    {
      title: "Day must return",
      dataIndex: "returnedAt",
      key: "returnedAt",
      render: (text) => {
        const date = new Date(text)
        const year = date.getFullYear()
        const month = String(date.getMonth() + 1).padStart(2, "0")
        const day = String(date.getDate()).padStart(2, "0")
        const hour = String(date.getHours()).padStart(2, "0")
        const minute = String(date.getMinutes()).padStart(2, "0")

        return `${day}/${month}/${year} ${hour}:${minute}`
      },
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      render: (text) => {
        return text === "Waiting" ? (
          <div className="text-yellow-500">{text}</div>
        ) : text === "Borrowing" ? (
          <div className="text-blue-500">{text}</div>
        ) : text === "Returned" ? (
          <div className="text-green-500">{text}</div>
        ) : (
          <div className="text-red-500">{text}</div>
        )
      },
    },
    {
      title: "Status Extend",
      dataIndex: "statusExtend",
      key: "statusExtend",
      render: (text, record) => {
        return (
          <div>
            {text === "Waiting" ? (
              <div className="text-yellow-500 inline-flex items-center">
                {text}
                <MdCancel
                  color="red"
                  className="ml-2 cursor-pointer w-6 h-6"
                  onClick={() => handleExtension(record.bookId, "Rejected")}
                />
                <FaCheck
                  color="green"
                  className="ml-2 cursor-pointer w-6 h-6"
                  onClick={() => handleExtension(record.bookId, "Approved")}
                />
              </div>
            ) : text === "Approved" ? (
              <div className="text-green-500">{text}</div>
            ) : (
              <div className="text-red-500">{text}</div>
            )}
          </div>
        )
      },
    },
    {
      title: "Action",
      dataIndex: "action",
      render: (text, record) =>
        record.status !== "Returned" && (
          <Popconfirm
            key="returned"
            title="Are you sure you want to mark this book as returned?"
            onConfirm={() => handleReturned(record.bookId)}
          >
            <Button type="primary" className="bg-green-500 hover:bg-green-600 text-white">
              Mark as returned
            </Button>
          </Popconfirm>
        ),
    },
  ]

  return (
    <div className="max-w-[1200px] mx-auto p-5 font-sans text-gray-800">
      <h1 className="text-2xl font-semibold text-slate-800 mb-6 pb-2 border-b border-gray-200">
        Borrowing Detail
      </h1>
      {loading ? (
        <Spin size="large" className="block my-8 mx-auto" />
      ) : (
        <>
          <h2 className="text-xl my-4 py-2 text-slate-700">
            <strong>Requestor</strong> {borrowingData.requestorName}
          </h2>
          <h2 className="text-xl my-4 py-2 text-slate-700">
            <strong>Status of borrowing: </strong>
            {borrowingData.status === "Waiting" ? (
              <span className="text-amber-500 font-semibold">{borrowingData.status}</span>
            ) : borrowingData.status === "Approved" ? (
              <span className="text-emerald-600 font-semibold">{borrowingData.status}</span>
            ) : (
              <span className="text-red-500 font-semibold">{borrowingData.status}</span>
            )}
          </h2>
          <div className="rounded-lg overflow-hidden shadow-sm mb-4">
            <Table
              columns={columns}
              dataSource={borrowingDetailData}
              rowKey="id"
              scroll={{ y: 300 }}
              pagination={false}
              className="bg-white"
            />
          </div>

          <Button 
            type="primary" 
            className="bg-blue-500 hover:bg-blue-600 text-white mt-4 py-2 px-4 h-auto rounded-md transition-all duration-300 hover:-translate-y-px hover:shadow-md"
            onClick={() => navigate(-1)}>
            Back
          </Button>
        </>
      )}
    </div>
  )
}

export default BorrowingDetailManagement
