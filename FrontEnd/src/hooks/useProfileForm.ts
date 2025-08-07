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
  const [formData, setFormData] = useState({
    fullName: user?.userName || "",
    phoneNumber: user?.phone || "",
    email: user?.email || "",
    dateOfBirth: user?.birthDate || "",
  });

  // 🔥 Update form data when user changes
  const updateFormFromUser = () => {
    if (user) {
      setFormData({
        fullName: user.userName || "",
        phoneNumber: user.phone || "",
        email: user.email || "",
        dateOfBirth: user.birthDate || "",
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

        // 🔥 Debug: Log data trước khi gửi
        const updateData = {
          ...user!,
          userName: formData.fullName,
          phone: formData.phoneNumber,
          birthDate: formData.dateOfBirth,
        };

        console.log("🔍 Data gửi đi:", updateData);
        console.log("🔍 formData.fullName:", formData.fullName);
        console.log("🔍 updateData.userName:", updateData.userName);

        await dispatch(updateUserThunk(updateData)).unwrap();

        setIsEditing(false);
        showSuccess("Cập nhật thông tin thành công!");
      } catch (error: any) {
        console.log("❌ Update error:", error);
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
    if (!user) return false;

    return (
      formData.fullName !== (user.userName || "") ||
      formData.phoneNumber !== (user.phone || "") ||
      formData.dateOfBirth !== (user.birthDate || "")
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
  };
};
