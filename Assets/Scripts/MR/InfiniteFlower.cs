using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InfiniteFlower : MonoBehaviour
{
    public GameObject flowerPrefab;

    public XRInteractionManager interactionManager;

    private XRGrabInteractable grabInteractable;

    private bool spawning = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (spawning)
            return;

        spawning = true;

        IXRSelectInteractor interactor =
            args.interactorObject;

        GameObject newFlower = Instantiate(
            flowerPrefab,
            transform.position,
            transform.rotation
        );

        Rigidbody rb = newFlower.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        XRGrabInteractable newGrab =
            newFlower.GetComponent<XRGrabInteractable>();

        interactionManager.SelectEnter(
            interactor,
            newGrab
        );

        Invoke(nameof(ResetSpawn), 0.1f);
    }

    void ResetSpawn()
    {
        spawning = false;
    }
}