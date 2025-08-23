using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM
{
    /// <summary>
    /// Repository interface for managing CaLamViec (work shift) entities.
    /// </summary>
    public interface ICaLamViecRepository
    {
        /// <summary>
        /// Retrieves all work shifts from the database.
        /// </summary>
        /// <returns>A list of all <see cref="CaLamViec"/> entities.</returns>
        Task<List<CaLamViec>> GetAllCaLamViecsAsync();

        /// <summary>
        /// Creates a new work shift in the database.
        /// </summary>
        /// <param name="caLamViec">The <see cref="CaLamViec"/> entity to create.</param>
        /// <returns>The created <see cref="CaLamViec"/> entity.</returns>
        Task<CaLamViec> CreateCaLamViecAsync(CaLamViec caLamViec);

        /// <summary>
        /// Retrieves a work shift by its unique identifier.
        /// </summary>
        /// <param name="maCa">The unique ID of the work shift.</param>
        /// <returns>The <see cref="CaLamViec"/> entity if found; otherwise, null.</returns>
        Task<CaLamViec> GetCaLamViecByIdAsync(int maCa);



        /// <summary>
        /// Updates an existing work shift in the database.
        /// </summary>
        /// <param name="caLamViec">The <see cref="CaLamViec"/> entity with updated information.</param>
        /// <returns>The updated <see cref="CaLamViec"/> entity.</returns>
        Task<CaLamViec> UpdateCaLamViecAsync(CaLamViec caLamViec);

        /// <summary>
        /// Deletes a work shift by its unique identifier.
        /// </summary>
        /// <param name="maCa">The unique ID of the work shift to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteCaLamViecAsync(int maCa);

        /// <summary>
        /// Retrieves all work shifts that match the specified name.
        /// </summary>
        /// <param name="tenCa">The name of the work shift to search for.</param>
        /// <returns>A list of <see cref="CaLamViec"/> entities with the specified name.</returns>
        Task<int> GetIdCaLamViecByTenCaAsync(string tenCa);

        Task<List<CaLamViec>> GetCaLamViecByTenCaAsync(List<string> tenCaList);

    }
}