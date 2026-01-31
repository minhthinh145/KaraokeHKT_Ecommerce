import React, { useEffect } from "react";
import { Modal, Form, Select, Button, Typography, Alert } from "antd";
import {
  EditOutlined,
  TeamOutlined,
  FieldTimeOutlined,
} from "@ant-design/icons";
import dayjs from "dayjs";
import { useLichLamViec } from "../../../../../hooks/QLNhanSu/useLichLamViec";
import { useCaLamViec } from "../../../../../hooks/QLNhanSu/useCaLamViec";
import { useNhanVien } from "../../../../../hooks/QLNhanSu/useNhanVien";
import { mapLoaiNhanVienToLabel } from "../../../../../api/types/admins/QLNhanSutypes";

const { Title, Text } = Typography;

export const EditLichLamViecModal: React.FC = () => {
  const [form] = Form.useForm();

  const { showEdit, closeEditModalAction, current, update, loading } =
    useLichLamViec();

  const { caLamViecList } = useCaLamViec();
  const { nhanVienData } = useNhanVien();

  // Lấy thông tin nhân viên hiện tại
  const nhanVien = nhanVienData.find((nv) => nv.maNv === current?.maNhanVien);

  const caOptions = (caLamViecList || []).map((ca) => ({
    label: `${ca.tenCa} (${ca.gioBatDauCa} → ${ca.gioKetThucCa})`,
    value: ca.maCa,
  }));

  const nhanVienOptions = (nhanVienData || []).map((nv) => ({
    label: `${nv.hoTen || nv.hoTen || nv.maNv} - ${mapLoaiNhanVienToLabel(
      nv.loaiNhanVien
    )}`,
    value: nv.maNv,
  }));

  useEffect(() => {
    if (showEdit && current) {
      form.setFieldsValue({
        maNhanVien: current.maNhanVien,
        maCa: current.maCa,
      });
    } else {
      form.resetFields();
    }
  }, [showEdit, current, form]);

  const handleSubmit = async () => {
    const values = await form.validateFields();
    if (!current) return;
    const payload = {
      ...current,
      maNhanVien: values.maNhanVien,
      maCa: values.maCa,
    };
    const res = await update(payload);
    if (res.success) {
      closeEditModalAction();
    }
  };

  return (
    <Modal
      open={showEdit}
      onCancel={closeEditModalAction}
      footer={null}
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full mb-3">
            <EditOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Cập nhật lịch làm việc
          </Title>
        </div>
      }
      width={600}
      destroyOnClose
    >
      <Alert
        type="info"
        showIcon
        className="mb-6 rounded-lg border-blue-200 bg-blue-50"
        message={
          <div className="text-base">
            <Text strong className="text-blue-800">
              Thông tin đang sửa
            </Text>
            <div className="mt-1 text-blue-700">
              • Ngày:{" "}
              <Text strong>
                {current
                  ? dayjs(current.ngayLamViec).format("DD/MM/YYYY")
                  : "--"}
              </Text>
              <br />• Mã lịch: <Text strong>{current?.maLichLamViec}</Text>
              <br />• Mã nhân viên:{" "}
              <Text strong>{nhanVien?.maNv || current?.maNhanVien}</Text>
              <br />• Tên nhân viên:{" "}
              <Text strong>{nhanVien?.hoTen || nhanVien?.hoTen || "--"}</Text>
              <br />• Loại nhân viên:{" "}
              <Text strong>
                {mapLoaiNhanVienToLabel(nhanVien?.loaiNhanVien)}
              </Text>
            </div>
          </div>
        }
      />
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item
          label={
            <span className="text-base font-semibold text-slate-700 flex items-center">
              <TeamOutlined className="mr-2 text-blue-600" />
              Nhân viên
            </span>
          }
          name="maNhanVien"
          rules={[{ required: true, message: "Vui lòng chọn nhân viên!" }]}
        >
          <Select
            size="large"
            placeholder="Chọn nhân viên"
            className="rounded-lg"
            showSearch
            optionFilterProp="label"
            options={nhanVienOptions}
          />
        </Form.Item>
        <Form.Item
          label={
            <span className="text-base font-semibold text-slate-700 flex items-center">
              <FieldTimeOutlined className="mr-2 text-indigo-600" />
              Ca làm việc
            </span>
          }
          name="maCa"
          rules={[{ required: true, message: "Vui lòng chọn ca!" }]}
        >
          <Select
            size="large"
            placeholder="Chọn ca làm việc"
            className="rounded-lg"
            showSearch
            optionFilterProp="label"
            options={caOptions}
          />
        </Form.Item>
        <div className="flex justify-end pt-4 border-t border-slate-200">
          <Button
            size="large"
            onClick={closeEditModalAction}
            className="px-8 rounded-lg font-medium mr-2"
          >
            Hủy
          </Button>
          <Button
            type="primary"
            size="large"
            loading={loading}
            htmlType="submit"
            icon={<EditOutlined />}
            className="px-8 rounded-lg font-medium bg-gradient-to-r from-blue-500 to-indigo-500 border-none hover:from-blue-600 hover:to-indigo-600 shadow-lg hover:shadow-xl transition-all duration-300"
          >
            Lưu thay đổi
          </Button>
        </div>
      </Form>
    </Modal>
  );
};

export default EditLichLamViecModal;
