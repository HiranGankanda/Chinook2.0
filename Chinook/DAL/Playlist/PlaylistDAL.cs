using Chinook.DAL.Artist;
using Chinook.Helpers;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.DAL.Playlist
{
    public class PlaylistDAL: IPlaylistDAL
    {
        #region Construction
        private readonly ILogger<ArtistDAL> _logger;
        private readonly ChinookContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistDAL"/> class.
        /// </summary>
        public PlaylistDAL(ILogger<ArtistDAL> logger, ChinookContext context)
        {
            _logger = logger;
            _context = context;
        }
        #endregion

        #region PublicMethodes

        /// <summary>
        /// This method is to create a new playlist
        /// </summary>
        /// <param name="newPlaylist">Playlist object to save record</param>
        /// <param name="userId">save user id for audit purpose</param>
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

        /// <summary>
        /// This method is to rename playlist
        /// </summary>
        /// <param name="playlistId">Playlist Id to filter/selection</param>
        /// <param name="newPlaylistName">new value for playlist name</param>
        /// <param name="userId">save user id for audit purpose</param>
        public async Task<bool> RenamePlaylist(long playlistId, string newPlaylistName, string userId)
        {
            try
            {
                Models.Playlist playlist = _context.Playlists.Where(p=> p.PlaylistId == playlistId).FirstOrDefault();
                if (playlist == null)
                {
                    playlist.Name = newPlaylistName;
                    _context.Playlists.Add(playlist);

                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogInformation($"[PlaylistDAL]RenamePlaylist(long playlistId, string newPlaylistName, string userId) [NULL RETURN] hit at {DateTime.UtcNow.ToLongTimeString()}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }

        /// <summary>
        /// This method is to add track to playlist
        /// </summary>
        /// <param name="track">Track object to insert record</param>
        /// <param name="playlistId">to filter/selection</param>
        /// <param name="userId">save user id for audit purpose</param>
        public async Task<bool> AddTrackToPlaylist(Track track, long playlistId, string userId)
        {
            try
            {
                Models.Playlist? availablePlaylist = _context.Playlists.Find(playlistId);
                if (availablePlaylist != null)
                {
                    Track? selectedTrack = _context.Tracks.Find(track.TrackId);
                    if (selectedTrack != null)
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

        /// <summary>
        /// This method is to remove track from playlist
        /// </summary>
        /// <param name="TrackId">Track id to remove record</param>
        /// <param name="playlistId">to filter/selection</param>
        /// <param name="userId">save user id for audit purpose</param>
        public async Task<bool> RemoveTrackFromPlaylist(long TrackId, long playlistId, string userId)
        {
            try
            {
                Models.Playlist? playlist = _context.Playlists
                    .Include(t => t.Tracks)
                    .SingleOrDefault(p => p.PlaylistId == playlistId);
                if (playlist != null)
                {
                    Track selectedTrack = _context.Tracks.Find(TrackId);
                    if (selectedTrack != null)
                    {
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

        /// <summary>
        /// This method is to remove playlist and all the related data
        /// </summary>
        /// <param name="playlistId">to filter/selection</param>
        /// <param name="userId">save user id for audit purpose</param>
        public async Task<bool> RemovePlaylist(long playlistId, string userId)
        {
            try
            {
                var playlistToRemove = new Models.Playlist { PlaylistId = playlistId };
                
                // Attach the entity to the context
                _context.Playlists.Attach(playlistToRemove);

                // Mark the entity for deletion
                _context.Playlists.Remove(playlistToRemove);

                // Save the changes to the database
                _ = await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }

        /// <summary>
        /// This method is to add favorite track to list
        /// </summary>
        /// <param name="track">Track object for save data</param>
        /// <param name="userId">save user id for audit purpose</param>
        public async Task<bool> AddToFavoriteTrackList(Track track, string userId)
        {
            try
            {
                int? isPlaylistAvailable = ValidateMyFavoriteTracksPlaylist.CheckMyFavoriteTracksPlaylistAvailable(userId);
                if (!isPlaylistAvailable.HasValue) 
                {
                    ValidateMyFavoriteTracksPlaylist.CreateMyFavoriteTracksPlaylist();                    
                }

                return await AddTrackToPlaylist(track, isPlaylistAvailable.Value, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }

        /// <summary>
        /// This method is to remove from fav playlist
        /// </summary>
        /// <param name="TrackId">Id is use to filter/selection</param>
        /// <param name="userId">save user id for audit purpose</param>
        public async Task<bool> RemoveFromFavoriteTrackList(long TrackId, string userId)
        {
            try
            {
                int? isPlaylistAvailable = ValidateMyFavoriteTracksPlaylist.CheckMyFavoriteTracksPlaylistAvailable(userId);
                if (!isPlaylistAvailable.HasValue)
                {
                    ValidateMyFavoriteTracksPlaylist.CreateMyFavoriteTracksPlaylist();
                    return false;
                }
                else
                {
                    _ = await RemoveTrackFromPlaylist(TrackId, isPlaylistAvailable.Value, userId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }

        /// <summary>
        /// This method is to get users all the playlists
        /// </summary>
        /// <param name="userId">save user id for audit purpose</param>
        /// <returns> Returns Playlist list object</returns>
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

        /// <summary>
        /// This method is to get user selected playlist data
        /// </summary>
        /// <param name="playlistId">id for filter/selection</param>
        /// <param name="userId">save user id for audit purpose</param>
        /// <returns> Returns Playlist object</returns>
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
        #endregion
    }
}
