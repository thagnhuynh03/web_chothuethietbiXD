using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using WebChoThueThietBiXD.Models;
using Microsoft.AspNetCore.Http;

namespace WebChoThueThietBiXD.ViewModels
{
    public class HinhAnhViewModel
    {
        public IFormFile hinhAnh { get; set; }

        [ForeignKey("ThietBi")]
        public int maThietBi { get; set; }
    }
}
