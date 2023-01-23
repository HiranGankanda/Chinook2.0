using Chinook.DAL.Artist;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.DAL.Artist
{
    public class ArtistDAL : IArtistDAL
    {
        #region Construction
        private readonly ILogger<ArtistDAL> _logger;
        private readonly ChinookContext _context;
        public ArtistDAL(ILogger<ArtistDAL> logger, ChinookContext context)
        {
            _logger = logger;
            _context = context;
        }
        #endregion

        #region PublicMethodes
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