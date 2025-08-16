import React, {
  useMemo,
  useRef,
  useLayoutEffect,
  useState,
  useCallback,
} from "react";
import dayjs from "dayjs";
import { Button, Tag } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import type { LichLamViecDTO } from "../../../../api";
import { EmployeeCard } from "../EmployeeCard";

export interface CaBrief {
  maCa: number;
  tenCaLamViec: string;
  gioBatDauCa: string;
  gioKetThucCa: string;
}

export interface PendingShiftChange {
  maLichLamViecGoc: number;
  ngayLamViecMoi: string;
  maCaMoi: number;
}

interface LichLamViecChildProps {
  caList: CaBrief[];
  days: dayjs.Dayjs[];
  today: string;
  tableMap: Map<string, LichLamViecDTO[]>;
  getNhanVienName: (item: LichLamViecDTO) => string;
  getNhanVienInfo: (item: LichLamViecDTO) => string | undefined;
  canAdd: boolean;
  canEdit: boolean;
  canDelete: boolean;
  onAdd?: (payload: { ngayLamViec: string; maCa: number }) => void;
  onEdit?: (item: LichLamViecDTO) => void;
  onDelete?: (item: LichLamViecDTO) => void;
  enableShiftRequestDrag: boolean;
  draggingLichId: number | null;
  onShiftDragStart?: (id: number) => void;
  onShiftDragEnd?: () => void;
  onShiftDropCreate?: (p: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }) => void;
  showShiftRequestHint: boolean;
  pendingShiftChanges: PendingShiftChange[];
  schedule: LichLamViecDTO[];
}

/**
 * Child grid: render cells + overlay lines for pending shift requests
 * Lines: drawn in SVG overlay; path shaped (L) to “né” các cell khác (đi ngang rồi dọc)
 */
export const LichLamViecChild: React.FC<LichLamViecChildProps> = ({
  caList,
  days,
  today,
  tableMap,
  getNhanVienName,
  getNhanVienInfo,
  canAdd,
  canEdit,
  canDelete,
  onAdd,
  onEdit,
  onDelete,
  enableShiftRequestDrag,
  draggingLichId,
  onShiftDragStart,
  onShiftDragEnd,
  onShiftDropCreate,
  showShiftRequestHint,
  pendingShiftChanges,
  schedule,
}) => {
  // Refs: cell key -> element; source card id -> element
  const cellRefs = useRef<Record<string, HTMLDivElement | null>>({});
  const sourceCardRefs = useRef<Record<number, HTMLDivElement | null>>({});
  const containerRef = useRef<HTMLDivElement | null>(null);

  // Build quick lookups
  const pendingSourceIds = useMemo(
    () => new Set(pendingShiftChanges.map((p) => p.maLichLamViecGoc)),
    [pendingShiftChanges]
  );
  const pendingTargetKeySet = useMemo(
    () =>
      new Set(
        pendingShiftChanges.map(
          (p) => `${p.maCaMoi}_${dayjs(p.ngayLamViecMoi).format("YYYY-MM-DD")}`
        )
      ),
    [pendingShiftChanges]
  );
  const reqBySource = useMemo(
    () =>
      new Map(pendingShiftChanges.map((p) => [p.maLichLamViecGoc, p] as const)),
    [pendingShiftChanges]
  );

  // Source schedule item mapping id -> its original cell key
  const sourceCellKeyById = useMemo(() => {
    const m = new Map<number, string>();
    for (const item of schedule) {
      const dStr = dayjs(item.ngayLamViec).format("YYYY-MM-DD");
      m.set(item.maLichLamViec, `${item.maCa}_${dStr}`);
    }
    return m;
  }, [schedule]);

  interface LineDef {
    id: number;
    from: { x: number; y: number };
    to: { x: number; y: number };
    color: string;
    offsetIndex: number; // for stacking multiple lines from same source
  }

  const [lines, setLines] = useState<LineDef[]>([]);

  const computeLines = useCallback(() => {
    if (!containerRef.current) return;
    const containerRect = containerRef.current.getBoundingClientRect();
    const groupBySource: Record<number, number> = {};
    const newLines: LineDef[] = [];

    for (const req of pendingShiftChanges) {
      const srcId = req.maLichLamViecGoc;
      const tgtKey = `${req.maCaMoi}_${dayjs(req.ngayLamViecMoi).format(
        "YYYY-MM-DD"
      )}`;

      const srcCardEl = sourceCardRefs.current[srcId];
      const tgtCellEl = cellRefs.current[tgtKey];

      if (!srcCardEl || !tgtCellEl) continue;

      const srcRect = srcCardEl.getBoundingClientRect();
      const tgtRect = tgtCellEl.getBoundingClientRect();

      const offset = groupBySource[srcId] ?? 0;
      groupBySource[srcId] = offset + 1;

      const from = {
        x: srcRect.right - containerRect.left,
        y: srcRect.top + srcRect.height / 2 - containerRect.top + offset * 4,
      };
      const to = {
        x: tgtRect.left - containerRect.left + tgtRect.width / 2,
        y: tgtRect.top + tgtRect.height / 2 - containerRect.top + offset * 4,
      };

      newLines.push({
        id: srcId,
        from,
        to,
        color: "#6366f1",
        offsetIndex: offset,
      });
    }
    setLines(newLines);
  }, [pendingShiftChanges]);

  useLayoutEffect(() => {
    computeLines();
  }, [computeLines, tableMap]);

  // Update on resize & scroll (basic)
  useLayoutEffect(() => {
    const handler = () => computeLines();
    window.addEventListener("resize", handler);
    document.addEventListener("scroll", handler, true);
    return () => {
      window.removeEventListener("resize", handler);
      document.removeEventListener("scroll", handler, true);
    };
  }, [computeLines]);

  const VI_DAY_LABEL = [
    "Thứ 2",
    "Thứ 3",
    "Thứ 4",
    "Thứ 5",
    "Thứ 6",
    "Thứ 7",
    "CN",
  ];

  return (
    <div ref={containerRef} className="relative">
      {enableShiftRequestDrag && showShiftRequestHint && (
        <div className="mb-3 text-xs p-2 rounded border bg-blue-50 border-blue-200 text-blue-600">
          Kéo & thả thẻ nhân viên sang ô Ca / Ngày khác để tạo yêu cầu chuyển
          ca.
        </div>
      )}

      {/* HEADER ROW */}
      <div
        className="grid"
        style={{ gridTemplateColumns: `160px repeat(${days.length}, 1fr)` }}
      >
        <div
          className="p-3 font-bold text-center bg-gray-50"
          style={{ fontFamily: "Space Grotesk" }}
        >
          Ca
        </div>
        {days.map((d, idx) => {
          const dStr = d.format("YYYY-MM-DD");
          const isToday = dStr === today;
          return (
            <div
              key={dStr}
              className="p-3 text-center bg-gray-50"
              style={{ fontFamily: "Space Grotesk" }}
            >
              <span
                className={`inline-block px-2 py-1 rounded ${
                  isToday
                    ? "bg-sky-100 text-sky-700 font-bold"
                    : "font-semibold"
                }`}
              >
                {VI_DAY_LABEL[idx]} {d.format("DD/MM")}
              </span>
              {isToday && (
                <Tag
                  color="blue"
                  style={{
                    marginLeft: 8,
                    fontWeight: 700,
                    borderRadius: 6,
                    padding: "0 8px",
                    fontSize: 12,
                  }}
                >
                  Hôm nay
                </Tag>
              )}
            </div>
          );
        })}
      </div>

      {/* BODY */}
      <div
        className="grid border border-gray-200 border-t-0"
        style={{ gridTemplateColumns: `160px repeat(${days.length}, 1fr)` }}
      >
        {caList.map((ca) => {
          return (
            <React.Fragment key={ca.maCa}>
              {/* Left fixed cell (Ca info) */}
              <div
                className="border-t border-gray-200 bg-gray-50 px-2 py-3 text-center font-semibold "
                style={{ fontFamily: "Space Grotesk" }}
              >
                <div className="uppercase">{ca.tenCaLamViec}</div>
                <div className="text-xs text-gray-600 font-bold mt-1">
                  {ca.gioBatDauCa} → {ca.gioKetThucCa}
                </div>
              </div>

              {days.map((d) => {
                const dateStr = d.format("YYYY-MM-DD");
                const cellKey = `${ca.maCa}_${dateStr}`;
                const list = tableMap.get(cellKey) || [];

                const isPendingTarget = pendingTargetKeySet.has(cellKey);

                // Determine if cell contains a source
                let isPendingSource = false;
                let sourceReq = undefined as PendingShiftChange | undefined;
                for (const item of list) {
                  if (pendingSourceIds.has(item.maLichLamViec)) {
                    isPendingSource = true;
                    sourceReq = reqBySource.get(item.maLichLamViec);
                    break;
                  }
                }

                // NEW: Không cho thao tác với ngày quá khứ
                const isPast = dayjs(dateStr).isBefore(dayjs().startOf("day"));

                const canDropBase =
                  enableShiftRequestDrag &&
                  draggingLichId !== null &&
                  list.length === 0 &&
                  !isPast;
                const canDrop = canDropBase && !isPendingTarget;
                const blockDragInside = isPendingSource || isPast;

                return (
                  <div
                    key={cellKey}
                    ref={(el) => {
                      cellRefs.current[cellKey] = el;
                    }}
                    className={`
                      relative border-t border-l border-gray-200 flex flex-col items-center justify-center gap-2 px-1 py-2
                      ${
                        canDrop
                          ? "bg-blue-50/40 outline outline-1 outline-dashed outline-blue-300"
                          : ""
                      }
                      ${
                        isPendingTarget
                          ? "bg-indigo-50 outline outline-1 outline-dashed outline-indigo-400"
                          : ""
                      }
                      ${
                        isPast
                          ? "opacity-60 pointer-events-none select-none"
                          : ""
                      }
                    `}
                    aria-dropeffect={canDrop ? "move" : "none"}
                    onDragOver={(e) => {
                      if (canDrop) e.preventDefault();
                    }}
                    onDrop={() => {
                      if (canDrop) {
                        onShiftDropCreate?.({
                          maLichLamViecGoc: draggingLichId!,
                          ngayLamViecMoi: dateStr,
                          maCaMoi: ca.maCa,
                        });
                      }
                    }}
                  >
                    {/* Cards */}
                    {list.map((item) => {
                      const faded =
                        pendingSourceIds.has(item.maLichLamViec) || isPast;
                      return (
                        <div
                          key={item.maLichLamViec}
                          className="w-full flex justify-center"
                        >
                          <div
                            ref={(el) => {
                              if (faded && !isPast) {
                                // store source card ref for line start
                                sourceCardRefs.current[item.maLichLamViec] = el;
                              }
                            }}
                            className={`mx-auto max-w-[220px] w-full relative transition
                              ${
                                enableShiftRequestDrag &&
                                !blockDragInside &&
                                !faded
                                  ? "cursor-grab active:cursor-grabbing"
                                  : ""
                              }
                              ${faded ? "opacity-55" : ""}
                            `}
                            draggable={
                              enableShiftRequestDrag &&
                              !blockDragInside &&
                              !faded &&
                              !isPast
                            }
                            onDragStart={() => {
                              if (
                                enableShiftRequestDrag &&
                                !blockDragInside &&
                                !faded &&
                                !isPast
                              ) {
                                onShiftDragStart?.(item.maLichLamViec);
                              }
                            }}
                            onDragEnd={() =>
                              enableShiftRequestDrag && onShiftDragEnd?.()
                            }
                          >
                            <EmployeeCard
                              name={getNhanVienName(item) || ""}
                              showId={false}
                              info={getNhanVienInfo(item) || ""}
                              onEdit={
                                canEdit && !faded && !isPast
                                  ? () => onEdit?.(item)
                                  : undefined
                              }
                              onDelete={
                                canDelete && !faded && !isPast
                                  ? () => onDelete?.(item)
                                  : undefined
                              }
                              editable={canEdit && !faded && !isPast}
                              deletable={canDelete && !faded && !isPast}
                            />
                            {faded && sourceReq && !isPast && (
                              <div className="absolute -top-2 -right-2 bg-indigo-600 text-white text-[10px] font-semibold px-2 py-0.5 rounded shadow">
                                Đang chuyển
                              </div>
                            )}
                          </div>
                        </div>
                      );
                    })}

                    {/* Target placeholder */}
                    {isPendingTarget && (
                      <div className="w-full max-w-[220px] rounded-md border border-dashed border-indigo-400 bg-white/70 text-[11px] font-medium text-indigo-600 flex flex-col items-center gap-1 px-2 py-2 shadow-sm">
                        <span>Chuyển đến</span>
                        <svg
                          className="w-4 h-4 text-indigo-500 animate-bounce"
                          fill="none"
                          stroke="currentColor"
                          strokeWidth="2"
                          viewBox="0 0 24 24"
                        >
                          <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M14 5l7 7m0 0l-7 7m7-7H3"
                          />
                        </svg>
                      </div>
                    )}

                    {/* Empty hint */}
                    {list.length === 0 &&
                      !isPendingTarget &&
                      enableShiftRequestDrag && (
                        <div
                          className={`flex justify-center items-center text-[11px] leading-tight font-medium rounded px-2 py-1
                            ${
                              canDrop
                                ? "text-blue-600 bg-blue-100/60 border border-blue-300"
                                : "text-neutral-400"
                            }`}
                        >
                          {canDrop ? "Thả để tạo yêu cầu" : "Trống"}
                        </div>
                      )}

                    {/* Add button (only when not in request drag mode) */}
                    {canAdd && !enableShiftRequestDrag && !isPendingTarget && (
                      <Button
                        type="dashed"
                        block
                        icon={<PlusOutlined />}
                        onClick={() =>
                          onAdd?.({
                            ngayLamViec: dateStr,
                            maCa: ca.maCa,
                          })
                        }
                        style={{
                          fontWeight: 600,
                          fontFamily: "Space Grotesk",
                        }}
                      >
                        Thêm
                      </Button>
                    )}
                  </div>
                );
              })}
            </React.Fragment>
          );
        })}
      </div>

      {/* SVG overlay lines */}
      <svg
        className="pointer-events-none absolute inset-0"
        width="100%"
        height="100%"
      >
        <defs>
          <marker
            id="arrow_head_shift"
            markerWidth="10"
            markerHeight="10"
            refX="8"
            refY="5"
            orient="auto"
            markerUnits="strokeWidth"
          >
            <path d="M0,0 L10,5 L0,10 z" fill="#6366f1" />
          </marker>
        </defs>
        {lines.map((ln) => {
          // Build path: L shaped (horizontal then vertical) to "avoid" other cells visually
          const midX = (ln.from.x + ln.to.x) / 2;
          const path = `M ${ln.from.x} ${ln.from.y} L ${midX} ${ln.from.y} L ${midX} ${ln.to.y} L ${ln.to.x} ${ln.to.y}`;
          return (
            <path
              key={ln.id + "_" + ln.offsetIndex}
              d={path}
              stroke={ln.color}
              strokeWidth={2}
              strokeDasharray="6 5"
              fill="none"
              markerEnd="url(#arrow_head_shift)"
              className="animate-[pulseLine_2.2s_linear_infinite]"
              opacity={0.85}
            />
          );
        })}
      </svg>
      {/* Animation */}
      <style>
        {`
          @keyframes pulseLine {
            0% { stroke-dashoffset: 24; opacity: .5; }
            50% { opacity: 1; }
            100% { stroke-dashoffset: 0; opacity: .5; }
          }
        `}
      </style>
    </div>
  );
};
