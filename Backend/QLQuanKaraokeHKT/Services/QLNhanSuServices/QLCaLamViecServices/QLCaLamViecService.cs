using AutoMapper;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLNhanSu.Interfaces;

namespace QLQuanKaraokeHKT.Services.QLNhanSuServices.QLCaLamViecServices
{
    public class QLCaLamViecService : IQLCaLamViecService
    {
        private readonly ICaLamViecRepository _repo;
        private readonly IMapper _mapper;

        public QLCaLamViecService(ICaLamViecRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<ServiceResult> CreateCaLamViecAsync(AddCaLamViecDTO addCaLamViecDto)
        {
            if (addCaLamViecDto == null) 
            {
                return ServiceResult.Failure("Không thể thêm ca làm việc vì dữ liệu không hợp lệ.");
            }
            var caLamVec = _mapper.Map<CaLamViec>(addCaLamViecDto);
            var resultCreate = await _repo.CreateCaLamViecAsync(caLamVec);
            if (resultCreate == null)
            {
                return ServiceResult.Failure("Không thể thêm ca làm việc do lỗi hệ thống.");
            }
            return ServiceResult.Success("Thêm ca làm việc thành công.", _mapper.Map<CaLamViecDTO>(resultCreate));
        }


        public Task<ServiceResult> GetAllCaLamViecsAsync()
        {
            var listCaLamViec = _repo.GetAllCaLamViecsAsync();
            if (listCaLamViec == null || !listCaLamViec.Result.Any())
            {
                return Task.FromResult(ServiceResult.Failure("Không có ca làm việc nào được tìm thấy."));
            }
            var caLamViecDtos = _mapper.Map<List<CaLamViecDTO>>(listCaLamViec.Result);
            return Task.FromResult(ServiceResult.Success("Lấy danh sách ca làm việc thành công.", caLamViecDtos));

        }

        public Task<ServiceResult> GetCaLamViecByIdAsync(int maCa)
        {
            var caLamViec = _repo.GetCaLamViecByIdAsync(maCa);
            if (caLamViec == null)
            {
                return Task.FromResult(ServiceResult.Failure($"Không tìm thấy ca làm việc với mã {maCa}."));
            }
            var caLamViecDto = _mapper.Map<CaLamViecDTO>(caLamViec.Result);
            return Task.FromResult(ServiceResult.Success("Lấy ca làm việc thành công.", caLamViecDto));
        }
    }
}
