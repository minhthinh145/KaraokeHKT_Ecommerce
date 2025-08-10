import { useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import type { RootState, AppDispatch } from "../redux/store";
import { updateUserThunk } from "../redux/auth/index";
import { useToast } from "./useToast";

export const useProfileForm = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { user, loading } = useSelector((state: RootState) => state.auth);
  const { showSuccess, showError } = useToast();

  const [isEditing, setIsEditing] = useState(false);

  // 🔥 SỬA: Lấy data từ user.profile thay vì trực tiếp từ user
  const [formData, setFormData] = useState({
    fullName: user?.profile?.userName || "",
    phoneNumber: user?.profile?.phone || "",
    email: user?.profile?.email || "",
    dateOfBirth: user?.profile?.birthDate || "",
  });

  // 🔥 SỬA: Update form data when user changes
  const updateFormFromUser = () => {
    if (user?.profile) {
      setFormData({
        fullName: user.profile.userName || "",
        phoneNumber: user.profile.phone || "",
        email: user.profile.email || "",
        dateOfBirth: user.profile.birthDate || "",
      });
    }
  };

  const handleInputChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleReset = () => {
    updateFormFromUser();
    setIsEditing(false);
  };

  const handleCancel = () => {
    updateFormFromUser(); // Reset to original values
    setIsEditing(false);
  };

  const handleUpdateInfo = async () => {
    if (isEditing) {
      try {
        if (!formData.fullName.trim()) {
          showError("Tên người dùng không được để trống");
          return;
        }

        if (!formData.phoneNumber.trim()) {
          showError("Số điện thoại không được để trống");
          return;
        }

        // 🔥 SỬA: Tạo updateData từ user.profile
        if (!user?.profile) {
          showError("Thông tin người dùng không tồn tại");
          return;
        }

        const updateData = {
          ...user.profile, // Spread user.profile thay vì user
          userName: formData.fullName,
          phone: formData.phoneNumber,
          email: formData.email,
          birthDate: formData.dateOfBirth,
        };

        await dispatch(updateUserThunk(updateData)).unwrap();

        setIsEditing(false);
        showSuccess("Cập nhật thông tin thành công!");
      } catch (error: any) {
        showError(error || "Cập nhật thất bại!");
      }
    } else {
      setIsEditing(true);
    }
  };

  const isFormValid = () => {
    return (
      formData.fullName.trim() !== "" &&
      formData.phoneNumber.trim() !== "" &&
      formData.email.trim() !== ""
    );
  };

  const hasChanges = () => {
    if (!user?.profile) return false;

    return (
      formData.fullName !== (user.profile.userName || "") ||
      formData.phoneNumber !== (user.profile.phone || "") ||
      formData.email !== (user.profile.email || "") ||
      formData.dateOfBirth !== (user.profile.birthDate || "")
    );
  };

  return {
    // State
    user,
    loading,
    isEditing,
    formData,

    // Actions
    handleInputChange,
    handleUpdateInfo,
    handleReset,
    handleCancel,
    setIsEditing,

    // Computed
    isFormValid: isFormValid(),
    hasChanges: hasChanges(),
    canSave: isEditing && isFormValid() && hasChanges(),

    // 🔥 THÊM: Helper để check có profile không
    hasProfile: !!user?.profile,
    profileLoaded: user?.profileLoaded || false,
  };
};
