using Oculus.Interaction;
using UnityEngine;

public class VideoSplinePokeHandler : MonoBehaviour
{
    public VideoSplineController controller;

    private PokeInteractable pokeInteractable;

  /*  void Awake()
    {
        pokeInteractable = GetComponent<PokeInteractable>();
        pokeInteractable.WhenPointerEventRaised += OnPoke;
    }*/

    public void OnPoke(PointerEvent evt)
    {
        Debug.Log("poking");
        if (evt.Type == PointerEventType.Select)
        {
            Debug.Log("poked");
            Vector3 pokePoint = evt.Pose.position;
            var sample = controller.follower.spline.Project(pokePoint);
            controller.BoostTowards(sample.percent);
        }
    }

    public void poke()
    {
        Debug.Log("poking");
    }

    private void OnTriggerEnter(Collider other)
    {
        poke();
        if (other.CompareTag("Hand"))
        {
            Vector3 pokePoint = other.transform.position;
            var sample = controller.follower.spline.Project(pokePoint);

            controller.BoostTowards(sample.percent);
        }
    }
}
