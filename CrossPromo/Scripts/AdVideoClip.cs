
public class AdVideoClip
{
    public AdVideoClip(string id, string click_url,string tracking_url,string local_path)
    {
        mId = id;
        mClick_url = click_url;
        mTracking_url = tracking_url;
        mLocal_path = local_path;
    }

    public string mId { get; set; }
    public string mClick_url { get; set; }
    public string mTracking_url { get; set; }
    public string mLocal_path { get; set; }
}
