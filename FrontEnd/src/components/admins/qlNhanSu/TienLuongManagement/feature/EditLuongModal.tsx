import React, { useEffect, useState } from "react";
import {
  Modal,
  Form,
  InputNumber,
  Button,
  Typography,
  Alert,
  DatePicker,
  Select,
} from "antd";
import {
  EditOutlined,
  CalendarOutlined,
  DollarOutlined,
  ClockCircleOutlined,
} from "@ant-design/icons";
import dayjs from "dayjs";
import { useTienLuong } from "../../../../../hooks/QLNhanSu/useTienLuong";
import type { LuongCaLamViecDTO } from "../../../../../api";
import { useCaLamViec } from "../../../../../hooks/QLNhanSu/useCaLamViec";

const { Title, Text } = Typography;
const { RangePicker } = DatePicker;

interface EditLuongModalProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
  luong: LuongCaLamViecDTO | null;
}

export const EditLuongModal: React.FC<EditLuongModalProps> = ({
  open,
  onClose,
  onSuccess,
  luong,
}) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);
  const { updateTienLuong } = useTienLuong();
  const { caLamViecList, loading: loadingCa } = useCaLamViec();

  // State cho tiền/h và tổng tiền
  const [tienMotGio, setTienMotGio] = useState<number | undefined>(undefined);
  const [tongTien, setTongTien] = useState<number>(luong?.giaCa || 0);

  // Khi chọn ca, tự động cập nhật tổng tiền nếu đã nhập tiền/h
  const handleCaChange = (maCa: number) => {
    const ca = caLamViecList.find((c) => c.maCa === maCa);
    const gioBatDau = ca?.gioBatDauCa ? dayjs(ca.gioBatDauCa, "HH:mm") : null;
    const gioKetThuc = ca?.gioKetThucCa
      ? dayjs(ca.gioKetThucCa, "HH:mm")
      : null;
    if (gioBatDau && gioKetThuc && tienMotGio) {
      let soGio = gioKetThuc.diff(gioBatDau, "hour", true);
      if (soGio < 0) soGio += 24; // qua ngày
      setTongTien(Math.round(soGio * tienMotGio));
      form.setFieldsValue({ giaCa: Math.round(soGio * tienMotGio) });
    }
  };

  // Khi nhập tiền/h, tự động cập nhật tổng tiền nếu đã chọn ca
  const handleTienMotGioChange = (value: number | null) => {
    setTienMotGio(value || undefined);
    const maCa = form.getFieldValue("maCa");
    const ca = caLamViecList.find((c) => c.maCa === maCa);
    const gioBatDau = ca?.gioBatDauCa ? dayjs(ca.gioBatDauCa, "HH:mm") : null;
    const gioKetThuc = ca?.gioKetThucCa
      ? dayjs(ca.gioKetThucCa, "HH:mm")
      : null;
    if (gioBatDau && gioKetThuc && value) {
      let soGio = gioKetThuc.diff(gioBatDau, "hour", true);
      if (soGio < 0) soGio += 24;
      setTongTien(Math.round(soGio * value));
      form.setFieldsValue({ giaCa: Math.round(soGio * value) });
    }
  };

  // Khi mở modal hoặc khi chọn ca:
  useEffect(() => {
    if (open && luong) {
      // Tìm ca hiện tại
      const ca = caLamViecList.find((c) => c.maCa === luong.maCa);

      let tienMotGio = undefined;
      if (ca && luong.giaCa) {
        const gioBatDau = dayjs(ca.gioBatDauCa, "HH:mm");
        const gioKetThuc = dayjs(ca.gioKetThucCa, "HH:mm");
        let soGio = gioKetThuc.diff(gioBatDau, "hour", true);
        if (soGio < 0) soGio += 24;
        tienMotGio = Math.round(luong.giaCa / soGio);
      }
      setTienMotGio(tienMotGio);
      setTongTien(luong.giaCa || 0);
      form.setFieldsValue({
        maCa: luong.maCa,
        tienMotGio,
        giaCa: luong.giaCa,
        range:
          luong.ngayApDung && luong.ngayKetThuc
            ? [dayjs(luong.ngayApDung), dayjs(luong.ngayKetThuc)]
            : [null, null],
      });
    }
  }, [open, luong, caLamViecList, form]);
  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setSubmitting(true);
      const ca = caLamViecList.find((c) => c.maCa === values.maCa);

      const updateData: LuongCaLamViecDTO = {
        ...luong!,
        maCa: values.maCa,
        tenCaLamViec: ca?.tenCa || "",
        giaCa: values.giaCa,
        ngayApDung: values.range?.[0]?.format("YYYY-MM-DD") || null,
        ngayKetThuc: values.range?.[1]?.format("YYYY-MM-DD") || null,
      };

      const result = await updateTienLuong(updateData);

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
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full mb-3">
            <EditOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Cập nhật lương đặc biệt
          </Title>
        </div>
      }
      open={open}
      onCancel={handleClose}
      footer={null}
      width={600}
      className="top-8"
      bodyStyle={{ padding: "24px 32px" }}
      destroyOnClose
    >
      <Alert
        type="info"
        showIcon
        className="mb-6 rounded-lg border-blue-200 bg-blue-50"
        message={
          <div className="text-base">
            <Text strong className="text-blue-800">
              Chỉnh sửa ca làm việc, tiền/h và ngày áp dụng/kết thúc.
            </Text>
            <div className="mt-1 text-blue-700">
              • Mã lương: <Text strong>{luong?.maLuongCaLamViec}</Text>
            </div>
          </div>
        }
      />

      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        {/* Hàng 1: Pick ca làm việc */}
        <Form.Item
          label="Chọn ca làm việc"
          name="maCa"
          rules={[{ required: true, message: "Vui lòng chọn ca!" }]}
        >
          <Select
            size="large"
            loading={loadingCa}
            placeholder="Chọn ca làm việc"
            onChange={handleCaChange}
            showSearch
            filterOption={(input, option) => {
              const label = option?.label ?? "";
              return label
                .toString()
                .toLowerCase()
                .includes(input.toLowerCase());
            }}
          >
            {caLamViecList.map((ca) => (
              <Select.Option key={ca.maCa} value={ca.maCa}>
                {ca.tenCa} ({ca.gioBatDauCa} - {ca.gioKetThucCa})
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        {/* Hàng 2: Tiền/giờ và tổng tiền ca */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <Form.Item
            label={
              <span className="text-base font-semibold text-slate-700 flex items-center">
                <ClockCircleOutlined className="mr-2 text-green-600" />
                Tiền / giờ (VNĐ)
              </span>
            }
            name="tienMotGio"
            rules={[{ required: true, message: "Vui lòng nhập tiền/giờ!" }]}
          >
            <InputNumber<number>
              size="large"
              min={0}
              step={1000}
              className="rounded-lg w-full"
              style={{ width: "auto" }}
              formatter={(value) =>
                `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")
              }
              parser={(value) => (value ? Number(value.replace(/,/g, "")) : 0)}
              value={luong?.giaCa}
              onChange={handleTienMotGioChange}
              prefix={<DollarOutlined />}
            />
          </Form.Item>
          <Form.Item
            label={
              <span className="text-base font-semibold text-slate-700 flex items-center">
                <DollarOutlined className="mr-2 text-green-600" />
                Tổng tiền ca (VNĐ)
              </span>
            }
            name="giaCa"
            rules={[{ required: true, message: "Vui lòng nhập tổng tiền ca!" }]}
          >
            <InputNumber<number>
              size="large"
              min={0}
              step={1000}
              className="rounded-lg w-full "
              style={{ width: "auto" }}
              formatter={(value) =>
                `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")
              }
              parser={(value) => (value ? Number(value.replace(/,/g, "")) : 0)}
              prefix={<DollarOutlined />}
              disabled
              value={tongTien}
            />
          </Form.Item>
        </div>

        {/* Hàng 3: Pick time range */}
        <Form.Item
          label={
            <span className="text-base font-semibold text-slate-700 flex items-center">
              <CalendarOutlined className="mr-2 text-blue-600" />
              Ngày áp dụng & kết thúc
            </span>
          }
          name="range"
          rules={[
            {
              required: true,
              message: "Chọn khoảng ngày áp dụng!",
              type: "array",
            },
          ]}
        >
          <RangePicker
            size="large"
            className="rounded-lg w-full"
            format="DD/MM/YYYY"
            allowClear
          />
        </Form.Item>
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
            className="px-8 rounded-lg font-medium bg-gradient-to-r from-blue-500 to-indigo-500 border-none hover:from-blue-600 hover:to-indigo-600 shadow-lg hover:shadow-xl transition-all duration-300"
          >
            {submitting ? "Đang cập nhật..." : "Cập nhật"}
          </Button>
        </div>
      </Form>
    </Modal>
  );
};

export default EditLuongModal;
