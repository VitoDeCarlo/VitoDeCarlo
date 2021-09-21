namespace VitoDeCarlo.Models.YouTube;

public class PlaylistItem
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string ChannelId { get; set; } = null!;
    public string VideoId { get; set; } = null!;
    public DateTime PublishedAt { get; set; } = DateTime.Now;
    public string ThumbnailUrl { get; set; } = null!;
    public string VideoOwnerChannelTitle { get; set; } = null!;
    public string VideoOwnerChannelId { get; set; } = null!;
    public bool IsPublic { get; set; } = true;
}
