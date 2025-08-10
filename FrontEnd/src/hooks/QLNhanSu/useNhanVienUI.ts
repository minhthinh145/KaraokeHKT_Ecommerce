import { useCallback } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  setShowAddModal,
  setShowEditModal,
  setSelectedNhanVien,
  selectShowAddModalQLNhanSu,
  selectShowEditModalQLNhanSu,
  selectSelectedNhanVienQLNhanSu,
  selectUIStateQLNhanSu,
} from "../../redux/admin";
import type { NhanVienDTO } from "../../api/types/admins/QLNhanSutypes";

export const useNhanVienUI = () => {
  const dispatch = useAppDispatch();

  const uiState = useAppSelector(selectUIStateQLNhanSu);
  const showAddModal = useAppSelector(selectShowAddModalQLNhanSu);
  const showEditModal = useAppSelector(selectShowEditModalQLNhanSu);
  const selectedNhanVien = useAppSelector(selectSelectedNhanVienQLNhanSu);

  const openAddModal = useCallback(() => {
    dispatch(setShowAddModal(true));
  }, [dispatch]);

  const closeAddModal = useCallback(() => {
    dispatch(setShowAddModal(false));
  }, [dispatch]);

  const openEditModal = useCallback(
    (nhanVien: NhanVienDTO) => {
      dispatch(setSelectedNhanVien(nhanVien));
      dispatch(setShowEditModal(true));
    },
    [dispatch]
  );

  const closeEditModal = useCallback(() => {
    dispatch(setShowEditModal(false));
    dispatch(setSelectedNhanVien(null));
  }, [dispatch]);

  return {
    uiState,
    showAddModal,
    showEditModal,
    selectedNhanVien,
    openAddModal,
    closeAddModal,
    openEditModal,
    closeEditModal,
  };
};
