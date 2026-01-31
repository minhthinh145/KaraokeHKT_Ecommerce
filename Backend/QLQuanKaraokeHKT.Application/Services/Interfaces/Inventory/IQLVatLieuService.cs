using QLQuanKaraokeHKT.Application.DTOs.QLKhoDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Inventory
{
    /// <summary>
    /// Service interface for managing VatLieu (materials) operations.
    /// </summary>
    public interface IQLVatLieuService
    {
        /// <summary>
        /// Creates a new material with full details (VatLieu, SanPhamDichVu, MonAn, GiaVatLieu).
        /// </summary>
        /// <param name="addVatLieuDto">Material information to create.</param>
        /// <returns>Service result with created material details.</returns>
        Task<ServiceResult> CreateVatLieuWithFullDetailsAsync(AddVatLieuDTO addVatLieuDto);

        /// <summary>
        /// Retrieves all materials with full details.
        /// </summary>
        /// <returns>Service result with list of materials.</returns>
        Task<ServiceResult> GetAllVatLieuWithDetailsAsync();

        /// <summary>
        /// Retrieves a specific material by ID with full details.
        /// </summary>
        /// <param name="maVatLieu">Material ID.</param>
        /// <returns>Service result with material details.</returns>
        Task<ServiceResult> GetVatLieuDetailByIdAsync(int maVatLieu);

        /// <summary>
        /// Updates material quantity in both VatLieu and MonAn tables.
        /// </summary>
        /// <param name="updateSoLuongDto">Update quantity information.</param>
        /// <returns>Service result.</returns>
        Task<ServiceResult> UpdateSoLuongVatLieuAsync(UpdateSoLuongDTO updateSoLuongDto);

        /// <summary>
        /// Marks material as discontinued or available for sale.
        /// </summary>
        /// <param name="maVatLieu">Material ID.</param>
        /// <param name="ngungCungCap">True to discontinue, false to make available.</param>
        /// <returns>Service result.</returns>
        Task<ServiceResult> UpdateNgungCungCapAsync(int maVatLieu, bool ngungCungCap);

        /// <summary>
        /// Updates material information including name, input price, selling price, and image.
        /// </summary>
        /// <param name="updateVatLieuDto">Updated material information.</param>
        /// <returns>Service result.</returns>
        Task<ServiceResult> UpdateVatLieuWithDetailsAsync(UpdateVatLieuDTO updateVatLieuDto);
    }
}