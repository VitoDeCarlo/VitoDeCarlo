using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VitoDeCarlo.Models.YouTube;

namespace VitoDeCarlo.Core.Services;

public class YouTubeService : IYouTubeService
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public YouTubeService(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    /// <summary>
    /// Get all playlists that a user owns.
    /// </summary>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a list of playlists for the channelId.</returns>
    public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
    {
        var apiKey = configuration["Secrets:Google:ApiKey"];
        var jsonResult = await httpClient.GetStringAsync("https://youtube.googleapis.com/youtube/v3/playlists?key=" + apiKey + "&part=contentDetails%2Csnippet&channelId=UC_ESAegzhlBCnWdYYL4DjaA");

        Rootobject? result = JsonConvert.DeserializeObject<Rootobject>(jsonResult);

        List<Playlist> playlists = new List<Playlist>();
        foreach (var list in result.items)
        {
            Playlist playlist = new Playlist
            {
                Id = list.id,
                Title = list.snippet.title,
                ThumbnailUrl = list.snippet.thumbnails.standard.url,
                ItemCount = list.contentDetails.itemCount
            };
            playlists.Add(playlist);
        }

        return playlists;
    }

    /// <summary>
    /// Gets the Playlist Items within the provided Playlists.
    /// </summary>
    /// <param name="playlistId">The ID of the playlist for which you want to retrieve playlist items.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the playlist items for the specified <paramref name="playlistId"/>.</returns>
    public async Task<IEnumerable<PlaylistItem>> GetPlaylistItemsAsync(string playlistId)
    {
        var apiKey = configuration["Secrets:Google:ApiKey"];
        var jsonResult = await httpClient.GetStringAsync("https://youtube.googleapis.com/youtube/v3/playlistItems?part=contentDetails%2Cid%2Csnippet%2Cstatus&maxResults=50&playlistId=PLSTiKS27BXmxGp0l1NQwnN21LkmErxRCB&key=" + apiKey );

        Rootobject? result = JsonConvert.DeserializeObject<Rootobject>(jsonResult);

        List<PlaylistItem> playlistItems = new List<PlaylistItem>();
        foreach (var item in result.items)
        {
            PlaylistItem playlistItem = new PlaylistItem
            {
                Id = item.id,
                Title = item.snippet.title,
                Description = item.snippet.description,
                ChannelId = item.snippet.channelId,
                VideoId = item.contentDetails.videoId,
                PublishedAt = item.contentDetails.videoPublishedAt,
                ThumbnailUrl = item.snippet.thumbnails.standard.url,
                VideoOwnerChannelId = item.snippet.videoOwnerChannelId,
                VideoOwnerChannelTitle = item.snippet.videoOwnerChannelTitle,
                IsPublic = item.status.privacyStatus.Equals("public")
            };
            playlistItems.Add(playlistItem);
        }
        return playlistItems;
    }
}
