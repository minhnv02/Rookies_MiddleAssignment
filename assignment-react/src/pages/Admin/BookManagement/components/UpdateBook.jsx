"use client"

import { useState, useEffect } from "react"
import { useParams, useNavigate } from "react-router-dom"
import axiosInstance from "../../../../utils/axiosInstance"
import {
  Button,
  Form,
  Input,
  Upload,
  Select,
  message,
  Spin,
  Card,
  Typography,
  Row,
  Col,
  Divider,
  Space,
  Alert,
} from "antd"
import { UploadOutlined, SaveOutlined, ArrowLeftOutlined } from "@ant-design/icons"

const { Title, Text } = Typography
const { TextArea } = Input
const { Option } = Select

const UpdateBook = () => {
  const { id } = useParams()
  const navigate = useNavigate()
  const [book, setBook] = useState(null)
  const [categories, setCategories] = useState([])
  const [originalImage, setOriginalImage] = useState(null)
  const [form] = Form.useForm()
  const [selectedFile, setSelectedFile] = useState(null)
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true)
        // Fetch book data
        const bookResponse = await axiosInstance.get(`/books/${id}`)
        if (bookResponse.data.success) {
          const bookData = bookResponse.data.data
          setBook(bookData)
          setOriginalImage(bookData.image)
          form.setFieldsValue({
            ...bookData,
            categoryId: bookData.categoryId || undefined,
          })
        } else {
          message.error(bookResponse.data.message)
        }

        // Fetch categories
        const categoriesResponse = await axiosInstance.get(`/categories`)
        if (categoriesResponse.data.success) {
          setCategories(categoriesResponse.data.data)
        } else {
          message.error(categoriesResponse.data.message)
        }

        setLoading(false)
      } catch (error) {
        message.error(error.message)
        setLoading(false)
      }
    }

    fetchData()
  }, [id, form])

  const handleUpdate = (values) => {
    setSubmitting(true)
    const formData = new FormData()
    formData.append("Name", values.name)
    formData.append("Author", values.author)
    formData.append("Description", values.description)
    formData.append("ReleaseYear", values.releaseYear)
    formData.append("CategoryId", values.categoryId)
    formData.append("DaysForBorrow", values.daysForBorrow)

    if (selectedFile) {
      formData.append("Image", selectedFile)
    } else {
      formData.append("ImageUrl", originalImage)
    }

    axiosInstance
      .put(`/books/${id}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((response) => {
        if (response.data.success) {
          alert("Book updated successfully")
          navigate(`/admin/books/${id}`)
        } else {
          message.error(response.data.message)
        }
        setSubmitting(false)
      })
      .catch((error) => {
        message.error(error.message)
        setSubmitting(false)
      })
  }

  const handleUploadFile = ({ file }) => {
    if (file && file.type.startsWith("image/")) {
      setSelectedFile(file)
      setOriginalImage(null)
    } else {
      alert("You can only upload image files!")
    }
  }

  const handleRemoveFile = () => {
    setSelectedFile(null)
    setOriginalImage(book.image)
  }

  if (loading) {
    return (
      <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "80vh" }}>
        <Spin size="large" />
      </div>
    )
  }

  if (!book) {
    return (
      <Card style={{ margin: 24 }}>
        <Alert
          message="Book Not Found"
          description="The book you are trying to edit could not be found."
          type="error"
          showIcon
          style={{ marginBottom: 16 }}
        />
        <Button type="primary" onClick={() => navigate(-1)} icon={<ArrowLeftOutlined />}>
          Go Back
        </Button>
      </Card>
    )
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
        Update Book
      </Title>
      <Divider />

      <Form form={form} onFinish={handleUpdate} layout="vertical" requiredMark={false} {...formItemLayout}>
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
          {!selectedFile && originalImage && (
            <div style={{ marginBottom: 16 }}>
              <Text strong>Current Image:</Text>
              <div style={{ marginTop: 8, marginBottom: 16 }}>
                <img
                  src={originalImage || "/placeholder.svg"}
                  alt="Book Cover"
                  style={{
                    width: 150,
                    height: "auto",
                    objectFit: "cover",
                    borderRadius: 4,
                    border: "1px solid #d9d9d9",
                  }}
                />
              </div>
            </div>
          )}

          <Upload
            listType="picture-card"
            beforeUpload={() => false}
            maxCount={1}
            onRemove={handleRemoveFile}
            onChange={handleUploadFile}
          >
            <div style={{ marginTop: 8 }}>
              <UploadOutlined style={{ fontSize: 24, color: "#1890ff" }} />
              <div style={{ marginTop: 8 }}>Change Image</div>
            </div>
          </Upload>
          <Text type="secondary">Upload a new image to replace the current one (optional)</Text>
        </Form.Item>

        <Divider />

        <Form.Item style={{ marginBottom: 0 }}>
          <Space>
            <Button icon={<ArrowLeftOutlined />} onClick={() => navigate(-1)}>
              Cancel
            </Button>
            <Button type="primary" htmlType="submit" loading={submitting} icon={<SaveOutlined />}>
              Save Changes
            </Button>
          </Space>
        </Form.Item>
      </Form>
    </Card>
  )
}

export default UpdateBook
