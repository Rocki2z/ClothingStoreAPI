using Microsoft.AspNetCore.Identity;

namespace ClothingStoreAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}