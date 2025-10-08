using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLKhoDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Application.Repositories.HRM;
using QLQuanKaraokeHKT.Application.Repositories.Inventory;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory;

namespace QLQuanKaraokeHKT.Application.Services.Inventory
{

    public class QLVatLieuService : IQLVatLieuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPricingService _pricingService;

        public QLVatLieuService(
            IUnitOfWork unitOfWork,
            IPricingService pricingService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        }

        public async Task<ServiceResult> CreateVatLieuWithFullDetailsAsync(AddVatLieuDTO addVatLieuDto)
        {
            try
            {
                if (addVatLieuDto == null)
                    return ServiceResult.Failure("Thông tin vật liệu không hợp lệ.");

               await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var sanPham = _mapper.Map<SanPhamDichVu>(addVatLieuDto);
                    var createdSanPham = await _unitOfWork.SanPhamDichVuRepository.CreateAsync(sanPham);

                    var vatLieu = _mapper.Map<VatLieu>(addVatLieuDto);
                    vatLieu.NgungCungCap = false;
                    var createdVatLieu = await _unitOfWork.VatLieuRepository.CreateAsync(vatLieu);

                    var monAn = new MonAn
                    {
                        MaSanPham = createdSanPham.MaSanPham,
                        MaVatLieu = createdVatLieu.MaVatLieu,
                        SoLuongConLai = addVatLieuDto.SoLuongTonKho
                    };
                    await _unitOfWork.MonAnRepository.CreateAsync(monAn);

                    var giaVatLieu = _mapper.Map<GiaVatLieu>(addVatLieuDto);
                    giaVatLieu.MaVatLieu = createdVatLieu.MaVatLieu;
                    await _unitOfWork.GiaVatLieuRepository.CreateAsync(giaVatLieu);

                    await _pricingService.ApplyPricingConfigAsync(createdSanPham.MaSanPham,addVatLieuDto.AsPricingConfig());

                });
              
                return ServiceResult.Success("Tạo vật liệu thành công với đầy đủ thông tin.", addVatLieuDto);
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
                var vatLieus = await _unitOfWork.VatLieuRepository.GetAllVatLieuWithDetailsAsync(includePricing: true);

                if (vatLieus == null || !vatLieus.Any())
                    return ServiceResult.Failure("Không có vật liệu nào trong hệ thống.");

                var vatLieuDTOs = new List<VatLieuDetailDTO>();

                foreach (var vatLieu in vatLieus)
                {
                    var dto = _mapper.Map<VatLieuDetailDTO>(vatLieu);

                    EnrichWithPricing(dto, vatLieu);

                    vatLieuDTOs.Add(dto);
                }

                return ServiceResult.Success("Lấy danh sách vật liệu thành công.", vatLieuDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy danh sách vật liệu: {ex.Message}");
            }
        }

        private void EnrichWithPricing(VatLieuDetailDTO dto, VatLieu vatLieu)
        {
            // Input pricing
            var latestInputPrice = vatLieu.GiaVatLieus?.OrderByDescending(g => g.NgayApDung).FirstOrDefault();
            if (latestInputPrice != null)
            {
                dto.GiaNhapHienTai = latestInputPrice.DonGia;
                dto.NgayApDungGiaNhap = latestInputPrice.NgayApDung;
                dto.TrangThaiGiaNhap = "HieuLuc";
            }

            // Selling pricing
            var giaDichVus = vatLieu.MonAn?.MaSanPhamNavigation?.GiaDichVus;
            if (giaDichVus?.Any() == true)
            {
                var flatRate = giaDichVus.FirstOrDefault(g => g.MaCa == null);
                dto.DongGiaAllCa = flatRate != null;
                dto.GiaBanChung = flatRate?.DonGia;

                dto.GiaBanCa1 = giaDichVus.FirstOrDefault(g => g.MaCaNavigation?.TenCa == "Ca 1")?.DonGia;
                dto.GiaBanCa2 = giaDichVus.FirstOrDefault(g => g.MaCaNavigation?.TenCa == "Ca 2")?.DonGia;
                dto.GiaBanCa3 = giaDichVus.FirstOrDefault(g => g.MaCaNavigation?.TenCa == "Ca 3")?.DonGia;

                // Current price logic
                dto.GiaBanHienTai = flatRate?.DonGia ?? giaDichVus.FirstOrDefault()?.DonGia;
                dto.NgayApDungGiaBan = giaDichVus.OrderByDescending(g => g.NgayApDung).FirstOrDefault()?.NgayApDung;
                dto.TrangThaiGiaBan = "HieuLuc";
            }
        }

        public async Task<ServiceResult> GetVatLieuDetailByIdAsync(int maVatLieu)
        {
            try
            {
                var vatLieu = await _unitOfWork.VatLieuRepository.GetVatLieuWithDetailsByIdAsync(maVatLieu);
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

                await _unitOfWork.ExecuteTransactionAsync(async () => {

                    var updateVatLieuResult = await _unitOfWork.VatLieuRepository.UpdateSoLuongVatLieuAsync(
                     updateSoLuongDto.MaVatLieu, updateSoLuongDto.SoLuongMoi);

                    await _unitOfWork.MonAnRepository.UpdateSoLuongByMaVatLieuAsync(
                        updateSoLuongDto.MaVatLieu, updateSoLuongDto.SoLuongMoi);
                });

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
                var result = await _unitOfWork.VatLieuRepository.UpdateNgungCungCapAsync(maVatLieu, ngungCungCap);
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

                var data = await _unitOfWork.ExecuteTransactionAsync(async () => {

                    var existingVatLieu = await _unitOfWork.VatLieuRepository.GetVatLieuWithDetailsByIdAsync(updateVatLieuDto.MaVatLieu);
                    if (existingVatLieu == null)


                    _mapper.Map(updateVatLieuDto, existingVatLieu);
                    await _unitOfWork.VatLieuRepository.UpdateAsync(existingVatLieu);

 
                    if (existingVatLieu.MonAn?.MaSanPhamNavigation != null)
                    {
                        _mapper.Map(updateVatLieuDto, existingVatLieu.MonAn.MaSanPhamNavigation);
                        await _unitOfWork.SanPhamDichVuRepository.UpdateAsync(existingVatLieu.MonAn.MaSanPhamNavigation);
                    }

                    // 4. Cập nhật giá nhập
                    if (updateVatLieuDto.GiaNhapMoi.HasValue)
                    {
                        await _unitOfWork.GiaVatLieuRepository.DisableCurrentPricesAsync(updateVatLieuDto.MaVatLieu, "HetHieuLuc");

                        var newGiaVatLieu = new GiaVatLieu
                        {
                            MaVatLieu = updateVatLieuDto.MaVatLieu,
                            DonGia = updateVatLieuDto.GiaNhapMoi.Value,
                            NgayApDung = updateVatLieuDto.NgayApDungGiaNhap ?? DateOnly.FromDateTime(DateTime.Now),
                            TrangThai = updateVatLieuDto.TrangThaiGiaNhap ?? "HieuLuc"
                        };
                        await _unitOfWork.GiaVatLieuRepository.CreateAsync(newGiaVatLieu);
                    }

                    // 5. ✅ CẬP NHẬT GIÁ BÁN THEO LOGIC MỚI
                    if (existingVatLieu.MonAn != null && updateVatLieuDto.CapNhatGiaBan == true)
                    {
                        await _pricingService.DisableCurrentPricesAsync(existingVatLieu.MonAn.MaSanPham);

                        await _pricingService.ApplyPricingConfigAsync(
                            existingVatLieu.MonAn.MaSanPham,
                            updateVatLieuDto.AsPricingConfig());
                    }

                    return _mapper.Map<VatLieuDetailDTO>(existingVatLieu);
                });
             
                return ServiceResult.Success("Cập nhật vật liệu thành công với đầy đủ thông tin.", data);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật vật liệu: {ex.Message}");
            }
        }
    }
}