using UnityEngine;
using System;

public class RockNPC : MonoBehaviour
{
    public Animator anim;

    public Transform spawnPoint;
    public Transform exitPoint;

    public float walkSpeed = 2f;

    private bool isLeaving = false;

    public Action OnExitFinished;

    void Start()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation * Quaternion.Euler(0, 180, 0);
        }

        anim.SetBool("isWaving", true);
    }

    public void StopWaving()
    {
        anim.SetBool("isWaving", false);
    }

    public void LeaveShop()
    {
        anim.SetBool("isWaving", false);
        anim.SetBool("isWalking", true);

        Vector3 dir = (exitPoint.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        isLeaving = true;
    }

    void Update()
    {
        if (isLeaving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                exitPoint.position,
                walkSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, exitPoint.position) < 0.1f)
            {
                OnExitFinished?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}