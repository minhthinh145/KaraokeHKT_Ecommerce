import React, { useState, useEffect } from "react";
import {
  Modal,
  Form,
  Select,
  Button,
  message,
  Spin,
  Input,
  Alert,
  Card,
  Typography,
} from "antd";
import { UserAddOutlined, MailOutlined, TeamOutlined } from "@ant-design/icons";
import {
  getEmployeesWithoutAccounts,
  addTaiKhoanNhanVien,
  type NhanVienDTO,
  type AddTaiKhoanForNhanVienDTO,
} from "../../../../../api/services/shared";

const { Option } = Select;
const { Title, Text } = Typography;

interface AddAccountModalProps {
  visible: boolean;
  onCancel: () => void;
  onSuccess: () => void;
}

export const AddAccountModal: React.FC<AddAccountModalProps> = ({
  visible,
  onCancel,
  onSuccess,
}) => {
  const [form] = Form.useForm();
  const [employees, setEmployees] = useState<NhanVienDTO[]>([]);
  const [selectedEmployee, setSelectedEmployee] = useState<string>("");
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (visible) {
      loadEmployeesWithoutAccounts();
      form.resetFields();
      setSelectedEmployee("");
    }
  }, [visible, form]);

  const loadEmployeesWithoutAccounts = async () => {
    try {
      setLoading(true);
      const response = await getEmployeesWithoutAccounts();
      if (response.isSuccess) {
        setEmployees(response.data || []);
      } else {
        message.error(response.message);
      }
    } catch (error) {
      message.error("Có lỗi xảy ra khi lấy danh sách nhân viên");
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setSubmitting(true);

      const addData: AddTaiKhoanForNhanVienDTO = {
        maNhanVien: values.maNhanVien,
        email: values.email,
      };

      const response = await addTaiKhoanNhanVien(addData);

      if (response.isSuccess) {
        message.success("🎉 Tạo tài khoản thành công!");
        form.resetFields();
        setSelectedEmployee("");
        onSuccess();
      } else {
        message.error(response.message);
      }
    } catch (error) {
      message.error("❌ Có lỗi xảy ra khi tạo tài khoản!");
    } finally {
      setSubmitting(false);
    }
  };

  const handleEmployeeChange = (maNv: string) => {
    setSelectedEmployee(maNv);
    const employee = employees.find((emp) => emp.maNv === maNv);
    if (employee) {
      form.setFieldsValue({
        email: employee.email,
      });
    }
  };

  return (
    <Modal
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-500 to-purple-500 rounded-full mb-3">
            <UserAddOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Thay đổi tài khoản cho nhân viên
          </Title>{" "}
        </div>
      }
      open={visible}
      onCancel={onCancel}
      footer={null}
      width={700}
      className="top-12"
      bodyStyle={{ padding: "24px 32px" }}
    >
      <Spin spinning={loading}>
        {/* Alert hướng dẫn */}
        <Alert
          type="info"
          showIcon
          className="mb-6 rounded-lg border-blue-200 bg-blue-50"
          message={
            <div className="text-base">
              <Text strong className="text-blue-800">
                💡 Lưu ý quan trọng:
              </Text>
              <div className="mt-1 text-blue-700">
                • Bạn có thể <Text strong>chỉnh sửa email</Text> khác với email
                gốc của nhân viên
                <br />• <Text strong>Mật khẩu tự động</Text> sẽ được gửi tới
                email này
                <br />• Nhân viên sẽ nhận được thông tin đăng nhập qua email
              </div>
            </div>
          }
        />

        <Form
          form={form}
          layout="vertical"
          onFinish={handleSubmit}
          className="space-y-6"
        >
          {/* Chọn nhân viên */}
          <Card className="bg-gradient-to-r from-purple-50 to-blue-50 border-purple-200 shadow-sm">
            <Form.Item
              label={
                <span className="text-lg font-semibold text-slate-700 flex items-center">
                  <TeamOutlined className="mr-2 text-purple-600" />
                  Chọn nhân viên
                </span>
              }
              name="maNhanVien"
              rules={[{ required: true, message: "Vui lòng chọn nhân viên!" }]}
            >
              <Select
                size="large"
                placeholder="🔍 Tìm và chọn nhân viên cần tạo tài khoản"
                onChange={handleEmployeeChange}
                showSearch
                optionFilterProp="label"
                className="rounded-lg"
                filterOption={(input, option) =>
                  (option?.label as string)
                    ?.toLowerCase()
                    .includes(input.toLowerCase())
                }
              >
                {employees.map((employee) => (
                  <Option
                    key={employee.maNv}
                    value={employee.maNv}
                    label={`${employee.hoTen} - ${employee.loaiNhanVien}`}
                  >
                    <span className="font-medium">
                      {employee.hoTen} - {employee.loaiNhanVien}
                    </span>
                  </Option>
                ))}
              </Select>
            </Form.Item>
          </Card>

          {/* Email */}
          <Card className="bg-gradient-to-r from-green-50 to-emerald-50 border-green-200 shadow-sm">
            <Form.Item
              label={
                <span className="text-lg font-semibold text-slate-700 flex items-center">
                  <MailOutlined className="mr-2 text-green-600" />
                  Email đăng nhập
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
                placeholder="📧 Nhập email đăng nhập (có thể khác email gốc)"
                disabled={!selectedEmployee}
                className="rounded-lg"
                prefix={<MailOutlined className="text-slate-400" />}
              />
            </Form.Item>
          </Card>

          {/* Buttons */}
          <div className="flex justify-end space-x-3 pt-4 border-t border-slate-200">
            <Button
              size="large"
              onClick={onCancel}
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
              className="px-8 rounded-lg font-medium bg-gradient-to-r from-blue-500 to-purple-500 border-none hover:from-blue-600 hover:to-purple-600 shadow-lg hover:shadow-xl transition-all duration-300"
            >
              {submitting ? "Đang tạo..." : "Thay đổi"}
            </Button>
          </div>
        </Form>
      </Spin>
    </Modal>
  );
};

export default AddAccountModal;
