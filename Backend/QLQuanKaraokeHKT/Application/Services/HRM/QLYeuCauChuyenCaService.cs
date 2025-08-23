using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;
using QLQuanKaraokeHKT.Core.Interfaces.Services.HRM;

namespace QLQuanKaraokeHKT.Application.Services.HRM
{
    public class QLYeuCauChuyenCaService : IQLYeuCauChuyenCaService
    {
        private readonly IYeuCauChuyenCaRepository _yeuCauChuyenCaRepository;
        private readonly ILichLamViecRepository _lichLamViecRepository;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _sendEmailService;

        public QLYeuCauChuyenCaService(
            IYeuCauChuyenCaRepository yeuCauChuyenCaRepository,
            ILichLamViecRepository lichLamViecRepository,
            ISendEmailService sendEmailService,
            IMapper mapper)
        {
            _yeuCauChuyenCaRepository = yeuCauChuyenCaRepository ?? throw new ArgumentNullException(nameof(yeuCauChuyenCaRepository));
            _lichLamViecRepository = lichLamViecRepository ?? throw new ArgumentNullException(nameof(lichLamViecRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sendEmailService = sendEmailService ?? throw new ArgumentNullException(nameof(sendEmailService));
        }

        public async Task<ServiceResult> CreateYeuCauChuyenCaAsync(AddYeuCauChuyenCaDTO addYeuCauChuyenCaDto)
        {
            try
            {
                if (addYeuCauChuyenCaDto == null)
                    return ServiceResult.Failure("Thông tin yêu cầu chuyển ca không hợp lệ.");

                // Kiểm tra lịch làm việc gốc có tồn tại
                var lichLamViecGoc = await _lichLamViecRepository.GetLichLamViecByIdAsync(addYeuCauChuyenCaDto.MaLichLamViecGoc);
                if (lichLamViecGoc == null)
                    return ServiceResult.Failure("Không tìm thấy lịch làm việc gốc.");

                // Kiểm tra xem có xung đột lịch làm việc không
                var hasConflict = await _yeuCauChuyenCaRepository.CheckConflictLichLamViecAsync(
                    lichLamViecGoc.MaNhanVien,
                    addYeuCauChuyenCaDto.NgayLamViecMoi,
                    addYeuCauChuyenCaDto.MaCaMoi);

                if (hasConflict)
                    return ServiceResult.Failure("Nhân viên đã có lịch làm việc trong ngày và ca này.");

                // Tạo yêu cầu chuyển ca
                var yeuCauChuyenCa = _mapper.Map<YeuCauChuyenCa>(addYeuCauChuyenCaDto);
                var createdYeuCau = await _yeuCauChuyenCaRepository.CreateYeuCauChuyenCaAsync(yeuCauChuyenCa);

                var result = _mapper.Map<YeuCauChuyenCaDTO>(createdYeuCau);
                return ServiceResult.Success("Tạo yêu cầu chuyển ca thành công.", result);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi tạo yêu cầu chuyển ca: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetAllYeuCauChuyenCaAsync()
        {
            try
            {
                var yeuCauList = await _yeuCauChuyenCaRepository.GetAllYeuCauChuyenCaAsync();
                if (yeuCauList == null || !yeuCauList.Any())
                    return ServiceResult.Failure("Không có yêu cầu chuyển ca nào trong hệ thống.");

                var yeuCauDTOs = _mapper.Map<List<YeuCauChuyenCaDTO>>(yeuCauList);
                return ServiceResult.Success("Lấy danh sách yêu cầu chuyển ca thành công.", yeuCauDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy danh sách yêu cầu chuyển ca: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetYeuCauChuyenCaByNhanVienAsync(Guid maNhanVien)
        {
            try
            {
                var yeuCauList = await _yeuCauChuyenCaRepository.GetYeuCauChuyenCaByNhanVienAsync(maNhanVien);
                if (yeuCauList == null || !yeuCauList.Any())
                    return ServiceResult.Failure("Nhân viên chưa có yêu cầu chuyển ca nào.");

                var yeuCauDTOs = _mapper.Map<List<YeuCauChuyenCaDTO>>(yeuCauList);
                return ServiceResult.Success("Lấy danh sách yêu cầu chuyển ca của nhân viên thành công.", yeuCauDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy yêu cầu chuyển ca của nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetYeuCauChuyenCaChuaPheDuyetAsync()
        {
            try
            {
                var yeuCauList = await _yeuCauChuyenCaRepository.GetYeuCauChuyenCaChuaPheDuyetAsync();
                if (yeuCauList == null || !yeuCauList.Any())
                    return ServiceResult.Failure("Không có yêu cầu chuyển ca nào cần phê duyệt.");

                var yeuCauDTOs = _mapper.Map<List<YeuCauChuyenCaDTO>>(yeuCauList);
                return ServiceResult.Success("Lấy danh sách yêu cầu chuyển ca cần phê duyệt thành công.", yeuCauDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy yêu cầu chuyển ca cần phê duyệt: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetYeuCauChuyenCaDaPheDuyetAsync()
        {
            try
            {
                var yeuCauList = await _yeuCauChuyenCaRepository.GetYeuCauChuyenCaDaPheDuyetAsync();
                if (yeuCauList == null || !yeuCauList.Any())
                    return ServiceResult.Failure("Không có yêu cầu chuyển ca nào đã phê duyệt.");

                var yeuCauDTOs = _mapper.Map<List<YeuCauChuyenCaDTO>>(yeuCauList);
                return ServiceResult.Success("Lấy danh sách yêu cầu chuyển ca đã phê duyệt thành công.", yeuCauDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy yêu cầu chuyển ca đã phê duyệt: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetYeuCauChuyenCaByIdAsync(int maYeuCau)
        {
            try
            {
                var yeuCau = await _yeuCauChuyenCaRepository.GetYeuCauChuyenCaByIdAsync(maYeuCau);
                if (yeuCau == null)
                    return ServiceResult.Failure("Không tìm thấy yêu cầu chuyển ca.");

                var yeuCauDTO = _mapper.Map<YeuCauChuyenCaDTO>(yeuCau);
                return ServiceResult.Success("Lấy thông tin yêu cầu chuyển ca thành công.", yeuCauDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy thông tin yêu cầu chuyển ca: {ex.Message}");
            }
        }

        public async Task<ServiceResult> PheDuyetYeuCauChuyenCaAsync(PheDuyetYeuCauChuyenCaDTO pheDuyetDto)
        {
            try
            {
                if (pheDuyetDto == null)
                    return ServiceResult.Failure("Thông tin phê duyệt không hợp lệ.");

                // 1. Lấy thông tin yêu cầu chuyển ca TRƯỚC KHI duyệt
                var yeuCau = await _yeuCauChuyenCaRepository.GetYeuCauChuyenCaByIdAsync(pheDuyetDto.MaYeuCau);
                if (yeuCau == null)
                    return ServiceResult.Failure("Không tìm thấy yêu cầu chuyển ca.");

                if (yeuCau.DaPheDuyet)
                    return ServiceResult.Failure("Yêu cầu chuyển ca đã được phê duyệt.");

                // 2. Phê duyệt yêu cầu (chỉ update trạng thái)
                var result = await _yeuCauChuyenCaRepository.PheDuyetYeuCauChuyenCaAsync(
                    pheDuyetDto.MaYeuCau,
                    pheDuyetDto.KetQuaPheDuyet,
                    pheDuyetDto.GhiChuPheDuyet);

                if (!result)
                    return ServiceResult.Failure("Phê duyệt yêu cầu chuyển ca thất bại.");

                // 3. NẾU DUYỆT -> CẬP NHẬT LỊCH LÀM VIỆC bằng method mới
                if (pheDuyetDto.KetQuaPheDuyet)
                {
                    var updateResult = await _lichLamViecRepository.UpdateLichLamViecForYeuCauChuyenCaAsync(
                        yeuCau.MaLichLamViecGoc,
                        yeuCau.NgayLamViecMoi,
                        yeuCau.MaCaMoi);

                    if (!updateResult)
                        return ServiceResult.Failure("Cập nhật lịch làm việc thất bại. Có thể bị xung đột lịch làm việc.");
                }

                // 4. Gửi email thông báo
                var emailBody = $"Yêu cầu chuyển ca của bạn từ {yeuCau.LichLamViecGoc.CaLamViec?.TenCa} - {yeuCau.LichLamViecGoc.NgayLamViec:dd/MM/yyyy} " +
                                $"đến {yeuCau.CaMoi?.TenCa} - {yeuCau.NgayLamViecMoi:dd/MM/yyyy} " +
                                $"đã {(pheDuyetDto.KetQuaPheDuyet ? "được chấp nhận" : "bị từ chối")}. " +
                                $"Ghi chú: {pheDuyetDto.GhiChuPheDuyet ?? "(không có)"}";

                await _sendEmailService.SendEmailByContentAsync(
                    yeuCau.LichLamViecGoc.NhanVien.Email,
                    "Thông báo phê duyệt yêu cầu chuyển ca",
                    emailBody);

                var message = pheDuyetDto.KetQuaPheDuyet
                    ? "Chấp nhận yêu cầu chuyển ca thành công."
                    : "Từ chối yêu cầu chuyển ca thành công.";

                // 5. TRẢ VỀ DTO THAY VÌ ENTITY ĐỂ TRÁNH CYCLE
                return ServiceResult.Success(message, new
                {
                    pheDuyetDto.MaYeuCau,
                    pheDuyetDto.KetQuaPheDuyet,
                    pheDuyetDto.GhiChuPheDuyet,
                    NgayPheDuyet = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi phê duyệt yêu cầu chuyển ca: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteYeuCauChuyenCaAsync(int maYeuCau)
        {
            try
            {
                var yeuCau = await _yeuCauChuyenCaRepository.GetYeuCauChuyenCaByIdAsync(maYeuCau);
                if (yeuCau == null)
                    return ServiceResult.Failure("Không tìm thấy yêu cầu chuyển ca.");

                if (yeuCau.DaPheDuyet)
                    return ServiceResult.Failure("Không thể xóa yêu cầu đã được phê duyệt.");

                var result = await _yeuCauChuyenCaRepository.DeleteYeuCauChuyenCaAsync(maYeuCau);
                if (!result)
                    return ServiceResult.Failure("Xóa yêu cầu chuyển ca thất bại.");

                return ServiceResult.Success("Xóa yêu cầu chuyển ca thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi xóa yêu cầu chuyển ca: {ex.Message}");
            }
        }
    }
}