import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
import { Button, message } from "antd";

const DetailBook = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [book, setBook] = useState(null);

  useEffect(() => {
    axiosInstance
      .get(`/books/${id}`)
      .then((response) => {
        if (response.data.success) {
          setBook(response.data.data);
        } else {
          message.error(response.data.message);
        }
      })
      .catch((error) => {
        message.error(error.message);
      });
  }, [id]);

  if (!book) {
    return (
      <div className="flex justify-center items-center h-screen">
        Loading...
      </div>
    );
  }

  return (
    <div className="p-6 max-w-4xl mx-auto bg-white rounded-xl shadow-md space-y-4">
      <h1 className="text-3xl font-bold mb-6">Book Detail</h1>
      <div className="space-y-4">
        <div className="text-lg">
          <strong>ID:</strong> {book.id}
        </div>
        <div className="text-lg">
          <strong>Name:</strong> {book.name}
        </div>
        <div className="text-lg">
          <strong>Author:</strong> {book.author}
        </div>
        <div className="text-lg">
          <strong>Description:</strong> {book.description}
        </div>
        <div className="text-lg">
          <strong>Release Year:</strong> {book.releaseYear}
        </div>
        <div className="text-lg">
          <strong>Category:</strong> {book.categoryName}
        </div>
        <div className="text-lg">
          <strong>Days For Borrow:</strong> {book.daysForBorrow}
        </div>
        {book.image && (
          <div className="mt-6">
            <img
              src={book.image}
              alt="Book"
              className="w-48 h-48 object-cover rounded-md"
            />
          </div>
        )}
      </div>
      <div className="mt-6 flex space-x-4">
        <Button
          type="primary"
          onClick={() => navigate(`/admin/books/edit/${id}`)}
          className="bg-blue-500 hover:bg-blue-700 text-white font-bold px-4 rounded"
        >
          Update
        </Button>
        <Button
          type="default"
          onClick={() => navigate(-1)}
          className="bg-gray-500 hover:bg-gray-700 text-white font-bold px-4 rounded"
        >
          Back
        </Button>
      </div>
    </div>
  );
};

export default DetailBook;
