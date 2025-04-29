import React, { useState, useEffect, useContext } from "react";
import { useParams } from "react-router-dom";
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
} from "antd";
import { AuthContext } from "../../../../contexts/AuthContext";
import axiosInstance from "../../../../utils/axiosInstance";
const { Content } = Layout;
const { Meta } = Card;

const BookDetail = () => {
  const { bookId } = useParams();
  const [book, setBook] = useState(null);
  const [comments, setComments] = useState([]);
  const [ratingDatas, setRatingDatas] = useState([]);
  const { auth } = useContext(AuthContext);
  const [newComment, setNewComment] = useState("");
  const [editingCommentId, setEditingCommentId] = useState(null);

  useEffect(() => {
    const fetchBookDetail = async () => {
      try {
        const response = await axiosInstance.get(`/books/${bookId}`);
        if (response.data.success) {
          setBook(response.data.data);
        } else {
          message.error(response.data.message);
        }
      } catch (error) {
        message.error("Failed to fetch book details");
      }
    };

    const fetchComments = async () => {
      try {
        const response = await axiosInstance.get(`/comments/book/${bookId}`);
        if (response.data.success) {
          setComments(response.data.data);
        } else {
          message.error(response.data.message);
        }
      } catch (error) {
        // message.error("Failed to fetch comments");
      }
    };

    const fetchRatings = async () => {
      try {
        const response = await axiosInstance.get(`/ratings/book/${bookId}`);
        if (response.data.success) {
          setRatingDatas(response.data.data);
        } else {
          message.error(response.data.message);
        }
      } catch (error) {
        // message.error("Failed to fetch ratings");
      }
    };

    fetchBookDetail();
    fetchComments();
    fetchRatings();
  }, [bookId]);

  const addToCart = (cart) => {
    const userId = auth.userId;
    if (!userId) return message.error("Please login to love books");
    axiosInstance
      .post(`/carts`, cart)
      .then((res) => {
        if (res.data.success) {
          message.success("Added to cart successfully");
        } else {
          message.warning("Book already in cart");
        }
      })
      .catch((error) => {
        if (error.response.status === 409) {
          message.warning("Book already in cart");
        } else {
          message.error("Failed to add to cart");
        }
      });
  };

  const handleAddComment = async () => {
    if (!auth.userId) return message.error("Please login to add a comment");

    try {
      const response = await axiosInstance.post(`/comments`, {
        content: newComment,
        bookId,
        userId: auth.userId,
      });
      if (response.data.success) {
        axiosInstance
          .get(`/comments/user/${auth.userId}/newest`)
          .then((res) => {
            if (res.data.success) {
              setComments([...comments, res.data.data]);
              setNewComment("");
            }
          });
        message.success("Comment added successfully");
      } else {
        message.error(response.data.message);
      }
    } catch (error) {
      message.error("Failed to add comment");
    }
  };

  const handleUpdateComment = async (id, updatedContent) => {
    try {
      const response = await axiosInstance.put(`/comments/${id}`, {
        content: updatedContent,
      });
      if (response.data.success) {
        setComments(
          comments.map((comment) =>
            comment.id === id
              ? { ...comment, content: updatedContent }
              : comment
          )
        );
        setEditingCommentId(null);
        message.success("Comment updated successfully");
      } else {
        message.error(response.data.message);
      }
    } catch (error) {
      message.error("Failed to update comment");
    }
  };

  const handleDeleteComment = async (id) => {
    try {
      const response = await axiosInstance.delete(`/comments/${id}`);
      if (response.data.success) {
        setComments(comments.filter((comment) => comment.id !== id));
        message.success("Comment deleted successfully");
      } else {
        message.error(response.data.message);
      }
    } catch (error) {
      message.error("Failed to delete comment");
    }
  };

  const renderCommentActions = (comment) => {
    if (comment.userId === auth.userId) {
      if (editingCommentId === comment.id) {
        return [
          <Button
            key="save"
            onClick={() => handleUpdateComment(comment.id, comment.content)}
          >
            Save
          </Button>,
          <Button key="cancel" onClick={() => setEditingCommentId(null)}>
            Cancel
          </Button>,
        ];
      } else {
        return [
          <Button key="edit" onClick={() => setEditingCommentId(comment.id)}>
            Edit
          </Button>,
          <Popconfirm
            key="delete"
            title="Are you sure you want to delete this comment?"
            onConfirm={() => handleDeleteComment(comment.id)}
          >
            <Button>Delete</Button>
          </Popconfirm>,
        ];
      }
    }
    return null;
  };

  return (
    <Layout className="min-h-screen">
      <Layout className="p-6">
        <Content>
          {book && (
            <>
              <Row gutter={[16, 16]}>
                <Col span={10}>
                  <img
                    alt={book.name}
                    src={book.image || "https://via.placeholder.com/240"}
                    className="h-full w-full object-cover"
                  />
                </Col>
                <Col span={14}>
                  <div className="book-info">
                    <h2 className="text-3xl font-semibold mb-4">{book.name}</h2>
                    <p>
                      <strong>Author:</strong> {book.author}
                    </p>
                    <p>
                      <strong>Release Year:</strong> {book.releaseYear}
                    </p>
                    <p>
                      <strong>Category:</strong> {book.categoryName}
                    </p>
                    <p>{book.description}</p>
                    <div className="flex justify-between items-center mb-4">
                      <Button
                        type="primary"
                        className="cursor-pointer text-white-500 hover:text-blue-500"
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
                        Add to cart
                      </Button>
                    </div>
                    <Rate disabled defaultValue={book.averageRating} />
                  </div>
                </Col>
              </Row>
              <div className="flex flex-wrap -mx-4">
                <div className="w-full md:w-1/2 px-4">
                  <List
                    header={`${comments.length} Comments`}
                    itemLayout="horizontal"
                    dataSource={comments}
                    renderItem={(comment) => (
                      <List.Item
                        key={comment.id || Math.random().toString()}
                        actions={renderCommentActions(comment)}
                      >
                        <List.Item.Meta
                          title={
                            <span
                              style={{
                                color:
                                  comment.userId === auth.userId
                                    ? "blue"
                                    : "inherit",
                              }}
                            >
                              {comment.userId === auth.userId
                                ? "Me"
                                : comment.userName}
                            </span>
                          }
                          description={
                            editingCommentId === comment.id ? (
                              <Input
                                value={comment.content}
                                onChange={(e) =>
                                  setComments(
                                    comments.map((c) =>
                                      c.id === comment.id
                                        ? { ...c, content: e.target.value }
                                        : c
                                    )
                                  )
                                }
                              />
                            ) : (
                              comment.content
                            )
                          }
                        />
                      </List.Item>
                    )}
                  />
                  {auth.role === "user" && (
                    <Form
                      onFinish={handleAddComment}
                      layout="inline"
                      className="mt-4"
                    >
                      <Form.Item
                        name="newComment"
                        rules={[
                          {
                            required: true,
                            message: "Please input a comment!",
                          },
                        ]}
                      >
                        <Input
                          value={newComment}
                          onChange={(e) => setNewComment(e.target.value)}
                          placeholder="Add a comment"
                        />
                      </Form.Item>
                      <Form.Item>
                        <Button type="primary" htmlType="submit">
                          Add Comment
                        </Button>
                      </Form.Item>
                    </Form>
                  )}
                </div>
                <div className="w-full md:w-1/2 px-4">
                  <List
                    header={`${ratingDatas.length} Ratings`}
                    itemLayout="horizontal"
                    dataSource={ratingDatas}
                    renderItem={(rating) => (
                      <List.Item
                        key={rating.bookId || Math.random().toString()}
                      >
                        <List.Item.Meta
                          title={
                            <span
                              style={{
                                color:
                                  rating.userId === auth.userId
                                    ? "blue"
                                    : "inherit",
                              }}
                            >
                              {rating.userId === auth.userId
                                ? "Me"
                                : rating.userName}
                            </span>
                          }
                          description={
                            <>
                              <Rate disabled value={rating.rate} />
                              <p> {rating.description}</p>
                            </>
                          }
                        />
                      </List.Item>
                    )}
                  />
                </div>
              </div>
            </>
          )}
        </Content>
      </Layout>
    </Layout>
  );
};

export default BookDetail;
