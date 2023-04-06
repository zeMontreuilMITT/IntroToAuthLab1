using IntroToAuthLab1.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace IntroToAuthLab1.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; } = default!;
        public double Balance { get; set; } = 0;

        public string UserId { get; set; } = default!;
        public virtual ApplicationUser? User { get; set; }
    }
}
