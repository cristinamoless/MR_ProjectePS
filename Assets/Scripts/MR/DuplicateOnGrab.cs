using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DuplicateOnGrab : MonoBehaviour
{
    [Header("Prefab que es crearà")]
    public GameObject clonePrefab;

    [Header("XR Interaction Manager")]
    public XRInteractionManager interactionManager;

    private XRGrabInteractable grab;

    // IMPORTANT:
    // només l'original pot duplicar
    private bool canDuplicate = true;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        if (interactionManager == null)
            interactionManager = FindFirstObjectByType<XRInteractionManager>();

        grab.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // les còpies NO poden duplicar
        if (!canDuplicate)
            return;

        IXRSelectInteractor interactor = args.interactorObject;

        // crear còpia
        GameObject clone = Instantiate(
            clonePrefab,
            transform.position,
            transform.rotation
        );

        // IMPORTANT:
        // la còpia ja NO pot duplicar més
        DuplicateOnGrab duplicateScript =
            clone.GetComponent<DuplicateOnGrab>();

        if (duplicateScript != null)
        {
            duplicateScript.canDuplicate = false;
        }

        // agafar automàticament la còpia
        XRGrabInteractable cloneGrab =
            clone.GetComponent<XRGrabInteractable>();

        if (cloneGrab != null)
        {
            interactionManager.SelectEnter(
                interactor,
                cloneGrab
            );
        }
    }
}