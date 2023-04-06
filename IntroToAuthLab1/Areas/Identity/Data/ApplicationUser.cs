using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IntroToAuthLab1.Models;
using Microsoft.AspNetCore.Identity;

namespace IntroToAuthLab1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = default!;

    public virtual HashSet<BankAccount> BankAccounts { get; set; } = new HashSet<BankAccount>();
}

