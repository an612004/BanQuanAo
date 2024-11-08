using BanQuanAo.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class SizeVM
    {
        public Guid Id { get; set; }
        public string? SizeName { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
    }
}
