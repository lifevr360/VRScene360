using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSphereColliderToTip : MonoBehaviour
{
    [System.Serializable]
    public class FingerSettings
    {
        public string fingerName; // Name of the fingertip bone
        public float colliderRadius = 0.05f; // Radius of the SphereCollider
        public Vector3 colliderOffset = Vector3.zero; // Offset for the SphereCollider
    }

    public GameObject parentObject; // Reference to RightOVRHand
    public string tagToAssign = "Hand"; // Tag to assign to each finger
    public FingerSettings[] fingers; // Array to define each finger's settings

    void Start()
    {
        StartCoroutine(AttachCollidersToFingers());
    }

    private IEnumerator AttachCollidersToFingers()
    {
        yield return new WaitForSeconds(1f);

        if (parentObject == null)
        {
            Debug.LogError("Parent object is not assigned.");
            yield break;
        }

        foreach (FingerSettings finger in fingers)
        {
            Transform fingerTip = FindChildRecursive(parentObject.transform, finger.fingerName);

            if (fingerTip == null)
            {
                Debug.LogError($"Finger bone named '{finger.fingerName}' not found under '{parentObject.name}'.");
                continue;
            }

            // Assign the specified tag
            fingerTip.gameObject.tag = tagToAssign;

            // Attach a SphereCollider to the found finger bone
            SphereCollider sphereCollider = fingerTip.gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = finger.colliderRadius;
            sphereCollider.center = finger.colliderOffset;
            sphereCollider.isTrigger = true;

            Debug.Log($"SphereCollider added to {finger.fingerName} with radius {finger.colliderRadius} and offset {finger.colliderOffset}");
        }
    }

    // Recursive method to find a child by name
    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            // Recursively search in the child's hierarchy
            Transform result = FindChildRecursive(child, childName);
            if (result != null)
                return result;
        }

        return null; // Return null if the child is not found
    }
}
