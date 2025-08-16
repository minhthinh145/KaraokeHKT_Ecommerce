import React, { useState } from "react";
import {
  Modal,
  Form,
  Input,
  Select,
  Button,
  Card,
  Typography,
  Alert,
  DatePicker,
  Radio,
} from "antd";
import {
  UserAddOutlined,
  UserOutlined,
  MailOutlined,
  PhoneOutlined,
  TeamOutlined,
  IdcardOutlined,
  CalendarOutlined,
} from "@ant-design/icons";
import { useQLNhanSu } from "../../../../../hooks/useQLNhanSu";
import type { AddNhanVienDTO } from "../../../../../api";

const { Option } = Select;
const { Title, Text } = Typography;

interface AddNhanVienModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  roleOptions: Array<{ value: string; label: string }>;
}

export const AddNhanVienModal: React.FC<AddNhanVienModalProps> = ({
  isOpen,
  onClose,
  onSuccess,
  roleOptions,
}) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);
  const { nhanVienHandlers } = useQLNhanSu();

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setSubmitting(true);

      const createData: AddNhanVienDTO = {
        hoTen: values.hoTen,
        email: values.email,
        soDienThoai: values.soDienThoai,
        loaiNhanVien: values.loaiNhanVien,
        ngaySinh: values.ngaySinh?.format("YYYY-MM-DD"),
        loaiTaiKhoan: values.loaiNhanVien,
      };

      const result = await nhanVienHandlers.add(createData);

      if (result.success) {
        form.resetFields();
        onSuccess();
        onClose();
      }
    } finally {
      setSubmitting(false);
    }
  };

  const handleClose = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-green-500 to-emerald-500 rounded-full mb-3">
            <UserAddOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Thêm nhân viên mới
          </Title>
        </div>
      }
      open={isOpen}
      onCancel={handleClose}
      footer={null}
      width={800}
      className="top-8"
      bodyStyle={{ padding: "24px 32px" }}
    >
      {/* Alert hướng dẫn */}
      <Alert
        type="info"
        showIcon
        className="mb-6 rounded-lg border-green-200 bg-green-50"
        message={
          <div className="text-base">
            <Text strong className="text-green-800">
              💡 Lưu ý quan trọng:
            </Text>
            <div className="mt-1 text-green-700">
              • Vui lòng điền đầy đủ thông tin nhân viên
              <br />• Email phải là duy nhất trong hệ thống
              <br />• Sau khi tạo, nhân viên sẽ được tạo tài khoản đăng nhập
            </div>
          </div>
        }
      />

      <Form
        form={form}
        layout="vertical"
        onFinish={handleSubmit}
        className="flex flex-col"
      >
        {/* Thông tin cá nhân */}
        <Card className="bg-gradient-to-r from-blue-50 to-indigo-50 !border-0 !shadow-none">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Form.Item
              label={
                <span className="text-base font-semibold text-slate-700 flex items-center">
                  <UserOutlined className="mr-2 text-blue-600" />
                  Họ và tên
                </span>
              }
              name="hoTen"
              rules={[{ required: true, message: "Vui lòng nhập họ tên!" }]}
            >
              <Input
                size="large"
                placeholder="Nhập họ và tên đầy đủ"
                className="rounded-lg"
                prefix={<UserOutlined className="text-slate-400" />}
              />
            </Form.Item>

            <Form.Item
              label={
                <span className="text-base font-semibold text-slate-700 flex items-center">
                  <MailOutlined className="mr-2 text-blue-600" />
                  Email
                </span>
              }
              name="email"
              rules={[
                { required: true, message: "Vui lòng nhập email!" },
                { type: "email", message: "Email không hợp lệ!" },
              ]}
            >
              <Input
                size="large"
                placeholder="email@example.com"
                className="rounded-lg"
                prefix={<MailOutlined className="text-slate-400" />}
              />
            </Form.Item>
          </div>
        </Card>

        {/* Liên hệ & Loại nhân viên */}
        <Card className="bg-gradient-to-r from-purple-50 to-pink-50 !border-0 !shadow-none">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Form.Item
              label={
                <span className="text-base font-semibold text-slate-700 flex items-center">
                  <PhoneOutlined className="mr-2 text-purple-600" />
                  Số điện thoại
                </span>
              }
              name="soDienThoai"
              rules={[
                { required: true, message: "Vui lòng nhập số điện thoại!" },
                {
                  pattern: /^[0-9]{10,11}$/,
                  message: "Số điện thoại không hợp lệ!",
                },
              ]}
            >
              <Input
                size="large"
                placeholder="0987654321"
                className="rounded-lg"
                prefix={<PhoneOutlined className="text-slate-400" />}
              />
            </Form.Item>

            <Form.Item
              label={
                <span className="text-base font-semibold text-slate-700 flex items-center">
                  <TeamOutlined className="mr-2 text-purple-600" />
                  Loại nhân viên
                </span>
              }
              name="loaiNhanVien"
              rules={[
                { required: true, message: "Vui lòng chọn loại nhân viên!" },
              ]}
            >
              <Select
                size="large"
                placeholder="Chọn loại nhân viên"
                className="rounded-lg"
              >
                {roleOptions
                  .filter((opt) => opt.value !== "") // Bỏ option "Tất cả"
                  .map((option) => (
                    <Option key={option.value} value={option.value}>
                      {option.label}
                    </Option>
                  ))}
              </Select>
            </Form.Item>
          </div>
        </Card>

        {/* Thông tin bổ sung */}
        <Card className="bg-gradient-to-r from-orange-50 to-yellow-50 !border-0 !shadow-none">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Form.Item
              label={
                <span className="text-base font-semibold text-slate-700 flex items-center">
                  <CalendarOutlined className="mr-2 text-orange-600" />
                  Ngày sinh
                </span>
              }
              name="ngaySinh"
            >
              <DatePicker
                size="large"
                placeholder="Chọn ngày sinh"
                className="rounded-lg w-full"
                format="DD/MM/YYYY"
              />
            </Form.Item>

            <Form.Item
              label={
                <span className="text-base font-semibold text-slate-700 flex items-center">
                  <IdcardOutlined className="mr-2 text-orange-600" />
                  Giới tính
                </span>
              }
              name="gioiTinh"
            >
              <Radio.Group size="large">
                <Radio value="Nam">Nam</Radio>
                <Radio value="Nữ">Nữ</Radio>
              </Radio.Group>
            </Form.Item>
          </div>
        </Card>

        {/* Buttons */}
        <div className="flex justify-end space-x-3 pt-4 border-t border-slate-200">
          <Button
            size="large"
            onClick={handleClose}
            className="px-8 rounded-lg font-medium"
          >
            Hủy bỏ
          </Button>
          <Button
            type="primary"
            size="large"
            loading={submitting}
            htmlType="submit"
            icon={<UserAddOutlined />}
            className="px-8 rounded-lg font-medium bg-gradient-to-r from-green-500 to-emerald-500 border-none hover:from-green-600 hover:to-emerald-600 shadow-lg hover:shadow-xl transition-all duration-300"
          >
            {submitting ? "Đang thêm..." : "Thêm nhân viên"}
          </Button>
        </div>
      </Form>
    </Modal>
  );
};

export default AddNhanVienModal;
