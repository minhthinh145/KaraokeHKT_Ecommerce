import React, { useMemo } from "react";
import { Modal, Form, Select, Typography, Alert } from "antd";
import type { AddLichLamViecDTO } from "../../../../../api";

const { Text } = Typography;

export interface NhanVienOption {
  value: string; // Guid
  label: string; // Tên
  description?: string; // loai, info phụ
}

interface AddLichLamViecModalProps {
  open: boolean;
  onClose: () => void;
  defaultNgay: string; // YYYY-MM-DD
  defaultMaCa: number;
  caTitle: string; // "Ca X - hh:mm → hh:mm"
  nhanVienOptions: NhanVienOption[];
  onSubmit: (
    payload: AddLichLamViecDTO
  ) => Promise<{ success: boolean } | void>;
  isDuplicate?: (maNhanVien: string) => boolean; // check trùng trong cell
  submitting?: boolean;
}

export const AddLichLamViecModal: React.FC<AddLichLamViecModalProps> = ({
  open,
  onClose,
  defaultNgay,
  defaultMaCa,
  caTitle,
  nhanVienOptions,
  onSubmit,
  isDuplicate,
  submitting = false,
}) => {
  const [form] = Form.useForm();

  const hasNV = useMemo(() => nhanVienOptions.length > 0, [nhanVienOptions]);

  // Map nhanh id -> option để hiện thông tin NV
  const optionMap = useMemo(
    () =>
      nhanVienOptions.reduce<Record<string, NhanVienOption>>((acc, cur) => {
        acc[cur.value] = cur;
        return acc;
      }, {}),
    [nhanVienOptions]
  );

  const selectedNV: string | undefined = Form.useWatch("maNhanVien", form);
  const selectedInfo = selectedNV ? optionMap[selectedNV] : undefined;

  const handleOk = async () => {
    try {
      const values = await form.validateFields();
      const payload: AddLichLamViecDTO = {
        ngayLamViec: defaultNgay,
        maNhanVien: values.maNhanVien,
        maCa: defaultMaCa,
      };
      await onSubmit(payload);
      form.resetFields();
      onClose();
    } catch {
      /* ignore */
    }
  };

  return (
    <Modal
      open={open}
      onCancel={() => {
        form.resetFields();
        onClose();
      }}
      onOk={handleOk}
      okText="Thêm"
      confirmLoading={submitting}
      title="Thêm lịch làm việc"
      destroyOnClose
    >
      <div className="mb-3">
        <Text strong>Ngày:</Text> <Text>{defaultNgay}</Text>
      </div>
      <div className="mb-3">
        <Text strong>Ca:</Text> <Text>{caTitle}</Text>
      </div>

      {!hasNV && (
        <Alert
          className="mb-3"
          type="warning"
          showIcon
          message="Danh sách nhân viên trống."
          description="Hãy kiểm tra dữ liệu Nhân viên trước khi tạo lịch."
        />
      )}

      <Form layout="vertical" form={form}>
        <Form.Item
          label="Nhân viên"
          name="maNhanVien"
          rules={[
            { required: true, message: "Chọn nhân viên" },
            {
              validator: (_, value) => {
                if (!value || !isDuplicate) return Promise.resolve();
                if (isDuplicate(value)) {
                  return Promise.reject(
                    new Error("Nhân viên đã có lịch ở ca và ngày này")
                  );
                }
                return Promise.resolve();
              },
            },
          ]}
        >
          <Select
            showSearch
            optionFilterProp="label"
            placeholder="Chọn nhân viên"
            options={nhanVienOptions}
          />
        </Form.Item>

        {selectedInfo && (
          <div className="bg-blue-50 border border-blue-200 rounded p-2">
            <div>
              <Text strong>Nhân viên:</Text> <Text>{selectedInfo.label}</Text>
            </div>
            {selectedInfo.description && (
              <div className="text-gray-600">
                <Text strong>Thông tin:</Text>{" "}
                <Text>{selectedInfo.description}</Text>
              </div>
            )}
          </div>
        )}
      </Form>
    </Modal>
  );
};
