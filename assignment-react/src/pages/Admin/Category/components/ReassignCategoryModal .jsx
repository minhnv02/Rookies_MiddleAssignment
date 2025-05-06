import React, { useState, useEffect } from "react";
import { Modal, Select, message } from "antd";
import axiosInstance from "../../../../utils/axiosInstance";

const { Option } = Select;

const ReassignCategoryModal = ({ visible, onCancel, onConfirm, chosenId }) => {
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);

  useEffect(() => {
    if (visible) {
      axiosInstance
        .get("/categories")
        .then((res) => {
          if (res.data.success) {
            setCategories(
              res.data.data.filter((category) => category.id !== chosenId)
            );
          } else {
            message.error(res.data.message);
          }
        })
        .catch((err) => {
          message.error(err.message);
        });
    }
  }, [visible]);

  const handleConfirm = () => {
    if (selectedCategory) {
      onConfirm(selectedCategory);
    } else {
      message.error("Please select or create a new category.");
    }
  };

  return (
    <Modal
      title="Reassign Category"
      visible={visible}
      onCancel={onCancel}
      onOk={handleConfirm}
    >
      <Select
        style={{ width: "100%" }}
        placeholder="Select an existing category"
        onChange={setSelectedCategory}
      >
        {categories.map((category) => (
          <Option key={category.id} value={category.id}>
            {category.name}
          </Option>
        ))}
      </Select>
    </Modal>
  );
};

export default ReassignCategoryModal;
  