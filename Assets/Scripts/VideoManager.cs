using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer; // url based video player
    public AudioSource audioSource;
    public List<string> videoUrls;

    private int currentVideoIndex = -1; // Track the currently playing video

    // Event fired once a new video has been prepared and started.
    // Args: videoIndex, resumeTime (or -1 if none), resumePercent (or -1 if none)
    public event Action<int, double, double> OnVideoStarted;

    /// <summary>
    /// Play a video by index. Optionally pass resumeTime (seconds) and resumePercent (0..1).
    /// If resumeTime >= 0 the player will set time after preparing.
    /// </summary>
    public void PlayVideo(int videoIndex, double resumeTime = -1, double resumePercent = -1)
    {
        // If same index and already playing, ignore
        if (videoIndex == currentVideoIndex && videoPlayer.isPlaying)
        {
            Debug.Log("Video is already playing. Ignoring request.");
            return;
        }

        if (videoIndex < 0 || videoIndex >= videoUrls.Count)
        {
            Debug.LogError("Invalid video index: " + videoIndex);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(StartPlayingVideo(videoIndex, resumeTime, resumePercent));
    }

    private IEnumerator StartPlayingVideo(int index, double resumeTime = -1, double resumePercent = -1)
    {
        // Stop current
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            Debug.Log("Stopped current video.");
        }

        // Set URL and prepare
        videoPlayer.url = videoUrls[index];
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // If a resume time is provided, set it before Play
        if (resumeTime >= 0)
        {
            // Clamp to video length if required
            double clamped = Mathf.Clamp((float)resumeTime, 0f, (float)videoPlayer.length);
            videoPlayer.time = clamped;
        }

        videoPlayer.Play();
        currentVideoIndex = index;

        Debug.Log($"Playing video index {index} from URL: {videoUrls[index]} (resumeTime: {resumeTime}, resumePercent: {resumePercent})");

        // Fire event so other systems (e.g., spline controller) can sync themselves
        OnVideoStarted?.Invoke(index, resumeTime, resumePercent);
    }

    public void OnPlayAudio(AudioClip newClip)
    {
        Debug.Log("Stopped video coroutines.");
        StopAllCoroutines();

        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            Debug.Log("Stopping videos");
            videoPlayer.Stop();
        }

        if (audioSource.isPlaying)
        {
            // Stop the currently playing audio
            audioSource.Stop();
        }

        // Assign the new clip and play it
        audioSource.clip = newClip;
        audioSource.Play();
    }
}
