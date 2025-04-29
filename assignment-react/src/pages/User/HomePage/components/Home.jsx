import React, { useState, useEffect, useContext, useCallback } from "react";
import axiosInstance from "../../../../utils/axiosInstance";
import {
  Card,
  Row,
  Col,
  message,
  Rate,
  Layout,
  Input,
  Space,
  Button,
  Spin,
  Pagination,
} from "antd";
import { Link, useNavigate } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
import {
  FaCartPlus,
  FaHeart,
  FaSearch,
  FaRegUser,
  FaCalendarAlt,
} from "react-icons/fa";
import { BiCategory } from "react-icons/bi";
import debounce from "lodash.debounce";

const { Content, Sider } = Layout;
const { Meta } = Card;

const Home = ({ endpoint }) => {
  const [books, setBooks] = useState([]);
  const [lovedBooks, setLovedBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [totalBooks, setTotalBooks] = useState(0);
  const { auth } = useContext(AuthContext);
  const navigate = useNavigate();

  const [searchParams, setSearchParams] = useState({
    name: "",
    author: "",
    releaseYearFrom: null,
    releaseYearTo: null,
    categoryName: "",
    pageNumber: 1,
    pageSize: 10,
  });

  const debouncedSearch = useCallback(
    debounce((newParams) => {
      setSearchParams((prev) => ({ ...prev, ...newParams, pageNumber: 1 }));
    }, 1000),
    []
  );

  useEffect(() => {
    const fetchBooks = async () => {
      setLoading(true);
      try {
        const formData = new FormData();
        Object.keys(searchParams).forEach((key) => {
          if (searchParams[key] !== null && searchParams[key] !== "") {
            formData.append(key, searchParams[key]);
          }
        });
        if (endpoint === "/books/get-books") {
          const response = await axiosInstance.post(endpoint, formData, {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          });
          if (response.data.success) {
            setBooks(response.data.data);
            setTotalBooks(response.data.totalCount);
          } else {
            message.error(response.data.message);
          }
        } else {
          const response = await axiosInstance.get(endpoint);
          if (response.data.success) {
            setBooks(response.data.data);
            setTotalBooks(response.data.totalCount);
          } else {
            message.error(response.data.message);
          }
        }
      } catch (error) {
        message.error("Failed to fetch books");
      }
      setLoading(false);
    };

    const fetchLovedBooks = async () => {
      if (!auth.userId) return;

      try {
        const response = await axiosInstance.get(
          `/loved-books/user/${auth.userId}`
        );
        if (response.data.success) {
          setLovedBooks(response.data.data);
        } else {
          message.error(response.data.message);
        }
      } catch (error) {
        message.error("Failed to fetch loved books");
      }
    };

    fetchBooks();
    fetchLovedBooks();
  }, [endpoint, auth.userId, searchParams]);

  const handleSearchChange = (e) => {
    const { name, value } = e.target;
    debouncedSearch({ [name]: value });
  };

  const handlePageChange = (page, pageSize) => {
    setSearchParams((prev) => ({
      ...prev,
      pageNumber: page,
      pageSize: pageSize,
    }));
  };

  const handleLoveToggle = (bookId, loved) => {
    const userId = auth.userId;
    if (!userId) return message.error("Please login to love books");

    if (loved) {
      axiosInstance
        .delete(`/loved-books/${userId}/${bookId}`)
        .then((response) => {
          if (response.data.success) {
            setBooks((prevBooks) =>
              prevBooks.map((book) =>
                book.id === bookId
                  ? { ...book, countLoved: book.countLoved - 1, loved: false }
                  : book
              )
            );
            setLovedBooks((prevLovedBooks) =>
              prevLovedBooks.filter((book) => book.bookId !== bookId)
            );
            message.success("Book unloved successfully");
          } else {
            message.error(response.data.message);
          }
        })
        .catch((error) => {
          message.error("Failed to unlove book");
        });
    } else {
      axiosInstance
        .post(`/loved-books`, { userId, bookId })
        .then((response) => {
          if (response.data.success) {
            setBooks((prevBooks) =>
              prevBooks.map((book) =>
                book.id === bookId
                  ? { ...book, countLoved: book.countLoved + 1, loved: true }
                  : book
              )
            );
            setLovedBooks((prevLovedBooks) => [
              ...prevLovedBooks,
              { bookId: bookId },
            ]);
            message.success("Book loved successfully");
          } else {
            message.error(response.data.message);
          }
        })
        .catch((error) => {
          message.error("Failed to love book");
        });
    }
  };

  const isBookLoved = (bookId) => {
    if (!auth.userId) return false; // If user not authenticated, return false
    return lovedBooks.some((lovedBook) => lovedBook.bookId === bookId);
  };

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

  return (
    <Layout className="py-6 px-4 flex">
      {endpoint === "/books/get-books" && (
        <div className="flex flex-col">
          <Sider
            width={200}
            style={{
              backgroundColor: "#f5f5f5",
              marginRight: "16px",
            }}
          >
            <Space
              direction="vertical"
              size="large"
              style={{
                backgroundColor: "rgb(229 231 235)",
                padding: "16px",
                borderRadius: "8px",
                boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
              }}
            >
              <Input
                prefix={<FaSearch />}
                placeholder="Search by Name...."
                name="name"
                onChange={handleSearchChange}
              />
              <Input
                prefix={<FaRegUser />}
                placeholder="Search by Author...."
                name="author"
                onChange={handleSearchChange}
              />
              <Input
                prefix={<BiCategory />}
                placeholder="Search by Category...."
                name="categoryName"
                onChange={handleSearchChange}
              />
              <Input
                prefix={<FaCalendarAlt />}
                placeholder="Release Year From"
                name="releaseYearFrom"
                type="number"
                onChange={handleSearchChange}
              />
              <Input
                prefix={<FaCalendarAlt />}
                placeholder="Release Year To"
                name="releaseYearTo"
                type="number"
                onChange={handleSearchChange}
              />
            </Space>
          </Sider>
          <div style={{ width: 200 }}>
            <Pagination
              current={searchParams.pageNumber}
              pageSize={searchParams.pageSize}
              total={totalBooks}
              onChange={handlePageChange}
            />
          </div>
        </div>
      )}
      <Layout>
        <Content>
          {loading ? (
            <div className="flex justify-center items-center">
              <Spin size="large" />
            </div>
          ) : (
            <>
              <Row gutter={[16, 16]}>
                {books.map((book) => {
                  const isLoved = isBookLoved(book.id);
                  return (
                    <Col key={book.id}>
                      <Card
                        hoverable
                        className="w-60 h-100"
                        cover={
                          <img
                            alt={book.name}
                            src={
                              book.image || "https://via.placeholder.com/240"
                            }
                            className="h-40 object-cover"
                            onClick={() => navigate(`/books/${book.id}`)}
                          />
                        }
                      >
                        <Meta
                          title={book.name}
                          description={
                            <div className="description">
                              <p>Author: {book.author}</p>
                              <p>Release Year: {book.releaseYear}</p>
                              <p>Category: {book.categoryName}</p>
                              <div className="flex justify-between">
                                <p className="flex items-center">
                                  <FaHeart
                                    className={`inline mr-2 cursor-pointer ${
                                      isLoved ? "text-red-500" : "text-gray-500"
                                    } hover:text-red-400`}
                                    onClick={() =>
                                      handleLoveToggle(book.id, isLoved)
                                    }
                                  />
                                  {book.countLoved}
                                </p>
                                <FaCartPlus
                                  className="cursor-pointer text-gray-500 hover:text-blue-500"
                                  onClick={() =>
                                    addToCart({
                                      userId: auth.userId,
                                      bookId: book.id,
                                      name: book.name,
                                      author: book.author,
                                      image: book.image,
                                      daysForBorrow: book.daysForBorrow,
                                    })
                                  }
                                />
                              </div>

                              <Rate
                                disabled
                                defaultValue={book.averageRating}
                              />
                            </div>
                          }
                        />
                      </Card>
                    </Col>
                  );
                })}
              </Row>
            </>
          )}
        </Content>
      </Layout>
    </Layout>
  );
};

export default Home;
