using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ScorpionMovement : MonoBehaviour
{
    
    enum Mode {
        Idle,
        Walk,
        InAir
    }


    public float speed = 2;
    public Transform groundRing;

    public ScorpionStickyFeet[] feet;

    private CharacterController pawn;

    ///<summary>The current vertical velocity in meters/second</summary>
    public float velocityY = 0;
    public float gravity = 50;
    public float jumpImpulse = 50;

    private Mode mode = Mode.Idle;
    private Vector3 input;

    private Camera cam;

    private Quaternion targetRotation;
    private Vector3 groundRingTarget;

    void Start() {
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        GetPlayerInputRelativeToCamera();


        // set movement mode based on movement input:

        float threshold = .1f;
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;

        if (pawn.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;

        pawn.Move((input * speed + Vector3.down * velocityY) * Time.deltaTime);

        if (pawn.isGrounded)
        {
            velocityY = 0;
        }
        else
        {
            mode = Mode.InAir;
        }

        Animate();
    }

    private void GetPlayerInputRelativeToCamera()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camRight * h;
        if (input.sqrMagnitude > 1) input.Normalize();
    }

    void Animate(){



        groundRingTarget = transform.InverseTransformDirection(input) + new Vector3(0, -.3f, 0);
        groundRing.localPosition = AnimMath.Ease(groundRing.localPosition, groundRingTarget, .001f);

        if(mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up);
        transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);

        switch(mode){
            case Mode.Idle:
                AnimateIdle();
                break;
            case Mode.Walk:
                AnimateWalk();
                break;
            case Mode.InAir:
                AnimateInAir();
                break;
        }
    }
    void AnimateInAir(){

    }
    void AnimateIdle(){

    }
    void AnimateWalk(){

    }
}
