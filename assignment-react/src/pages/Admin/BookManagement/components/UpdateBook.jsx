import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
import { Button, Form, Input, Upload, Select, message } from "antd";
import { UploadOutlined } from "@ant-design/icons";

const UpdateBook = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [book, setBook] = useState(null);
  const [categories, setCategories] = useState([]);
  const [originalImage, setOriginalImage] = useState(null);
  const [form] = Form.useForm();
  const [selectedFile, setSelectedFile] = useState(null);

  useEffect(() => {
    axiosInstance
      .get(`/books/${id}`)
      .then((response) => {
        if (response.data.success) {
          const bookData = response.data.data;
          setBook(bookData);
          setOriginalImage(bookData.image);
          form.setFieldsValue(bookData);
        } else {
          message.error(response.data.message);
        }
      })
      .catch((error) => {
        message.error(error.message);
      });

    axiosInstance
      .get(`/categories`)
      .then((response) => {
        if (response.data.success) {
          setCategories(response.data.data);
        } else {
          message.error(response.data.message);
        }
      })
      .catch((error) => {
        message.error(error.message);
      });
  }, [id, form]);

  const handleUpdate = (values) => {
    const formData = new FormData();
    formData.append("Name", values.name);
    formData.append("Author", values.author);
    formData.append("Description", values.description);
    formData.append("ReleaseYear", values.releaseYear);
    formData.append("CategoryId", values.categoryId);
    formData.append("DaysForBorrow", values.daysForBorrow);

    if (selectedFile) {
      formData.append("Image", selectedFile);
    } else {
      formData.append("ImageUrl", originalImage);
    }

    axiosInstance
      .put(`/books/${id}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((response) => {
        if (response.data.success) {
          message.success("Book updated successfully");
          navigate(`/admin/books/${id}`);
        } else {
          message.error(response.data.message);
        }
      })
      .catch((error) => {
        message.error(error.message);
      });
  };

  const handleUploadFile = ({ file }) => {
    if (file && file.type.startsWith("image/")) {
      setSelectedFile(file);
      setOriginalImage(null);
    } else {
      message.error("You can only upload image files!");
    }
  };

  const handleRemoveFile = () => {
    setSelectedFile(null);
    setOriginalImage(book.image);
  };

  if (!book) {
    return <div>Loading...</div>;
  }

  return (
    <div className="p-4">
      <h1 className="text-2xl mb-4">Update Book</h1>
      <Form form={form} onFinish={handleUpdate}>
        <Form.Item
          name="name"
          label="Name"
          rules={[{ required: true, message: "Please input the book name!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="author"
          label="Author"
          rules={[{ required: true, message: "Please input the author!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="description"
          label="Description"
          rules={[{ required: true, message: "Please input the description!" }]}
        >
          <Input.TextArea />
        </Form.Item>
        <Form.Item name="releaseYear" label="Release Year">
          <Input
            type="number"
            min={1}
            rules={[
              { required: true, message: "Please input the release year!" },
            ]}
          />
        </Form.Item>
        <Form.Item
          name="categoryId"
          label="Category"
          rules={[{ required: true, message: "Please select a category!" }]}
        >
          <Select>
            {categories.map((category) => (
              <Select.Option key={category.id} value={category.id}>
                {category.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          name="daysForBorrow"
          label="Days For Borrow"
          rules={[
            { required: true, message: "Please input the days for borrow!" },
          ]}
        >
          <Input type="number" min={1} />
        </Form.Item>
        <Form.Item name="image" label="Image">
          <Upload
            listType="picture"
            beforeUpload={() => false}
            maxCount={1}
            onRemove={handleRemoveFile}
            onChange={handleUploadFile}
          >
            <Button icon={<UploadOutlined />}>Upload</Button>
          </Upload>
        </Form.Item>
        {!selectedFile && originalImage && (
          <div>
            <img
              src={originalImage}
              alt="Book"
              style={{ width: "200px", marginTop: "10px" }}
            />
          </div>
        )}
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Save
          </Button>
          <Button
            type="default"
            onClick={() => navigate(-1)}
            style={{ marginLeft: "8px" }}
          >
            Back
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

export default UpdateBook;
