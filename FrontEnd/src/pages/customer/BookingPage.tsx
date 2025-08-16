import React, { useEffect } from "react";
import dayjs from "dayjs";
import {
  Input,
  Button,
  Modal,
  Form,
  InputNumber,
  Spin,
  Empty,
  message,
  DatePicker,
  TimePicker,
} from "antd";
import { useNavigate } from "react-router-dom";
import { useBooking } from "../../hooks/customer/useBooking";
import { RoomCard } from "../../components/customer/booking/RoomCard";
import { CustomerLayout } from "../../components/customer/CustomerLayout";
import { InvoicePreview } from "../../components/customer/booking/InvoicePreview";
import type { DatPhongDTO } from "../../api/customer/bookingApi";

export const BookingPage: React.FC = () => {
  const navigate = useNavigate();
  const {
    filteredRooms,
    ui,
    loading,
    bookingCreating,
    confirmingPayment,
    invoice,
    paymentUrl,
    actions: {
      refresh,
      create,
      confirm,
      setSearchQuery,
      openBooking,
      closeBooking,
      closeInvoice,
      setDurationHours,
      setNote,
    },
  } = useBooking({ autoLoad: true });

  const [form] = Form.useForm();

  const roomsDisplay = filteredRooms;

  // Helpers chặn chọn ngày quá khứ
  const disabledDate = (current: dayjs.Dayjs) =>
    current && current.startOf("day").isBefore(dayjs().startOf("day"));

  // Chặn giờ / phút quá khứ nếu cùng ngày
  const disabledTime = (current: dayjs.Dayjs | null) => {
    if (!current) return {};
    const now = dayjs();
    if (!current.isSame(now, "day")) return {};
    const disabledHours = Array.from({ length: 24 }, (_, h) => h).filter(
      (h) => h < now.hour()
    );
    const disabledMinutes = (hour: number) =>
      hour === now.hour()
        ? Array.from({ length: 60 }, (_, m) => m).filter(
            (m) => m < now.minute()
          )
        : [];
    return {
      disabledHours: () => disabledHours,
      disabledMinutes,
    };
  };

  // Quick set giờ/phút
  const quickSet = (mins: number) => {
    const v = dayjs().add(mins, "minute");
    form.setFieldValue("thoiGianBatDauDate", v.startOf("day"));
    form.setFieldValue("thoiGianBatDauTime", v);
  };

  const submitBooking = async () => {
    if (!ui.selectedRoom) return;
    const date: dayjs.Dayjs = form.getFieldValue("thoiGianBatDauDate");
    const time: dayjs.Dayjs = form.getFieldValue("thoiGianBatDauTime");
    if (!date || !time) return message.warning("Chọn ngày và giờ bắt đầu");
    const start = date.hour(time.hour()).minute(time.minute()).second(0);
    if (start.isBefore(dayjs()))
      return message.error("Thời gian phải lớn hơn hiện tại");

    const res = await create({
      maPhong: ui.selectedRoom.maPhong,
      thoiGianBatDau: start.toISOString(),
      soGioSuDung: ui.durationHours,
      ghiChu: ui.note,
    });
    if (!res.success) message.error(res.error || "Tạo hóa đơn thất bại");
  };

  // Redirect effect
  useEffect(() => {
    if (paymentUrl) {
      // đóng modal trước khi rời trang
      closeInvoice();
      window.location.href = paymentUrl;
    }
  }, [paymentUrl, closeInvoice]);

  const handleConfirmPayment = async () => {
    if (!invoice) return;
    const res = await confirm({
      maHoaDon: invoice.maHoaDon,
      maThuePhong: invoice.maThuePhong,
    });
    if (!res.success) {
      message.error(res.error || "Không tạo được URL thanh toán");
    }
  };

  const handleBook = async (payload: DatPhongDTO) => {
    const rs = await create(payload); // wrap createBooking thunk
    if (!rs.success) message.error(rs.error || "Lỗi tạo hóa đơn");
    // thành công sẽ tự mở modal (extraReducers đã set showInvoiceModal = true)
  };

  return (
    <CustomerLayout>
      <div className="min-h-screen bg-gradient-to-br from-indigo-600 to-indigo-900 mt-2">
        <section className="relative overflow-hidden">
          <div className="absolute inset-0 bg-black/20"></div>
          <div className="relative max-w-7xl mx-auto px-6 pt-16 pb-20 flex flex-col lg:flex-row gap-12">
            <div className="flex-1 space-y-8 text-white">
              <h1 className="text-5xl font-extrabold leading-tight">
                <span className="bg-gradient-to-r from-yellow-300 to-orange-400 bg-clip-text text-transparent">
                  Đặt Phòng Karaoke
                </span>
                <br />
                <span className="text-white">Nhanh – Trải Nghiệm Đỉnh Cao</span>
              </h1>
              <p className="text-blue-100 text-lg max-w-xl leading-relaxed">
                Chọn phòng yêu thích, xem hóa đơn tức thì và thanh toán an toàn
                qua VNPay. Đơn giản, nhanh chóng, tiện lợi.
              </p>
              <div className="flex flex-wrap gap-4">
                <Input.Search
                  placeholder="Tìm phòng theo tên / loại..."
                  allowClear
                  onSearch={setSearchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="w-80"
                  size="large"
                />
                <Button
                  onClick={refresh}
                  size="large"
                  className="bg-white/20 hover:bg-white/30 border-white/30 text-white font-medium backdrop-blur"
                >
                  🔄 Làm mới
                </Button>
                <Button
                  size="large"
                  onClick={() => navigate("/customer/booking/history")}
                  className="bg-gradient-to-r from-purple-500 to-purple-600 hover:from-purple-600 hover:to-purple-700 border-none text-white font-medium shadow-lg"
                >
                  📋 Lịch sử đặt phòng
                </Button>
              </div>
            </div>
            <div className="w-full lg:w-96 bg-white/10 backdrop-blur-lg rounded-3xl border border-white/20 p-8 shadow-2xl space-y-6">
              <h3 className="font-bold text-white text-xl flex items-center gap-3">
                <div className="w-3 h-3 bg-gradient-to-r from-yellow-400 to-orange-500 rounded-full animate-pulse"></div>
                Cài đặt nhanh
              </h3>
              <div className="text-blue-100 text-sm leading-relaxed">
                Chọn phòng để bắt đầu đặt & tạo hóa đơn. Thời gian thanh toán
                tối đa 15 phút.
              </div>
              <div className="grid grid-cols-3 gap-3">
                {[
                  {
                    mins: 30,
                    label: "+30p",
                    color: "from-green-400 to-green-500",
                  },
                  {
                    mins: 60,
                    label: "+1h",
                    color: "from-blue-400 to-blue-500",
                  },
                  {
                    mins: 120,
                    label: "+2h",
                    color: "from-purple-400 to-purple-500",
                  },
                ].map(({ mins, label, color }) => (
                  <Button
                    key={mins}
                    size="small"
                    onClick={() => quickSet(mins)}
                    className={`bg-gradient-to-r ${color} hover:scale-105 border-none text-white font-medium shadow-lg transition-transform`}
                  >
                    {label}
                  </Button>
                ))}
              </div>
              <div className="text-xs text-blue-200 bg-white/5 rounded-lg p-3 border border-white/10">
                ⚡ Hóa đơn sẽ hết hạn sau 15 phút nếu chưa thanh toán
              </div>
            </div>
          </div>
        </section>

        <section className="relative">
          <div className="max-w-7xl mx-auto px-6 pb-20">
            <div className="bg-white/10 backdrop-blur-lg rounded-3xl p-8 shadow-2xl">
              <div className="flex items-center justify-between mb-8">
                <h2 className="text-2xl font-bold text-white flex items-center gap-3">
                  <div className="w-1 h-6 bg-gradient-to-b from-yellow-400 to-orange-500 rounded-full"></div>
                  Phòng khả dụng
                </h2>
                {loading && (
                  <div className="text-blue-200 flex items-center gap-2">
                    <Spin size="small" />
                    <span>Đang tải...</span>
                  </div>
                )}
              </div>
              {loading ? (
                <div className="py-32 flex justify-center">
                  <Spin size="large" />
                </div>
              ) : roomsDisplay.length === 0 ? (
                <Empty
                  description={
                    <span className="text-blue-200">
                      Không có phòng phù hợp
                    </span>
                  }
                  className="py-16"
                />
              ) : (
                <div className="grid gap-8 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                  {roomsDisplay.map((r) => (
                    <RoomCard key={r.maPhong} room={r} onBook={openBooking} />
                  ))}
                </div>
              )}
            </div>
          </div>
        </section>
      </div>

      <Modal
        open={ui.showBookingModal}
        title={<span className="text-lg font-semibold">🏠 Đặt phòng</span>}
        onCancel={closeBooking}
        onOk={submitBooking}
        okText="📝 Tạo hóa đơn"
        cancelText="Hủy"
        confirmLoading={bookingCreating}
        destroyOnClose
        className="booking-modal"
        okButtonProps={{
          className:
            "bg-gradient-to-r from-indigo-500 to-indigo-600 hover:from-indigo-600 hover:to-indigo-700 border-none font-medium",
        }}
      >
        {ui.selectedRoom && (
          <Form
            form={form}
            layout="vertical"
            initialValues={{
              thoiGianBatDauDate: dayjs().add(30, "minute").startOf("day"),
              thoiGianBatDauTime: dayjs().add(30, "minute"),
            }}
          >
            <Form.Item label="Phòng">
              <div className="font-semibold text-gray-800 bg-gray-50 rounded-lg p-3">
                🎤 {ui.selectedRoom.tenPhong}
              </div>
            </Form.Item>
            <Form.Item
              label="Ngày bắt đầu"
              name="thoiGianBatDauDate"
              rules={[{ required: true, message: "Chọn ngày bắt đầu" }]}
            >
              <DatePicker
                style={{ width: "100%" }}
                disabledDate={disabledDate}
                format="DD/MM/YYYY"
                size="large"
              />
            </Form.Item>
            <Form.Item
              label="Giờ bắt đầu"
              name="thoiGianBatDauTime"
              rules={[{ required: true, message: "Chọn giờ bắt đầu" }]}
            >
              <TimePicker
                style={{ width: "100%" }}
                format="HH:mm"
                minuteStep={5}
                disabledTime={disabledTime}
                hideDisabledOptions
                size="large"
              />
            </Form.Item>
            <div className="flex gap-2 mb-4">
              {[
                {
                  mins: 30,
                  label: "+30 phút",
                  color: "from-green-400 to-green-500",
                },
                {
                  mins: 60,
                  label: "+1 giờ",
                  color: "from-blue-400 to-blue-500",
                },
                {
                  mins: 120,
                  label: "+2 giờ",
                  color: "from-purple-400 to-purple-500",
                },
              ].map(({ mins, label, color }) => (
                <Button
                  key={mins}
                  size="small"
                  onClick={() => quickSet(mins)}
                  className={`bg-gradient-to-r ${color} hover:scale-105 border-none text-white font-medium transition-transform`}
                >
                  {label}
                </Button>
              ))}
            </div>
            <Form.Item label="Số giờ sử dụng">
              <InputNumber
                min={1}
                max={24}
                value={ui.durationHours}
                onChange={(v) => setDurationHours(Number(v) || 1)}
                addonAfter="giờ"
                style={{ width: "100%" }}
                size="large"
              />
            </Form.Item>
            <Form.Item label="Ghi chú">
              <Input.TextArea
                value={ui.note}
                onChange={(e) => setNote(e.target.value)}
                autoSize={{ minRows: 3, maxRows: 5 }}
                placeholder="Sinh nhật, họp mặt, sự kiện đặc biệt..."
              />
            </Form.Item>
          </Form>
        )}
      </Modal>
      <Modal
        open={!!invoice && ui.showInvoiceModal}
        title={
          <span className="text-lg font-semibold">
            💰 Xác nhận hóa đơn đặt phòng
          </span>
        }
        onCancel={closeInvoice}
        footer={null}
        destroyOnClose
        width={720}
      >
        {invoice && (
          <InvoicePreview
            invoice={invoice}
            onConfirm={handleConfirmPayment}
            loading={confirmingPayment}
          />
        )}
      </Modal>
    </CustomerLayout>
  );
};
