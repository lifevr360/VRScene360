using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer; // /storage/emulated/0/Android/obb/com.LifeVR.VRScene360/files/0.mp4
    public AudioSource audioSource;
    public List<string> videoUrls;

    private int currentVideoIndex = -1; // Track the currently playing video

    private void Start()
    {
        PlayVideo(0);
    }

    public void PlayVideo(int videoIndex)
    {
        // Check if the requested video is already playing
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
        Debug.Log("Stopped coroutines.");
        StopAllCoroutines();
        StartCoroutine(StartPlayingVideo(videoIndex));
       
    }


    private IEnumerator StartPlayingVideo(int index)
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            Debug.Log("Stopped current video.");
        }

       /* if (audioSource.isPlaying)
        {
            // Stop the currently playing audio
            audioSource.Stop();
            Debug.Log("Stopped current audio.");
        }*/

        videoPlayer.url = videoUrls[index];
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        videoPlayer.Play();
        currentVideoIndex = index;
       

        Debug.Log("Playing video from URL: " + videoUrls[index]);
    }

    public void OnPlayAudio(AudioClip newClip)
    {
        Debug.Log("Stopped video  coroutines.");
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
