using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLKhoDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Inventory;

namespace QLQuanKaraokeHKT.Application.Services.Inventory
{
    /// <summary>
    /// Service implementation for managing VatLieu operations.
    /// </summary>
    public class QLVatLieuService : IQLVatLieuService
    {
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly ISanPhamDichVuRepository _sanPhamRepository;
        private readonly IMonAnRepository _monAnRepository;
        private readonly IGiaVatLieuRepository _giaVatLieuRepository;
        private readonly IGiaDichVuRepository _giaDichVuRepository;
        private readonly ICaLamViecRepository _caLamViecRepository;
        private readonly IMapper _mapper;

        public QLVatLieuService(
            IVatLieuRepository vatLieuRepository,
            ISanPhamDichVuRepository sanPhamRepository,
            IMonAnRepository monAnRepository,
            IGiaVatLieuRepository giaVatLieuRepository,
            IGiaDichVuRepository giaDichVuRepository,
            ICaLamViecRepository caLamViecRepository,
            IMapper mapper)
        {
            _vatLieuRepository = vatLieuRepository ?? throw new ArgumentNullException(nameof(vatLieuRepository));
            _sanPhamRepository = sanPhamRepository ?? throw new ArgumentNullException(nameof(sanPhamRepository));
            _monAnRepository = monAnRepository ?? throw new ArgumentNullException(nameof(monAnRepository));
            _giaVatLieuRepository = giaVatLieuRepository ?? throw new ArgumentNullException(nameof(giaVatLieuRepository));
            _giaDichVuRepository = giaDichVuRepository ?? throw new ArgumentNullException(nameof(giaDichVuRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _caLamViecRepository = caLamViecRepository ?? throw new ArgumentNullException(nameof(caLamViecRepository));
        }

        public async Task<ServiceResult> CreateVatLieuWithFullDetailsAsync(AddVatLieuDTO addVatLieuDto)
        {
            try
            {
                if (addVatLieuDto == null)
                    return ServiceResult.Failure("Thông tin vật liệu không hợp lệ.");

                // 1. Tạo SanPhamDichVu
                var sanPham = _mapper.Map<SanPhamDichVu>(addVatLieuDto);
                var createdSanPham = await _sanPhamRepository.CreateAsync(sanPham);

                // 2. Tạo VatLieu
                var vatLieu = _mapper.Map<VatLieu>(addVatLieuDto);
                vatLieu.NgungCungCap = false;
                var createdVatLieu = await _vatLieuRepository.CreateVatLieuAsync(vatLieu);

                // 3. Tạo MonAn liên kết
                var monAn = new MonAn
                {
                    MaSanPham = createdSanPham.MaSanPham,
                    MaVatLieu = createdVatLieu.MaVatLieu,
                    SoLuongConLai = addVatLieuDto.SoLuongTonKho
                };
                await _monAnRepository.CreateMonAnAsync(monAn);

                // 4. Tạo GiaVatLieu (giá nhập)
                var giaVatLieu = _mapper.Map<GiaVatLieu>(addVatLieuDto);
                giaVatLieu.MaVatLieu = createdVatLieu.MaVatLieu;
                await _giaVatLieuRepository.CreateGiaVatLieuAsync(giaVatLieu);

                await CreateGiaDichVuForVatLieuAsync(createdSanPham.MaSanPham, addVatLieuDto);

                return ServiceResult.Success("Tạo vật liệu thành công với đầy đủ thông tin.", createdVatLieu.MaVatLieu);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi tạo vật liệu: {ex.Message}");
            }
        }


        public async Task<ServiceResult> GetAllVatLieuWithDetailsAsync()
        {
            try
            {
                var vatLieus = await _vatLieuRepository.GetAllVatLieuWithDetailsAsync();
                if (vatLieus == null || !vatLieus.Any())
                    return ServiceResult.Failure("Không có vật liệu nào trong hệ thống.");

                var vatLieuDTOs = _mapper.Map<List<VatLieuDetailDTO>>(vatLieus);
                return ServiceResult.Success("Lấy danh sách vật liệu thành công.", vatLieuDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy danh sách vật liệu: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetVatLieuDetailByIdAsync(int maVatLieu)
        {
            try
            {
                var vatLieu = await _vatLieuRepository.GetVatLieuWithDetailsByIdAsync(maVatLieu);
                if (vatLieu == null)
                    return ServiceResult.Failure("Không tìm thấy vật liệu.");

                var vatLieuDTO = _mapper.Map<VatLieuDetailDTO>(vatLieu);
                return ServiceResult.Success("Lấy thông tin vật liệu thành công.", vatLieuDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy thông tin vật liệu: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateSoLuongVatLieuAsync(UpdateSoLuongDTO updateSoLuongDto)
        {
            try
            {
                if (updateSoLuongDto == null)
                    return ServiceResult.Failure("Thông tin cập nhật không hợp lệ.");

                var updateVatLieuResult = await _vatLieuRepository.UpdateSoLuongVatLieuAsync(
                    updateSoLuongDto.MaVatLieu, updateSoLuongDto.SoLuongMoi);

                if (!updateVatLieuResult)
                    return ServiceResult.Failure("Cập nhật số lượng vật liệu thất bại.");

                await _monAnRepository.UpdateSoLuongMonAnAsync(
                    updateSoLuongDto.MaVatLieu, updateSoLuongDto.SoLuongMoi);

                return ServiceResult.Success("Cập nhật số lượng vật liệu thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật số lượng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateNgungCungCapAsync(int maVatLieu, bool ngungCungCap)
        {
            try
            {
                var result = await _vatLieuRepository.UpdateNgungCungCapAsync(maVatLieu, ngungCungCap);
                if (!result)
                    return ServiceResult.Failure("Cập nhật trạng thái vật liệu thất bại.");

                var message = ngungCungCap
                    ? "Vật liệu đã được đánh dấu ngừng cung cấp."
                    : "Vật liệu đã được đánh dấu tiếp tục cung cấp.";

                return ServiceResult.Success(message);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật trạng thái: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateVatLieuWithDetailsAsync(UpdateVatLieuDTO updateVatLieuDto)
        {
            try
            {
                if (updateVatLieuDto == null)
                    return ServiceResult.Failure("Thông tin cập nhật vật liệu không hợp lệ.");

                // 1. Lấy vật liệu hiện tại
                var existingVatLieu = await _vatLieuRepository.GetVatLieuWithDetailsByIdAsync(updateVatLieuDto.MaVatLieu);
                if (existingVatLieu == null)
                    return ServiceResult.Failure("Không tìm thấy vật liệu cần cập nhật.");

                // 2. Cập nhật thông tin VatLieu
                _mapper.Map(updateVatLieuDto, existingVatLieu);
                var updateVatLieuResult = await _vatLieuRepository.UpdateVatLieuAsync(existingVatLieu);
                if (!updateVatLieuResult)
                    return ServiceResult.Failure("Cập nhật thông tin vật liệu thất bại.");

                // 3. Cập nhật thông tin SanPhamDichVu
                if (existingVatLieu.MonAn?.MaSanPhamNavigation != null)
                {
                    _mapper.Map(updateVatLieuDto, existingVatLieu.MonAn.MaSanPhamNavigation);
                    await _sanPhamRepository.UpdateAsync(existingVatLieu.MonAn.MaSanPhamNavigation);
                }

                // 4. Cập nhật giá nhập
                if (updateVatLieuDto.GiaNhapMoi.HasValue)
                {
                    await _giaVatLieuRepository.UpdateGiaVatLieuStatusAsync(updateVatLieuDto.MaVatLieu, "HetHieuLuc");

                    var newGiaVatLieu = new GiaVatLieu
                    {
                        MaVatLieu = updateVatLieuDto.MaVatLieu,
                        DonGia = updateVatLieuDto.GiaNhapMoi.Value,
                        NgayApDung = updateVatLieuDto.NgayApDungGiaNhap ?? DateOnly.FromDateTime(DateTime.Now),
                        TrangThai = updateVatLieuDto.TrangThaiGiaNhap ?? "HieuLuc"
                    };
                    await _giaVatLieuRepository.CreateGiaVatLieuAsync(newGiaVatLieu);
                }

                // 5. ✅ CẬP NHẬT GIÁ BÁN THEO LOGIC MỚI
                if (existingVatLieu.MonAn != null && updateVatLieuDto.CapNhatGiaBan == true)
                {
                    await UpdateGiaDichVuForVatLieuAsync(existingVatLieu.MonAn.MaSanPham, updateVatLieuDto);
                }

                var data = _mapper.Map<VatLieuDetailDTO>(existingVatLieu);
                return ServiceResult.Success("Cập nhật vật liệu thành công với đầy đủ thông tin.", data);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật vật liệu: {ex.Message}");
            }
        }

        #region Helper Methods
        private async Task CreateGiaDichVuForVatLieuAsync(int maSanPham, AddVatLieuDTO dto)
        {
            if (dto.DongGiaAllCa)
            {
                // Tạo giá chung cho tất cả ca (MaCa = null)
                if (dto.GiaBanChung.HasValue)
                {
                    var giaDichVu = new GiaDichVu
                    {
                        MaSanPham = maSanPham,
                        MaCa = null, // Giá chung
                        DonGia = dto.GiaBanChung.Value,
                        NgayApDung = dto.NgayApDungGiaBan,
                        TrangThai = dto.TrangThaiGiaBan
                    };
                    await _giaDichVuRepository.CreateAsync(giaDichVu);
                }
            }
            else
            {
                var tenCaList = new List<string> { "Ca 1", "Ca 2", "Ca 3" };
                var caLamViecList = await _caLamViecRepository.GetCaLamViecByTenCaAsync(tenCaList);

                // Tạo giá riêng cho từng ca dựa trên tên ca
                foreach (var ca in caLamViecList)
                {
                    decimal? gia = GetGiaBanTheoTenCa(dto, ca.TenCa);
                    if (gia.HasValue)
                    {
                        var giaDichVu = new GiaDichVu
                        {
                            MaSanPham = maSanPham,
                            MaCa = ca.MaCa, 
                            DonGia = gia.Value,
                            NgayApDung = dto.NgayApDungGiaBan,
                            TrangThai = dto.TrangThaiGiaBan
                        };
                        await _giaDichVuRepository.CreateAsync(giaDichVu);
                    }
                }
            }
        }

        private async Task UpdateGiaDichVuForVatLieuAsync(int maSanPham, UpdateVatLieuDTO dto)
        {
            if (dto.DongGiaAllCa == true)
            {
                await _giaDichVuRepository.BulkUpdateStatusByProductAsync(maSanPham, "HetHieuLuc");

                // Tạo giá chung mới
                if (dto.GiaBanChung.HasValue)
                {
                    var giaDichVu = new GiaDichVu
                    {
                        MaSanPham = maSanPham,
                        MaCa = null, // Giá chung
                        DonGia = dto.GiaBanChung.Value,
                        NgayApDung = dto.NgayApDungGiaBan ?? DateOnly.FromDateTime(DateTime.Now),
                        TrangThai = dto.TrangThaiGiaBan ?? "HieuLuc"
                    };
                    await _giaDichVuRepository.CreateAsync(giaDichVu);
                }
            }
            else
            {
                await _giaDichVuRepository.BulkUpdateStatusByProductAsync(maSanPham, "HetHieuLuc", null);

                var tenCaList = new List<string> { "Ca 1", "Ca 2", "Ca 3" };
                var caLamViecList = await _caLamViecRepository.GetCaLamViecByTenCaAsync(tenCaList);

                // Tạo giá mới cho từng ca
                foreach (var ca in caLamViecList)
                {
                    decimal? gia = GetGiaBanTheoTenCa(dto, ca.TenCa);
                    if (gia.HasValue)
                    {
                        var giaDichVu = new GiaDichVu
                        {
                            MaSanPham = maSanPham,
                            MaCa = ca.MaCa,
                            DonGia = gia.Value,
                            NgayApDung = dto.NgayApDungGiaBan ?? DateOnly.FromDateTime(DateTime.Now),
                            TrangThai = dto.TrangThaiGiaBan ?? "HieuLuc"
                        };
                        await _giaDichVuRepository.CreateAsync(giaDichVu);
                    }
                }
            }
        }
        private decimal? GetGiaBanTheoTenCa(AddVatLieuDTO dto, string tenCa)
        {
            return tenCa switch
            {
                "Ca 1" => dto.GiaBanCa1,
                "Ca 2" => dto.GiaBanCa2,
                "Ca 3" => dto.GiaBanCa3,
                _ => null
            };
        }

        private decimal? GetGiaBanTheoTenCa(UpdateVatLieuDTO dto, string tenCa)
        {
            return tenCa switch
            {
                "Ca 1" => dto.GiaBanCa1,
                "Ca 2" => dto.GiaBanCa2,
                "Ca 3" => dto.GiaBanCa3,
                _ => null
            };
        }

        #endregion
    }
}