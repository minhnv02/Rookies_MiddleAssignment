"use client"

import { useEffect, useState } from "react"
import axiosInstance from "../../../../utils/axiosInstance"
import { Button, Spin, Table, message } from "antd"

const BorrowingExtend = () => {
  const [borrowingDetailData, setBorrowingDetailData] = useState([])
  const [loading, setLoading] = useState(true)
  useEffect(() => {
    setLoading(true)
    axiosInstance
      .get(`/borrowing-details/request-extend`)
      .then((res) => {
        if (res.data.success) {
          setBorrowingDetailData(res.data.data)
        } else {
          message.error(res.data.message)
        }
        setLoading(false)
      })
      .catch((err) => {
        if (err.response && err.response.status === 404) {
          message.warning("No borrowing data")
        } else {
          message.error(err.message)
        }
        setLoading(false)
      })
  }, [])

  const handleExtension = (bookId, statusExtend, borrowId) => {
    let returnedAt = new Date()
    const borrowingDetail = borrowingDetailData.find((item) => item.bookId === bookId)
    if (statusExtend === "Approved") {
      returnedAt = new Date(
        new Date(borrowingDetail.returnedAt).getTime() + borrowingDetail.daysForBorrow * 24 * 60 * 60 * 1000,
      )
    }

    axiosInstance
      .put(`/borrowing-details/handle-extension/${borrowId}/${bookId}`, {
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
    {
      title: "Requestor",
      dataIndex: "requestorName",
      key: "requestorName",
      sorter: {
        compare: (a, b) => a.requestorName.localeCompare(b.requestorName),
        multiple: 1,
      },
    },
    {
      title: "Book Name",
      dataIndex: "bookName",
      key: "bookName",
      sorter: {
        compare: (a, b) => a.bookName.localeCompare(b.bookName),
        multiple: 2,
      },
    },
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
      sorter: {
        compare: (a, b) => {
          const dateA = new Date(a.createdAt)
          const dateB = new Date(b.createdAt)
          return dateA.getTime() - dateB.getTime()
        },
        multiple: 3,
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
      sorter: {
        compare: (a, b) => {
          const dateA = new Date(a.returnedAt)
          const dateB = new Date(b.returnedAt)
          return dateA.getTime() - dateB.getTime()
        },
        multiple: 4,
      },
    },
    {
      title: "Action",
      dataIndex: "action",
      render: (text, record) => (
        <div>
          {record.statusExtend === "Waiting" ? (
            <div>
              <Button
                type="primary"
                className="bg-green-500 hover:bg-green-600 text-white mr-2"
                onClick={() => handleExtension(record.bookId, "Approved", record.borrowingId)}
              >
                Approve
              </Button>
              <Button
                type="primary"
                danger
                onClick={() => handleExtension(record.bookId, "Rejected", record.borrowingId)}
              >
                Reject
              </Button>
            </div>
          ) : (
            <div className={record.statusExtend === "Approved" ? "text-green-500" : "text-red-500"}>
              {record.statusExtend}
            </div>
          )}
        </div>
      ),
    },
  ]
  return (
    <div>
      <h1 className="text-2xl mb-4">Borrowing Extend</h1>
      {loading ? (
        <Spin size="large" className="flex justify-center items-center" />
      ) : (
        <Table columns={columns} dataSource={borrowingDetailData} rowKey="id" scroll={{ y: 300 }} />
      )}
    </div>
  )
}

export default BorrowingExtend
