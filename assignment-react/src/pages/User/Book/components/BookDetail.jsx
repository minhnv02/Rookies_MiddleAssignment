"use client"

import { useState, useEffect, useContext } from "react"
import { useParams, useNavigate } from "react-router-dom"
import {
  Card,
  Row,
  Col,
  message,
  Rate,
  Layout,
  Button,
  Form,
  Input,
  List,
  Popconfirm,
  Typography,
  Divider,
  Tag,
  Avatar,
  Empty,
  Spin,
} from "antd"
import { AuthContext } from "../../../../contexts/AuthContext"
import axiosInstance from "../../../../utils/axiosInstance"
import {
  UserOutlined,
  CommentOutlined,
  StarOutlined,
  ShoppingCartOutlined,
  EditOutlined,
  DeleteOutlined,
  SaveOutlined,
  CloseOutlined,
  ArrowLeftOutlined,
  MessageOutlined,
} from "@ant-design/icons"

const { Content } = Layout
const { Meta } = Card
const { Title, Text, Paragraph } = Typography
const { TextArea } = Input

const BookDetail = () => {
  const { bookId } = useParams()
  const [book, setBook] = useState(null)
  const [comments, setComments] = useState([])
  const [ratingDatas, setRatingDatas] = useState([])
  const { auth } = useContext(AuthContext)
  const [newComment, setNewComment] = useState("")
  const [editingCommentId, setEditingCommentId] = useState(null)
  const [loading, setLoading] = useState(true)
  const navigate = useNavigate()

  useEffect(() => {
    const fetchBookDetail = async () => {
      setLoading(true)
      try {
        const response = await axiosInstance.get(`/books/${bookId}`)
        if (response.data.success) {
          setBook(response.data.data)
        } else {
          message.error(response.data.message)
        }
      } catch (error) {
        message.error(error.message || "Failed to fetch book details")
      } finally {
        setLoading(false)
      }
    }

    const fetchComments = async () => {
      try {
        const response = await axiosInstance.get(`/comments/book/${bookId}`)
        if (response.data.success) {
          setComments(response.data.data)
        } else {
          message.error(response.data.message)
        }
      } catch (error) {
        console.log("Comments fetch error:", error.message)
      }
    }

    const fetchRatings = async () => {
      try {
        const response = await axiosInstance.get(`/ratings/book/${bookId}`)
        if (response.data.success) {
          setRatingDatas(response.data.data)
        } else {
          message.error(response.data.message)
        }
      } catch (error) {
        console.log("Ratings fetch error:", error.message)
      }
    }

    fetchBookDetail()
    fetchComments()
    fetchRatings()
  }, [bookId])

  const addToCart = (cart) => {
    const userId = auth.userId
    if (!userId) return alert("Please login to add books to cart")
    axiosInstance
      .post(`/carts`, cart)
      .then((res) => {
        if (res.data.success) {
          alert("Added to cart successfully")
        } else {
          alert("Book already in cart")
        }
      })
      .catch((error) => {
        if (error.response && error.response.status === 409) {
          alert("Book already in cart")
        } else {
          alert(error.message || "Failed to add to cart")
        }
      })
  }

  const handleAddComment = async () => {
    if (!auth.userId) return message.error("Please login to add a comment")
    if (!newComment.trim()) return message.error("Comment cannot be empty")

    try {
      const response = await axiosInstance.post(`/comments`, {
        content: newComment,
        bookId,
        userId: auth.userId,
      })
      if (response.data.success) {
        axiosInstance
          .get(`/comments/user/${auth.userId}/newest`)
          .then((res) => {
            if (res.data.success) {
              setComments([...comments, res.data.data])
              setNewComment("")
            }
          })
          .catch((error) => {
            console.log("Get newest comment error:", error.message)
          })
        alert("Comment added successfully")
      } else {
        message.error(response.data.message)
      }
    } catch (error) {
      message.error(error.message || "Failed to add comment")
    }
  }

  const handleUpdateComment = async (id, updatedContent) => {
    if (!updatedContent.trim()) return message.error("Comment cannot be empty")

    try {
      const response = await axiosInstance.put(`/comments/${id}`, {
        content: updatedContent,
      })
      if (response.data.success) {
        setComments(comments.map((comment) => (comment.id === id ? { ...comment, content: updatedContent } : comment)))
        setEditingCommentId(null)
        alert("Comment updated successfully")
      } else {
        message.error(response.data.message)
      }
    } catch (error) {
      message.error(error.message || "Failed to update comment")
    }
  }

  const handleDeleteComment = async (id) => {
    try {
      const response = await axiosInstance.delete(`/comments/${id}`)
      if (response.data.success) {
        setComments(comments.filter((comment) => comment.id !== id))
        alert("Comment deleted successfully")
      } else {
        message.error(response.data.message)
      }
    } catch (error) {
      message.error(error.message || "Failed to delete comment")
    }
  }

  const renderCommentActions = (comment) => {
    if (comment.userId === auth.userId) {
      if (editingCommentId === comment.id) {
        return [
          <Button
            key="save"
            type="primary"
            size="small"
            icon={<SaveOutlined />}
            onClick={() => handleUpdateComment(comment.id, comment.content)}
          >
            Save
          </Button>,
          <Button key="cancel" size="small" icon={<CloseOutlined />} onClick={() => setEditingCommentId(null)}>
            Cancel
          </Button>,
        ]
      } else {
        return [
          <Button
            key="edit"
            type="text"
            size="small"
            icon={<EditOutlined />}
            onClick={() => setEditingCommentId(comment.id)}
          >
            Edit
          </Button>,
          <Popconfirm
            key="delete"
            title="Delete Comment"
            description="Are you sure you want to delete this comment?"
            onConfirm={() => handleDeleteComment(comment.id)}
            okText="Yes"
            cancelText="No"
          >
            <Button type="text" danger size="small" icon={<DeleteOutlined />}>
              Delete
            </Button>
          </Popconfirm>,
        ]
      }
    }
    return null
  }

  const handleGoBack = () => {
    navigate("/home")
  }

  if (loading) {
    return (
      <div className="loading-container">
        <Spin size="large" tip="Loading book details..." />
      </div>
    )
  }

  return (
    <Layout className="book-detail-layout">
      <Content className="book-detail-content">
        <div className="back-button-container">
          <Button type="primary" icon={<ArrowLeftOutlined />} onClick={handleGoBack} className="back-button">
            Back to Home
          </Button>
        </div>

        {book && (
          <>
            <Card className="book-detail-card">
              <Row gutter={[32, 24]}>
                <Col xs={24} md={10} lg={8}>
                  <div className="book-image-container">
                    <img
                      alt={book.name}
                      src={book.image || "https://via.placeholder.com/300x450"}
                      className="book-image"
                    />
                  </div>
                  <div className="book-rating-container">
                    <Rate disabled allowHalf value={book.averageRating || 0} />
                    <Text className="rating-text">
                      {book.averageRating ? book.averageRating.toFixed(1) : "No ratings yet"}
                    </Text>
                  </div>
                </Col>
                <Col xs={24} md={14} lg={16}>
                  <div className="book-info">
                    <Title level={2} className="book-title">
                      {book.name}
                    </Title>

                    <div className="book-meta">
                      <div className="meta-item">
                        <Text strong>Author:</Text>
                        <Text>{book.author}</Text>
                      </div>
                      <div className="meta-item">
                        <Text strong>Release Year:</Text>
                        <Text>{book.releaseYear}</Text>
                      </div>
                      <div className="meta-item">
                        <Text strong>Category:</Text>
                        <Tag color="blue">{book.categoryName}</Tag>
                      </div>
                      {book.daysForBorrow && (
                        <div className="meta-item">
                          <Text strong>Borrow Period:</Text>
                          <Tag color="green">{book.daysForBorrow} days</Tag>
                        </div>
                      )}
                    </div>

                    <Divider />

                    <div className="book-description">
                      <Title level={4}>Description</Title>
                      <Paragraph>{book.description || "No description available."}</Paragraph>
                    </div>

                    <div className="book-actions">
                      <Button
                        type="primary"
                        size="large"
                        icon={<ShoppingCartOutlined />}
                        onClick={() =>
                          addToCart({
                            bookId: book.id,
                            userId: auth.userId,
                            name: book.name,
                            author: book.author,
                            image: book.image,
                            daysForBorrow: book.daysForBorrow,
                          })
                        }
                      >
                        Add to Cart
                      </Button>
                    </div>
                  </div>
                </Col>
              </Row>
            </Card>

            <div className="book-interactions">
              <Row gutter={[24, 24]}>
                <Col xs={24} md={12}>
                  <Card
                    title={
                      <div className="section-header">
                        <MessageOutlined className="section-icon" />
                        <span>Comments ({comments.length})</span>
                      </div>
                    }
                    className="comments-card"
                  >
                    {comments.length > 0 ? (
                      <List
                        itemLayout="horizontal"
                        dataSource={comments}
                        renderItem={(comment) => (
                          <List.Item
                            key={comment.id || Math.random().toString()}
                            actions={renderCommentActions(comment)}
                            className="comment-item"
                          >
                            <List.Item.Meta
                              avatar={
                                <Avatar
                                  style={{
                                    backgroundColor: comment.userId === auth.userId ? "#1890ff" : "#d9d9d9",
                                  }}
                                  icon={<UserOutlined />}
                                />
                              }
                              title={
                                <div className="comment-author">
                                  <Text strong className={comment.userId === auth.userId ? "current-user" : ""}>
                                    {comment.userId === auth.userId ? "Me" : comment.userName}
                                  </Text>
                                </div>
                              }
                              description={
                                editingCommentId === comment.id ? (
                                  <TextArea
                                    value={comment.content}
                                    onChange={(e) =>
                                      setComments(
                                        comments.map((c) =>
                                          c.id === comment.id ? { ...c, content: e.target.value } : c,
                                        ),
                                      )
                                    }
                                    autoSize={{ minRows: 2 }}
                                    className="edit-comment-input"
                                  />
                                ) : (
                                  <div className="comment-content">{comment.content}</div>
                                )
                              }
                            />
                          </List.Item>
                        )}
                      />
                    ) : (
                      <Empty description="No comments yet" image={Empty.PRESENTED_IMAGE_SIMPLE} />
                    )}

                    {auth.role === "user" && (
                      <div className="add-comment-section">
                        <Divider />
                        <Form onFinish={handleAddComment} layout="vertical" className="comment-form">
                          <Form.Item
                            name="newComment"
                            rules={[
                              {
                                required: true,
                                message: "Please input a comment!",
                              },
                            ]}
                          >
                            <TextArea
                              value={newComment}
                              onChange={(e) => setNewComment(e.target.value)}
                              placeholder="Add your comment..."
                              autoSize={{ minRows: 3, maxRows: 6 }}
                              className="comment-input"
                            />
                          </Form.Item>
                          <Form.Item className="comment-submit">
                            <Button
                              type="primary"
                              htmlType="submit"
                              icon={<CommentOutlined />}
                              className="add-comment-button"
                            >
                              Add Comment
                            </Button>
                          </Form.Item>
                        </Form>
                      </div>
                    )}
                  </Card>
                </Col>

                <Col xs={24} md={12}>
                  <Card
                    title={
                      <div className="section-header">
                        <StarOutlined className="section-icon" />
                        <span>Ratings ({ratingDatas.length})</span>
                      </div>
                    }
                    className="ratings-card"
                  >
                    {ratingDatas.length > 0 ? (
                      <List
                        itemLayout="horizontal"
                        dataSource={ratingDatas}
                        renderItem={(rating) => (
                          <List.Item
                            key={rating.bookId + rating.userId || Math.random().toString()}
                            className="rating-item"
                          >
                            <List.Item.Meta
                              avatar={
                                <Avatar
                                  style={{
                                    backgroundColor: rating.userId === auth.userId ? "#1890ff" : "#d9d9d9",
                                  }}
                                  icon={<UserOutlined />}
                                />
                              }
                              title={
                                <div className="rating-author">
                                  <Text strong className={rating.userId === auth.userId ? "current-user" : ""}>
                                    {rating.userId === auth.userId ? "Me" : rating.userName}
                                  </Text>
                                </div>
                              }
                              description={
                                <div className="rating-content">
                                  <Rate disabled value={rating.rate} className="rating-stars" />
                                  {rating.description && <div className="rating-description">{rating.description}</div>}
                                </div>
                              }
                            />
                          </List.Item>
                        )}
                      />
                    ) : (
                      <Empty description="No ratings yet" image={Empty.PRESENTED_IMAGE_SIMPLE} />
                    )}
                  </Card>
                </Col>
              </Row>
            </div>
          </>
        )}
      </Content>

      <style jsx global>{`
        .book-detail-layout {
          min-height: 100vh;
          background: #f5f5f5;
          padding: 24px;
        }

        .loading-container {
          display: flex;
          justify-content: center;
          align-items: center;
          height: 80vh;
        }

        .book-detail-content {
          max-width: 1200px;
          margin: 0 auto;
        }

        .back-button-container {
          margin-bottom: 16px;
        }

        .back-button {
          display: flex;
          align-items: center;
        }

        .book-detail-card {
          margin-bottom: 24px;
          border-radius: 8px;
          box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
        }

        .book-image-container {
          display: flex;
          justify-content: center;
          margin-bottom: 16px;
          border-radius: 8px;
          overflow: hidden;
          box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        }

        .book-image {
          width: 100%;
          height: auto;
          object-fit: cover;
          max-height: 450px;
        }

        .book-rating-container {
          display: flex;
          align-items: center;
          justify-content: center;
          margin-top: 16px;
        }

        .rating-text {
          margin-left: 8px;
          font-size: 16px;
        }

        .book-title {
          margin-bottom: 16px !important;
        }

        .book-meta {
          margin-bottom: 16px;
        }

        .meta-item {
          margin-bottom: 8px;
          display: flex;
          align-items: center;
          gap: 8px;
        }

        .book-description {
          margin-bottom: 24px;
        }

        .book-actions {
          margin-top: 24px;
        }

        .book-interactions {
          margin-top: 24px;
        }

        .comments-card, .ratings-card {
          height: 100%;
          border-radius: 8px;
          box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
        }

        .section-header {
          display: flex;
          align-items: center;
          gap: 8px;
        }

        .section-icon {
          font-size: 18px;
        }

        .comment-item, .rating-item {
          padding: 12px 0;
        }

        .comment-author, .rating-author {
          display: flex;
          align-items: center;
          gap: 8px;
        }

        .current-user {
          color: #1890ff;
        }

        .comment-content, .rating-description {
          margin-top: 4px;
          white-space: pre-wrap;
          background-color: #f9f9f9;
          padding: 8px 12px;
          border-radius: 8px;
          border: 1px solid #f0f0f0;
        }

        .rating-stars {
          margin-bottom: 8px;
        }

        .add-comment-section {
          margin-top: 16px;
        }

        .comment-form {
          display: flex;
          flex-direction: column;
        }

        .comment-input {
          border-radius: 8px;
          resize: none;
          padding: 12px;
          font-size: 14px;
          border: 1px solid #d9d9d9;
          transition: all 0.3s;
        }

        .comment-input:hover, .comment-input:focus {
          border-color: #40a9ff;
          box-shadow: 0 0 0 2px rgba(24, 144, 255, 0.2);
        }

        .comment-submit {
          display: flex;
          justify-content: flex-end;
          margin-bottom: 0;
        }

        .add-comment-button {
          display: flex;
          align-items: center;
          gap: 8px;
          height: 40px;
          padding: 0 16px;
          font-size: 14px;
          border-radius: 6px;
        }

        .edit-comment-input {
          margin-top: 8px;
          border-radius: 8px;
          resize: none;
          padding: 12px;
        }

        @media (max-width: 768px) {
          .book-detail-layout {
            padding: 16px;
          }
          
          .book-image-container {
            max-width: 300px;
            margin: 0 auto 16px;
          }
        }
      `}</style>
    </Layout>
  )
}

export default BookDetail
