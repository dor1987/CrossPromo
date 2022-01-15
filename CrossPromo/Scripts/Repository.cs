using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Repository : GenericSingletonClass<Repository> 
{
    [HideInInspector]
    public UnityEvent OnDataReady = new UnityEvent();

    HashSet<string> tarckingRequestsDataSet = new HashSet<string>();
    private VideoObjectsMapper mapper = new VideoObjectsMapper();
    string url = "https://run.mocky.io/v3/81fab340-9550-4ab4-8859-836b01ee48ff";
    MyHttpClient httpClient = new MyHttpClient(new JsonSerializationOption());
    private VideosContainerResponse result;
    private string fileDirectoryPath;
    private string correctFormatEnding = ".mp4";
    private void Start()
    {
        //DeleteAllSavedFiles();
        fileDirectoryPath = Application.persistentDataPath + "/AdsVideos";
        GetData();
    }


    public List<AdVideoClip> FetchVideos()
    {
        List<AdVideoClip> tempList = new List<AdVideoClip>();
        foreach(VideoDataModel vdm in result.results)
        {
            tempList.Add(mapper.Map(vdm));
        }
        return tempList;
    }

    //FOR DEBUG
    private void DeleteAllSavedFiles()
    {
        if (Directory.Exists(fileDirectoryPath))
        {
            Directory.Delete(fileDirectoryPath,true);
            Debug.Log(" Directory.Exists " + Directory.Exists(fileDirectoryPath).ToString());
        }
    }

    private List<string> GetVideosPathFormDataBase()
    {
        List<string> videoSource = new List<string>();

        if (Directory.Exists(fileDirectoryPath))
        {
            DirectoryInfo d = new DirectoryInfo(fileDirectoryPath);
            foreach (FileInfo fileInfo in d.GetFiles("*"+ correctFormatEnding))
            {
                videoSource.Add(fileInfo.FullName);
                Debug.Log(fileInfo.FullName);
            }
            return videoSource;
        } else {
            Directory.CreateDirectory(fileDirectoryPath);
            return videoSource;
        }
    }

    private string GetSingleVideoPathFormDataBase(string id)
    {
        if (Directory.Exists(fileDirectoryPath))
        {
            DirectoryInfo d = new DirectoryInfo(fileDirectoryPath);
            foreach (FileInfo fileInfo in d.GetFiles("*"+ correctFormatEnding))
            {
                if (fileInfo.Name.Equals(id + correctFormatEnding))
                    return fileInfo.FullName;
            }
            return "";
        }
        else
        {
            Directory.CreateDirectory(fileDirectoryPath);
            return "";
        }
    }
    private async void GetData()
    {
        result = await httpClient.Get<VideosContainerResponse>(url);
        if(result!= null) { 
            GetVideosFromWeb(result);
            DeleteUnusedVideosFromDataBase(result);
        }
    }
    private void DeleteUnusedVideosFromDataBase(VideosContainerResponse videosContainerResponse)
    {
        if (!Directory.Exists(fileDirectoryPath))
            Directory.CreateDirectory(fileDirectoryPath);

        DirectoryInfo d = new DirectoryInfo(fileDirectoryPath);

        HashSet<string> dataHolder = new HashSet<string>();
        foreach (VideoDataModel vdm in videosContainerResponse.results)
            dataHolder.Add(vdm.id + correctFormatEnding);

        foreach (FileInfo fileInfo in d.GetFiles("*"+ correctFormatEnding))
        {
            if (!dataHolder.Contains(fileInfo.Name))
            {
                File.Delete(fileInfo.FullName);
                Debug.Log(fileInfo.FullName + " DELETED");
            }
        }
    }
    private async void GetVideosFromWeb(VideosContainerResponse videosContainerResponse)
    {
        foreach(VideoDataModel vdm in videosContainerResponse.results){
            if (!IsVideoSavedLocaly(vdm.id))
            {
                string path = SaveVideoToCache(await httpClient.GetFile<byte[]>(vdm.video_url), vdm.id);
                vdm.local_path = path;
            }
            else
            {
                vdm.local_path = GetSingleVideoPathFormDataBase(vdm.id);
            }
        }
        OnDataReady?.Invoke();
    }
    private bool IsVideoSavedLocaly(string id)
    {
        if (Directory.Exists(fileDirectoryPath))
        {
            DirectoryInfo d = new DirectoryInfo(fileDirectoryPath);
            foreach (FileInfo fileInfo in d.GetFiles("*"+ correctFormatEnding))
            {
                if(fileInfo.Name.Equals(id+ correctFormatEnding))
                    return true;
            }
            return false;
        }
        else
        {
            Directory.CreateDirectory(fileDirectoryPath);
            return false;
        }
    }

    private string SaveVideoToCache(byte[] fileInByte,string id)
    {
        if (!Directory.Exists(fileDirectoryPath))
            Directory.CreateDirectory(fileDirectoryPath);

        if (!File.Exists(fileDirectoryPath + id + correctFormatEnding))
        {
            File.WriteAllBytes(fileDirectoryPath + id + correctFormatEnding, fileInByte);
        }

        return fileDirectoryPath + id + correctFormatEnding;
    }

    public async void SendTrackingRequest(string playerId, AdVideoClip adVideoClip)
    {
        if (!tarckingRequestsDataSet.Contains(adVideoClip.mTracking_url))
        {
            tarckingRequestsDataSet.Add(adVideoClip.mTracking_url);
            await httpClient.Post<string>(adVideoClip.mTracking_url.Replace("[PLAYER_ID]", playerId));
        }
    }
}

