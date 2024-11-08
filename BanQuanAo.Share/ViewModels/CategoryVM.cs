using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class CategoryVM
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public Guid? IdCategoryPa { get; set; }
    }
}
