using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChoThueThietBiXD.Data;

namespace WebChoThueThietBiXD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThietBiApiController : ControllerBase
    {
        private readonly WebChoThueThietBiXDContext _context;

        public ThietBiApiController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        [HttpGet("GetGiaThue/{maThietBi}")]
        public async Task<IActionResult> GetGiaThue(int maThietBi)
        {
            var thietBi = await _context.ThietBi.FindAsync(maThietBi);
            if (thietBi == null)
            {
                return NotFound();
            }

            return Ok(thietBi.giaThue);
        }
    }
}
