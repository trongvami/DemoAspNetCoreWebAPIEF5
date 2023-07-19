using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hanghoas = new List<HangHoa>();

        [HttpGet]
        public IActionResult GetAll() {
            return Ok(hanghoas);
        }

        [HttpGet("{guid}")]
        public IActionResult GetById(string guid)
        {
            try {
                var hh = hanghoas.SingleOrDefault(x => x.HangHoaId == Guid.Parse(guid));
                if (hh == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(hh);
                }
            }
            catch (Exception ex) {
                return BadRequest();
            }
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(HangHoa hangHoa)
        {
            var newHH = new HangHoa()
            {
                HangHoaId = Guid.NewGuid(),
                TenHangHoa = hangHoa.TenHangHoa,
                DonGia = hangHoa.DonGia

            };
            hanghoas.Add(newHH);
            return Ok(new { 
                Success = true, Data = newHH
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, HangHoa hangHoa)
        {
            try
            {
                var hh = hanghoas.SingleOrDefault(x => x.HangHoaId == Guid.Parse(id));
                if (hh == null)
                {
                    return NotFound();
                }
                else
                {
                    hh.DonGia = hangHoa.DonGia;
                    hh.TenHangHoa = hangHoa.TenHangHoa;
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
