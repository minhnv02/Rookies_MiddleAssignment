"use client"

import { useEffect, useState, useCallback } from "react"
import axiosInstance from "../../../../utils/axiosInstance"
import { Button, Table, message, Input, Space, Spin, Card, Typography, Row, Col, Divider, Tag } from "antd"
import { Link } from "react-router-dom"
import { BiCategory, BiDetail } from "react-icons/bi"
import { MdDelete } from "react-icons/md"
import { CiEdit } from "react-icons/ci"
import { IoIosCreate } from "react-icons/io"
import debounce from "lodash.debounce"
import { FaCalendarAlt, FaRegUser, FaSearch } from "react-icons/fa"

const { Title, Text } = Typography

const TableBook = () => {
  const [booksData, setBooksData] = useState([])
  const [loading, setLoading] = useState(true)
  const [searchParams, setSearchParams] = useState({
    name: "",
    author: "",
    releaseYearFrom: null,
    releaseYearTo: null,
    categoryName: "",
    pageNumber: 1,
    pageSize: 10, // Default to 10 books per page
  })
  const [totalBooks, setTotalBooks] = useState(0)

  const debouncedSearch = useCallback(
    debounce((newParams) => {
      setSearchParams((prev) => ({ ...prev, ...newParams }))
    }, 1000),
    [],
  )

  useEffect(() => {
    setLoading(true)
    const formData = new FormData()
    Object.keys(searchParams).forEach((key) => {
      if (searchParams[key] !== null && searchParams[key] !== "") {
        formData.append(key, searchParams[key])
      }
    })
    axiosInstance
      .post("/books/get-books", formData)
      .then((res) => {
        if (res.data.success) {
          setBooksData(res.data.data)
          setTotalBooks(res.data.totalCount)
        } else {
          message.error(res.data.message)
        }
        setLoading(false)
      })
      .catch((err) => {
        message.error(err.message)
        setLoading(false)
      })
  }, [searchParams])

  const handleSearchChange = (e) => {
    const { name, value } = e.target
    debouncedSearch({ [name]: value })
  }

  const handlePageChange = (page, pageSize) => {
    setSearchParams((prev) => ({
      ...prev,
      pageNumber: page,
      pageSize: pageSize,
    }))
  }

  const handleDelete = (id) => {
    if (
      window.confirm(
        "If you proceed with deleting this book, all related borrowing detail requests will also be permanently removed. Are you sure you want to proceed with deleting this book?",
      )
    ) {
      axiosInstance
        .delete(`/carts/${id}`)
        .then(() => {
          // Cart deleted successfully
        })
        .catch(() => {
          // Handle silently or add message.error if needed
        })
      axiosInstance
        .delete(`/books/${id}`)
        .then((res) => {
          if (res.data.success) {
            message.success("Book deleted successfully")
            setBooksData(booksData.filter((book) => book.id !== id))
          } else {
            message.error(res.data.message)
          }
        })
        .catch((err) => {
          message.error(err.message)
        })
    }
  }

  const columns = [
    {
      title: "ID",
      dataIndex: "id",
      key: "id",
      sorter: {
        compare: (a, b) => a.id - b.id,
        multiple: 1,
      },
      width: 80,
    },
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      sorter: {
        compare: (a, b) => a.name.localeCompare(b.name),
        multiple: 2,
      },
      render: (text) => <Text strong>{text}</Text>,
    },
    {
      title: "Author",
      dataIndex: "author",
      key: "author",
      sorter: {
        compare: (a, b) => a.author.localeCompare(b.author),
        multiple: 3,
      },
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
      ellipsis: true,
      width: 200,
    },
    {
      title: "Release Year",
      dataIndex: "releaseYear",
      key: "releaseYear",
      sorter: {
        compare: (a, b) => a.releaseYear - b.releaseYear,
        multiple: 4,
      },
      width: 120,
    },
    {
      title: "Days For Borrow",
      dataIndex: "daysForBorrow",
      key: "daysForBorrow",
      sorter: {
        compare: (a, b) => a.daysForBorrow - b.daysForBorrow,
        multiple: 5,
      },
      width: 150,
      render: (text) => <Tag color="blue">{text} days</Tag>,
    },
    {
      title: "Image",
      dataIndex: "image",
      key: "image",
      width: 120,
      render: (text) => (
        <img
          src={text || "/placeholder.svg"}
          alt="Book"
          style={{
            width: "100px",
            height: "60px",
            objectFit: "cover",
            borderRadius: "4px",
            boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
          }}
        />
      ),
    },
    {
      title: "Category Name",
      dataIndex: "categoryName",
      key: "categoryName",
      sorter: {
        compare: (a, b) => a.categoryName.localeCompare(b.categoryName),
      },
      render: (text) => <Tag color="green">{text}</Tag>,
    },
    {
      title: "Action",
      key: "action",
      width: 180,
      render: (text, record) => (
        <Space>
          <Button type="primary" ghost icon={<BiDetail />} size="middle">
            <Link to={`/admin/books/${record.id}`}>View</Link>
          </Button>
          <Button type="default" icon={<CiEdit />} size="middle">
            <Link to={`/admin/books/edit/${record.id}`}>Edit</Link>
          </Button>
          <Button danger icon={<MdDelete />} size="middle" onClick={() => handleDelete(record.id)} />
        </Space>
      ),
    },
  ]

  return (
    <Card bordered={false} style={{ margin: 24, borderRadius: 8 }}>
      <Title level={2} style={{ marginBottom: 24 }}>
        Book Management
      </Title>

      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Button type="primary" size="large" icon={<IoIosCreate />} style={{ display: "flex", alignItems: "center" }}>
            <Link to="/admin/books/create">Create Book</Link>
          </Button>
        </Col>

        <Col xs={24} md={24} style={{ marginTop: 16, marginBottom: 16 }}>
          <Row gutter={[8, 8]} justify="end">
            <Col>
              <Input
                prefix={<FaSearch />}
                placeholder="Search by Name"
                name="name"
                onChange={handleSearchChange}
                style={{ width: 200 }}
                allowClear
              />
            </Col>
            <Col>
              <Input
                prefix={<FaRegUser />}
                placeholder="Search by Author"
                name="author"
                onChange={handleSearchChange}
                style={{ width: 200 }}
                allowClear
              />
            </Col>
            <Col>
              <Input
                prefix={<BiCategory />}
                placeholder="Search by Category"
                name="categoryName"
                onChange={handleSearchChange}
                style={{ width: 200 }}
                allowClear
              />
            </Col>
            <Col>
              <Input
                prefix={<FaCalendarAlt />}
                placeholder="Year From"
                name="releaseYearFrom"
                type="number"
                onChange={handleSearchChange}
                style={{ width: 150 }}
                allowClear
              />
            </Col>
            <Col>
              <Input
                prefix={<FaCalendarAlt />}
                placeholder="Year To"
                name="releaseYearTo"
                type="number"
                onChange={handleSearchChange}
                style={{ width: 150 }}
                allowClear
              />
            </Col>
          </Row>
        </Col>
      </Row>

      <Divider style={{ margin: "16px 0" }} />

      {loading ? (
        <div style={{ display: "flex", justifyContent: "center", padding: "40px 0" }}>
          <Spin size="large" />
        </div>
      ) : (
        <Table
          columns={columns}
          dataSource={booksData}
          rowKey="id"
          pagination={{
            current: searchParams.pageNumber,
            pageSize: 10, 
            total: totalBooks,
            onChange: handlePageChange,
            showSizeChanger: false, 
          }}
          scroll={{ x: 1200, y: 500 }}
          bordered
          size="middle"
        />
      )}
    </Card>
  )
}

export default TableBook
