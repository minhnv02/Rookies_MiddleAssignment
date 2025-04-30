"use client"

import { useState, useEffect } from "react"
import { useParams, useNavigate } from "react-router-dom"
import axiosInstance from "../../../../utils/axiosInstance"
import { Button, message, Spin, Card, Typography, Descriptions, Divider, Space, Tag, Row, Col, Empty } from "antd"
import { ArrowLeftOutlined, EditOutlined } from "@ant-design/icons"

const { Title, Text } = Typography

const DetailBook = () => {
  const { id } = useParams()
  const navigate = useNavigate()
  const [book, setBook] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    axiosInstance
      .get(`/books/${id}`)
      .then((response) => {
        if (response.data.success) {
          setBook(response.data.data)
        } else {
          message.error(response.data.message)
        }
        setLoading(false)
      })
      .catch((error) => {
        message.error(error.message)
        setLoading(false)
      })
  }, [id])

  if (loading) {
    return (
      <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "80vh" }}>
        <Spin size="large" />
      </div>
    )
  }

  if (!book) {
    return (
      <Card style={{ margin: 24, textAlign: "center" }}>
        <Empty description="Book not found" image={Empty.PRESENTED_IMAGE_SIMPLE} />
        <Button type="primary" onClick={() => navigate(-1)} style={{ marginTop: 16 }} icon={<ArrowLeftOutlined />}>
          Go Back
        </Button>
      </Card>
    )
  }

  return (
    <Card
      bordered={false}
      style={{
        margin: 24,
        borderRadius: 8,
        maxWidth: 1000,
        marginLeft: "auto",
        marginRight: "auto",
      }}
    >
      <Title level={2} style={{ marginBottom: 16 }}>
        Book Details
      </Title>
      <Divider />

      <Row gutter={24}>
        <Col xs={24} md={8} style={{ marginBottom: 24 }}>
          {book.image ? (
            <img
              src={book.image || "/placeholder.svg"}
              alt={book.name}
              style={{
                width: "100%",
                maxWidth: 300,
                borderRadius: 8,
                boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
                objectFit: "cover",
                display: "block",
                margin: "0 auto",
              }}
            />
          ) : (
            <div
              style={{
                width: "100%",
                height: 300,
                background: "#f5f5f5",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                borderRadius: 8,
              }}
            >
              <Text type="secondary">No image available</Text>
            </div>
          )}
        </Col>

        <Col xs={24} md={16}>
          <Title level={3} style={{ marginTop: 0, marginBottom: 16 }}>
            {book.name}
          </Title>
          <Text type="secondary" style={{ fontSize: 16 }}>
            by {book.author}
          </Text>

          <Divider style={{ margin: "16px 0" }} />

          <Descriptions bordered column={{ xs: 1, sm: 2 }} size="middle">
            <Descriptions.Item label="ID">{book.id}</Descriptions.Item>
            <Descriptions.Item label="Release Year">{book.releaseYear}</Descriptions.Item>
            <Descriptions.Item label="Category">
              <Tag color="green">{book.categoryName}</Tag>
            </Descriptions.Item>
            <Descriptions.Item label="Days For Borrow">
              <Tag color="blue">{book.daysForBorrow} days</Tag>
            </Descriptions.Item>
          </Descriptions>

          <Divider orientation="left">Description</Divider>
          <div
            style={{
              background: "#f9f9f9",
              padding: 16,
              borderRadius: 8,
              marginBottom: 24,
            }}
          >
            <Text>{book.description}</Text>
          </div>
        </Col>
      </Row>

      <Divider />

      <div style={{ display: "flex", justifyContent: "flex-end" }}>
        <Space>
          <Button icon={<ArrowLeftOutlined />} onClick={() => navigate(-1)}>
            Back
          </Button>
          <Button type="primary" icon={<EditOutlined />} onClick={() => navigate(`/admin/books/edit/${id}`)}>
            Edit Book
          </Button>
        </Space>
      </div>
    </Card>
  )
}

export default DetailBook
