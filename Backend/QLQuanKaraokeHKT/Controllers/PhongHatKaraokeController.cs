using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Services.Interfaces;

namespace QLQuanKaraokeHKT.Controllers
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
