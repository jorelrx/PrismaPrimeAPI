using Microsoft.AspNetCore.Identity;

public class CustomIdentityException(IdentityResult identityResult) : Exception("Identity operation failed.")
{
    public IdentityResult IdentityResult { get; } = identityResult;
}
