using UnityEngine;

public class ScissorsBlade : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FlowerCuttable cuttable = other.GetComponentInParent<FlowerCuttable>();

        if (cuttable != null)
        {
            cuttable.CutFlower();
        }
    }
}
