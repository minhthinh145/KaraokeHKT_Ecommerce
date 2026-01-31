﻿namespace QLQuanKaraokeHKT.Application.Services.Interfaces.External
{
    public interface IMaOtpService
    {
        Task<ServiceResult> GenerateAndSendOtpAsync(string email);
        Task<ServiceResult> VerifyOtpAsync(string email, string otpCode);
        Task<ServiceResult> MarkOtpAsUsedAsync(string email, string otpCode);
    }
}
