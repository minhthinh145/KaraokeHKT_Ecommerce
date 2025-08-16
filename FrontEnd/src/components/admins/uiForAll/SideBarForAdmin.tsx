import React, { useState, useMemo, useCallback } from "react";
import { ChevronDownIcon, ChevronRightIcon } from "@heroicons/react/24/outline";

export interface SideBarForAdminProps<
  TabParent extends string,
  TabChild extends string
> {
  isCollapsed: boolean;
  activeParent: TabParent;
  activeTab: TabChild;
  onParentChange: (parent: TabParent) => void;
  onTabChange: (tab: TabChild) => void;
  treeData: {
    id: TabParent;
    title: string;
    icon: React.ComponentType<{ className?: string }>;
    color: "blue" | "green" | "purple" | "orange" | "red";
    children: {
      id: TabChild;
      title: string;
      icon: React.ComponentType<{ className?: string }>;
    }[];
  }[];
  sidebarTitle?: string;
}

export function SideBarForAdmin<
  TabParent extends string,
  TabChild extends string
>({
  isCollapsed,
  activeParent,
  activeTab,
  onParentChange,
  onTabChange,
  treeData,
  sidebarTitle = "Danh mục",
}: SideBarForAdminProps<TabParent, TabChild>) {
  const [expanded, setExpanded] = useState<Record<string, boolean>>(() =>
    Object.fromEntries(treeData.map((n) => [n.id, n.id === activeParent]))
  );

  React.useEffect(() => {
    setExpanded((prev) => ({ ...prev, [activeParent]: true }));
  }, [activeParent]);

  const toggleParent = useCallback((p: TabParent) => {
    setExpanded((prev) => ({
      ...prev,
      [p]: !prev[p],
    }));
    // XÓA dòng này để không set activeParent khi bấm cha
    // if (p !== activeParent) onParentChange(p);
  }, []);

  const colorStyles = useMemo(
    () => ({
      blue: {
        activeBg: "bg-blue-100",
        ring: "ring-2 ring-blue-300",
        childActive: "bg-blue-50 text-blue-700",
        hoverParent: "hover:bg-blue-50",
        dot: "bg-blue-400",
      },
      green: {
        activeBg: "bg-green-100",
        ring: "ring-2 ring-green-300",
        childActive: "bg-green-50 text-green-700",
        hoverParent: "hover:bg-green-50",
        dot: "bg-green-400",
      },
      purple: {
        activeBg: "bg-purple-100",
        ring: "ring-2 ring-purple-300",
        childActive: "bg-purple-50 text-purple-700",
        hoverParent: "hover:bg-purple-50",
        dot: "bg-purple-400",
      },
      orange: {
        activeBg: "bg-orange-100",
        ring: "ring-2 ring-orange-300",
        childActive: "bg-orange-50 text-orange-700",
        hoverParent: "hover:bg-orange-50",
        dot: "bg-orange-400",
      },
      red: {
        activeBg: "bg-red-100",
        ring: "ring-2 ring-red-300",
        childActive: "bg-red-50 text-red-700",
        hoverParent: "hover:bg-red-50",
        dot: "bg-red-400",
      },
    }),
    []
  );

  const renderParent = (node: (typeof treeData)[number]) => {
    // Cha active nếu: chính nó active hoặc có con active
    const isActiveParent =
      activeParent === node.id ||
      node.children.some((child) => activeTab === child.id);
    const isExpanded = expanded[node.id];
    const styles = colorStyles[node.color];

    return (
      <div key={node.id} className="select-none">
        <button
          onClick={() => toggleParent(node.id)}
          className={`group w-full flex items-center justify-between gap-3 px-3 py-2 rounded-lg transition-colors text-left
            ${
              isActiveParent
                ? `${styles.activeBg} ${styles.ring}`
                : `bg-white ${styles.hoverParent}`
            }
          `}
        >
          <div className="flex items-center gap-3 overflow-hidden">
            <node.icon className="w-6 h-6 flex-shrink-0" />
            {!isCollapsed && (
              <span
                className={`font-semibold text-sm truncate ${
                  isActiveParent ? "text-black" : "text-gray-700"
                }`}
              >
                {node.title}
              </span>
            )}
          </div>
          {!isCollapsed && (
            <span
              className={`transition-transform ${
                isExpanded ? "rotate-180" : ""
              } text-gray-500 group-hover:text-gray-700`}
            >
              <ChevronDownIcon className="w-5 h-5" />
            </span>
          )}
          {isCollapsed && (
            <span className="text-gray-500">
              {isExpanded ? (
                <ChevronDownIcon className="w-5 h-5" />
              ) : (
                <ChevronRightIcon className="w-5 h-5" />
              )}
            </span>
          )}
        </button>

        {/* Children */}
        {isExpanded && !isCollapsed && (
          <div className="mt-1 pl-2 border-l border-neutral-200 space-y-1">
            {node.children.map((child) => {
              const ChildIcon = child.icon;
              const isActiveChild = activeTab === child.id;
              return (
                <button
                  key={child.id}
                  onClick={() => {
                    // Khi chọn con, set luôn parent là active
                    if (activeParent !== node.id) onParentChange(node.id);
                    onTabChange(child.id);
                  }}
                  className={`w-full flex items-center gap-2 px-3 py-2 rounded-md text-sm transition-colors
                    ${
                      isActiveChild
                        ? `${styles.childActive} font-semibold shadow-sm`
                        : "text-gray-600 hover:bg-neutral-50 hover:text-gray-900"
                    }
                  `}
                >
                  <span
                    className={`w-1.5 h-1.5 rounded-full ${
                      isActiveChild ? styles.dot : "bg-gray-300"
                    }`}
                  />
                  <ChildIcon className="w-4 h-4 flex-shrink-0 opacity-70" />
                  <span className="truncate">{child.title}</span>
                </button>
              );
            })}
          </div>
        )}
      </div>
    );
  };

  return (
    <div className="p-4 space-y-4">
      {!isCollapsed && (
        <div className="px-2">
          <h3 className="text-xs font-bold tracking-widest text-gray-500 uppercase">
            {sidebarTitle}
          </h3>
        </div>
      )}

      <div
        className={`space-y-2 ${
          isCollapsed ? "flex flex-col items-center gap-2" : ""
        }`}
      >
        {treeData.map(renderParent)}
      </div>

      {/* Collapsed hint */}
      {isCollapsed && (
        <div className="mt-4 flex flex-col items-center gap-1">
          <span className="text-[10px] tracking-wide uppercase text-gray-400">
            Mở rộng
          </span>
        </div>
      )}
    </div>
  );
}
