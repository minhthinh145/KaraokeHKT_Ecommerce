using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface ITaiKhoanService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="signup">User registration details (username, email, password, etc.).</param>
        /// <returns>
        /// Returns an <see cref="IdentityResult"/> indicating success or failure.
        /// </returns>
        Task<ServiceResult> SignUpAsync(SignUpDTO signup);

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="signin">User login details (email, password, etc.).</param>
        /// <returns>
        /// Returns a JWT token as a string if successful, otherwise returns an empty string.
        /// </returns>
        Task<ServiceResult> SignInAsync(SignInDTO signin);

        /// <summary>
        /// Find a user and return UserProfileDTO if successful
        /// </summary>
        /// <param name="userID">User id</param>
        /// <returns>
        /// Return UserProfileDTO of user
        /// </returns> 
        Task<ServiceResult> GetProfileUserAsync(Guid userID);

        /// <summary>
        /// Update a user 
        /// </summary>
        /// <param name="userid">User id</param>
        /// <param name="user">Userprofile DTO</param>
        /// <returns>
        /// Return UserProfileDTO
        /// </returns>
        Task<ServiceResult> UpdateUserById(Guid userid, UserProfileDTO user);

        /// <summary>
        /// Check if a password is correct for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="password">Password to verify</param>
        /// <returns>True if correct, false otherwise</returns>
        Task<ServiceResult> CheckPasswordAsync(Guid userId, string password);


    }
}