import React, { useEffect, useState } from "react";
import {
  Modal,
  Form,
  Input,
  InputNumber,
  Select,
  Checkbox,
  Typography,
  Switch,
  Divider,
} from "antd";
import dayjs from "dayjs";
import { EditOutlined } from "@ant-design/icons";
import type { VatLieuDetailDTO, UpdateVatLieuDTO } from "../../../../../api";

const { Title } = Typography;
const DVT_OPTIONS = ["Lon", "Chai", "Bịch", "Hộp", "Hủ"];

interface Props {
  open: boolean;
  row?: VatLieuDetailDTO | null;
  onClose: () => void;
  onSubmit: (payload: UpdateVatLieuDTO) => Promise<{ success: boolean } | void>;
}

export const EditVatLieuModal: React.FC<Props> = ({
  open,
  row,
  onClose,
  onSubmit,
}) => {
  const [form] = Form.useForm();
  const [imgUrl, setImgUrl] = useState<string>("");
  const [updNhap, setUpdNhap] = useState(false);
  const [updBan, setUpdBan] = useState(false);
  const [capNhatGiaBan, setCapNhatGiaBan] = useState<boolean>(false);
  const [dongGiaAllCa, setDongGiaAllCa] = useState<boolean>(true);

  useEffect(() => {
    if (row) {
      form.setFieldsValue({
        tenVatLieu: row.tenVatLieu,
        donViTinh: row.donViTinh,
        tenSanPham: row.tenSanPham || "",
        hinhAnhSanPham: row.hinhAnhSanPham || "",
        giaNhapMoi: row.giaNhapHienTai ?? undefined,
        giaBanMoi: row.giaBanHienTai ?? undefined,
      });
      setImgUrl(row.hinhAnhSanPham || "");
      setUpdNhap(false);
      setUpdBan(false);
    }
  }, [row]);

  useEffect(() => {
    // Khi mở modal, set lại theo row (nếu backend trả các field này)
    if (row) {
      if (row.dongGiaAllCa !== undefined && row.dongGiaAllCa !== null)
        setDongGiaAllCa(!!row.dongGiaAllCa);
    }
  }, [row]);

  const vndFormatter = (val?: string | number) => {
    if (val == null || val === "") return "";
    const n =
      typeof val === "number" ? val : Number(String(val).replace(/[^\d]/g, ""));
    if (Number.isNaN(n)) return "";
    return n.toLocaleString("vi-VN") + " ₫";
  };
  const vndParser = (v?: string) =>
    v ? Number(v.replace(/\./g, "").replace(/[^\d]/g, "")) : 0;
  const todayISO = dayjs().format("YYYY-MM-DD");
  const todayLabel = dayjs().format("DD/MM/YYYY");

  const isImageUrl = (url?: string) =>
    !!url && /\.(png|jpe?g|gif|webp|bmp|svg)$/i.test(url.split("?")[0]);

  const submit = (v: any) => {
    const payload: UpdateVatLieuDTO = {
      maVatLieu: row!.maVatLieu,
      tenVatLieu: v.tenVatLieu,
      donViTinh: v.donViTinh,
      tenSanPham: v.tenSanPham,
      hinhAnhSanPham: v.hinhAnhSanPham || null,
      giaNhapMoi: v.capNhatGiaNhap ? Number(v.giaNhapMoi ?? 0) : undefined,
      ngayApDungGiaNhap: v.capNhatGiaNhap ? todayISO : undefined,
      trangThaiGiaNhap: v.capNhatGiaNhap ? "HieuLuc" : undefined,
      capNhatGiaBan: capNhatGiaBan || undefined,
      dongGiaAllCa: capNhatGiaBan ? dongGiaAllCa : undefined,
      giaBanChung:
        capNhatGiaBan && dongGiaAllCa ? Number(v.giaBanChung ?? 0) : undefined,
      giaBanCa1:
        capNhatGiaBan && !dongGiaAllCa ? Number(v.giaBanCa1 ?? 0) : undefined,
      giaBanCa2:
        capNhatGiaBan && !dongGiaAllCa ? Number(v.giaBanCa2 ?? 0) : undefined,
      giaBanCa3:
        capNhatGiaBan && !dongGiaAllCa ? Number(v.giaBanCa3 ?? 0) : undefined,
      ngayApDungGiaBan: capNhatGiaBan ? todayISO : undefined,
      trangThaiGiaBan: capNhatGiaBan ? "HieuLuc" : undefined,
    };
    onSubmit(payload);
  };

  return (
    <Modal
      open={open}
      onCancel={onClose}
      onOk={() => form.submit()}
      destroyOnClose
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full mb-3">
            <EditOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Cập nhật vật tư
          </Title>
        </div>
      }
    >
      <Form
        form={form}
        layout="vertical"
        onValuesChange={(changed) => {
          if ("hinhAnhSanPham" in changed)
            setImgUrl(changed.hinhAnhSanPham || "");
        }}
        onFinish={submit}
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
          <Select options={DVT_OPTIONS.map((v) => ({ value: v, label: v }))} />
        </Form.Item>

        <Form.Item
          label="Tên sản phẩm"
          name="tenSanPham"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>

        <Form.Item label="Hình ảnh (URL)" name="hinhAnhSanPham">
          <Input placeholder="Dán URL ảnh (.jpg/.png/...)" />
        </Form.Item>

        {imgUrl && (
          <div className="mb-3 flex justify-center items-center">
            {isImageUrl(imgUrl) ? (
              <img
                src={imgUrl}
                alt="Preview"
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
            ) : (
              <span className="text-xs text-amber-600">
                URL không phải ảnh trực tiếp
              </span>
            )}
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
          <div>
            <Checkbox
              checked={updNhap}
              onChange={(e) => setUpdNhap(e.target.checked)}
            >
              Cập nhật giá nhập
            </Checkbox>
            <Form.Item
              name="giaNhapMoi"
              rules={[{ required: updNhap, message: "Nhập giá nhập" }]}
            >
              <InputNumber
                min={0}
                step={10000}
                className="w-full"
                style={{ width: "100%" }}
                disabled={!updNhap}
                formatter={vndFormatter}
                parser={vndParser}
              />
            </Form.Item>
          </div>
        </div>

        <Divider className="!my-3">Giá bán</Divider>
        <div className="flex items-center gap-4 mb-3">
          <Switch
            checked={capNhatGiaBan}
            onChange={(val) => setCapNhatGiaBan(val)}
          />
          <span className="text-sm">
            {capNhatGiaBan ? "Đang cập nhật giá bán" : "Không cập nhật giá bán"}
          </span>
        </div>

        {capNhatGiaBan && (
          <>
            <div className="flex items-center gap-2 mb-2">
              <Switch checked={dongGiaAllCa} onChange={setDongGiaAllCa} />
              <span className="text-sm">
                {dongGiaAllCa
                  ? "Đồng giá cho tất cả ca"
                  : "Giá bán theo từng ca"}
              </span>
            </div>
            {dongGiaAllCa ? (
              <Form.Item
                label="Giá bán chung"
                name="giaBanChung"
                rules={[{ required: true }, { type: "number", min: 0 }]}
                initialValue={row?.giaBanChung ?? 0}
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
                  label="Giá Ca 1"
                  name="giaBanCa1"
                  rules={[{ required: true }, { type: "number", min: 0 }]}
                  initialValue={row?.giaBanCa1 ?? 0}
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
                  label="Giá Ca 2"
                  name="giaBanCa2"
                  rules={[{ required: true }, { type: "number", min: 0 }]}
                  initialValue={row?.giaBanCa2 ?? 0}
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
                  label="Giá Ca 3"
                  name="giaBanCa3"
                  rules={[{ required: true }, { type: "number", min: 0 }]}
                  initialValue={row?.giaBanCa3 ?? 0}
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
            <Form.Item label="Áp dụng giá bán">
              <Input value={todayLabel} disabled />
            </Form.Item>
          </>
        )}
      </Form>
    </Modal>
  );
};
