import React from "react";
import dayjs from "dayjs";
import type { LichLamViecDTO } from "../../../api";
import type { CaBrief } from "./LichLamViecTable";

interface LichLamViecTablePrintProps {
  caList: CaBrief[];
  weekStart: string;
  data: LichLamViecDTO[];
  getNhanVienName?: (item: LichLamViecDTO) => string;
  getNhanVienInfo?: (item: LichLamViecDTO) => string | undefined;
}

export const LichLamViecTablePrint: React.FC<LichLamViecTablePrintProps> = ({
  caList,
  weekStart,
  data,
  getNhanVienName = (item) => item.tenNhanVien || item.maNhanVien,
  getNhanVienInfo = (item) => item.loaiNhanVien,
}) => {
  const weekStartD = dayjs(weekStart);
  const weekEndD = weekStartD.add(6, "day");
  const days = Array.from({ length: 7 }, (_, i) => weekStartD.add(i, "day"));
  const VI_DAY_LABEL = [
    "Thứ 2",
    "Thứ 3",
    "Thứ 4",
    "Thứ 5",
    "Thứ 6",
    "Thứ 7",
    "CN",
  ];

  // Map ca/ngày -> danh sách nhân viên
  const tableMap = React.useMemo(() => {
    const m = new Map<string, LichLamViecDTO[]>();
    for (const item of data) {
      const day = dayjs(item.ngayLamViec).format("YYYY-MM-DD");
      const key = `${item.maCa}_${day}`;
      if (!m.has(key)) m.set(key, []);
      m.get(key)!.push(item);
    }
    for (const [k, arr] of m) {
      arr.sort((a, b) =>
        (getNhanVienName(a) || "").localeCompare(getNhanVienName(b) || "", "vi")
      );
    }
    return m;
  }, [data, getNhanVienName]);

  return (
    <div
      style={{
        padding: 24,
        background: "#fff",
        fontFamily: "Arial, sans-serif",
        color: "#111827",
        width: "100%",
      }}
    >
      {/* Header */}
      <div style={{ textAlign: "center", marginBottom: 18 }}>
        <div
          style={{
            fontSize: 26,
            fontWeight: 800,
            letterSpacing: 0.5,
            color: "#0f172a",
          }}
        >
          LỊCH LÀM VIỆC TUẦN
        </div>
        <div style={{ marginTop: 6, color: "#475569", fontSize: 14 }}>
          {weekStartD.format("DD/MM/YYYY")} - {weekEndD.format("DD/MM/YYYY")}
        </div>
      </div>

      <table
        style={{
          width: "100%",
          borderCollapse: "separate",
          borderSpacing: 0,
          tableLayout: "fixed",
          background: "#fff",
          fontSize: 15,
          boxShadow: "0 2px 8px rgba(0,0,0,0.04)",
          border: "1px solid #e5e7eb",
        }}
      >
        <thead>
          <tr>
            <th
              style={{
                background: "#f1f5f9",
                padding: "14px 12px",
                minWidth: 180,
                borderRight: "1px solid #e5e7eb",
                textAlign: "center",
                fontWeight: 700,
                fontSize: 16,
              }}
            >
              Ca
            </th>
            {days.map((d, i) => (
              <th
                key={i}
                style={{
                  background: "#f1f5f9",
                  padding: "14px 10px",
                  minWidth: 160, // dài hơn mỗi cột ngày
                  borderLeft: "1px solid #e5e7eb",
                  borderRight:
                    i === days.length - 1 ? "none" : "1px solid #e5e7eb",
                  textAlign: "center",
                  fontWeight: 700,
                  fontSize: 16,
                }}
              >
                <div>{VI_DAY_LABEL[i]}</div>
                <div
                  style={{
                    fontWeight: 500,
                    fontSize: 13,
                    color: "#475569",
                    marginTop: 2,
                  }}
                >
                  {d.format("DD/MM")}
                </div>
              </th>
            ))}
          </tr>
        </thead>

        <tbody>
          {caList.map((ca, caIdx) => (
            <tr key={ca.maCa}>
              {/* Cột Ca */}
              <td
                style={{
                  padding: "16px 12px",
                  verticalAlign: "top",
                  background: caIdx % 2 === 0 ? "#f8fafc" : "#fff",
                  borderTop: "1px solid #e5e7eb",
                  borderRight: "1px solid #e5e7eb",
                  textAlign: "center",
                  fontWeight: 700,
                }}
              >
                <div style={{ fontSize: 15 }}>{ca.tenCaLamViec}</div>
                <div style={{ fontSize: 12, color: "#64748b", marginTop: 4 }}>
                  {ca.gioBatDauCa} → {ca.gioKetThucCa}
                </div>
              </td>

              {/* Các cột ngày */}
              {days.map((d, i) => {
                const key = `${ca.maCa}_${d.format("YYYY-MM-DD")}`;
                const list = tableMap.get(key) || [];
                return (
                  <td
                    key={key}
                    style={{
                      padding: "14px 10px",
                      verticalAlign: "top",
                      background: caIdx % 2 === 0 ? "#ffffff" : "#fcfcfd",
                      borderTop: "1px solid #e5e7eb",
                      borderLeft: "1px solid #e5e7eb",
                      borderRight:
                        i === days.length - 1 ? "none" : "1px solid #e5e7eb",
                      textAlign: "left",
                      wordBreak: "break-word",
                    }}
                  >
                    {list.length === 0 ? (
                      <span style={{ color: "#cbd5e1" }}>—</span>
                    ) : (
                      <ul style={{ margin: 0, padding: 0, listStyle: "none" }}>
                        {list.map((item) => (
                          <li
                            key={`${item.maLichLamViec}-${item.maNhanVien}`}
                            style={{
                              marginBottom: 10,
                              lineHeight: 1.7,
                              fontSize: 15,
                            }}
                          >
                            <span style={{ fontWeight: 600 }}>
                              {getNhanVienName(item)}
                            </span>
                            {getNhanVienInfo(item) && (
                              <span
                                style={{
                                  color: "#0369a1",
                                  fontSize: 13,
                                  marginLeft: 8,
                                }}
                              >
                                ({getNhanVienInfo(item)})
                              </span>
                            )}
                          </li>
                        ))}
                      </ul>
                    )}
                  </td>
                );
              })}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
