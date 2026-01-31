import React, { useMemo } from "react";
import { Modal, Form, Select, InputNumber, Typography, DatePicker } from "antd";
import { useCaLamViec } from "../../../../../hooks/QLNhanSu/useCaLamViec";
import dayjs, { Dayjs } from "dayjs";
import isSameOrAfter from "dayjs/plugin/isSameOrAfter";
dayjs.extend(isSameOrAfter);

const { Text } = Typography;
const { RangePicker } = DatePicker;
interface AddLuongModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (form: {
    maCa: number;
    giaCa: number;
    ngayApDung: string;
    ngayKetThuc: string;
  }) => Promise<void>;
}

export const AddLuongModal: React.FC<AddLuongModalProps> = ({
  open,
  onClose,
  onSubmit,
}) => {
  const [form] = Form.useForm();
  const { caLamViecList, caOptions } = useCaLamViec();

  const maCa: number | undefined = Form.useWatch("maCa", form);
  const luong1h: number | undefined = Form.useWatch("luong1h", form);

  const soGio = useMemo(() => {
    if (!maCa) return 0;
    const ca = caLamViecList.find((c) => c.maCa === maCa);
    if (!ca) return 0;
    const parseTime = (t: string) => {
      const [h, m] = t.split(":").map(Number);
      return h + (m || 0) / 60;
    };
    const start = parseTime(ca.gioBatDauCa);
    const end = parseTime(ca.gioKetThucCa);
    let diff = end - start;
    if (diff <= 0) diff += 24;
    return diff;
  }, [maCa, caLamViecList]);

  const giaCa = useMemo(() => {
    if (!luong1h || !soGio) return 0;
    return Math.round(luong1h * soGio);
  }, [luong1h, soGio]);

  const disabledFields = !maCa;

  const handleOk = async () => {
    try {
      const values = await form.validateFields();
      const [ap, kt] = values.range;

      await onSubmit({
        maCa: values.maCa,
        giaCa,
        ngayApDung: ap.format("YYYY-MM-DD"),
        ngayKetThuc: kt.format("YYYY-MM-DD"),
      });
      form.resetFields();
    } catch {
      /* ignore */
    }
  };

  return (
    <Modal
      open={open}
      title="Thêm lương ngày đặc biệt"
      onCancel={() => {
        form.resetFields();
        onClose();
      }}
      onOk={handleOk}
      okText="Thêm"
      destroyOnClose
    >
      <Form layout="vertical" form={form} initialValues={{}}>
        <Form.Item
          label="Ca làm việc"
          name="maCa"
          rules={[{ required: true, message: "Chọn ca" }]}
        >
          <Select
            placeholder="Chọn ca"
            options={caOptions}
            showSearch
            optionFilterProp="label"
          />
        </Form.Item>

        <Form.Item
          label="Lương 1 giờ (VNĐ)"
          name="luong1h"
          rules={[{ required: true, message: "Nhập lương 1 giờ" }]}
        >
          <InputNumber<number>
            style={{ width: "100%" }}
            min={0}
            disabled={disabledFields}
            formatter={(v) =>
              v ? `${v}`.replace(/\B(?=(\d{3})+(?!\d))/g, ".") : ""
            }
            parser={(v) => (v && v.length ? Number(v.replace(/\./g, "")) : 0)}
            placeholder="Nhập lương theo giờ"
          />
        </Form.Item>

        <Form.Item label="Lương ca (VNĐ)">
          <InputNumber<number>
            style={{ width: "100%" }}
            value={giaCa}
            readOnly
            disabled
            formatter={(v) =>
              v ? `${v}`.replace(/\B(?=(\d{3})+(?!\d))/g, ".") : ""
            }
          />
          <Text type="secondary">
            = Lương 1 giờ × số giờ ca ({soGio || "?"}h)
          </Text>
        </Form.Item>

        <Form.Item
          label="Khoảng ngày áp dụng"
          name="range"
          rules={[
            {
              required: true,
              message: "Chọn khoảng ngày áp dụng!",
              type: "array",
            },
            {
              validator: (_, value: [Dayjs, Dayjs]) => {
                if (!value || value.length !== 2)
                  return Promise.reject("Chọn đủ 2 ngày!");
                if (value[1].isSameOrAfter(value[0], "day"))
                  return Promise.resolve();
                return Promise.reject("Ngày kết thúc phải >= ngày áp dụng");
              },
            },
          ]}
        >
          <RangePicker
            style={{ width: "100%" }}
            disabled={disabledFields}
            format="YYYY-MM-DD"
          />
        </Form.Item>
      </Form>
    </Modal>
  );
};
