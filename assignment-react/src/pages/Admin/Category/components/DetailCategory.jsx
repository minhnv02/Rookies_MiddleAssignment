import React, { useEffect, useState } from "react";
import axiosInstance from "../../../../utils/axiosInstance";
import { Table, message, Spin, Button, Card, Tag } from "antd";
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
    { 
      title: "ID", 
      dataIndex: "id", 
      key: "id", 
      sorter: true,
      className: "font-medium" 
    },
    { 
      title: "Name", 
      dataIndex: "name", 
      key: "name", 
      sorter: true,
      className: "font-medium text-blue-600" 
    },
    { 
      title: "Author", 
      dataIndex: "author", 
      key: "author", 
      sorter: true 
    },
    { 
      title: "Description", 
      dataIndex: "description", 
      key: "description",
      render: (text) => text || <span className="text-gray-400">No description</span>
    },
    {
      title: "Release Year",
      dataIndex: "releaseYear",
      key: "releaseYear",
      sorter: true,
      render: (year) => <Tag color="blue">{year}</Tag>
    },
    {
      title: "Image",
      dataIndex: "image",
      key: "image",
      render: (text) => (
        <img 
          src={text} 
          alt="Book" 
          className="w-20 h-16 object-cover rounded shadow"
        />
      ),
    },
  ];

  return (
    <div className="p-6 max-w-7xl mx-auto">
      <Card
        title="Category Details"
        bordered={false}
        className="shadow-lg rounded-lg"
      >
        {loading ? (
          <div className="flex justify-center py-12">
            <Spin size="large" />
          </div>
        ) : (
          <>
            <div className="mb-6 space-y-3">
              
              <div className="flex items-center space-x-2">
                <span className="font-semibold text-gray-700">Category Name:</span>
                <span className="text-lg font-medium text-blue-600">
                  {categoryData.name}
                </span>
              </div>
            </div>

            <h3 className="text-lg font-semibold mb-4 text-gray-800">
              Books in this category
            </h3>
            
            <Table
              columns={columns}
              dataSource={categoryData.books}
              rowKey="id"
              scroll={{ x: true }}
              className="border rounded-lg overflow-hidden"
              pagination={{
                pageSize: 5,
                showSizeChanger: false,
                className: "px-4 py-2"
              }}
            />
          </>
        )}

        <div className="flex justify-end mt-6">
          <Button type="primary" className="bg-blue-600 hover:bg-blue-700">
            <Link to="/admin/category">Back to Categories</Link>
          </Button>
        </div>
      </Card>
    </div>
  );
};

export default DetailCategory;