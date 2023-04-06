using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntroToAuthLab1.Data;
using IntroToAuthLab1.Models;
using Microsoft.AspNetCore.Authorization;
using IntroToAuthLab1.Areas.Identity.Data;
using System.Runtime.CompilerServices;

namespace IntroToAuthLab1.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BankAccountsController : Controller
    {
        private readonly BankingDbContext _context;

        public BankAccountsController(BankingDbContext context)
        {
            _context = context;
        }

        // GET: BankAccounts
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = _context.Users
                .Include(u => u.BankAccounts)
                .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if(user == null)
            {
                return BadRequest();
            }

            return View(user.BankAccounts.ToList());
        }

        // GET: BankAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public IActionResult Create()
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                return BadRequest();
            }

            ViewBag.UserId = user.Id;
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Balance,UserId")] BankAccount bankAccount)
        {
            if(bankAccount.Balance < 0)
            {
                ViewBag.Message = "Initial balances cannot be set to less than 0.";
            } else if (ModelState.IsValid)
            {
                _context.Add(bankAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserId = bankAccount.UserId;
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bankAccount.UserId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Balance,UserId")] BankAccount bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountExists(bankAccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bankAccount.UserId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BankAccounts == null)
            {
                return Problem("Entity set 'BankingDbContext.BankAccounts'  is null.");
            }
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount != null)
            {
                _context.BankAccounts.Remove(bankAccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountExists(int id)
        {
          return (_context.BankAccounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
