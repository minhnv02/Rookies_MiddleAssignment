import React, { useEffect, useState, useCallback } from "react";
import axiosInstance from "../../../../utils/axiosInstance";
import { Button, Table, message, Input, Space, Spin } from "antd";
import { Link } from "react-router-dom";
import { BiCategory, BiDetail } from "react-icons/bi";
import { MdDelete } from "react-icons/md";
import { CiEdit } from "react-icons/ci";
import { IoIosCreate } from "react-icons/io";
import debounce from "lodash.debounce";
import { FaCalendarAlt, FaRegUser, FaSearch } from "react-icons/fa";
import {
  Box,
  Typography,
  TextField,
  Stack,
  Button as MuiButton,
  CircularProgress,
} from "@mui/material";

const TableBook = () => {
  const [booksData, setBooksData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchParams, setSearchParams] = useState({
    name: "",
    author: "",
    releaseYearFrom: null,
    releaseYearTo: null,
    categoryName: "",
    pageNumber: 1,
    pageSize: 10,
  });
  const [totalBooks, setTotalBooks] = useState(0);

  const debouncedSearch = useCallback(
    debounce((newParams) => {
      setSearchParams((prev) => ({ ...prev, ...newParams }));
    }, 1000),
    []
  );

  useEffect(() => {
    setLoading(true);
    const formData = new FormData();
    Object.keys(searchParams).forEach((key) => {
      if (searchParams[key] !== null && searchParams[key] !== "") {
        formData.append(key, searchParams[key]);
      }
    });
    axiosInstance
      .post("/books/get-books", formData)
      .then((res) => {
        if (res.data.success) {
          setBooksData(res.data.data);
          setTotalBooks(res.data.totalCount);
        } else {
          message.error(res.data.message);
        }
        setLoading(false);
      })
      .catch((err) => {
        message.error(err.message);
        setLoading(false);
      });
  }, [searchParams]);

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

  const handleDelete = (id) => {
    if (
      window.confirm(
        "If you proceed with deleting this book, all related borrowing detail requests will also be permanently removed. Are you sure you want to proceed with deleting this book?"
      )
    ) {
      axiosInstance
        .delete(`/carts/${id}`)
        .then((res) => {})
        .catch((err) => {});
      axiosInstance
        .delete(`/books/${id}`)
        .then((res) => {
          if (res.data.success) {
            message.success("Book deleted successfully");
            setBooksData(booksData.filter((book) => book.id !== id));
          } else {
            message.error(res.data.message);
          }
        })
        .catch((err) => {
          message.error(err.message);
        });
    }
  };

  const columns = [
    {
      title: "ID",
      dataIndex: "id",
      key: "id",
      sorter: {
        compare: (a, b) => a.id - b.id,
        multiple: 1,
      },
    },
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      sorter: {
        compare: (a, b) => a.name.localeCompare(b.name),
        multiple: 2,
      },
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
    { title: "Description", dataIndex: "description", key: "description" },
    {
      title: "Release Year",
      dataIndex: "releaseYear",
      key: "releaseYear",
      sorter: {
        compare: (a, b) => a.releaseYear - b.releaseYear,
        multiple: 4,
      },
    },
    {
      title: "Days For Borrow",
      dataIndex: "daysForBorrow",
      key: "daysForBorrow",
      sorter: {
        compare: (a, b) => a.daysForBorrow - b.daysForBorrow,
        multiple: 5,
      },
    },
    {
      title: "Image",
      dataIndex: "image",
      key: "image",
      render: (text) => (
        <img src={text} alt="Book" style={{ width: "100px", height: "60px" }} />
      ),
    },
    {
      title: "Category Name",
      dataIndex: "categoryName",
      key: "categoryName",
      sorter: {
        compare: (a, b) => a.categoryName.localeCompare(b.categoryName),
      },
    },
    {
      title: "Action",
      key: "action",
      render: (text, record) => (
        <>
          <Button className="text-green-700 px-3 py-2">
            <Link to={`/admin/books/${record.id}`}>
              <BiDetail />
            </Link>
          </Button>
          <Button className="text-yellow-700 px-3 py-2">
            <Link to={`/admin/books/edit/${record.id}`}>
              <CiEdit />
            </Link>
          </Button>
          <Button
            className="text-red-600 px-3 py-2"
            onClick={() => handleDelete(record.id)}
          >
            <MdDelete />
          </Button>
        </>
      ),
    },
  ];

  return (
    <div className="p-4">
      <h1 className="text-2xl">Book Management</h1>
      <div className="flex justify-between">
        <Button className="text-white bg-green-600 my-5">
          <Link to="/admin/books/create" className="text-center">
            Create Book
            <IoIosCreate className="inline mb-1 ml-2" />
          </Link>
        </Button>
        <Space>
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
      </div>
      {loading ? (
        <Spin size="large" className="flex justify-center items-center" />
      ) : (
        <Table
          columns={columns}
          dataSource={booksData}
          pagination={{
            current: searchParams.pageNumber,
            pageSize: searchParams.pageSize,
            total: totalBooks,
            onChange: handlePageChange,
          }}
          scroll={{ y: 500 }}
        />
      )}
    </div>
  );
};

export default TableBook;
