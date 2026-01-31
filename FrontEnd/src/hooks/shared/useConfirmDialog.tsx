import { useState, useCallback } from "react";
import { ConfirmDialog } from "../../components/admins/uiForAll/ConfirmDialog";

interface ConfirmOptions {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  variant?: "danger" | "primary" | "success";
}

export const useConfirmDialog = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [config, setConfig] = useState<ConfirmOptions>({
    title: "",
    message: "",
  });
  const [onConfirmCallback, setOnConfirmCallback] = useState<(() => void | Promise<void>) | null>(null);

  const showConfirm = useCallback((options: ConfirmOptions, onConfirm: () => void | Promise<void>) => {
    setConfig(options);
    setOnConfirmCallback(() => onConfirm);
    setIsOpen(true);
  }, []);

  const handleConfirm = useCallback(async () => {
    if (onConfirmCallback) {
      setLoading(true);
      try {
        await onConfirmCallback();
      } finally {
        setLoading(false);
        setIsOpen(false);
        setOnConfirmCallback(null);
      }
    }
  }, [onConfirmCallback]);

  const handleCancel = useCallback(() => {
    if (!loading) {
      setIsOpen(false);
      setOnConfirmCallback(null);
    }
  }, [loading]);

  const ConfirmDialogComponent = (
    <ConfirmDialog
      isOpen={isOpen}
      title={config.title}
      message={config.message}
      confirmText={config.confirmText}
      cancelText={config.cancelText}
      confirmButtonVariant={config.variant}
      onConfirm={handleConfirm}
      onCancel={handleCancel}
      loading={loading}
    />
  );

  return {
    showConfirm,
    ConfirmDialogComponent,
  };
};