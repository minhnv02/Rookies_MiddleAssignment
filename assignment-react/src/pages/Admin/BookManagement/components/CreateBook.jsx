import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../../utils/axiosInstance";
import { Button, Form, Input, Upload, Select, message } from "antd";
import { UploadOutlined } from "@ant-design/icons";

const CreateBook = () => {
  const navigate = useNavigate();
  const [categories, setCategories] = useState([]);
  const [form] = Form.useForm();
  const [selectedFile, setSelectedFile] = useState(null);

  useEffect(() => {
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
  }, []);

  const handleCreate = (values) => {
    const formData = new FormData();
    formData.append("Name", values.name);
    formData.append("Author", values.author);
    formData.append("Description", values.description);
    formData.append("ReleaseYear", values.releaseYear);
    formData.append("CategoryId", values.categoryId);
    formData.append("DaysForBorrow", values.daysForBorrow);

    if (selectedFile) {
      formData.append("Image", selectedFile);
    }

    axiosInstance
      .post(`/books`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((response) => {
        if (response.data.success) {
          message.success("Book created successfully");
          navigate(-1);
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
    } else {
      message.error("You can only upload image files!");
    }
  };

  const handleRemoveFile = () => {
    setSelectedFile(null);
  };

  return (
    <div className="p-4">
      <h1 className="text-2xl mb-4">Create Book</h1>
      <Form form={form} onFinish={handleCreate}>
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
        <Form.Item
          name="releaseYear"
          label="Release Year"
          rules={[
            { required: true, message: "Please input the release year!" },
          ]}
        >
          <Input type="number" min={1} />
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

export default CreateBook;
