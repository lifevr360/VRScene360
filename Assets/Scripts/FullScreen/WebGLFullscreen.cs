using UnityEngine;
using System.Runtime.InteropServices;

public class WebGLFullscreen : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void EnterFullscreen();
    [DllImport("__Internal")] private static extern void ExitFullscreen();
    [DllImport("__Internal")] private static extern bool IsFullscreen();
#endif

    [Header("UI Buttons")]
    public GameObject enterFullscreenButton;
    public GameObject exitFullscreenButton;

    bool lastFullscreenState = false;

    void Start()
    {
        UpdateButtons();
    }

    public void OnEnterFullscreenClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EnterFullscreen();
#endif
        Invoke(nameof(UpdateButtons), 0.1f); // allow browser state update
    }

    public void OnExitFullscreenClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ExitFullscreen();
#endif
        Invoke(nameof(UpdateButtons), 0.1f);
    }

    void Update()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        bool currentState = IsFullscreen();

        if (currentState != lastFullscreenState)
        {
            UpdateButtons();
            lastFullscreenState = currentState;
        }
#endif
    }

    void UpdateButtons()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        bool isFs = IsFullscreen();
#else
        bool isFs = Screen.fullScreen;
#endif
        if (enterFullscreenButton)
            enterFullscreenButton.SetActive(!isFs);

        if (exitFullscreenButton)
            exitFullscreenButton.SetActive(isFs);
    }
}
