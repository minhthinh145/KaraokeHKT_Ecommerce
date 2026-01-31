import React, { useState } from "react";
import { ProfileHeader } from "../components/customer/userProfile/ProfileHeader";
import { ProfileForm } from "../components/customer/userProfile/ProfileForm";
import { ProfileStats } from "../components/customer/userProfile/ProfileStats";
import { ChangePasswordSection } from "../components/customer/ChangePassword/ChangePasswordSection";
import { NoFooterLayOut } from "../components/customer/Main/NoFooterLayout";
import { useProfileForm } from "../hooks/useProfileForm";
import { UserIcon, LockClosedIcon } from "@heroicons/react/24/outline";

export const ProfilePage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<"profile" | "password">("profile");

  const {
    user,
    loading,
    isEditing,
    formData,
    handleInputChange,
    handleUpdateInfo,
    handleCancel,
    canSave,
  } = useProfileForm();
  if (!user) {
    return (
      <NoFooterLayOut>
        <div className="flex items-center justify-center min-h-[60vh]">
          <div className="text-lg text-gray-600">Bạn chưa đăng nhập.</div>
        </div>
      </NoFooterLayOut>
    );
  }
  return (
    <NoFooterLayOut>
      <div className="bg-gray-50 py-4">
        <div className="max-w-6xl mx-auto px-2 sm:px-4 lg:px-6">
          <div className="bg-white rounded-xl shadow-lg overflow-hidden">
            {/* Header với tabs */}
            <ProfileHeader
              user={user}
              isEditing={isEditing}
              onUpdateInfo={handleUpdateInfo}
              onCancel={handleCancel}
              loading={loading}
              canSave={canSave}
              activeTab={activeTab}
              onTabChange={setActiveTab}
            />

            {/* Tab Navigation */}
            <div className="border-b border-gray-200">
              <nav className="px-6 flex space-x-8">
                <button
                  onClick={() => setActiveTab("profile")}
                  className={`py-4 px-1 border-b-2 font-medium text-sm flex items-center gap-2 ${
                    activeTab === "profile"
                      ? "border-indigo-500 text-indigo-600"
                      : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300"
                  }`}
                >
                  <UserIcon className="w-4 h-4" />
                  Thông tin cá nhân
                </button>
                <button
                  onClick={() => setActiveTab("password")}
                  className={`py-4 px-1 border-b-2 font-medium text-sm flex items-center gap-2 ${
                    activeTab === "password"
                      ? "border-red-500 text-red-600"
                      : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300"
                  }`}
                >
                  <LockClosedIcon className="w-4 h-4" />
                  Đổi mật khẩu
                </button>
              </nav>
            </div>

            {/* Tab Content */}
            {activeTab === "profile" ? (
              <>
                <ProfileForm
                  formData={formData}
                  isEditing={isEditing}
                  onInputChange={handleInputChange}
                />
                <ProfileStats />
              </>
            ) : (
              <ChangePasswordSection />
            )}
          </div>
        </div>
      </div>
    </NoFooterLayOut>
  );
};
