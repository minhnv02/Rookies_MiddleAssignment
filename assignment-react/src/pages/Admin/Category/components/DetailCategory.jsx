import React, { useEffect, useState } from "react";
import axiosInstance from "../../../../utils/axiosInstance";
import { Table, message, Spin, Button } from "antd";
import { Link, useParams } from "react-router-dom";

const DetailCategory = () => {
  const { id } = useParams();
  const [categoryData, setCategoryData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true);
    axiosInstance
      .get(`/categories/${id}`)
      .then((res) => {
        if (res.data.success) {
          setCategoryData(res.data.data);
        } else {
          message.error(res.data.message);
        }
        setLoading(false);
      })
      .catch((err) => {
        message.error(err.message);
        setLoading(false);
      });
  }, [id]);

  const columns = [
    { title: "ID", dataIndex: "id", key: "id", sorter: true },
    { title: "Name", dataIndex: "name", key: "name", sorter: true },
    { title: "Author", dataIndex: "author", key: "author", sorter: true },
    { title: "Description", dataIndex: "description", key: "description" },
    {
      title: "Release Year",
      dataIndex: "releaseYear",
      key: "releaseYear",
      sorter: true,
    },
    {
      title: "Image",
      dataIndex: "image",
      key: "image",
      render: (text) => (
        <img src={text} alt="Book" style={{ width: "100px", height: "60px" }} />
      ),
    },
  ];

  return (
    <div className="p-4">
      <h1 className="text-2xl">Category Detail</h1>
      {loading ? (
        <Spin size="large" className="flex justify-center items-center" />
      ) : (
        <>
          <h2 className="text-xl mt-4 mb-2">
            <strong>Category Id:</strong> {categoryData.id}
          </h2>
          <h2 className="text-xl mt-4 mb-2">
            <strong>Category Name:</strong> {categoryData.name}
          </h2>
          <h2 className="text-xl mt-4 mb-2">
            <strong>List books</strong>
          </h2>
          <Table
            columns={columns}
            dataSource={categoryData.books}
            rowKey="id"
            scroll={{ y: 300 }}
          />
        </>
      )}
      <div className="flex mt-4">
        <Button className="bg-blue-600 text-white">
          <Link to="/admin/category">Back</Link>
        </Button>
      </div>
    </div>
  );
};

export default DetailCategory;
