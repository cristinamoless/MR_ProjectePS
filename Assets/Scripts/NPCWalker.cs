using UnityEngine;

public class NPCWalker : MonoBehaviour
{
    public float speed = 2f;
    public float leftX = -10f;
    public float rightX = 10f;

    private Animator anim;
    private bool goingRight = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Walking", true);
    }

    void Update()
    {
        float direction = goingRight ? 1 : -1;

        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, goingRight ? 90 : -90, 0);

        if (transform.position.x >= rightX)
            goingRight = false;

        if (transform.position.x <= leftX)
            goingRight = true;
    }
}
