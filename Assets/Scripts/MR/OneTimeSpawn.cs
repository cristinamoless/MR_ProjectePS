using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OneTimeSpawn : MonoBehaviour
{
    public GameObject objectPrefab;

    public XRInteractionManager interactionManager;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private bool hasSpawned = false;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (hasSpawned)
            return;

        hasSpawned = true;

        UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor =
            args.interactorObject;

        GameObject spawnedObject = Instantiate(
            objectPrefab,
            transform.position,
            transform.rotation
        );

        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable newGrab =
            spawnedObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        interactionManager.SelectEnter(
            interactor,
            newGrab
        );

        // opcional:
        // amagar les tisores fake de la taula

        gameObject.SetActive(false);
    }
}