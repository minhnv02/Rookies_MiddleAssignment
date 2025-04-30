"use client"

import { useState, useEffect, useContext, useCallback } from "react"
import { useNavigate } from "react-router-dom"
import {
  Card,
  Row,
  Col,
  message,
  Layout,
  Input,
  Space,
  Button,
  Spin,
  Pagination,
  Typography,
  Divider,
  Tag,
  Tooltip,
} from "antd"
import { FaCartPlus, FaHeart, FaSearch, FaRegUser, FaCalendarAlt } from "react-icons/fa"
import { BiCategory } from "react-icons/bi"
import debounce from "lodash.debounce"
import axiosInstance from "../../../../utils/axiosInstance"
import { AuthContext } from "../../../../contexts/AuthContext"

const { Content, Sider } = Layout
const { Meta } = Card
const { Title, Text } = Typography

const Home = ({ endpoint }) => {
  const [books, setBooks] = useState([])
  const [lovedBooks, setLovedBooks] = useState([])
  const [loading, setLoading] = useState(true)
  const [totalBooks, setTotalBooks] = useState(0)
  const { auth } = useContext(AuthContext)
  const navigate = useNavigate()

  const [searchParams, setSearchParams] = useState({
    name: "",
    author: "",
    releaseYearFrom: null,
    releaseYearTo: null,
    categoryName: "",
    pageNumber: 1,
    pageSize: 12,
  })

  const debouncedSearch = useCallback(
    debounce((newParams) => {
      setSearchParams((prev) => ({ ...prev, ...newParams, pageNumber: 1 }))
    }, 1000),
    []
  )

  useEffect(() => {
    const fetchBooks = async () => {
      setLoading(true)
      try {
        const formData = new FormData()
        Object.keys(searchParams).forEach((key) => {
          if (searchParams[key] !== null && searchParams[key] !== "") {
            formData.append(key, searchParams[key])
          }
        })

        const response =
          endpoint === "/books/get-books"
            ? await axiosInstance.post(endpoint, formData, { headers: { "Content-Type": "multipart/form-data" } })
            : await axiosInstance.get(endpoint)

        if (response.data.success) {
          setBooks(response.data.data)
          setTotalBooks(response.data.totalCount)
        } else {
          message.error(response.data.message)
        }
      } catch (error) {
        message.error(error.message || "Failed to fetch books")
      }
      setLoading(false)
    }

    const fetchLovedBooks = async () => {
      if (!auth.userId) return
      try {
        const res = await axiosInstance.get(`/loved-books/user/${auth.userId}`)
        if (res.data.success) setLovedBooks(res.data.data)
        else message.error(res.data.message)
      } catch (err) {
        message.error(err.message || "Failed to fetch loved books")
      }
    }

    fetchBooks()
    fetchLovedBooks()
  }, [endpoint, auth.userId, searchParams])

  const handleSearchChange = (e) => {
    const { name, value } = e.target
    debouncedSearch({ [name]: value })
  }

  const handlePageChange = (page, pageSize) => {
    setSearchParams((prev) => ({ ...prev, pageNumber: page, pageSize }))
  }

  const handleLoveToggle = (bookId, loved) => {
    if (!auth.userId) return message.error("Please login to love books")
    if (loved) {
      axiosInstance
        .delete(`/loved-books/${auth.userId}/${bookId}`)
        .then((res) => {
          if (res.data.success) {
            setBooks((prev) =>
              prev.map((b) => (b.id === bookId ? { ...b, countLoved: b.countLoved - 1, loved: false } : b))
            )
            setLovedBooks((prev) => prev.filter((b) => b.bookId !== bookId))
            message.success("Book unloved successfully")
          } else message.error(res.data.message)
        })
        .catch((err) => message.error(err.message || "Failed to unlove book"))
    } else {
      axiosInstance
        .post(`/loved-books`, { userId: auth.userId, bookId })
        .then((res) => {
          if (res.data.success) {
            setBooks((prev) =>
              prev.map((b) => (b.id === bookId ? { ...b, countLoved: b.countLoved + 1, loved: true } : b))
            )
            setLovedBooks((prev) => [...prev, { bookId }])
            message.success("Book loved successfully")
          } else message.error(res.data.message)
        })
        .catch((err) => message.error(err.message || "Failed to love book"))
    }
  }

  const isBookLoved = (bookId) => {
    return auth.userId && lovedBooks.some((b) => b.bookId === bookId)
  }

  const addToCart = (cart) => {
    if (!auth.userId) return message.error("Please login to add books to cart")
    axiosInstance
      .post(`/carts`, cart)
      .then((res) => {
        if (res.data.success) message.success("Added to cart successfully")
        else message.warning("Book already in cart")
      })
      .catch((err) => {
        if (err.response?.status === 409) message.warning("Book already in cart")
        else message.error(err.message || "Failed to add to cart")
      })
  }

  return (
    <Layout className="book-catalog-layout">
      {endpoint === "/books/get-books" && (
        <Sider width={280} className="filter-sidebar" style={{ background: "transparent", padding: "20px 16px" }}>
          <div className="filter-header">
            <Title level={4} style={{ marginBottom: "24px" }}>Search Filters</Title>
          </div>
          <Divider style={{ margin: "0 0 24px 0" }} />
          <Space direction="vertical" size="large" style={{ width: "100%" }}>
            <Input 
              prefix={<FaSearch />} 
              name="name" 
              placeholder="Book name..." 
              allowClear 
              onChange={handleSearchChange}
              size="large"
              style={{ height: "42px" }}
            />
            <Input 
              prefix={<FaRegUser />} 
              name="author" 
              placeholder="Author..." 
              allowClear 
              onChange={handleSearchChange}
              size="large"
              style={{ height: "42px" }}
            />
            <Input 
              prefix={<BiCategory />} 
              name="categoryName" 
              placeholder="Category..." 
              allowClear 
              onChange={handleSearchChange}
              size="large"
              style={{ height: "42px" }}
            />
            <Input 
              prefix={<FaCalendarAlt />} 
              name="releaseYearFrom" 
              placeholder="From year" 
              type="number" 
              allowClear 
              onChange={handleSearchChange}
              size="large"
              style={{ height: "42px" }}
            />
            <Input 
              prefix={<FaCalendarAlt />} 
              name="releaseYearTo" 
              placeholder="To year" 
              type="number" 
              allowClear 
              onChange={handleSearchChange}
              size="large"
              style={{ height: "42px" }}
            />
          </Space>
        </Sider>
      )}

      <Layout>
        <Content className="books-content" style={{ padding: "24px", background: "#f5f5f5" }}>
          {loading ? (
            <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "60vh" }}>
              <Spin size="large" tip="Loading books..." />
            </div>
          ) : (
            <>
              <Row gutter={[24, 32]}>
                {books.map((book) => {
                  const loved = isBookLoved(book.id)
                  return (
                    <Col key={book.id} xs={24} sm={12} md={8} lg={6} xl={4}>
                      <Card
                        hoverable
                        cover={
                          <div onClick={() => navigate(`/books/${book.id}`)} style={{ height: "100px", overflow: "hidden" }}>
                            <img 
                              alt={book.name} 
                              src={book.image || "https://via.placeholder.com/240"} 
                              style={{ width: "100%", height: "100%", objectFit: "cover" }}
                            />
                          </div>
                        }
                        actions={[
                          <Tooltip title={loved ? "Unlove" : "Love"}>
                            <FaHeart
                              color={loved ? "red" : "gray"}
                              onClick={() => handleLoveToggle(book.id, loved)}
                            />
                          </Tooltip>,
                          <Tooltip title="Add to cart">
                            <FaCartPlus
                              onClick={() =>
                                addToCart({ userId: auth.userId, bookId: book.id, quantity: 1 })
                              }
                            />
                          </Tooltip>,
                        ]}
                      >
                        <Meta 
                          title={<div style={{ height: "44px", overflow: "hidden" }}>{book.name}</div>} 
                          description={
                            <div style={{ minHeight: "90px" }}>
                              <div style={{ marginBottom: "6px" }}>
                                <Text type="secondary">Author:</Text> {book.author}
                              </div>
                              <div style={{ marginBottom: "6px" }}>
                                <Text type="secondary">Year:</Text> {book.releaseYear}
                              </div>
                              <div style={{ display: "flex", alignItems: "center" }}>
                                <Text type="secondary" style={{ marginRight: "4px" }}>Category:</Text>
                                <Tag color="green" style={{ maxWidth: "100%", overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>
                                  {book.categoryName}
                                </Tag>
                              </div>
                            </div>
                          } 
                        />
                      </Card>
                    </Col>
                  )
                })}
              </Row>
              <div style={{ marginTop: "32px", textAlign: "center" }}>
                <Pagination
                  current={searchParams.pageNumber}
                  pageSize={searchParams.pageSize}
                  total={totalBooks}
                  onChange={handlePageChange}
                  showSizeChanger={false}
                />
              </div>
            </>
          )}
        </Content>
      </Layout>
    </Layout>
  )
}

export default Home