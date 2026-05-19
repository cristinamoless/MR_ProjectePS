using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabDetach : MonoBehaviour
{
    Rigidbody rb;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grab.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        grab.transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
