using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.Models;

namespace MiniCRM.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        [HttpPost]
        public JsonResult AutoCompleteName(string Prefix)
        {
            var firstNames = from c in _context.ContactDetails select c.FirstName;
            if(!String.IsNullOrEmpty(Prefix))
            {
                firstNames = firstNames.Where(c => c.StartsWith(Prefix));
            }
            return Json(firstNames);
        }

        [HttpPost]
        public JsonResult AutoCompleteEmail(string Prefix)
        {
            var emails = from c in _context.ContactDetails select c.Email;
            if (!String.IsNullOrEmpty(Prefix))
            {
                emails = emails.Where(c => c.StartsWith(Prefix));
            }
            return Json(emails);
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string searchString, string filter)
        {
            var contacts = from c in _context.ContactDetails select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                if (!string.IsNullOrEmpty(filter) && filter == "email")
                {
                    contacts = contacts.Where(s => s.Email.Contains(searchString));
                }
                else
                {
                    contacts = contacts.Where(s => s.FirstName.Contains(searchString));
                }
            }
            return View(await contacts.ToListAsync());
        }


        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactDetails = await _context.ContactDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (contactDetails == null)
            {
                return NotFound();
            }

            return View(contactDetails);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,City,Country,Email,FirstName,LastName,Phone,State,StreetAddress")] Contact contactDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contactDetails);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactDetails = await _context.ContactDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (contactDetails == null)
            {
                return NotFound();
            }
            return View(contactDetails);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,City,Country,Email,FirstName,LastName,Phone,State,StreetAddress")] Contact contactDetails)
        {
            if (id != contactDetails.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactDetailsExists(contactDetails.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(contactDetails);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactDetails = await _context.ContactDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (contactDetails == null)
            {
                return NotFound();
            }

            return View(contactDetails);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactDetails = await _context.ContactDetails.SingleOrDefaultAsync(m => m.ID == id);
            _context.ContactDetails.Remove(contactDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ContactDetailsExists(int id)
        {
            return _context.ContactDetails.Any(e => e.ID == id);
        }
    }
}
