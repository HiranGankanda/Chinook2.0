namespace Chinook.DAL.Playlist
{
    public interface IPlaylistDAL
    {
        Task FavoriteTracksAutoUpdate(string userId);

        Task<List<Models.Playlist>> GetCurrentUserAllPlaylists(string userId);
        Task<Models.Playlist?> GetCurrentUserSelectedPlaylist(long playlistId, string userId);
        Task<bool> CreateNewPlaylist(Models.Playlist newPlaylist, string userId);
        Task<bool> AddTrackToPlaylist(Models.Track track, long playlistId, string userId);
        Task<bool> RemoveTrackFromPlaylist(long TrackId, long playlistId, string userId);
        Task<bool> RenamePlaylist(long playlistId, string newPlaylistName, string userId);
        Task<bool> RemovePlaylist(long playlistId, string userId);
    }
}