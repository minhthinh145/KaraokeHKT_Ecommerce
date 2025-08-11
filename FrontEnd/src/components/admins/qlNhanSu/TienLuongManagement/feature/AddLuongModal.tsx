import React, { useMemo } from "react";
import { Modal, Form, Select, InputNumber, DatePicker, Typography } from "antd";
import { useCaLamViec } from "../../../../../hooks/QLNhanSu/useCaLamViec";
import dayjs, { Dayjs } from "dayjs";
import isSameOrAfter from "dayjs/plugin/isSameOrAfter";
dayjs.extend(isSameOrAfter);

const { Text } = Typography;

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
    let start = parseTime(ca.gioBatDauCa);
    let end = parseTime(ca.gioKetThucCa);
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
      const ap: Dayjs = values.ngayApDung;
      const kt: Dayjs = values.ngayKetThuc;
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
          label="Ngày áp dụng"
          name="ngayApDung"
          rules={[{ required: true, message: "Chọn ngày áp dụng" }]}
        >
          <DatePicker
            style={{ width: "100%" }}
            disabled={disabledFields}
            format="YYYY-MM-DD"
          />
        </Form.Item>

        <Form.Item
          label="Ngày kết thúc"
          name="ngayKetThuc"
          dependencies={["ngayApDung"]}
          rules={[
            { required: true, message: "Chọn ngày kết thúc" },
            ({ getFieldValue }) => ({
              validator(_, value: Dayjs) {
                const start = getFieldValue("ngayApDung");
                if (!value || !start || value.isSameOrAfter(start, "day"))
                  return Promise.resolve();
                return Promise.reject(
                  new Error("Ngày kết thúc phải >= ngày áp dụng")
                );
              },
            }),
          ]}
        >
          <DatePicker
            style={{ width: "100%" }}
            disabled={disabledFields}
            format="YYYY-MM-DD"
          />
        </Form.Item>
      </Form>
    </Modal>
  );
};
