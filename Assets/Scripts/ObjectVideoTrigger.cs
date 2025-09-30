using UnityEngine;

public class ObjectVideoTrigger : MonoBehaviour
{
   
    public AnimalVideoSwitcher videoSwitcher;
    public UIManager uiManager;

    [Tooltip("Enter the sub video index to play")]
    public int subVideoIndex = 1; // Example: 1 for first sub video

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the user's hand
        if (other.CompareTag("Hand"))
        {
            if (videoSwitcher != null)
            {
                videoSwitcher.PlaySubVideo(subVideoIndex);
                uiManager.PopulateInfopanel(subVideoIndex);
            }
            else
            {
                Debug.LogWarning("AnimalVideoSwitcher is not assigned in ObjectVideoTrigger.");
            }
        }
    }
}
