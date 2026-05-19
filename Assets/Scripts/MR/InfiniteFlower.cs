using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InfiniteFlower : MonoBehaviour
{
    public GameObject flowerPrefab;

    public XRInteractionManager interactionManager;

    private XRGrabInteractable grabInteractable;
    private bool spawning = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (interactionManager == null)
            interactionManager = FindFirstObjectByType<XRInteractionManager>();

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (spawning)
            return;

        spawning = true;

        UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor = args.interactorObject;

        GameObject newFlower = Instantiate(
            flowerPrefab,
            transform.position,
            transform.rotation
        );

        XRGrabInteractable newGrab = newFlower.GetComponent<XRGrabInteractable>();

        if (newGrab != null)
        {
            interactionManager.SelectEnter(interactor, newGrab);
        }

        Invoke(nameof(ResetSpawn), 0.1f);
    }

    void ResetSpawn()
    {
        spawning = false;
    }
}
