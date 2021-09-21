﻿using VitoDeCarlo.Models.YouTube;

namespace VitoDeCarlo.Core.Services.YouTube;

public interface IPlaylistService
{
    Task<IEnumerable<Playlist>> GetPlaylistsAsync();
}

public class Rootobject
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string nextPageToken { get; set; }
    public Pageinfo pageInfo { get; set; }
    public Item[] items { get; set; }
}

public class Pageinfo
{
    public int totalResults { get; set; }
    public int resultsPerPage { get; set; }
}

public class Item
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string id { get; set; }
    public Snippet snippet { get; set; }
    public Status status { get; set; }
    public Contentdetails contentDetails { get; set; }
}

public class Snippet
{
    public DateTime publishedAt { get; set; }
    public string channelId { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public Thumbnails thumbnails { get; set; }
    public string channelTitle { get; set; }
    public Localized localized { get; set; }
}

public class Thumbnails
{
    public Default _default { get; set; }
    public Medium medium { get; set; }
    public High high { get; set; }
    public Standard standard { get; set; }
    public Maxres maxres { get; set; }
}

public class Default
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Medium
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class High
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Standard
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Maxres
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Localized
{
    public string title { get; set; }
    public string description { get; set; }
}

public class Status
{
    public string privacyStatus { get; set; }
}

public class Contentdetails
{
    public int itemCount { get; set; }
}