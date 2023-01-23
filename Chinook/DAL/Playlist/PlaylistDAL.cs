using Chinook.DAL.Artist;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.DAL.Playlist
{
    public class PlaylistDAL: IPlaylistDAL
    {
        #region Construction
        private readonly ILogger<ArtistDAL> _logger;
        private readonly ChinookContext _context;
        public PlaylistDAL(ILogger<ArtistDAL> logger, ChinookContext context)
        {
            _logger = logger;
            _context = context;
        }
        #endregion

        #region PublicMethodes
        public async Task<List<Models.Playlist>> GetCurrentUserAllPlaylists(string userId)
        {
            try
            {
                return await (from playlist in _context.Playlists
                             join userplaylist in _context.UserPlaylists 
                                on playlist.PlaylistId equals userplaylist.PlaylistId
                             where userplaylist.UserId == userId
                             select playlist).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return new List<Models.Playlist>();
            }
        }
        public async Task<Models.Playlist?> GetCurrentUserSelectedPlaylist(long playlistId, string userId)
        {
            try
            {
                return await _context.Playlists.Where(p => p.PlaylistId == playlistId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return null;
            }
        }
        public async Task<bool> CreateNewPlaylist(Models.Playlist newPlaylist, string userId)
        {
            try
            {
                bool available = _context.Playlists.Any(p => p.Name == newPlaylist.Name);
                if (available == true)
                {
                    _logger.LogInformation($"[PlaylistDAL]CreateNewPlaylist(Models.Playlist newPlaylist, string userId) [PLAYLIST ALREADY CREATED] hit at {DateTime.UtcNow.ToLongTimeString()}");
                    return false;
                }
                else
                {
                    _context.Playlists.Add(newPlaylist);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }
        public async Task<bool> AddTrackToPlaylist(Track track, long playlistId, string userId)
        {
            try
            {
                Models.Playlist? availablePlaylist = _context.Playlists.Find(playlistId);
                if(availablePlaylist != null)
                {
                    Track? selectedTrack = _context.Tracks.Find(track.TrackId);
                    if(selectedTrack != null)
                    {
                        availablePlaylist.Tracks.Add(selectedTrack);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning($"[PlaylistDAL]AddTrackToPlaylist(), Selected track id not found {DateTime.UtcNow.ToLongTimeString()}");
                        return false;
                    }                        
                }
                else
                {
                    _logger.LogWarning($"[PlaylistDAL]AddTrackToPlaylist(), Selected playlist id not found {DateTime.UtcNow.ToLongTimeString()}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }
        public async Task<bool> RemoveTrackFromPlaylist(long TrackId, long playlistId, string userId)
        {
            try
            {
                Models.Playlist? playlist = _context.Playlists
                    .Include(t => t.Tracks)
                    .SingleOrDefault(p => p.PlaylistId == playlistId);
                if(playlist != null)
                {
                    Track selectedTrack = _context.Tracks.Find(TrackId);
                    if(selectedTrack != null) { 
                        playlist.Tracks.Remove(selectedTrack);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    else
                    {
                        _logger.LogWarning($"[PlaylistDAL]RemoveTrackFromPlaylist(), Selected track id not found {DateTime.UtcNow.ToLongTimeString()}");
                        return false;
                    }
                }
                else
                {
                    _logger.LogWarning($"[PlaylistDAL]RemoveTrackFromPlaylist(), Selected playlist id not found {DateTime.UtcNow.ToLongTimeString()}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }
        public async Task FavoriteTracksAutoUpdate(string userId)
        {
            try
            {
                //1. Get no of each track played count

                //2. Get top 10 most played tracks list

                //3. Add those top 10 items to favorite playlist

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
            }
        }
        public async Task<bool> RemovePlaylist(long playlistId, string userId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }
        public async Task<bool> RenamePlaylist(long playlistId, string newPlaylistName, string userId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }
        #endregion
    }
}
