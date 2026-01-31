import React, { useState } from "react";
import {
  Modal,
  Form,
  Input,
  InputNumber,
  Select,
  Typography,
  Switch,
  Divider,
} from "antd";
import dayjs from "dayjs";
import type { AddVatLieuDTO } from "../../../../../api/index";
import { PlusOutlined } from "@ant-design/icons";

const { Title } = Typography;
const DVT_OPTIONS = ["Lon", "Chai", "Bịch", "Hộp", "Hủ"];

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (payload: AddVatLieuDTO) => Promise<{ success: boolean } | void>;
}

export const CreateVatLieuModal: React.FC<Props> = ({
  open,
  onClose,
  onSubmit,
}) => {
  const [form] = Form.useForm();
  const [imgUrl, setImgUrl] = useState<string>("");
  const [dongGiaAllCa, setDongGiaAllCa] = useState<boolean>(true);

  const isImageUrl = (url?: string) =>
    !!url && /\.(png|jpe?g|gif|webp|bmp|svg)$/i.test(url.split("?")[0]);

  const handleValuesChange = (changed: any) => {
    if ("hinhAnhSanPham" in changed) setImgUrl(changed.hinhAnhSanPham || "");
  };

  const todayISO = dayjs().format("YYYY-MM-DD");
  const todayLabel = dayjs().format("DD/MM/YYYY");

  const vndFormatter = (val?: string | number) => {
    if (val == null || val === "") return "";
    const n =
      typeof val === "number" ? val : Number(String(val).replace(/[^\d]/g, ""));
    if (Number.isNaN(n)) return "";
    return n.toLocaleString("vi-VN") + " ₫";
  };
  const vndParser = (v?: string) =>
    v ? Number(v.replace(/\./g, "").replace(/[^\d]/g, "")) : 0;

  return (
    <Modal
      open={open}
      onCancel={onClose}
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full mb-3">
            <PlusOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Thêm vật tư
          </Title>
        </div>
      }
      onOk={() => form.submit()}
    >
      <Form
        form={form}
        layout="vertical"
        onValuesChange={handleValuesChange}
        onFinish={(v) => {
          const payload = {
            tenVatLieu: v.tenVatLieu,
            donViTinh: v.donViTinh,
            soLuongTonKho: 0,
            tenSanPham: v.tenSanPham,
            hinhAnhSanPham: v.hinhAnhSanPham || null,
            giaNhap: Number(v.giaNhap ?? 0),
            ngayApDungGiaNhap: todayISO,
            trangThaiGiaNhap: "HieuLuc",
            dongGiaAllCa,
            giaBanChung: dongGiaAllCa ? Number(v.giaBanChung ?? 0) : null,
            giaBanCa1: !dongGiaAllCa ? Number(v.giaBanCa1 ?? 0) : null,
            giaBanCa2: !dongGiaAllCa ? Number(v.giaBanCa2 ?? 0) : null,
            giaBanCa3: !dongGiaAllCa ? Number(v.giaBanCa3 ?? 0) : null,
            ngayApDungGiaBan: todayISO,
            trangThaiGiaBan: "HieuLuc",
          };
          onSubmit(payload as any);
        }}
        initialValues={{
          giaNhap: 0,
          giaBanChung: 0,
          giaBanCa1: 0,
          giaBanCa2: 0,
          giaBanCa3: 0,
        }}
      >
        <Form.Item
          label="Tên vật tư"
          name="tenVatLieu"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Đơn vị tính"
          name="donViTinh"
          rules={[{ required: true }]}
        >
          <Select
            placeholder="Chọn đơn vị"
            options={DVT_OPTIONS.map((v) => ({ value: v, label: v }))}
          />
        </Form.Item>

        <Form.Item
          label="Tên sản phẩm"
          name="tenSanPham"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>

        <Form.Item label="Hình ảnh (URL)" name="hinhAnhSanPham">
          <Input placeholder="Dán URL hình ảnh (định dạng .jpg/.png/...)" />
        </Form.Item>

        {imgUrl && !isImageUrl(imgUrl) && (
          <div className="mb-2 text-xs text-amber-700 bg-amber-50 border border-amber-200 rounded px-2 py-1">
            URL này không phải là ảnh trực tiếp. Vui lòng dán link đuôi
            .jpg/.png/.webp... Ví dụ: nhấn chuột phải vào ảnh và chọn “Sao chép
            địa chỉ hình ảnh”.
          </div>
        )}

        {imgUrl && isImageUrl(imgUrl) && (
          <div className="mb-3 flex justify-center items-center">
            <img
              src={imgUrl}
              alt={form.getFieldValue("tenSanPham") || "Preview"}
              style={{
                maxWidth: 180,
                maxHeight: 180,
                borderRadius: 8,
                border: "1px solid #e5e7eb",
                background: "#f8fafc",
                objectFit: "contain",
              }}
              onError={(e) => (e.currentTarget.style.display = "none")}
            />
          </div>
        )}

        {/* Giá nhập và áp dụng giá nhập: căn hàng ngang giống Edit */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
          <div>
            <Form.Item
              label="Giá nhập"
              name="giaNhap"
              rules={[
                { required: true, message: "Nhập giá nhập" },
                { type: "number", min: 0, message: "Giá nhập phải lớn hơn 0" },
              ]}
              initialValue={0}
            >
              <InputNumber
                min={0}
                step={10000}
                className="w-full"
                style={{ width: "100%" }}
                formatter={vndFormatter}
                parser={vndParser}
              />
            </Form.Item>
          </div>
          <div>
            <Form.Item label="Áp dụng giá nhập">
              <Input value={todayLabel} disabled />
            </Form.Item>
          </div>
        </div>

        <Divider className="!my-3">Giá bán</Divider>
        <div className="flex items-center gap-2 mb-2">
          <Switch checked={dongGiaAllCa} onChange={setDongGiaAllCa} />
          <span className="text-sm">
            {dongGiaAllCa
              ? "Đồng giá cho tất cả ca"
              : "Thiết lập giá theo từng ca"}
          </span>
        </div>

        {dongGiaAllCa ? (
          <Form.Item
            label="Giá bán chung"
            name="giaBanChung"
            rules={[
              { required: true, message: "Nhập giá bán" },
              { type: "number", min: 0 },
            ]}
          >
            <InputNumber
              min={0}
              step={10000}
              className="w-full"
              formatter={vndFormatter}
              parser={vndParser}
            />
          </Form.Item>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
            <Form.Item
              label="Giá bán Ca 1"
              name="giaBanCa1"
              rules={[{ required: true }, { type: "number", min: 0 }]}
            >
              <InputNumber
                min={0}
                step={10000}
                className="w-full"
                formatter={vndFormatter}
                parser={vndParser}
              />
            </Form.Item>
            <Form.Item
              label="Giá bán Ca 2"
              name="giaBanCa2"
              rules={[{ required: true }, { type: "number", min: 0 }]}
            >
              <InputNumber
                min={0}
                step={10000}
                className="w-full"
                formatter={vndFormatter}
                parser={vndParser}
              />
            </Form.Item>
            <Form.Item
              label="Giá bán Ca 3"
              name="giaBanCa3"
              rules={[{ required: true }, { type: "number", min: 0 }]}
            >
              <InputNumber
                min={0}
                step={10000}
                className="w-full"
                formatter={vndFormatter}
                parser={vndParser}
              />
            </Form.Item>
          </div>
        )}
        {/* Áp dụng ngày giá bán */}
        <Form.Item label="Áp dụng giá bán">
          <Input value={todayLabel} disabled />
        </Form.Item>
      </Form>
    </Modal>
  );
};
