using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace QLQuanKaraokeHKT.Controllers.Helper
{
    public static class ControllerExtension
    {
        /// <summary>
        /// Validates user authentication and extracts user ID from JWT token
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <param name="userId">Output user ID if validation succeeds</param>
        /// <returns>Error response if validation fails, null if successful</returns>
        public static IActionResult? ValidateUserAuthentication(this ControllerBase controller, out Guid userId)
        {
            userId = Guid.Empty;

            var userIdString = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return controller.Unauthorized(new { message = "Invalid token." });
            }

            if (!Guid.TryParse(userIdString, out userId))
            {
                return controller.BadRequest(new { message = "Invalid user ID format." });
            }

            return null; // Success
        }

        /// <summary>
        /// Validates model state
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <returns>Error response if validation fails, null if successful</returns>
        public static IActionResult? ValidateModelState(this ControllerBase controller)
        {
            if (!controller.ModelState.IsValid)
            {
                return controller.BadRequest(new
                {
                    message = "Dữ liệu không hợp lệ.",
                    errors = controller.ModelState
                });
            }

            return null; // Success
        }

        /// <summary>
        /// Combined validation for authentication and model state
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <param name="userId">Output user ID if validation succeeds</param>
        /// <returns>Error response if any validation fails, null if all successful</returns>
        public static IActionResult? ValidateAuthenticationAndModel(this ControllerBase controller, out Guid userId)
        {
            // Validate authentication first
            var authResult = controller.ValidateUserAuthentication(out userId);
            if (authResult != null)
                return authResult;

            // Then validate model state
            return controller.ValidateModelState();
        }
    }

}
