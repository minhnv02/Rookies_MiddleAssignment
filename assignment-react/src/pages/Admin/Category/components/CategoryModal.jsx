import React, { useState, useEffect } from "react";
import { Modal, Button, Input, message } from "antd";
import axiosInstance from "../../../../utils/axiosInstance";

const CategoryModal = ({ visible, onCancel, onConfirm, category }) => {
  const [name, setName] = useState("");

  useEffect(() => {
    if (category) {
      setName(category.name);
    } else {
      setName("");
    }
  }, [category]);

  const handleConfirm = () => {
    if (name.trim()) {
      onConfirm({ name: name.trim(), id: category ? category.id : null });
    } else {
      message.error("Category name is required.");
    }
  };

  return (
    <Modal
      title={category ? "Update Category" : "Create Category"}
      visible={visible}
      onCancel={onCancel}
      onOk={handleConfirm}
    >
      <Input
        placeholder="Category Name"
        value={name}
        onChange={(e) => setName(e.target.value)}
      />
    </Modal>
  );
};

export default CategoryModal;
