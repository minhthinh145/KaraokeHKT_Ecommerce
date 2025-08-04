using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.Interfaces
{
    public interface IPhongHatKaraokeRepository
    {
        /// <summary>
        /// Find all of list Karaoke Room where it's is active
        /// </summary>
        /// <returns>List PhongHarKaraoke Models</returns>
        Task<List<PhongHatKaraoke>> GetAllPhongHatKarokeAsync();

        /// <summary>
        /// Find a Karaoke Room by its ID
        /// </summary>
        /// <returns> PhongHatKaraoke Models</returns>
        Task<PhongHatKaraoke> FindPhongHatKaraokeByIdAsync(string KaraokeRoom_ID);

        /// <summary>
        /// Delete a Karaoke Room by its ID
        /// </summary>
        /// <returns> true if successul, otherwise false</returns>
        Task<bool> DeletePhongHatKaraokeAsync(string KaraokeRoom_ID);

        /// <summary>
        /// Update a Karaoke Room
        /// </summary>
        /// <param name="phongHatKaraoke"></param>
        /// <returns>true if successful, otherwise false</returns>
        Task<bool> UpdatePhongHatKaraokeAsync(PhongHatKaraoke phongHatKaraoke);

    }
}
