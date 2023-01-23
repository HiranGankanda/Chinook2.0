namespace Chinook.Helpers
{
    public class ValidateMyFavoriteTracksPlaylist
    {
        public const string LIST_NAME = "My Favorite Tracks";
        private static readonly ILogger _logger;
        static ValidateMyFavoriteTracksPlaylist()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole()
                    .AddFilter(level => level >= LogLevel.Information);
            });

            _logger = loggerFactory.CreateLogger<ValidateMyFavoriteTracksPlaylist>();
        }

        public static int? CheckMyFavoriteTracksPlaylistAvailable(string userId)
        {
            try
            {
                //Need to implement this section

                //Return playlist Id
                return 1;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error happened [User: {0}]", userId);
                return null;
            }
        }
        public static bool CreateMyFavoriteTracksPlaylist()
        {
            try
            {
                //Need to implement this section
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened");
                return false;
            }
        }
    }
}