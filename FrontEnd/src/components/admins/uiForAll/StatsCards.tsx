import React from "react";

export interface StatCardData {
  id: string;
  title: string;
  value: number | string;
  icon: string;
  color:
    | "blue"
    | "green"
    | "orange"
    | "red"
    | "purple"
    | "yellow"
    | "pink"
    | "indigo";
  bgColor?: string;
  textColor?: string;
}

interface StatsCardsProps {
  cards: StatCardData[];
  className?: string;
  cardClassName?: string;
  gridCols?: 1 | 2 | 3 | 4 | 5 | 6;
}

export const StatsCards: React.FC<StatsCardsProps> = ({
  cards,
  className = "",
  cardClassName = "",
  gridCols = 4,
}) => {
  const getColorClasses = (color: StatCardData["color"]) => {
    const colorMap = {
      blue: {
        text: "text-blue-600",
        bg: "bg-blue-100",
      },
      green: {
        text: "text-green-600",
        bg: "bg-green-100",
      },
      orange: {
        text: "text-orange-600",
        bg: "bg-orange-100",
      },
      red: {
        text: "text-red-600",
        bg: "bg-red-100",
      },
      purple: {
        text: "text-purple-600",
        bg: "bg-purple-100",
      },
      yellow: {
        text: "text-yellow-600",
        bg: "bg-yellow-100",
      },
      pink: {
        text: "text-pink-600",
        bg: "bg-pink-100",
      },
      indigo: {
        text: "text-indigo-600",
        bg: "bg-indigo-100",
      },
    };
    return colorMap[color];
  };

  const getGridClass = (cols: number) => {
    const gridMap = {
      1: "grid-cols-1",
      2: "grid-cols-1 md:grid-cols-2",
      3: "grid-cols-1 md:grid-cols-3",
      4: "grid-cols-1 md:grid-cols-4",
      5: "grid-cols-1 md:grid-cols-5",
      6: "grid-cols-1 md:grid-cols-6",
    };
    return (
      gridMap[cols as keyof typeof gridMap] || "grid-cols-1 md:grid-cols-4"
    );
  };

  return (
    <div className={`grid ${getGridClass(gridCols)} gap-4 ${className}`}>
      {cards.map((card) => {
        const colors = getColorClasses(card.color);
        return (
          <div
            key={card.id}
            className={`bg-white p-4 rounded-lg border border-neutral-200 shadow-sm hover:shadow-md transition-shadow ${cardClassName}`}
          >
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-gray-600">{card.title}</p>
                <p
                  className={`text-2xl font-bold ${
                    card.textColor || colors.text
                  }`}
                >
                  {card.value}
                </p>
              </div>
              <div
                className={`w-12 h-12 ${
                  card.bgColor || colors.bg
                } rounded-lg flex items-center justify-center text-xl`}
              >
                {card.icon}
              </div>
            </div>
          </div>
        );
      })}
    </div>
  );
};

// Helper để tạo nhanh stats cards
export const StatsCardHelpers = {
  createCard: (
    id: string,
    title: string,
    value: number | string,
    icon: string,
    color: StatCardData["color"]
  ): StatCardData => ({
    id,
    title,
    value,
    icon,
    color,
  }),

  // Predefined cards cho các use case phổ biến
  totalCard: (value: number, entity: string = "items"): StatCardData => ({
    id: "total",
    title: `Tổng ${entity}`,
    value,
    icon: "👥",
    color: "blue",
  }),

  filteredCard: (value: number): StatCardData => ({
    id: "filtered",
    title: "Đang hiển thị",
    value,
    icon: "📊",
    color: "green",
  }),

  activeCard: (value: number): StatCardData => ({
    id: "active",
    title: "Hoạt động",
    value,
    icon: "✅",
    color: "orange",
  }),

  lockedCard: (value: number): StatCardData => ({
    id: "locked",
    title: "Đã khóa",
    value,
    icon: "🔒",
    color: "red",
  }),
  nhanVienPhucVuCard: (value: number): StatCardData => ({
    id: "nhanVienPhucVu",
    title: "Nhân viên phục vụ",
    value,
    icon: "🍽️",
    color: "purple",
  }),
  nhanVienTiepTanCard: (value: number): StatCardData => ({
    id: "nhanVienTiepTan",
    title: "Nhân viên tiếp tân",
    value,
    icon: "👤",
    color: "indigo",
  }),
  nhanVienKhoCard: (value: number): StatCardData => ({
    id: "nhanVienKho",
    title: "Nhân viên kho",
    value,
    icon: "📦",
    color: "yellow",
  }),
  totalNhanVienCard: (value: number): StatCardData => ({
    id: "totalNhanVien",
    title: "Tổng nhân viên",
    value,
    icon: "👥",
    color: "blue",
  }),
};
