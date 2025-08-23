using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;

namespace QLQuanKaraokeHKT.Presentation.Controllers.Room
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongHatKaraokeController : ControllerBase
    {
        private readonly IPhongHatKaraokeService _service;

        public PhongHatKaraokeController(IPhongHatKaraokeService service) {
            _service = service;
        }



        //Update


        //Delete


    }
}
