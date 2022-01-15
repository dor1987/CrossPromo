using System.Collections.Generic;

public class VideoDataModel
{
    public string id { get; set; }
    public string video_url { get; set; }
    public string click_url { get; set; }
    public string tracking_url { get; set; }
    public string local_path { get; set; }
}

public class VideosContainerResponse
{
    public List<VideoDataModel> results { get; set; }
}


