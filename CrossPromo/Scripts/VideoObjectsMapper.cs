using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoObjectsMapper
{
    public AdVideoClip Map(VideoDataModel inputObject)
    {
        return new AdVideoClip((inputObject as VideoDataModel).id,
           (inputObject as VideoDataModel).click_url,
           (inputObject as VideoDataModel).tracking_url,
           (inputObject as VideoDataModel).local_path);
    }
}
