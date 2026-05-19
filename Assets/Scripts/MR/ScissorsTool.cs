using UnityEngine;

public class ScissorsTool : MonoBehaviour
{
    public float minVelocity = 0.5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rb.linearVelocity.magnitude < minVelocity)
            return;

        FlowerCuttable flower =
            other.GetComponent<FlowerCuttable>();

        if (flower != null)
        {
            flower.CutFlower();
        }
    }
}