"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import axiosInstance from "../../../../utils/axiosInstance"
import { Button, Form, Input, Upload, Select, message, Card, Typography, Row, Col, Divider, Space } from "antd"
import { UploadOutlined, SaveOutlined, ArrowLeftOutlined } from "@ant-design/icons"

const { Title, Text } = Typography
const { TextArea } = Input
const { Option } = Select

const CreateBook = () => {
  const navigate = useNavigate()
  const [categories, setCategories] = useState([])
  const [form] = Form.useForm()
  const [selectedFile, setSelectedFile] = useState(null)
  const [loading, setLoading] = useState(false)

  useEffect(() => {
    axiosInstance
      .get(`/categories`)
      .then((response) => {
        if (response.data.success) {
          setCategories(response.data.data)
        } else {
          message.error(response.data.message)
        }
      })
      .catch((error) => {
        message.error(error.message)
      })
  }, [])

  const handleCreate = (values) => {
    setLoading(true)
    const formData = new FormData()
    formData.append("Name", values.name)
    formData.append("Author", values.author)
    formData.append("Description", values.description)
    formData.append("ReleaseYear", values.releaseYear)
    formData.append("CategoryId", values.categoryId)
    formData.append("DaysForBorrow", values.daysForBorrow)

    if (selectedFile) {
      formData.append("Image", selectedFile)
    }

    axiosInstance
      .post(`/books`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((response) => {
        if (response.data.success) {
          alert("Book created successfully")
          navigate(-1)
        } else {
          message.error(response.data.message)
        }
        setLoading(false)
      })
      .catch((error) => {
        message.error(error.message)
        setLoading(false)
      })
  }

  const handleUploadFile = ({ file }) => {
    if (file && file.type.startsWith("image/")) {
      setSelectedFile(file)
    } else {
      alert("You can only upload image files!")
    }
  }

  const handleRemoveFile = () => {
    setSelectedFile(null)
  }

  const formItemLayout = {
    labelCol: { span: 24 },
    wrapperCol: { span: 24 },
  }

  return (
    <Card
      bordered={false}
      style={{
        margin: 24,
        borderRadius: 8,
        maxWidth: 800,
        marginLeft: "auto",
        marginRight: "auto",
      }}
    >
      <Title level={2} style={{ marginBottom: 24 }}>
        Create New Book
      </Title>
      <Divider />

      <Form form={form} onFinish={handleCreate} layout="vertical" requiredMark={false} {...formItemLayout}>
        <Row gutter={16}>
          <Col xs={24} md={12}>
            <Form.Item
              name="name"
              label="Book Name"
              rules={[{ required: true, message: "Please input the book name!" }]}
            >
              <Input placeholder="Enter book name" />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item name="author" label="Author" rules={[{ required: true, message: "Please input the author!" }]}>
              <Input placeholder="Enter author name" />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item
          name="description"
          label="Description"
          rules={[{ required: true, message: "Please input the description!" }]}
        >
          <TextArea rows={4} placeholder="Enter book description" />
        </Form.Item>

        <Row gutter={16}>
          <Col xs={24} md={8}>
            <Form.Item
              name="releaseYear"
              label="Release Year"
              rules={[{ required: true, message: "Please input the release year!" }]}
            >
              <Input type="number" min={1} placeholder="YYYY" />
            </Form.Item>
          </Col>
          <Col xs={24} md={8}>
            <Form.Item
              name="categoryId"
              label="Category"
              rules={[{ required: true, message: "Please select a category!" }]}
            >
              <Select placeholder="Select category">
                {categories.map((category) => (
                  <Option key={category.id} value={category.id}>
                    {category.name}
                  </Option>
                ))}
              </Select>
            </Form.Item>
          </Col>
          <Col xs={24} md={8}>
            <Form.Item
              name="daysForBorrow"
              label="Days For Borrow"
              rules={[{ required: true, message: "Please input the days for borrow!" }]}
            >
              <Input type="number" min={1} placeholder="Number of days" />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item name="image" label="Book Cover Image">
          <Upload
            listType="picture-card"
            beforeUpload={() => false}
            maxCount={1}
            onRemove={handleRemoveFile}
            onChange={handleUploadFile}
          >
            <div style={{ marginTop: 8 }}>
              <UploadOutlined style={{ fontSize: 24, color: "#1890ff" }} />
              <div style={{ marginTop: 8 }}>Upload</div>
            </div>
          </Upload>
          <Text type="secondary">Please upload a cover image for the book (optional)</Text>
        </Form.Item>

        <Divider />

        <Form.Item style={{ marginBottom: 0 }}>
          <Space>
            <Button icon={<ArrowLeftOutlined />} onClick={() => navigate(-1)}>
              Cancel
            </Button>
            <Button type="primary" htmlType="submit" loading={loading} icon={<SaveOutlined />}>
              Create Book
            </Button>
          </Space>
        </Form.Item>
      </Form>
    </Card>
  )
}

export default CreateBook
