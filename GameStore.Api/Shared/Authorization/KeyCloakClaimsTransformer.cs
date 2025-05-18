using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameStore.Api.Shared.Authorization
{
    public class KeyCloakClaimsTransformer(ILogger<KeyCloakClaimsTransformer> logger)
    {
        public void Transform (TokenValidatedContext context)
        {
            var identity = context.Principal?.Identity as ClaimsIdentity;

            var scopeClaim = identity?.FindFirst(ClaimTypes.Scope);

            if (scopeClaim is null) {
                return;
            }

            var scopes = scopeClaim.Value.Split(' ');

            identity?.RemoveClaim(scopeClaim);

            identity?.AddClaims(scopes.Select(scope => new Claim(ClaimTypes.Scope, scope)));

            var claims = context.Principal?.Claims;
            if (claims is null)
            {
                return ;
            }
           
            foreach (var claim in claims)
            {
                logger.LogTrace($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }
    }
}
