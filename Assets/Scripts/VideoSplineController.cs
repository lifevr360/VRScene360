using UnityEngine;
using UnityEngine.Video;
using Dreamteck.Splines;

public class VideoSplineController : MonoBehaviour
{
    [Header("References")]
    public VideoManager videoManager; // assign so we can subscribe to OnVideoStarted
    public VideoPlayer videoPlayer;   // the same VideoPlayer used by VideoManager
    public SplineFollower follower;   // Dreamteck follower

    [Header("Playback Settings")]
    public bool loop = false;         // toggle looping from Inspector
    public float fastFactor = 3f;     // how much faster when skipping

    private float normalSpeed;
    private bool speeding = false;
    public double targetPercent = -1;

    void Start()
    {
        // We keep the VideoPlayer's isLooping false and handle behavior ourselves
        if (videoPlayer != null) videoPlayer.isLooping = false;

        // compute normalSpeed later once video is prepared.
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;

        // subscribe to follower end
        follower.onEndReached += OnLoopComplete;

        // Subscribe to VideoManager so we can restore spline percent when needed
        if (videoManager != null)
        {
            videoManager.OnVideoStarted += HandleVideoStarted;
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null) videoPlayer.prepareCompleted -= OnVideoPrepared;
        if (follower != null) follower.onEndReached -= OnLoopComplete;
        if (videoManager != null) videoManager.OnVideoStarted -= HandleVideoStarted;
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        // calculate normalSpeed based on video length and spline length
        float videoLength = Mathf.Max(0.0001f, (float)videoPlayer.length);
        double splineLength = follower.spline.CalculateLength();
        normalSpeed = (float)(splineLength / videoLength);

        follower.followSpeed = normalSpeed;

        // If not playing yet, start follower if video is already playing
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
        follower.Restart();
    }

    void Update()
    {
        // Click detection for boosting (only in editor or non-VR pointer)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                SplineSample sample = follower.spline.Project(hit.point);
                BoostTowards(sample.percent);
            }
        }

        // Stop fast-forward when target reached
        if (speeding && targetPercent >= 0)
        {
            double currentPercent = follower.result.percent;
            if (Mathf.Abs((float)(currentPercent - targetPercent)) < 0.01f)
            {
                follower.followSpeed = normalSpeed;
                videoPlayer.playbackSpeed = 1f;
                speeding = false;
                targetPercent = -1;
            }
        }
    }

    public void BoostTowards(double percent)
    {
        targetPercent = percent;
        follower.followSpeed = normalSpeed * fastFactor;
        videoPlayer.playbackSpeed = fastFactor;
        speeding = true;
    }

    // If we reach the spline end:
    private void OnLoopComplete(double percent)
    {
        if (!loop) return;

        follower.Restart();
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    // Handler for when VideoManager actually starts a video.
    // We use this to sync spline position when base (index 0) is resumed.
    private void HandleVideoStarted(int index, double resumeTime, double resumePercent)
    {
        // Only care about base video (index 0) resuming
        if (index != 0) return;

        // If a valid resumePercent was supplied, set follower to that percent and resume motion
        if (resumePercent >= 0 && resumePercent <= 1.0)
        {
            // Place follower on the requested percent
            // Dreamteck provides SetPercent on followers
            follower.SetPercent(resumePercent);

            // Ensure follower is moving at normal speed
            follower.followSpeed = normalSpeed;
        }

        // If resumeTime provided, ensure video time is set; videoManager already handles this,
        // but we defensively ensure the follower/video are playing
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }
}
