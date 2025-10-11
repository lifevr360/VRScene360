using UnityEngine;
using UnityEngine.Video;

public class AnimalVideoSwitcher : MonoBehaviour
{
    [Header("Reference to Video Manager")]
    public VideoManager videoManager;

    [Header("Reference to Spline Controller (optional)")]
    public VideoSplineController videoSplineController; // assign if you're using splines

    [Header("UI References")]
    public GameObject backButton;     // UI with back button

    private double savedBaseVideoTime = 0;
    private double savedBasePercent = -1;
    private bool isInSubVideo = false;

    private void Start()
    {
        // Play the base video on start (index 0 in videoUrls)
        videoManager.PlayVideo(0);
        backButton.SetActive(false);

        // Subscribe to video end event (optional)
        if (videoManager != null && videoManager.videoPlayer != null)
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

        // Save the base video timestamp and spline percent ONLY when switching from base -> first sub
        if (!isInSubVideo)
        {
            if (videoManager != null && videoManager.videoPlayer != null)
            {
                savedBaseVideoTime = videoManager.videoPlayer.time;
            }

            if (videoSplineController != null && videoSplineController.follower != null)
            {
                // follower.result.percent is the current percent on spline
                savedBasePercent = videoSplineController.follower.result.percent;
            }

            isInSubVideo = true;
            backButton.SetActive(true);
        }

        // Play the selected sub video (we don't touch savedBase values when swapping between subs)
        videoManager.PlayVideo(index);
    }

    public void BackToBaseVideo()
    {
        isInSubVideo = false;
        backButton.SetActive(false);

        // Resume base video (index 0) and pass both resumeTime and resumePercent
        videoManager.PlayVideo(0, savedBaseVideoTime, savedBasePercent);
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (isInSubVideo)
        {
            BackToBaseVideo();
        }
    }
}
