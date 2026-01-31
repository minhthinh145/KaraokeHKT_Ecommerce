import React, { useEffect, useState } from "react";
import {
  Modal,
  Form,
  Input,
  InputNumber,
  Switch,
  Divider,
  Typography,
  Select,
} from "antd";
import dayjs from "dayjs";
import { EditOutlined } from "@ant-design/icons";
import type {
  PhongHatDetailDTO,
  UpdatePhongHatDTO,
} from "../../../../../redux/admin/QLPhong/types";

const { Title } = Typography;

interface Props {
  open: boolean;
  row?: PhongHatDetailDTO | null;
  onClose: () => void;
  onSubmit: (p: UpdatePhongHatDTO) => Promise<any>;
  loaiPhongOptions: {
    maLoaiPhong: number;
    tenLoaiPhong: string;
    sucChua: number;
  }[];
}

const vndFormatter = (val?: string | number) => {
  if (val == null || val === "") return "";
  const n =
    typeof val === "number" ? val : Number(String(val).replace(/[^\d]/g, ""));
  if (Number.isNaN(n)) return "";
  return n.toLocaleString("vi-VN") + " ₫";
};
const vndParser = (v?: string) =>
  v ? Number(v.replace(/\./g, "").replace(/[^\d]/g, "")) : 0;

export const EditPhongHatModal: React.FC<Props> = ({
  open,
  row,
  onClose,
  onSubmit,
  loaiPhongOptions,
}) => {
  const [form] = Form.useForm();
  const [imgUrl, setImgUrl] = useState("");
  const [capNhatGia, setCapNhatGia] = useState(false);
  const [dongGiaAllCa, setDongGiaAllCa] = useState(true);

  useEffect(() => {
    if (row) {
      form.setFieldsValue({
        tenPhong: row.tenSanPham || row.tenSanPham,
        hinhAnhPhong: row.hinhAnhSanPham || "",
        maLoaiPhong: row.maLoaiPhong, // <--- set
        giaThueChung: row.giaThueChung ?? 0,
        giaThueCa1: row.giaThueCa1 ?? 0,
        giaThueCa2: row.giaThueCa2 ?? 0,
        giaThueCa3: row.giaThueCa3 ?? 0,
      });
      setImgUrl(row.hinhAnhSanPham || "");
      setDongGiaAllCa(!!row.dongGiaAllCa);
      setCapNhatGia(false);
    }
  }, [row]);

  const todayISO = dayjs().format("YYYY-MM-DD");
  const todayLabel = dayjs().format("DD/MM/YYYY");

  const submit = (v: any) => {
    const payload: UpdatePhongHatDTO = {
      maPhong: row!.maPhong,
      tenPhong: v.tenPhong,
      hinhAnhPhong: v.hinhAnhPhong || null,
      maLoaiPhong: v.maLoaiPhong ?? null, // <--- gửi lên
      capNhatGiaThue: capNhatGia,
      dongGiaAllCa: capNhatGia ? dongGiaAllCa : undefined,
      giaThueChung:
        capNhatGia && dongGiaAllCa ? Number(v.giaThueChung ?? 0) : undefined,
      giaThueCa1:
        capNhatGia && !dongGiaAllCa ? Number(v.giaThueCa1 ?? 0) : undefined,
      giaThueCa2:
        capNhatGia && !dongGiaAllCa ? Number(v.giaThueCa2 ?? 0) : undefined,
      giaThueCa3:
        capNhatGia && !dongGiaAllCa ? Number(v.giaThueCa3 ?? 0) : undefined,
      ngayApDungGia: capNhatGia ? todayISO : undefined,
      trangThaiGia: capNhatGia ? "HieuLuc" : undefined,
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
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-indigo-500 to-blue-600 rounded-full mb-3">
            <EditOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Cập nhật phòng hát
          </Title>
        </div>
      }
      width={680}
    >
      <Form
        form={form}
        layout="vertical"
        onValuesChange={(c) => {
          if ("hinhAnhPhong" in c) setImgUrl(c.hinhAnhPhong || "");
        }}
        onFinish={submit}
      >
        <Form.Item
          name="tenPhong"
          label="Tên phòng"
          rules={[{ required: true }, { max: 200 }]}
        >
          <Input />
        </Form.Item>
        <Form.Item name="hinhAnhPhong" label="Hình ảnh (URL)">
          <Input placeholder="Dán URL ảnh (.jpg/.png/...)" />
        </Form.Item>
        {imgUrl && (
          <div className="mb-3 flex justify-center">
            <img
              src={imgUrl}
              alt="Preview"
              className="max-h-40 max-w-40 object-contain border rounded bg-slate-50"
              onError={(e) => (e.currentTarget.style.display = "none")}
            />
          </div>
        )}

        <Form.Item
          name="maLoaiPhong"
          label="Loại phòng"
          rules={[{ required: true, message: "Chọn loại phòng" }]}
        >
          <Select
            placeholder="Chọn loại phòng"
            options={loaiPhongOptions.map((lp) => ({
              value: lp.maLoaiPhong,
              label: `${lp.tenLoaiPhong} (${lp.sucChua} người)`,
            }))}
          />
        </Form.Item>

        <Divider className="!my-3">Giá thuê</Divider>
        <div className="flex items-center gap-3 mb-3">
          <Switch checked={capNhatGia} onChange={setCapNhatGia} />
          <span className="text-sm">
            {capNhatGia ? "Đang cập nhật giá thuê" : "Không cập nhật giá"}
          </span>
        </div>

        {capNhatGia && (
          <>
            <div className="flex items-center gap-2 mb-2">
              <Switch checked={dongGiaAllCa} onChange={setDongGiaAllCa} />
              <span className="text-sm">
                {dongGiaAllCa
                  ? "Đồng giá cho tất cả ca"
                  : "Giá thuê theo từng ca"}
              </span>
            </div>
            {dongGiaAllCa ? (
              <Form.Item
                label="Giá thuê chung"
                name="giaThueChung"
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
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                <Form.Item
                  label="Giá Ca 1"
                  name="giaThueCa1"
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
                  label="Giá Ca 2"
                  name="giaThueCa2"
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
                  label="Giá Ca 3"
                  name="giaThueCa3"
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
            <Form.Item label="Áp dụng giá">
              <Input value={todayLabel} disabled />
            </Form.Item>
          </>
        )}
      </Form>
    </Modal>
  );
};
