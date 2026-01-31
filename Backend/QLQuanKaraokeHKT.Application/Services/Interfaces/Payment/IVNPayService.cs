﻿using QLQuanKaraokeHKT.Application.DTOs.VNPayDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Payment
{
    public interface IVNPayService
    {

        Task<ServiceResult> CreatePaymentUrlAsync(VNPayRequestDTO request, HttpContext httpContext);

        Task<ServiceResult> ProcessVNPayResponseAsync(IQueryCollection vnpayData);

        bool ValidateSignature(IQueryCollection vnpayData);
    }
}