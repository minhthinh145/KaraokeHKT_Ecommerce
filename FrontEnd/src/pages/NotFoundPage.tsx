export const NotFoundPage: React.FC = () => {
  return (
    <div className="flex items-center justify-center min-h-[400px]">
      <div className="text-center space-y-4">
        <div className="text-6xl">🚫</div>
        <h2 className="text-2xl font-bold text-gray-700">404 - Not Found</h2>
        <p className="text-gray-500 max-w-md">
          Trang bạn tìm kiếm không tồn tại. Vui lòng kiểm tra lại đường dẫn.
        </p>
      </div>
    </div>
  );
};
