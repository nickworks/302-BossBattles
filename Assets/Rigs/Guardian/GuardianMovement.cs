using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuardianMovement : MonoBehaviour
{
    
    public float walkSpeed = 5;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        float v = Input.GetAxis("Vertical");

        transform.position += transform.forward * v * Time.deltaTime * walkSpeed;


        animator.SetFloat("speed", Mathf.Abs(v * walkSpeed));
    }
}
