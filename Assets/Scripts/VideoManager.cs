using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public List<VideoClip> videoClips;  // Drag and drop VideoClips here in the Inspector

    private int currentVideoIndex = -1;

    private void Start()
    {
        // Play the first video automatically (optional)
        if (videoClips.Count > 0)
            PlayVideo(0);
        else
            Debug.LogWarning("No video clips assigned to VideoManager.");
    }

    public void PlayVideo(int videoIndex)
    {
        // Ignore request if the same video is already playing
        if (videoIndex == currentVideoIndex && videoPlayer.isPlaying)
        {
            Debug.Log("Video is already playing. Ignoring request.");
            return;
        }

        // Check for valid index
        if (videoIndex < 0 || videoIndex >= videoClips.Count)
        {
            Debug.LogError("Invalid video index: " + videoIndex);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(StartPlayingVideo(videoIndex));
    }

    private IEnumerator StartPlayingVideo(int index)
    {
        // Stop current video if playing
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            Debug.Log("Stopped current video.");
        }

        // Assign new clip and prepare
        videoPlayer.clip = videoClips[index];
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Play video
        videoPlayer.Play();
        currentVideoIndex = index;
        Debug.Log("Playing video: " + videoClips[index].name);
    }

    public void OnPlayAudio(AudioClip newClip)
    {
        Debug.Log("Stopped video coroutines.");
        StopAllCoroutines();

        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            Debug.Log("Stopping video playback.");
            videoPlayer.Stop();
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = newClip;
        audioSource.Play();
    }
}
