import debounce from "lodash.debounce";
import React, { useCallback, useEffect, useState } from "react";
import axiosInstance from "../../../../utils/axiosInstance";
import { Button, Input, Space, Spin, Table, message } from "antd";
import { Link } from "react-router-dom";
import { BiDetail } from "react-icons/bi";
import { CiEdit } from "react-icons/ci";
import { MdDelete } from "react-icons/md";
import { IoIosCreate } from "react-icons/io";
import { FaSearch } from "react-icons/fa";
import ReassignCategoryModal from "./ReassignCategoryModal ";
import CategoryModal from "./CategoryModal";

const TableCategory = () => {
  const [categoriesData, setCategoriesData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalVisible, setModalVisible] = useState(false);
  const [deleteCategoryId, setDeleteCategoryId] = useState(null);
  const [editCategory, setEditCategory] = useState(null);
  const [reassignModalVisible, setReassignModalVisible] = useState(false);
  const [searchParams, setSearchParams] = useState({
    name: "",
    pageNumber: 1,
    pageSize: 10,
  });

  const debouncedSearch = useCallback(
    debounce((newParams) => {
      setSearchParams((prev) => ({ ...prev, ...newParams }));
    }, 1000),
    []
  );

  useEffect(() => {
    setLoading(true);
    axiosInstance
      .get("/categories", { params: searchParams })
      .then((res) => {
        if (res.data.success) {
          setCategoriesData(res.data.data);
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
    setDeleteCategoryId(id);
    setReassignModalVisible(true);
  };

  const handleModalConfirm = (selectedCategory) => {
    const newCategoryId = selectedCategory ? selectedCategory : null;

    axiosInstance
      .delete(`/categories/${deleteCategoryId}/${newCategoryId}`)
      .then((res) => {
        if (res.data.success) {
          message.success("Category deleted successfully");
          setCategoriesData(
            categoriesData.filter(
              (category) => category.id !== deleteCategoryId
            )
          );
        } else {
          message.error(res.data.message);
        }
      })
      .catch((err) => {
        message.error(err.message);
      })
      .finally(() => {
        setReassignModalVisible(false);
        setDeleteCategoryId(null);
      });
  };

  const handleCategoryModalConfirm = ({ name, id }) => {
    if (id) {
      // Update category
      axiosInstance
        .put(`/categories/${id}`, { name })
        .then((res) => {
          if (res.data.success) {
            message.success("Category updated successfully");
            setCategoriesData(
              categoriesData.map((category) =>
                category.id === id ? { ...category, name } : category
              )
            );
          } else {
            message.error(res.data.message);
          }
        })
        .catch((err) => {
          message.error(err.message);
        });
    } else {
      // Create category
      axiosInstance
        .post("/categories", { name })
        .then((res) => {
          if (res.data.success) {
            message.success("Category created successfully");
            setCategoriesData([...categoriesData, res.data.data]);
          } else {
            message.error(res.data.message);
          }
        })
        .catch((err) => {
          message.error(err.message);
        });
    }
    setModalVisible(false);
    setEditCategory(null);
  };

  const handleEdit = (category) => {
    setEditCategory(category);
    setModalVisible(true);
  };

  const handleCreate = () => {
    setEditCategory(null);
    setModalVisible(true);
  };

  const columns = [
    { title: "ID", dataIndex: "id", key: "id", sorter: true },
    { title: "Name", dataIndex: "name", key: "name", sorter: true },
    {
      title: "Action",
      key: "action",
      render: (text, record) => (
        <>
          <Button className="text-green-700 px-3 py-2">
            <Link to={`/admin/category/${record.id}`}>
              <BiDetail />
            </Link>
          </Button>
          <Button
            className="text-yellow-700 px-3 py-2"
            onClick={() => handleEdit(record)}
          >
            <CiEdit />
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
      <h1 className="text-2xl">Category Management</h1>
      <div className="">
        <Button className="text-white bg-green-600 my-5" onClick={handleCreate}>
          Create Category
          <IoIosCreate className="inline mb-1 ml-2" />
        </Button>
        <Space className="ms-4">
          <Input
            prefix={<FaSearch />}
            placeholder="Search by Name...."
            name="name"
            onChange={handleSearchChange}
          />
        </Space>
      </div>
      {loading ? (
        <Spin size="large" className="flex justify-center items-center" />
      ) : (
        <Table
          columns={columns}
          dataSource={categoriesData}
          pagination={{
            current: searchParams.pageNumber,
            pageSize: searchParams.pageSize,
            onChange: handlePageChange,
          }}
          scroll={{ y: 250 }}
        />
      )}
      <CategoryModal
        visible={modalVisible}
        onCancel={() => setModalVisible(false)}
        onConfirm={handleCategoryModalConfirm}
        category={editCategory}
      />
      <ReassignCategoryModal
        visible={reassignModalVisible}
        onCancel={() => setReassignModalVisible(false)}
        onConfirm={handleModalConfirm}
        chosenId={deleteCategoryId}
      />
    </div>
  );
};

export default TableCategory;
