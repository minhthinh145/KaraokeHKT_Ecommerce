import React, { useState } from "react";
import {
  Modal,
  Form,
  Input,
  Select,
  Typography,
  Alert,
  Switch,
  Divider,
  InputNumber,
} from "antd";
import dayjs from "dayjs";
import { PlusOutlined, InfoCircleOutlined } from "@ant-design/icons";
import type {
  AddPhongHatDTO,
  LoaiPhongDTO,
} from "../../../../../redux/admin/QLPhong/types";

const { Title } = Typography;

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (payload: AddPhongHatDTO) => Promise<{ success: boolean } | void>;
  loaiPhongOptions: LoaiPhongDTO[];
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

export const CreatePhongHatModal: React.FC<Props> = ({
  open,
  onClose,
  onSubmit,
  loaiPhongOptions,
}) => {
  const [form] = Form.useForm();
  const [imgUrl, setImgUrl] = useState("");
  const [dongGiaAllCa, setDongGiaAllCa] = useState(true);

  const todayISO = dayjs().format("YYYY-MM-DD");
  const todayLabel = dayjs().format("DD/MM/YYYY");
  const isImageUrl = (u?: string) =>
    !!u && /\.(png|jpe?g|gif|webp|bmp|svg)$/i.test(u.split("?")[0]);

  return (
    <Modal
      open={open}
      onCancel={onClose}
      onOk={() => form.submit()}
      width={680}
      destroyOnClose
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-emerald-500 to-green-600 rounded-full mb-3">
            <PlusOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Thêm phòng hát
          </Title>
        </div>
      }
    >
      <Form
        form={form}
        layout="vertical"
        initialValues={{
          giaThueChung: 0,
          giaThueCa1: 0,
          giaThueCa2: 0,
          giaThueCa3: 0,
        }}
        onValuesChange={(c) => {
          if ("hinhAnhPhong" in c) setImgUrl(c.hinhAnhPhong || "");
        }}
        onFinish={(v) =>
          onSubmit({
            maLoaiPhong: v.maLoaiPhong,
            tenPhong: v.tenPhong,
            hinhAnhPhong: v.hinhAnhPhong || null,
            dongGiaAllCa,
            giaThueChung: dongGiaAllCa ? Number(v.giaThueChung ?? 0) : null,
            giaThueCa1: !dongGiaAllCa ? Number(v.giaThueCa1 ?? 0) : null,
            giaThueCa2: !dongGiaAllCa ? Number(v.giaThueCa2 ?? 0) : null,
            giaThueCa3: !dongGiaAllCa ? Number(v.giaThueCa3 ?? 0) : null,
            ngayApDungGia: todayISO,
            trangThaiGia: "HieuLuc",
          })
        }
      >
        <Alert
          type="info"
          icon={<InfoCircleOutlined />}
          className="mb-4"
          message={
            <div className="text-sm">
              <b>Hướng dẫn:</b> Chọn loại phòng, nhập tên, giá thuê (≥ 0). Ảnh
              có thể bỏ trống.
              <br />
              Ngày áp dụng giá: <b>{todayLabel}</b>
            </div>
          }
        />
        <Form.Item
          name="maLoaiPhong"
          label="Loại phòng"
          rules={[{ required: true, message: "Chọn loại phòng" }]}
        >
          <Select
            placeholder="Chọn loại phòng"
            options={loaiPhongOptions.map((l) => ({
              value: l.maLoaiPhong,
              label: `${l.tenLoaiPhong} (${l.sucChua} người)`,
            }))}
          />
        </Form.Item>
        <Form.Item
          name="tenPhong"
          label="Tên phòng"
          rules={[{ required: true }, { max: 200 }]}
        >
          <Input placeholder="VD: Phòng VIP 1" />
        </Form.Item>
        <Form.Item
          name="hinhAnhPhong"
          label="Link ảnh phòng (tùy chọn)"
          rules={[
            { type: "url", message: "Phải là đường dẫn hợp lệ" },
            { max: 500, message: "Tối đa 500 ký tự" },
          ]}
        >
          <Input
            placeholder="https://...jpg/png"
            allowClear
            suffix={
              imgUrl && isImageUrl(imgUrl) ? (
                <img
                  src={imgUrl}
                  alt="Preview"
                  style={{
                    width: 32,
                    height: 32,
                    objectFit: "cover",
                    borderRadius: 4,
                  }}
                />
              ) : null
            }
          />
        </Form.Item>
        {imgUrl && isImageUrl(imgUrl) && (
          <div className="mb-3">
            <img
              src={imgUrl}
              alt="Preview"
              style={{
                maxWidth: 180,
                maxHeight: 120,
                borderRadius: 8,
                border: "1px solid #eee",
                marginTop: 4,
              }}
            />
          </div>
        )}
        <Divider className="!my-3">Giá thuê</Divider>
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
      </Form>
    </Modal>
  );
};
