﻿using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Services.Interfaces.HRM;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.HRM
{
    public class QLCaLamViecService : IQLCaLamViecService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QLCaLamViecService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResult> CreateCaLamViecAsync(AddCaLamViecDTO addCaLamViecDto)
        {
            try
            {
                if (addCaLamViecDto == null)
                {
                    return ServiceResult.Failure("Không thể thêm ca làm việc vì dữ liệu không hợp lệ.");
                }
                var caLamVec = _mapper.Map<CaLamViec>(addCaLamViecDto);

                var resultCreate = await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var createdCaLamViec = await _unitOfWork.CaLamViecRepository.CreateAsync(caLamVec);
                    return createdCaLamViec;
                });

                if (resultCreate == null)
                {
                    return ServiceResult.Failure("Không thể thêm ca làm việc do lỗi hệ thống.");
                }
                return ServiceResult.Success("Thêm ca làm việc thành công.", _mapper.Map<CaLamViecDTO>(resultCreate));
            } catch(Exception ex)
            {
                return ServiceResult.Failure($"Không thể thêm ca làm việc: {ex.Message}");
            }
        }


        public Task<ServiceResult> GetAllCaLamViecsAsync()
        {
            var listCaLamViec = _unitOfWork.CaLamViecRepository.GetAllAsync();
            if (listCaLamViec == null || !listCaLamViec.Result.Any())
            {
                return Task.FromResult(ServiceResult.Failure("Không có ca làm việc nào được tìm thấy."));
            }
            var caLamViecDtos = _mapper.Map<List<CaLamViecDTO>>(listCaLamViec.Result);
            return Task.FromResult(ServiceResult.Success("Lấy danh sách ca làm việc thành công.", caLamViecDtos));

        }

        public Task<ServiceResult> GetCaLamViecByIdAsync(int maCa)
        {
            var caLamViec = _unitOfWork.CaLamViecRepository.GetByIdAsync(maCa);
            if (caLamViec == null)
            {
                return Task.FromResult(ServiceResult.Failure($"Không tìm thấy ca làm việc với mã {maCa}."));
            }
            var caLamViecDto = _mapper.Map<CaLamViecDTO>(caLamViec.Result);
            return Task.FromResult(ServiceResult.Success("Lấy ca làm việc thành công.", caLamViecDto));
        }
    }
}
