using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MvcClient {
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement> {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement) {
            context.Succeed(requirement);
        }
    }
}