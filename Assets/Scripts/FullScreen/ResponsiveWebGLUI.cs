using UnityEngine;

public class ResponsiveUILayout : MonoBehaviour
{
    public GameObject desktopLayout;
    public GameObject mobileLayout;

    const float MOBILE_ASPECT_THRESHOLD = 0.9f;
    float lastAspect = -1f;

    void Update()
    {
        float aspect = (float)Screen.width / Screen.height;

        if (Mathf.Abs(aspect - lastAspect) > 0.01f)
        {
            ApplyLayout(aspect);
            lastAspect = aspect;
        }
    }

    void ApplyLayout(float aspect)
    {
        bool isMobile = aspect < MOBILE_ASPECT_THRESHOLD;

        desktopLayout.SetActive(!isMobile);
        mobileLayout.SetActive(isMobile);
    }
}
