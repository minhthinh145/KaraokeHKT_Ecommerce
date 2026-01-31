﻿using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.HRM;
using QLQuanKaraokeHKT.Application.Services.Interfaces.HRM;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.HRM
{
    public class QuanLyTienLuongService : IQuanLyTienLuongService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuanLyTienLuongService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> CreateLuongCaLamViecAsync(AddLuongCaLamViecDTO addLuongCaLamViecDto)
        {
            var luongCaLamViec = _mapper.Map<LuongCaLamViec>(addLuongCaLamViecDto);
            if (luongCaLamViec == null)
            {
                return ServiceResult.Failure("Không thể thêm lương ca làm việc vì dữ liệu không hợp lệ.");
            }
            var resultCreate = await _unitOfWork.LuongCaLamViecRepository.CreateAsync(luongCaLamViec);
            if (resultCreate == null)
            {
                return ServiceResult.Failure("Không thể thêm lương ca làm việc do lỗi hệ thống.");
            }
            return ServiceResult.Success("Thêm lương ca làm việc thành công.", _mapper.Map<LuongCaLamViecDTO>(resultCreate));
        }

        public async Task<ServiceResult> DeleteLuongCaLamViecAsync(int maLuongCaLamViec)
        {
           var resultDelete = await _unitOfWork.LuongCaLamViecRepository.DeleteAsync(maLuongCaLamViec);
            if (!resultDelete)
            {
                return ServiceResult.Failure($"Không thể xóa lương ca làm việc với mã {maLuongCaLamViec} do không tồn tại.");
            }
            return ServiceResult.Success($"Xóa lương ca làm việc với mã {maLuongCaLamViec} thành công.");

        }

        public async Task<ServiceResult> GetAllLuongCaLamViecsAsync()
        {
           var listLuongCaLamViec = await _unitOfWork.LuongCaLamViecRepository.GetAllAsync();
            if (listLuongCaLamViec == null || !listLuongCaLamViec.Any())
            {
                return ServiceResult.Failure("Không có lương ca làm việc nào được tìm thấy.");
            }
            var luongCaLamViecDtos = _mapper.Map<List<LuongCaLamViecDTO>>(listLuongCaLamViec);
            return ServiceResult.Success("Lấy danh sách lương ca làm việc thành công.", luongCaLamViecDtos);
        }

        public async Task<ServiceResult> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec)
        {
            var luongCaLamViec = await _unitOfWork.LuongCaLamViecRepository.GetLuongCaLamViecByIdAsync(maLuongCaLamViec);
            if (luongCaLamViec == null)
            {
                return ServiceResult.Failure($"Không tìm thấy lương ca làm việc với mã {maLuongCaLamViec}.");
            }
            var luongCaLamViecDto = _mapper.Map<LuongCaLamViecDTO>(luongCaLamViec);
            return ServiceResult.Success("Lấy lương ca làm việc thành công.", luongCaLamViecDto);

        }

        public async Task<ServiceResult> GetLuongCaLamViecByMaCaAsync(int maCa)
        {
            var luongCaLamViec = await _unitOfWork.LuongCaLamViecRepository.GetLuongCaLamViecByMaCaAsync(maCa);
            if (luongCaLamViec == null)
            {
                return ServiceResult.Failure($"Không tìm thấy lương ca làm việc với mã ca {maCa}.");
            }
            var luongCaLamViecDto = _mapper.Map<LuongCaLamViecDTO>(luongCaLamViec);
            return ServiceResult.Success("Lấy lương ca làm việc thành công.", luongCaLamViecDto);
        }

        public async Task<ServiceResult> UpdateLuongCaLamViecAsync(LuongCaLamViecDTO luongCaLamViecDto)
        {
           var luongCaLamViec = _mapper.Map<LuongCaLamViec>(luongCaLamViecDto);
            if (luongCaLamViec == null)
            {
                return ServiceResult.Failure("Không thể cập nhật lương ca làm việc vì dữ liệu không hợp lệ.");
            }
            var resultUpdate = await _unitOfWork.LuongCaLamViecRepository.UpdateAsync(luongCaLamViec);
            if (resultUpdate == null)
            {
                return ServiceResult.Failure("Không thể cập nhật lương ca làm việc do lỗi hệ thống.");
            }
            return ServiceResult.Success("Cập nhật lương ca làm việc thành công.", _mapper.Map<LuongCaLamViecDTO>(resultUpdate));
        }
    }
}
