using IdentityApp.Model;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Authorization
{
    public class InvoiceAdminAuthorizationHandler :AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        protected override Task HandleRequirementAsync(
          AuthorizationHandlerContext context,
          OperationAuthorizationRequirement requirement, Invoice Invoice)
        {
            if (context.User == null || Invoice == null)
            {
                return Task.CompletedTask;
            }
            //if (requirement.Name != Constants.ApprovedOperationName &&
            //   requirement.Name != Constants.RejectOperationName&&
            //   requirement.Name != Constants.CreateOperationName &&
            //  requirement.Name != Constants.ReadOperationName &&
            //  requirement.Name != Constants.UpdateOperationName &&
            //  requirement.Name != Constants.DeleteOperationName)
            //{
            //    return Task.CompletedTask;
            //}

            if (context.User.IsInRole(Constants.InvoiceAdminRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
