using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent OnVideosAreAvilable = new UnityEvent();
    public string playerId = ""; //If developer doesnt set id keep empty

    private Repository mRepository;
    private List<AdVideoClip> currentVideoList;

    void Start()
    {
        mRepository = Repository.Instance;
        mRepository.OnDataReady.AddListener(NewData);
    }

    private void OnDestroy()
    {
        mRepository.OnDataReady.RemoveListener(NewData);
    }

    public void UserClickedOnVideo(AdVideoClip currentVideoPlaying)
    {
        Application.OpenURL(currentVideoPlaying.mClick_url);
        mRepository.SendTrackingRequest(playerId, currentVideoPlaying);
    }
    private void NewData()
    {
        currentVideoList = mRepository.FetchVideos();
        OnVideosAreAvilable?.Invoke();
    }

    public List<AdVideoClip> GetVideosToPlay()
    {
        return currentVideoList;
    }
}
