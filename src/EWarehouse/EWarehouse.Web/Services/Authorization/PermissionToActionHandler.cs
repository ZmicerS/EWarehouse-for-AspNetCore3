using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EWarehouse.Web.Services.Authorization
{
    public class RolesRequirement : IAuthorizationRequirement
    {
        public RolesRequirement(List<string> roles)
        {
            Roles = roles;
        }

        public List<string> Roles { get; private set; }
    }

    public class PermissionToActionHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(
          AuthorizationHandlerContext context,
          RolesRequirement requirement
          )
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            var hasPermission = context.User.HasClaim(c => c.Type == ClaimsIdentity.DefaultRoleClaimType && requirement.Roles.Contains(c.Value));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
