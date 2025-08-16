using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLKho.Interfaces;
using QLQuanKaraokeHKT.Repositories.QLNhanSu.Interfaces;
using QLQuanKaraokeHKT.Repositories.QLPhong.Interfaces;

namespace QLQuanKaraokeHKT.Services.QLPhongServices
{
    public class QLPhongHatService : IQLPhongHatService
    {
        private readonly IPhongHatRepository _phongHatRepository;
        private readonly ISanPhamDichVuRepository _sanPhamRepository;
        private readonly IGiaDichVuRepository _giaDichVuRepository;
        private readonly IMapper _mapper;
        private readonly ICaLamViecRepository _caLamViecRepository;

        public QLPhongHatService(
            IPhongHatRepository phongHatRepository,
            ISanPhamDichVuRepository sanPhamRepository,
            IGiaDichVuRepository giaDichVuRepository,
            ICaLamViecRepository caLamViecRepository,
            IMapper mapper)
        {
            _phongHatRepository = phongHatRepository ?? throw new ArgumentNullException(nameof(phongHatRepository));
            _sanPhamRepository = sanPhamRepository ?? throw new ArgumentNullException(nameof(sanPhamRepository));
            _giaDichVuRepository = giaDichVuRepository ?? throw new ArgumentNullException(nameof(giaDichVuRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _caLamViecRepository = caLamViecRepository ?? throw new ArgumentNullException(nameof(caLamViecRepository));
        }

        public async Task<ServiceResult> CreatePhongHatWithFullDetailsAsync(AddPhongHatDTO addPhongHatDto)
        {
            try
            {
                if (addPhongHatDto == null)
                    return ServiceResult.Failure("Thông tin phòng hát không hợp lệ.");

                // 1. Tạo SanPhamDichVu cho phòng hát
                var sanPham = _mapper.Map<SanPhamDichVu>(addPhongHatDto);
                var createdSanPham = await _sanPhamRepository.CreateSanPhamDichVuAsync(sanPham);

                // 2. Tạo PhongHatKaraoke
                var phongHat = _mapper.Map<PhongHatKaraoke>(addPhongHatDto);
                phongHat.MaSanPham = createdSanPham.MaSanPham;
                phongHat.DangSuDung = false;
                phongHat.NgungHoatDong = false;
                var createdPhongHat = await _phongHatRepository.CreatePhongHatAsync(phongHat);

                await CreateGiaDichVuForPhongHatAsync(createdSanPham.MaSanPham, addPhongHatDto);

                var result = _mapper.Map<PhongHatDetailDTO>(createdPhongHat);
                return ServiceResult.Success("Tạo phòng hát thành công với đầy đủ thông tin.", result);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi tạo phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetAllPhongHatWithDetailsAsync()
        {
            try
            {
                var phongHats = await _phongHatRepository.GetAllPhongHatWithDetailsAsync();
                if (phongHats == null || !phongHats.Any())
                    return ServiceResult.Failure("Không có phòng hát nào trong hệ thống.");

                var phongHatDTOs = _mapper.Map<List<PhongHatDetailDTO>>(phongHats);
                return ServiceResult.Success("Lấy danh sách phòng hát thành công.", phongHatDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy danh sách phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetPhongHatDetailByIdAsync(int maPhong)
        {
            try
            {
                var phongHat = await _phongHatRepository.GetPhongHatWithDetailsByIdAsync(maPhong);
                if (phongHat == null)
                    return ServiceResult.Failure("Không tìm thấy phòng hát.");

                var phongHatDTO = _mapper.Map<PhongHatDetailDTO>(phongHat);
                return ServiceResult.Success("Lấy thông tin phòng hát thành công.", phongHatDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy thông tin phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdatePhongHatWithDetailsAsync(UpdatePhongHatDTO updatePhongHatDto)
        {
            try
            {
                if (updatePhongHatDto == null)
                    return ServiceResult.Failure("Thông tin cập nhật phòng hát không hợp lệ.");

                // 1. Lấy phòng hát hiện tại
                var existingPhongHat = await _phongHatRepository.GetPhongHatWithDetailsByIdAsync(updatePhongHatDto.MaPhong);
                if (existingPhongHat == null)
                    return ServiceResult.Failure("Không tìm thấy phòng hát cần cập nhật.");

                // 2. Cập nhật thông tin SanPhamDichVu
                if (existingPhongHat.MaSanPhamNavigation != null)
                {
                    _mapper.Map(updatePhongHatDto, existingPhongHat.MaSanPhamNavigation);
                    await _sanPhamRepository.UpdateSanPhamDichVuAsync(existingPhongHat.MaSanPhamNavigation);
                }

                //3 cập nhật thông tin phòng hát
                _mapper.Map(updatePhongHatDto, existingPhongHat);
                await _phongHatRepository.UpdatePhongHatAsync(existingPhongHat);

                if (updatePhongHatDto.CapNhatGiaThue == true)
                {
                    await UpdateGiaDichVuForPhongHatAsync(existingPhongHat.MaSanPham, updatePhongHatDto);
                }

                var data = _mapper.Map<PhongHatDetailDTO>(existingPhongHat);
                return ServiceResult.Success("Cập nhật phòng hát thành công với đầy đủ thông tin.", data);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateNgungHoatDongAsync(int maPhong, bool ngungHoatDong)
        {
            try
            {
                var result = await _phongHatRepository.UpdateNgungHoatDongAsync(maPhong, ngungHoatDong);
                if (!result)
                    return ServiceResult.Failure("Cập nhật trạng thái phòng hát thất bại.");

                var message = ngungHoatDong
                    ? "Phòng hát đã được đánh dấu ngừng hoạt động."
                    : "Phòng hát đã được đánh dấu hoạt động trở lại.";

                return ServiceResult.Success(message);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật trạng thái phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateDangSuDungAsync(int maPhong, bool dangSuDung)
        {
            try
            {
                var result = await _phongHatRepository.UpdateDangSuDungAsync(maPhong, dangSuDung);
                if (!result)
                    return ServiceResult.Failure("Cập nhật trạng thái sử dụng phòng thất bại.");

                var message = dangSuDung
                    ? "Phòng đã được đánh dấu đang sử dụng."
                    : "Phòng đã được đánh dấu không sử dụng.";

                return ServiceResult.Success(message);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật trạng thái sử dụng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetPhongHatByLoaiPhongAsync(int maLoaiPhong)
        {
            try
            {
                var phongHats = await _phongHatRepository.GetPhongHatByLoaiPhongAsync(maLoaiPhong);
                if (phongHats == null || !phongHats.Any())
                    return ServiceResult.Failure("Không có phòng hát nào thuộc loại phòng này.");

                var phongHatDTOs = _mapper.Map<List<PhongHatDetailDTO>>(phongHats);
                return ServiceResult.Success("Lấy danh sách phòng hát theo loại thành công.", phongHatDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy danh sách phòng hát theo loại: {ex.Message}");
            }
        }

        #region Helper Methods - TỰ ĐỘNG TỪNG CA GIỐNG NHƯ VATLIEUSERVICE

        private async Task CreateGiaDichVuForPhongHatAsync(int maSanPham, AddPhongHatDTO dto)
        {
            if (dto.DongGiaAllCa)
            {
                // Tạo giá chung cho tất cả ca (MaCa = null)
                if (dto.GiaThueChung.HasValue)
                {
                    var giaDichVu = _mapper.Map<GiaDichVu>(dto);
                    giaDichVu.MaSanPham = maSanPham;
                    giaDichVu.MaCa = null; // Đồng giá
                    giaDichVu.DonGia = dto.GiaThueChung.Value;
                    await _giaDichVuRepository.CreateGiaDichVuAsync(giaDichVu);
                }
            }
            else
            {
                // Tạo giá riêng cho từng ca
                var caLamViecs = new[] {
                    new { TenCa = "Ca 1", Gia = dto.GiaThueCa1 },
                    new { TenCa = "Ca 2", Gia = dto.GiaThueCa2 },
                    new { TenCa = "Ca 3", Gia = dto.GiaThueCa3 }
                };

                foreach (var ca in caLamViecs)
                {
                    if (ca.Gia.HasValue)
                    {
                        // Lấy MaCa từ tên ca
                        var caLamViec = await _caLamViecRepository.GetIdCaLamViecByTenCaAsync(ca.TenCa);
                        if (caLamViec != null)
                        {
                            var giaDichVu = _mapper.Map<GiaDichVu>(dto);
                            giaDichVu.MaSanPham = maSanPham;
                            giaDichVu.MaCa = caLamViec;
                            giaDichVu.DonGia = ca.Gia.Value;
                            await _giaDichVuRepository.CreateGiaDichVuAsync(giaDichVu);
                        }
                    }
                }
            }
        }

        private async Task UpdateGiaDichVuForPhongHatAsync(int maSanPham, UpdatePhongHatDTO dto)
        {
            // Vô hiệu hóa các giá cũ
            var oldGiaDichVus = await _giaDichVuRepository.GetGiaDichVuByMaSanPhamAsync(maSanPham);
            foreach (var oldGia in oldGiaDichVus.Where(g => g.TrangThai == "HieuLuc"))
            {
                await _giaDichVuRepository.UpdateGiaDichVuStatusAsync(oldGia.MaGiaDichVu, "HetHieuLuc");
            }

            if (dto.DongGiaAllCa == true)
            {
                // Tạo giá chung cho tất cả ca (MaCa = null)
                if (dto.GiaThueChung.HasValue)
                {
                    var giaDichVu = _mapper.Map<GiaDichVu>(dto);
                    giaDichVu.MaSanPham = maSanPham;
                    giaDichVu.MaCa = null; // Đồng giá
                    giaDichVu.DonGia = dto.GiaThueChung.Value;
                    await _giaDichVuRepository.CreateGiaDichVuAsync(giaDichVu);
                }
            }
            else if (dto.DongGiaAllCa == false)
            {
                // Tạo giá riêng cho từng ca
                var caLamViecs = new[] {
                    new { TenCa = "Ca 1", Gia = dto.GiaThueCa1 },
                    new { TenCa = "Ca 2", Gia = dto.GiaThueCa2 },
                    new { TenCa = "Ca 3", Gia = dto.GiaThueCa3 }
                };

                foreach (var ca in caLamViecs)
                {
                    if (ca.Gia.HasValue)
                    {
                        // Lấy MaCa từ tên ca
                        var caLamViec = await _caLamViecRepository.GetIdCaLamViecByTenCaAsync(ca.TenCa);
                        if (caLamViec != null)
                        {
                            var giaDichVu = _mapper.Map<GiaDichVu>(dto);
                            giaDichVu.MaSanPham = maSanPham;
                            giaDichVu.MaCa = caLamViec;
                            giaDichVu.DonGia = ca.Gia.Value;
                            await _giaDichVuRepository.CreateGiaDichVuAsync(giaDichVu);
                        }
                    }
                }
            }
        }

        #endregion
    }
}