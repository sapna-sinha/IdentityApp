using IdentityApp.Model;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Authorization
{
    public class InvoiceManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, Invoice Invoice)
        {
           if(context.User == null || Invoice == null)
           {
                return Task.CompletedTask;
           }
            if (requirement.Name != Constants.ApprovedOperationName &&
               requirement.Name != Constants.RejectedOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.InvoiceManagerRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
