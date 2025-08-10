import React, { useState, useEffect } from "react";
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
  EditOutlined,
  UserOutlined,
  MailOutlined,
  PhoneOutlined,
  TeamOutlined,
  IdcardOutlined,
  CalendarOutlined,
} from "@ant-design/icons";
import dayjs from "dayjs";
import { useQLNhanSu } from "../../../../../hooks/useQLNhanSu";
import type { NhanVienDTO } from "../../../../../api/services/shared";

const { Option } = Select;
const { Title, Text } = Typography;

interface EditNhanVienModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  nhanVien: NhanVienDTO | null;
}

export const EditNhanVienModal: React.FC<EditNhanVienModalProps> = ({
  isOpen,
  onClose,
  onSuccess,
  nhanVien,
}) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);
  const { handlers } = useQLNhanSu();

  // Role options
  const ROLE_OPTIONS = [
    { value: "NhanVienKho", label: "Nhân viên kho" },
    { value: "NhanVienPhucVu", label: "Nhân viên phục vụ" },
    { value: "NhanVienTiepTan", label: "Nhân viên tiếp tân" },
  ];

  useEffect(() => {
    if (isOpen && nhanVien) {
      form.setFieldsValue({
        hoTen: nhanVien.hoTen,
        email: nhanVien.email,
        soDienThoai: nhanVien.soDienThoai,
        loaiNhanVien: nhanVien.loaiNhanVien,
        ngaySinh: nhanVien.ngaySinh ? dayjs(nhanVien.ngaySinh) : null,
      });
    }
  }, [isOpen, nhanVien, form]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setSubmitting(true);

      const updateData: NhanVienDTO = {
        maNv: nhanVien?.maNv || "",
        hoTen: values.hoTen,
        email: values.email,
        soDienThoai: values.soDienThoai,
        loaiNhanVien: values.loaiNhanVien,
        ngaySinh: values.ngaySinh?.format("YYYY-MM-DD"),
      };

      const result = await handlers.updateNhanVien(updateData);

      if (result.success) {
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
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-orange-500 to-red-500 rounded-full mb-3">
            <EditOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Cập nhật thông tin nhân viên
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
        type="warning"
        showIcon
        className="mb-6 rounded-lg border-orange-200 bg-orange-50"
        message={
          <div className="text-base">
            <Text strong className="text-orange-800">
              ⚠️ Lưu ý khi cập nhật:
            </Text>
            <div className="mt-1 text-orange-700">
              • Vui lòng kiểm tra kỹ thông tin trước khi lưu
              <br />• Mã nhân viên: <Text strong>{nhanVien?.maNv}</Text>
            </div>
          </div>
        }
      />

      <Form
        form={form}
        layout="vertical"
        onFinish={handleSubmit}
        // KHÔNG thêm gap-y-4 hay space-y-4
      >
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
                disabled
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
                {ROLE_OPTIONS.map((option) => (
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
            icon={<EditOutlined />}
            className="px-8 rounded-lg font-medium bg-gradient-to-r from-orange-500 to-red-500 border-none hover:from-orange-600 hover:to-red-600 shadow-lg hover:shadow-xl transition-all duration-300"
          >
            {submitting ? "Đang cập nhật..." : "Cập nhật"}
          </Button>
        </div>
      </Form>
    </Modal>
  );
};

export default EditNhanVienModal;
