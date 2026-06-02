using UnityEngine;

public class ModelSwapReceiver : MonoBehaviour
{
    public GameObject flowerFullModel;
    public GameObject flowerCutModel;

    private bool alreadySwapped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadySwapped)
            return;

        InteractableType type = other.GetComponent<InteractableType>();

        if (type == null)
            return;

        // si tisores toca flor
        if (type.type == InteractableType.Type.Scissors)
        {
            SwapFlower();
        }
    }

    void SwapFlower()
    {
        alreadySwapped = true;

        flowerFullModel.SetActive(false);
        flowerCutModel.SetActive(true);

        // Notify the Flower component that the leaves have been removed
        Flower flower = GetComponent<Flower>();
        if (flower == null)
            flower = GetComponentInParent<Flower>();
        if (flower == null)
            flower = GetComponentInChildren<Flower>();

        if (flower != null)
        {
            flower.RemoveLeaves();
        }
    }
}