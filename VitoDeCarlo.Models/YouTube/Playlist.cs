namespace VitoDeCarlo.Models.YouTube;

public class Playlist
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description {  get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
    public int ItemCount { get; set; } = 0;
}
