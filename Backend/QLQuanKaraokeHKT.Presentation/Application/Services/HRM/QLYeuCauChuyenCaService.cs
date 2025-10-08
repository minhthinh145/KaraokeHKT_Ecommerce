using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;
using QLQuanKaraokeHKT.Core.Interfaces.Services.HRM;

namespace QLQuanKaraokeHKT.Application.Services.HRM
{
    public class QLYeuCauChuyenCaService : IQLYeuCauChuyenCaService
    {
        private readonly IMapper _mapper;
        private readonly ISendEmailService _sendEmailService;
        private readonly IUnitOfWork _unitOfWork;

        public QLYeuCauChuyenCaService(
            IUnitOfWork unitOfWork,
            ISendEmailService sendEmailService,
            IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sendEmailService = sendEmailService ?? throw new ArgumentNullException(nameof(sendEmailService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ServiceResult> CreateYeuCauChuyenCaAsync(AddYeuCauChuyenCaDTO addYeuCauChuyenCaDto)
        {
            try
            {
                if (addYeuCauChuyenCaDto == null)
                    return ServiceResult.Failure("Thông tin yêu cầu chuyển ca không hợp lệ.");

                var lichLamViecGoc = await _unitOfWork.YeuCauChuyenCaRepository.GetByIdAsync(addYeuCauChuyenCaDto.MaLichLamViecGoc);
                if (lichLamViecGoc == null)
                    return ServiceResult.Failure("Không tìm thấy lịch làm việc gốc.");

                var hasConflict = await _unitOfWork.YeuCauChuyenCaRepository.CheckConflictLichLamViecAsync(
                    lichLamViecGoc.LichLamViecGoc.MaNhanVien,
                    addYeuCauChuyenCaDto.NgayLamViecMoi,
                    addYeuCauChuyenCaDto.MaCaMoi);

                if (hasConflict)
                    return ServiceResult.Failure("Nhân viên đã có lịch làm việc trong ngày và ca này.");

                var yeuCauChuyenCa = _mapper.Map<YeuCauChuyenCa>(addYeuCauChuyenCaDto);
                var createdYeuCau = await _unitOfWork.YeuCauChuyenCaRepository.CreateAsync(yeuCauChuyenCa);

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
                var yeuCauList = await _unitOfWork.YeuCauChuyenCaRepository.GetAllAsync();
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
                var yeuCauList = await _unitOfWork.YeuCauChuyenCaRepository.GetYeuCauChuyenCaByNhanVienAsync(maNhanVien);
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
                var yeuCauList = await _unitOfWork.YeuCauChuyenCaRepository.GetYeuCauChuyenCaChuaPheDuyetAsync();
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
                var yeuCauList = await _unitOfWork.YeuCauChuyenCaRepository.GetYeuCauChuyenCaDaPheDuyetAsync();
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
                var yeuCau = await _unitOfWork.YeuCauChuyenCaRepository.GetByIdAsync(maYeuCau);
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
                var result = await _unitOfWork.ExecuteTransactionAsync(async () =>
                {

                    var yeuCau = await _unitOfWork.YeuCauChuyenCaRepository.GetByIdAsync(pheDuyetDto.MaYeuCau);
                    if (yeuCau == null)
                        throw new InvalidOperationException("Không tìm thấy yêu cầu chuyển ca.");

                    var pheDuyetResult = await _unitOfWork.YeuCauChuyenCaRepository.PheDuyetYeuCauChuyenCaAsync(
                        pheDuyetDto.MaYeuCau,
                        pheDuyetDto.KetQuaPheDuyet,
                        pheDuyetDto.GhiChuPheDuyet);

                    if (!pheDuyetResult)
                        throw new InvalidOperationException("Phê duyệt yêu cầu chuyển ca thất bại.");

                    if (pheDuyetDto.KetQuaPheDuyet)
                    {
                        var updateResult = await _unitOfWork.LichLamViecRepository.UpdateLichLamViecForYeuCauChuyenCaAsync(
                            yeuCau.MaLichLamViecGoc,
                            yeuCau.NgayLamViecMoi,
                            yeuCau.MaCaMoi);

                        if (!updateResult)
                            throw new InvalidOperationException("Cập nhật lịch làm việc thất bại. Có thể bị xung đột lịch làm việc.");
                    }

                    await _sendEmailService.SendShiftChangeApprovalEmailAsync(
                                                yeuCau.LichLamViecGoc.NhanVien.Email,
                        yeuCau.LichLamViecGoc.NhanVien.HoTen,
                        pheDuyetDto.KetQuaPheDuyet,
                        yeuCau.LichLamViecGoc.CaLamViec?.TenCa ?? "N/A",
                        yeuCau.CaMoi?.TenCa ?? "N/A",
                        yeuCau.LichLamViecGoc.NgayLamViec,
                        yeuCau.NgayLamViecMoi,
                        pheDuyetDto.GhiChuPheDuyet
                    );

                    return new
                    {
                        pheDuyetDto.MaYeuCau,
                        pheDuyetDto.KetQuaPheDuyet,
                        pheDuyetDto.GhiChuPheDuyet,
                        NgayPheDuyet = DateTime.Now
                    };
                });

                return ServiceResult.Success("Phê duyệt yêu cầu chuyển ca thành công.", result);
            }
            catch (InvalidOperationException ex)
            {
                return ServiceResult.Failure(ex.Message);
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
                var yeuCau = await _unitOfWork.YeuCauChuyenCaRepository.GetByIdAsync(maYeuCau);
                if (yeuCau == null)
                    return ServiceResult.Failure("Không tìm thấy yêu cầu chuyển ca.");

                if (yeuCau.DaPheDuyet)
                    return ServiceResult.Failure("Không thể xóa yêu cầu đã được phê duyệt.");

                var result = await _unitOfWork.YeuCauChuyenCaRepository.DeleteAsync(maYeuCau);
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