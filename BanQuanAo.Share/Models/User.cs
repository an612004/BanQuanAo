using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime? DateOfBirth { get; set; }
        public int? Points { get; set; }
        public Guid RankId { get; set; }
        [ForeignKey("RankId")]
        public virtual Rank? Rank { get; set; }
        public virtual List<WishList>? WishLists { get; set; }
        public virtual List<Adress>? Adresses { get; set; }
        public virtual List<UserVoucher>? VoucherUsers { get; set; }
        public virtual List<Post>? Posts { get; set; }
        public virtual List<Order>? Orders { get; set; }

    }
}
