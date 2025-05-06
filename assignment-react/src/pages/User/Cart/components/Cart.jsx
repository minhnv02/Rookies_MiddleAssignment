import React, { useState, useEffect, useContext } from "react";
import { Table, Button, message } from "antd";
import { MdDelete } from "react-icons/md";
import { Link } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
import axiosInstance from "../../../../utils/axiosInstance";

const Cart = () => {
  const [cart, setCart] = useState([]);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const { auth } = useContext(AuthContext);

  useEffect(() => {
    const fetchCart = async () => {
      try {
        const response = await axiosInstance.get(`/carts/${auth.userId}`);
        if (response.data.success) {
          setCart(response.data.data);
        } else {
          message.warning(response.data.message);
        }
      } catch (error) {
        if (error.response && error.response.status === 404) {
          alert("No books in cart");
        } else {
          message.error("Failed to fetch cart");
        }
      }
    };

    fetchCart();
  }, [auth.userId]);

  const handleRemoveFromCart = async (selectedRows) => {
    try {
      const response = await axiosInstance.delete(
        `/carts/${auth.userId}/${selectedRows[0]}`
      );
      if (response.data.success) {
        const updatedCart = cart.filter(
          (book) => !selectedRows.includes(book.bookId)
        );
        setCart(updatedCart);
        setSelectedRowKeys([]);
        message.success(response.data.message);
      } else {
        message.error(response.data.message);
      }
    } catch (error) {
      message.error(`Failed to remove book from cart: ${error.message}`);
    }
  };

  const handleBorrowSelectedBooks = async () => {
    if (!selectedRowKeys.length) {
      message.warning("Please select books to borrow");
      return;
    }

    if (selectedRowKeys.length > 5) {
      alert("You cannot borrow more than 5 books at a time");
      return;
    }
    
    const selectedBooks = cart.filter((book) =>
      selectedRowKeys.includes(book.bookId)
    );

    const borrowingDetails = selectedBooks.map((book) => ({
      bookId: book.bookId,
      returnedAt: new Date(
        new Date().getTime() + book.daysForBorrow * 24 * 60 * 60 * 1000
      ),
    }));

    const borrowingRequest = {
      requestorId: auth.userId,
      createdAt: new Date(),
      borrowingDetails,
    };

    try {
      const response = await axiosInstance.post(
        "/borrowings",
        borrowingRequest
      );
      if (response.data.success) {
        alert("Books borrowed successfully");

        // Remove borrowed books from cart in the DB
        for (const book of selectedBooks) {
          await axiosInstance.delete(`/carts/${auth.userId}/${book.bookId}`);
        }

        // Update the UI
        const remainingCart = cart.filter(
          (book) => !selectedRowKeys.includes(book.bookId)
        );
        setCart(remainingCart);
        setSelectedRowKeys([]);
      } else {
        alert(response.data.message);
      }
    } catch (error) {
      if (error.response && error.response.status === 409) {
        alert(error.response.data.message);
      } else {
        message.error("Failed to borrow books");
      }
    }
  };

  const rowSelection = {
    selectedRowKeys,
    onChange: (selectedRowKeys) => {
      if (selectedRowKeys.length > 5) {
        message.error("You can only borrow up to 5 books at a time");
      } else {
        setSelectedRowKeys(selectedRowKeys);
      }
    },
  };

  const columns = [
    {
      title: "Book Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Author",
      dataIndex: "author",
      key: "author",
    },
    {
      title: "Image",
      dataIndex: "image",
      key: "image",
      render: (image) => (
        <img
          src={image || "https://via.placeholder.com/240"}
          alt="Book"
          style={{ width: 100 }}
        />
      ),
    },
    {
      title: "Action",
      dataIndex: "bookId",
      render: (bookId) => (
        <Button
          type="link"
          danger
          onClick={() => handleRemoveFromCart([bookId])}
        >
          <MdDelete />
        </Button>
      ),
    },
  ];

  const dataSource = cart.map((book) => ({ ...book, key: book.bookId }));

  return (
    <div>
      <Table
        columns={columns}
        dataSource={dataSource}
        rowSelection={rowSelection}
      />
      <div className="flex justify-between mt-4 mx-5">
        <Button type="primary">
          <Link to="/home">Add more books</Link>
        </Button>
        <Button
          type="primary"
          disabled={!selectedRowKeys.length || selectedRowKeys.length > 5}
          onClick={handleBorrowSelectedBooks}
        >
          Borrow Selected Books
        </Button>
      </div>
    </div>
  );
};

export default Cart;
