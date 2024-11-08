using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class PostVM
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Tittle { get; set; }
        public string? TittleImage { get; set; }
        public string? Contents { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public IFormFile? filecollection { get; set; }

    }
}
