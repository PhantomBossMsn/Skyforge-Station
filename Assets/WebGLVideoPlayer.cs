using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoPlayer : MonoBehaviour
{
    [Header("Video file name with extension (e.g. video.mp4)")]
    public string videoFileName = "video.mp4";

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void PlayVideo()
    {
        if (videoPlayer == null)
        {
            Debug.LogWarning("No VideoPlayer component found!");
            return;
        }

        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL needs direct path access, not file://
        videoPath = Application.streamingAssetsPath + "/" + videoFileName;
#endif

        Debug.Log("Video Path: " + videoPath);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }
}
