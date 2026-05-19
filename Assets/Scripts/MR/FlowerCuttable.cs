using UnityEngine;

public class FlowerCuttable : MonoBehaviour
{
    public GameObject fullModel;
    public GameObject cutModel;

    private bool isCut = false;

    void Start()
    {
        fullModel.SetActive(true);
        cutModel.SetActive(false);
    }

    public void CutFlower()
    {
        if (isCut)
            return;

        isCut = true;

        fullModel.SetActive(false);
        cutModel.SetActive(true);

        // Afegir física al model tallat
        Rigidbody rb = cutModel.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

}