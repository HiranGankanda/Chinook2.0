namespace Chinook.DAL.Artist
{
    /// <summary>
    /// Represents an interface for artist related methodes.
    /// </summary>
    public interface IArtistDAL
    {
        /// <summary>
        /// Interface to get single artist data
        /// </summary>
        Task<Models.Artist?> GetArtistById(long artistId);

        /// <summary>
        /// This interface allows to retrieve a list of artists 
        /// that match the provided search keyword for artist name
        /// </summary>
        Task<List<Models.Artist>> GetArtistsByName(string artistName);

        /// <summary>
        /// Interface to get all artists data
        /// </summary>
        Task<List<Models.Artist>> GetAllArtists();
    }
}