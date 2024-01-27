using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeterReaderAPI.Services
{
    public interface IUserRepository
    {
        Task<IdentityResult> Create(IdentityUser user, UserCredentials userCredentials);
        Task<Microsoft.AspNetCore.Identity.SignInResult> Login(string email, string password);
        Task<IdentityUser> GetUser(string email);
    }
}
