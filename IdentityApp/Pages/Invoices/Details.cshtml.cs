using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Data;
using IdentityApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
           
        }

      public Invoice Invoice { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Invoice == null)
            {
                return NotFound();
            }

            Invoice = await Context.Invoice.FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (Invoice == null)
            {
                return NotFound();
            }

            var isCreator = await AuthorizationService.AuthorizeAsync(
              User, Invoice, InvoiceOperations.Read);

            var isManager = User.IsInRole(Constants.InvoiceManagerRole);


            if (isCreator.Succeeded == false && isManager == false)
            {
                return Forbid();
            }
            else 
            {
                Invoice = Invoice;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, Invoice.InvoiceStatus status)
        {
            Invoice = await Context.Invoice.FindAsync(id);
            if (Invoice == null)
            {
                return NotFound();
            }
            var invoiceOperation = status == Invoice.InvoiceStatus.Approved
                ?
                InvoiceOperations.Approved:
                InvoiceOperations.Rejected;

            var isAuthorize = await AuthorizationService.AuthorizeAsync(
                User, Invoice, invoiceOperation);

            if(isAuthorize.Succeeded == false)
            {
                return Forbid();
            }
            Invoice.Status = status;
            Context.Invoice.Update(Invoice);
            await Context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
