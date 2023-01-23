namespace Chinook.DAL.Playlist
{
    /// <summary>
    /// Represents an interface for play-list related methodes.
    /// </summary>
    public interface IPlaylistDAL
    {
        /// <summary>
        /// Interface to create a new playlist
        /// </summary>
        Task<bool> CreateNewPlaylist(Models.Playlist newPlaylist, string userId);

        /// <summary>
        /// Interface to rename existing playlist
        /// </summary>
        Task<bool> RenamePlaylist(long playlistId, string newPlaylistName, string userId);

        /// <summary>
        /// Interface to add single track to given playlist
        /// </summary>
        Task<bool> AddTrackToPlaylist(Models.Track track, long playlistId, string userId);

        /// <summary>
        /// Interface to remove track from playlist
        /// </summary>
        Task<bool> RemoveTrackFromPlaylist(long TrackId, long playlistId, string userId);

        /// <summary>
        /// Interface to remove playlist with 
        /// all the tracks added to the playlist
        /// </summary>
        Task<bool> RemovePlaylist(long playlistId, string userId);

        /// <summary>
        /// Interface to add track to My favorite tracks playlist
        /// </summary>
        Task<bool> AddToFavoriteTrackList(Models.Track track, string userId);

        /// <summary>
        /// Interface to remove track from My favorite tracks playlist
        /// </summary>
        Task<bool> RemoveFromFavoriteTrackList(long TrackId, string userId);

        /// <summary>
        /// Interface to get all the playlist available for selected user
        /// </summary>
        Task<List<Models.Playlist>> GetCurrentUserAllPlaylists(string userId);

        /// <summary>
        /// Interface to get current users, all the track records from selected playlist
        /// </summary>
        Task<Models.Playlist?> GetCurrentUserSelectedPlaylist(long playlistId, string userId); 
    }
}