using UnityEngine;
using UnityEngine.Video;
using Dreamteck.Splines;

public class VideoSplineController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public SplineFollower follower;

    [Header("Playback Settings")]
    public bool loop = true;         // Toggle looping from Inspector
    public float fastFactor = 3f;    // How much faster when skipping

    private float normalSpeed;
    private bool speeding = false;
    public double targetPercent = -1;

    void Start()
    {
        // Video player loop handled manually (we'll control it via "loop" variable)
        videoPlayer.isLooping = false;
        follower.onEndReached += OnLoopComplete; // subscribe to event
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        float videoLength = (float)videoPlayer.length;
        double splineLength = follower.spline.CalculateLength();

        // Calculate base speed
        normalSpeed = (float)(splineLength / videoLength);

        follower.followSpeed = normalSpeed;
        videoPlayer.Play();
        follower.Restart();
    }

    void Update()
    {
        // --- Click detection ---
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Project to spline
                SplineSample sample = follower.spline.Project(hit.point);
                BoostTowards(sample.percent);
            }
        }

        // --- Fast forward stop check ---
        if (speeding && targetPercent >= 0)
        {
            double currentPercent = follower.result.percent;

            if (Mathf.Abs((float)(currentPercent - targetPercent)) < 0.01f)
            {
                // Restore normal speeds
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

    private void OnLoopComplete(double percent)
    {
        if (loop)
        {
            follower.Restart();
            videoPlayer.frame = 0;
            videoPlayer.Play();
        }
        else
        {
            // If video finished, stop follower too
            if (videoPlayer.isPlaying == false)
            {
                follower.followSpeed = 0f;
            }
        }
    }
}
