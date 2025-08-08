import React, { useState } from "react";
import {
  Modal,
  Form,
  Select,
  Button,
  Input,
  Alert,
  Card,
  Typography,
  Spin,
} from "antd";
import {
  UserAddOutlined,
  PhoneOutlined,
  TeamOutlined,
} from "@ant-design/icons";
import type { AddAdminAccountDTO } from "../../../../../api/types/admins/QLHeThongtypes";
import { useQLHeThong } from "../../../../../hooks/useQLHeThong";

const { Option } = Select;
const { Title, Text } = Typography;

interface AddAdminAccountModalProps {
  visible: boolean;
  onCancel: () => void;
  onSuccess: () => void;
}

export const AddAdminAccountModal: React.FC<AddAdminAccountModalProps> = ({
  visible,
  onCancel,
  onSuccess,
}) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);
  const { handlers } = useQLHeThong();

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setSubmitting(true);

      const addData: AddAdminAccountDTO = {
        userName: values.userName,
        phoneNumber: values.phoneNumber,
        loaiTaiKhoan: values.loaiTaiKhoan,
      };

      const result = await handlers.addAdminAccount(addData);

      if (result.success) {
        form.resetFields();
        onSuccess();
      }
    } finally {
      setSubmitting(false);
    }
  };

  const handleClose = () => {
    form.resetFields();
    onCancel();
  };

  // Password marco info
  const getPasswordInfo = (roleCode: string) => {
    const passwordMap: Record<string, string> = {
      QuanLyKho: "qlKho@Admin123",
      QuanTriHeThong: "qtHeThong@Admin123",
      QuanLyNhanSu: "qlNhanSu@Admin123",
      QuanLyPhongHat: "qlPhongHat@Admin123",
    };
    return passwordMap[roleCode] || "admin@Admin123";
  };

  const selectedRole = Form.useWatch("loaiTaiKhoan", form) || "QuanLyKho";

  return (
    <Modal
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-purple-500 to-pink-500 rounded-full mb-3">
            <UserAddOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Thêm tài khoản quản trị
          </Title>
        </div>
      }
      open={visible}
      onCancel={handleClose}
      footer={null}
      width={700}
      className="top-12"
      bodyStyle={{ padding: "24px 32px" }}
    >
      <Spin spinning={submitting}>
        {/* Alert hướng dẫn */}
        <Alert
          type="info"
          showIcon
          className="mb-6 rounded-lg border-purple-200 bg-purple-50"
          message={
            <div className="text-base">
              <Text strong className="text-purple-800">
                🔐 Thông tin mật khẩu tự động:
              </Text>
              <div className="mt-1 text-purple-700">
                • <Text strong>Mật khẩu</Text> sẽ được tạo tự động theo vai trò
                <br />• Mật khẩu hiện tại:{" "}
                <Text code className="bg-purple-100 text-purple-800 font-bold">
                  {getPasswordInfo(selectedRole)}
                </Text>
                <br />• Có thể thay đổi mật khẩu về sau
              </div>
            </div>
          }
        />

        <Form
          form={form}
          layout="vertical"
          onFinish={handleSubmit}
          className="space-y-6"
          initialValues={{ loaiTaiKhoan: "QuanLyKho" }}
        >
          {/* Username */}
          <Card className="bg-gradient-to-r from-blue-50 to-cyan-50 border-blue-200 shadow-sm">
            <Form.Item
              label={
                <span className="text-lg font-semibold text-slate-700 flex items-center">
                  <UserAddOutlined className="mr-2 text-blue-600" />
                  Tên đăng nhập
                </span>
              }
              name="userName"
              rules={[
                { required: true, message: "Vui lòng nhập tên đăng nhập!" },
                { min: 8, message: "Tên đăng nhập phải có ít nhất 8 ký tự!" },
                {
                  pattern:
                    /^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]).{8,}$/,
                  message:
                    "Tên đăng nhập phải có ít nhất 1 chữ in hoa và 1 ký tự đặc biệt!",
                },
              ]}
            >
              <Input
                size="large"
                placeholder="👤 Nhập tên đăng nhập (ví dụ: admin_quanlykho)"
                className="rounded-lg"
                prefix={<UserAddOutlined className="text-slate-400" />}
              />
            </Form.Item>
          </Card>

          {/* Phone Number */}
          <Card className="bg-gradient-to-r from-green-50 to-emerald-50 border-green-200 shadow-sm">
            <Form.Item
              label={
                <span className="text-lg font-semibold text-slate-700 flex items-center">
                  <PhoneOutlined className="mr-2 text-green-600" />
                  Số điện thoại
                </span>
              }
              name="phoneNumber"
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
                placeholder="📱 Nhập số điện thoại (10 số)"
                className="rounded-lg"
                prefix={<PhoneOutlined className="text-slate-400" />}
              />
            </Form.Item>
          </Card>

          {/* Role */}
          <Card className="bg-gradient-to-r from-purple-50 to-pink-50 border-purple-200 shadow-sm">
            <Form.Item
              label={
                <span className="text-lg font-semibold text-slate-700 flex items-center">
                  <TeamOutlined className="mr-2 text-purple-600" />
                  Vai trò quản trị
                </span>
              }
              name="loaiTaiKhoan"
              rules={[{ required: true, message: "Vui lòng chọn vai trò!" }]}
            >
              <Select
                size="large"
                placeholder="Chọn vai trò quản trị"
                className="rounded-lg"
              >
                <Option value="QuanLyKho">
                  <span className="font-medium">📦 Quản lý kho</span>
                </Option>
                <Option value="QuanLyNhanSu">
                  <span className="font-medium">👥 Quản lý nhân sự</span>
                </Option>
                <Option value="QuanLyPhongHat">
                  <span className="font-medium">🎤 Quản lý phòng hát</span>
                </Option>
                <Option value="QuanTriHeThong">
                  <span className="font-medium">⚙️ Quản trị hệ thống</span>
                </Option>
              </Select>
            </Form.Item>
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
              className="px-8 rounded-lg font-medium bg-gradient-to-r from-purple-500 to-pink-500 border-none hover:from-purple-600 hover:to-pink-600 shadow-lg hover:shadow-xl transition-all duration-300"
            >
              {submitting ? "Đang tạo..." : "Tạo tài khoản"}
            </Button>
          </div>
        </Form>
      </Spin>
    </Modal>
  );
};
