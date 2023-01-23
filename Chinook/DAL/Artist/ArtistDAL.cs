using Microsoft.EntityFrameworkCore;

namespace Chinook.DAL.Artist
{
    public class ArtistDAL : IArtistDAL
    {
        #region Construction
        private readonly ILogger<ArtistDAL> _logger;
        private readonly ChinookContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistDAL"/> class.
        /// </summary>
        public ArtistDAL(ILogger<ArtistDAL> logger, ChinookContext context)
        {
            _logger = logger;
            _context = context;
        }
        #endregion

        #region PublicMethodes

        /// <summary>
        /// This method is to get artist from artist Id
        /// </summary>
        /// <param>
        /// <c>artistId</c> is for filter artist by Id
        /// </param>
        /// <returns>
        /// Returns artist data if the artist Id is matched
        /// </returns>
        public async Task<Models.Artist?> GetArtistById(long artistId)
        {
            try
            {
                return await _context.Artists.FindAsync(artistId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return null;
            }
        }

        /// <summary>
        /// This method is to get artist list from matched artist name
        /// </summary>
        /// <param>
        /// <c>artistName</c> is for filter artists by name
        /// </param>
        /// <returns>
        /// Returns artists data list if the artist name matched
        /// </returns>
        public async Task<List<Models.Artist>> GetArtistsByName(string artistName)
        {
            try
            {
                return await (from artist in _context.Artists
                       where artist.Name.Contains(artistName)
                       select artist).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return null;
            }
        }

        /// <summary>
        /// This method is to get all available artist list
        /// </summary>
        /// <returns>
        /// Returns all available artists list
        /// </returns>
        public async Task<List<Models.Artist>> GetAllArtists()
        {
            try
            {
                return await _context.Artists.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return null;
            }
        }
        #endregion
    }
}