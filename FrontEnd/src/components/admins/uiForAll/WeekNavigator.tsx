import React, { useMemo } from "react";
import dayjs from "dayjs";
import { Select, Button, Space, Typography, Card, Tag } from "antd";
import {
  LeftOutlined,
  RightOutlined,
  CalendarOutlined,
} from "@ant-design/icons";

const { Text } = Typography;

export interface WeekNavigatorProps {
  weekStart: string; // YYYY-MM-DD (Thứ 2)
  onChangeWeekStart: (nextWeekStart: string) => void;
  className?: string;
  yearRange?: { min: number; max: number };
}

const toMonday = (d: dayjs.Dayjs) => {
  const dow = d.day();
  const delta = dow === 0 ? -6 : 1 - dow;
  return d.add(delta, "day").startOf("day");
};

export const WeekNavigator: React.FC<WeekNavigatorProps> = ({
  weekStart,
  onChangeWeekStart,
  className = "",
  yearRange,
}) => {
  const weekDays = useMemo(
    () => Array.from({ length: 7 }, (_, i) => dayjs(weekStart).add(i, "day")),
    [weekStart]
  );
  const weekLabel = `${weekDays[0].format("DD/MM")} - ${weekDays[6].format(
    "DD/MM"
  )}`;
  const yearLabel = weekDays[6].format("YYYY");

  const month = dayjs(weekStart).month() + 1;
  const year = dayjs(weekStart).year();

  const years = useMemo(() => {
    const nowY = dayjs().year();
    const min = yearRange?.min ?? nowY - 2;
    const max = yearRange?.max ?? nowY + 2;
    return Array.from({ length: max - min + 1 }, (_, i) => min + i);
  }, [yearRange]);

  const monthOptions = useMemo(
    () =>
      Array.from({ length: 12 }, (_, i) => ({
        value: i + 1,
        label: `Tháng ${i + 1}`,
      })),
    []
  );
  const yearOptions = useMemo(
    () => years.map((y) => ({ value: y, label: `Năm ${y}` })),
    [years]
  );

  const onMonthChange = (value: number) => {
    const d = dayjs(`${year}-${String(value).padStart(2, "0")}-01`);
    onChangeWeekStart(toMonday(d).format("YYYY-MM-DD"));
  };
  const onYearChange = (value: number) => {
    const d = dayjs(`${value}-${String(month).padStart(2, "0")}-01`);
    onChangeWeekStart(toMonday(d).format("YYYY-MM-DD"));
  };

  const onPrev = () =>
    onChangeWeekStart(dayjs(weekStart).subtract(7, "day").format("YYYY-MM-DD"));
  const onNext = () =>
    onChangeWeekStart(dayjs(weekStart).add(7, "day").format("YYYY-MM-DD"));
  const onToday = () =>
    onChangeWeekStart(toMonday(dayjs()).format("YYYY-MM-DD"));

  return (
    <Card
      className={`shadow-sm border ${className}`}
      bodyStyle={{ padding: 12 }}
    >
      <Space size="middle" wrap className="w-full">
        {/* Selectors */}
        <Space size="small">
          <Text className="text-gray-600">Tháng:</Text>
          <Select
            value={month}
            onChange={onMonthChange}
            style={{ width: 120 }}
            options={monthOptions}
          />
        </Space>
        <Space size="small">
          <Text className="text-gray-600">Năm:</Text>
          <Select
            value={year}
            onChange={onYearChange}
            style={{ width: 110 }}
            options={yearOptions}
          />
        </Space>

        {/* Week label */}
        <Tag color="blue" className="!px-3 !py-1 !text-base !font-semibold">
          {weekLabel}{" "}
          <Text type="secondary" className="!ml-1">
            ({yearLabel})
          </Text>
        </Tag>

        {/* Nav buttons */}
        <Space size="small">
          <Button icon={<LeftOutlined />} onClick={onPrev} shape="circle" />
          <Button icon={<CalendarOutlined />} onClick={onToday}>
            Hiện tại
          </Button>
          <Button icon={<RightOutlined />} onClick={onNext} shape="circle" />
        </Space>
      </Space>
    </Card>
  );
};
