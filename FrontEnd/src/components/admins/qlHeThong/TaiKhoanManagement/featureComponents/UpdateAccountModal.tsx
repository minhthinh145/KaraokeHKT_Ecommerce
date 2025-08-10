import React, { useEffect, useState } from "react";
import { Modal, Form, Input, Select, Button, Typography } from "antd";
import { useQLHeThong } from "../../../../../hooks/useQLHeThong";
import { ROLE_OPTIONS_ADMIN } from "../../../../../constants/auth"; // hoặc import đúng chỗ bạn khai báo
import { UserSwitchOutlined } from "@ant-design/icons";

const { Title } = Typography;
interface UpdateAccountModalProps {
  open: boolean;
  onClose: () => void;
  defaultValues: {
    maTaiKhoan: string;
    userName: string;
    loaiTaiKhoan: string;
  };
  onSuccess?: () => void;
}

export const UpdateAccountModal: React.FC<UpdateAccountModalProps> = ({
  open,
  onClose,
  defaultValues,
  onSuccess,
}) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);
  const { handlers } = useQLHeThong(); // 🔥 Lấy handler trực tiếp như AddAdminAccountModal

  useEffect(() => {
    if (open) {
      form.setFieldsValue({
        userName: defaultValues.userName,
        password: "",
        loaiTaiKhoan: defaultValues.loaiTaiKhoan,
      });
    }
  }, [open, defaultValues, form]);

  const handleFinish = async (values: any) => {
    setSubmitting(true);
    try {
      const result = await handlers.updateAdminAccount({
        maTaiKhoan: defaultValues.maTaiKhoan,
        newUserName: values.userName.trim(),
        newPassword: values.password?.trim() || "",
        newLoaiTaiKhoan: values.loaiTaiKhoan,
      });
      setSubmitting(false);
      if (result?.success) {
        form.resetFields();
        onClose();
        if (onSuccess) onSuccess();
      }
    } catch (e) {
      setSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onCancel={onClose}
      footer={null}
      destroyOnClose
      maskClosable={false}
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-purple-500 to-pink-500 rounded-full mb-3">
            <UserSwitchOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Cập nhật tài khoản quản trị
          </Title>
        </div>
      }
    >
      <Form
        form={form}
        layout="vertical"
        onFinish={handleFinish}
        initialValues={{
          userName: defaultValues.userName,
          password: "",
          loaiTaiKhoan: defaultValues.loaiTaiKhoan,
        }}
      >
        <Form.Item
          label="Tên đăng nhập"
          name="userName"
          rules={[{ required: true, message: "Vui lòng nhập tên đăng nhập" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Mật khẩu mới (bỏ trống nếu không đổi)"
          name="password"
        >
          <Input.Password placeholder="Để trống nếu không đổi" />
        </Form.Item>
        <Form.Item
          label="Vai trò"
          name="loaiTaiKhoan"
          rules={[{ required: true, message: "Vui lòng chọn vai trò" }]}
        >
          <Select options={ROLE_OPTIONS_ADMIN} />
        </Form.Item>
        <Form.Item className="mb-0">
          <div style={{ display: "flex", justifyContent: "flex-end", gap: 8 }}>
            <Button onClick={onClose} disabled={submitting}>
              Hủy
            </Button>
            <Button type="primary" htmlType="submit" loading={submitting}>
              Lưu
            </Button>
          </div>
        </Form.Item>
      </Form>
    </Modal>
  );
};
