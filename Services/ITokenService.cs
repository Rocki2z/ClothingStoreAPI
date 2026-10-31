
using ClothingStoreAPI.DTOs;
using ClothingStoreAPI.Models;

namespace ClothingStoreAPI.Services;

public interface ITokenService
{
    Task<AuthResponseDto> CreateTokenAsync(ApplicationUser user); // Method to create a token for a given user
}
