﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json;
using VitoDeCarlo.Models.YouTube;

namespace VitoDeCarlo.Core.Services.YouTube;

public class PlaylistService : IPlaylistService
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public PlaylistService(HttpClient httpClient, IConfiguration configuration)
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
        foreach(var list in result.items)
        {
            Playlist playlist = new Playlist
            {
                Id = list.id,
                Title = list.snippet.title
            };
            playlists.Add(playlist);
        }

        return playlists;

        //using var document = JsonDocument.Parse(jsonResult);
        //var query =
        //    from playlist in document.RootElement.GetProperty("items").EnumerateArray()
        //    select new Playlist
        //    {
        //        Id = playlist.GetProperty("id").GetString() ?? String.Empty,
        //        Title = playlist.GetProperty("snippet").GetProperty("title").GetString() ?? String.Empty
        //    };
        //return query;

        //var playlists =
        //    from playlist in result.items
        //    where playlist.status.privacyStatus == "public"
        //    select new Playlist
        //    {
        //        Id = playlist.id,
        //        Title = playlist.snippet.title,
        //        Description = playlist.snippet.description,
        //        ThumbnailUrl = playlist.snippet.thumbnails.medium.url
        //    };
        //return playlists;
    }
}
