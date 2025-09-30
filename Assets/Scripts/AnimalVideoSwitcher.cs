using UnityEngine;
using UnityEngine.Video;

public class AnimalVideoSwitcher : MonoBehaviour
{
    [Header("Reference to Video Manager")]
    public VideoManager videoManager;

    [Header("UI References")]
    public GameObject backButton;     // UI with back button

    private double savedBaseVideoTime = 0;
    private bool isInSubVideo = false;

    private void Start()
    {
        // Play the base video on start (index 0 in videoUrls)
        videoManager.PlayVideo(0);
        backButton.SetActive(false);

        // Subscribe to video end event
        videoManager.videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlaySubVideo(int index)
    {
        // index corresponds to videoUrls[index]
        // base is 0, so sub videos should start from index 1
        if (index < 1 || index >= videoManager.videoUrls.Count)
        {
            Debug.LogWarning("Invalid sub video index: " + index);
            return;
        }

        // Save the base video timestamp
        if (!isInSubVideo)
        {
            savedBaseVideoTime = videoManager.videoPlayer.time;
        }

        isInSubVideo = true;
        backButton.SetActive(true);

        // Play the selected sub video
        videoManager.PlayVideo(index);
    }

    public void BackToBaseVideo()
    {
        isInSubVideo = false;
        backButton.SetActive(false);

        // Resume base video (index 0)
        videoManager.PlayVideo(0);

        // Restore timestamp
        videoManager.videoPlayer.time = savedBaseVideoTime;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (isInSubVideo)
        {
            BackToBaseVideo();
        }
    }
}
