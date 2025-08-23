using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;

namespace QLQuanKaraokeHKT.Application.Services.Room
{
    public class PhongHatKaraokeService : IPhongHatKaraokeService
    {
        private readonly IMapper _mapper;
        private readonly IPhongHatKaraokeRepository _repo;

        public PhongHatKaraokeService(IMapper mapper , IPhongHatKaraokeRepository repo)
        {
            //Constructor Injection
            _mapper = mapper;
            _repo = repo;
        }


        public async Task<ServiceResult> GetAllPhongHatKaraokeIsActiveAsync()
        {
            var result = await _repo.GetAllPhongHatKarokeAsync();
            if(result == null || !result.Any())
            {
               return ServiceResult.Success("Không có phòng hát karaoke nào đang hoạt động", null);
            }
            return ServiceResult.Success("Danh sách phòng hát Karaoke đang hoạt động" , data: result);
        }
    }
}
