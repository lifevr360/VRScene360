using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSwitcher : MonoBehaviour
{
    public RawImage videoDisplay;
    public VideoPlayer videoPlayer1;
    public VideoPlayer videoPlayer2;
    public float switchTimeBeforeEnd = 0.2f; // Switch before video ends (adjust if needed)

    private bool isFirstVideoPlaying = true;

    void Start()
    { 
    }

    public void PlayFirstVideo()
    {
        // Play the first video
        videoPlayer1.Play();

        // Prepare the second video in advance
        videoPlayer2.Prepare();

        Debug.Log("Video Player 1 length " + (float)videoPlayer1.length);
        // Schedule the switch before the first video ends
        //Invoke("SwitchToSecondVideo", (float)videoPlayer1.length - switchTimeBeforeEnd);
    }

    void SwitchToSecondVideo()
    {
        if (!isFirstVideoPlaying) return;

        // Switch display to the second video player
        videoDisplay.texture = videoPlayer2.targetTexture;

        // Play the second video
        videoPlayer2.Play();

        // Disable the first video player after switching
        videoPlayer1.Stop();
        videoPlayer1.gameObject.SetActive(false);

        isFirstVideoPlaying = false;
    }
}
