using UnityEngine;
using UnityEngine.Video;

public class VideoUrlPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if UNITY_WEBGL
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"static_tv_1.mp4");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
