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
                Title = list.snippet.title
            };
            playlists.Add(playlist);
        }

        return playlists;
    }
}
