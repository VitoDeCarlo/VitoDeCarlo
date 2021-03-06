using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VitoDeCarlo.Models.YouTube;

namespace VitoDeCarlo.Core.Services;

public class YouTubeService : IYouTubeService
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    private readonly string googleApiKey = string.Empty;

    public YouTubeService(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
        googleApiKey = configuration["Secrets:Google:ApiKey"];
    }

    /// <summary>
    /// Get all playlists that a user owns.
    /// </summary>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a list of playlists for the channelId.</returns>
    public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
    {
        var jsonResult = await httpClient.GetStringAsync("playlists?key=" + googleApiKey + "&part=contentDetails%2Csnippet&channelId=UC_ESAegzhlBCnWdYYL4DjaA");

        Rootobject? result = JsonConvert.DeserializeObject<Rootobject>(jsonResult);

        List<Playlist> playlists = new();
        foreach (var list in result.items)
        {
            Playlist playlist = new()
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
        string requestUri = "playlistItems?playlistId=" + playlistId + "&key=" + googleApiKey;
        string part = Uri.EscapeDataString("contentDetails,id,snippet,status");
        requestUri += "&part=" + part;
        requestUri += "&maxResults=50";
        var jsonResult = await httpClient.GetStringAsync(requestUri);

        Rootobject? result = JsonConvert.DeserializeObject<Rootobject>(jsonResult);

        List<PlaylistItem> playlistItems = new();
        foreach (var item in result.items)
        {
            PlaylistItem playlistItem = new()
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
