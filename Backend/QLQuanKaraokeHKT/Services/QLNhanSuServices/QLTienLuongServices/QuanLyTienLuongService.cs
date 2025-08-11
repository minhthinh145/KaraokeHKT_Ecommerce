using AutoMapper;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLNhanSu.Interfaces;

namespace QLQuanKaraokeHKT.Services.QLNhanSuServices.QLTienLuongServices
{
    public class QuanLyTienLuongService : IQuanLyTienLuongService
    {
        public readonly ILuongCaLamViecRepository _repo;
        private readonly IMapper _mapper;

        public QuanLyTienLuongService(ILuongCaLamViecRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper;
        }

        public async Task<ServiceResult> CreateLuongCaLamViecAsync(AddLuongCaLamViecDTO addLuongCaLamViecDto)
        {
            var luongCaLamViec = _mapper.Map<LuongCaLamViec>(addLuongCaLamViecDto);
            if (luongCaLamViec == null)
            {
                return ServiceResult.Failure("Không thể thêm lương ca làm việc vì dữ liệu không hợp lệ.");
            }
            var resultCreate = await _repo.CreateLuongCaLamViecAsync(luongCaLamViec);
            if (resultCreate == null)
            {
                return ServiceResult.Failure("Không thể thêm lương ca làm việc do lỗi hệ thống.");
            }
            return ServiceResult.Success("Thêm lương ca làm việc thành công.", _mapper.Map<LuongCaLamViecDTO>(resultCreate));
        }

        public async Task<ServiceResult> DeleteLuongCaLamViecAsync(int maLuongCaLamViec)
        {
           var resultDelete = await _repo.DeleteLuongCaLamViecAsync(maLuongCaLamViec);
            if (!resultDelete)
            {
                return ServiceResult.Failure($"Không thể xóa lương ca làm việc với mã {maLuongCaLamViec} do không tồn tại.");
            }
            return ServiceResult.Success($"Xóa lương ca làm việc với mã {maLuongCaLamViec} thành công.");

        }

        public async Task<ServiceResult> GetAllLuongCaLamViecsAsync()
        {
           var listLuongCaLamViec = await _repo.GetAllLuongCaLamViecsAsync();
            if (listLuongCaLamViec == null || !listLuongCaLamViec.Any())
            {
                return ServiceResult.Failure("Không có lương ca làm việc nào được tìm thấy.");
            }
            var luongCaLamViecDtos = _mapper.Map<List<LuongCaLamViecDTO>>(listLuongCaLamViec);
            return ServiceResult.Success("Lấy danh sách lương ca làm việc thành công.", luongCaLamViecDtos);
        }

        public async Task<ServiceResult> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec)
        {
            var luongCaLamViec = await _repo.GetLuongCaLamViecByIdAsync(maLuongCaLamViec);
            if (luongCaLamViec == null)
            {
                return ServiceResult.Failure($"Không tìm thấy lương ca làm việc với mã {maLuongCaLamViec}.");
            }
            var luongCaLamViecDto = _mapper.Map<LuongCaLamViecDTO>(luongCaLamViec);
            return ServiceResult.Success("Lấy lương ca làm việc thành công.", luongCaLamViecDto);

        }

        public async Task<ServiceResult> GetLuongCaLamViecByMaCaAsync(int maCa)
        {
            var luongCaLamViec = await _repo.GetLuongCaLamViecByMaCaAsync(maCa);
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
            var resultUpdate = await _repo.UpdateLuongCaLamViecAsync(luongCaLamViec);
            if (resultUpdate == null)
            {
                return ServiceResult.Failure("Không thể cập nhật lương ca làm việc do lỗi hệ thống.");
            }
            return ServiceResult.Success("Cập nhật lương ca làm việc thành công.", _mapper.Map<LuongCaLamViecDTO>(resultUpdate));
        }
    }
}
