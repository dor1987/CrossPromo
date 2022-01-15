using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideosDisplayManager : MonoBehaviour
{
    public Controller controller;
    private List<AdVideoClip> videoClips;
    private VideoPlayer videoPlayer;
    private int videoClipIndex;
    private RenderTexture renderTexture;
    public RawImage rawImage;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }
    void Start()
    {
        controller.OnVideosAreAvilable.AddListener(VideosAreAvilable);

        //Create the renderTexture
        renderTexture = new RenderTexture(200, 200, 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        //Attach the renderTexture to the rawImage and to the videoPlayer
        rawImage.GetComponent<RawImage>().texture = renderTexture;
        videoPlayer.targetTexture = renderTexture;
    }

    private void OnDestroy()
    {
        controller.OnVideosAreAvilable.RemoveListener(VideosAreAvilable);
        renderTexture.Release();
    }
    private void VideosAreAvilable()
    {
        videoClips = controller.GetVideosToPlay();
        videoPlayer.url = videoClips[0].mLocal_path;
        Resume();
    }

    public void OnVideoClicked()
    {
        if(videoClips != null)
            controller.UserClickedOnVideo(videoClips[videoClipIndex]);
    }

    public void Next()
    {
        if (videoClips != null) 
        { 
            videoClipIndex++;
            if(videoClipIndex >= videoClips.Count)
            {
                videoClipIndex = videoClipIndex % videoClips.Count;
            }
            videoPlayer.url = videoClips[videoClipIndex].mLocal_path;
        }
    }

    public void Previous()
    {
        if (videoClips != null)
        {
            videoClipIndex--;
            if (videoClipIndex < 0)
            {
                videoClipIndex = videoClips.Count - 1;
            }
            videoPlayer.url = videoClips[videoClipIndex].mLocal_path;
        }
    }

    public void Pause()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
    }

    public void Resume()
    {
        if (!videoPlayer.isPlaying)
            videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        Next();
    }
}
