using IntroToAuthLab1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntroToAuthLab1.Controllers
{
    // instance of ROLE-BASED AUTHORIZATION
    // If you possess this role, you may access this resource
    [Authorize(Roles = "Administrator")]
    public class AdministratorController: Controller
    {
        private readonly BankingDbContext _context;
        public AdministratorController(BankingDbContext context)
        {
            _context = context;
        }

        public IActionResult AllUsers()
        {
            return View(_context.Users.Include(u => u.BankAccounts).ToList());
        }
        
        public IActionResult CreateManager()
        {
            // get a user and give them the manager role
            return View();
        }
    }
}
