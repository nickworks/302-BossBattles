using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour {
    

    enum Mode {
        Idle,
        Walk,
        InAir
    }

    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float footSeparateAmount = .2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkFootSpeed = 4;
    private CharacterController pawn;

    ///<summary>The current vertical velocity in meters/second</summary>
    public float velocityY = 0;
    public float gravity = 50;
    public float jumpImpulse = 50;

    private Mode mode = Mode.Idle;
    private Vector3 input;

    private float walkTime;

    private Camera cam;

    private Quaternion targetRotation;

    void Start() {
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camRight * h;
        if(input.sqrMagnitude > 1) input.Normalize();


        // set movement mode based on movement input:

        float threshold = .1f;
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;

        if(mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up);

        if(pawn.isGrounded){
            if(Input.GetButtonDown("Jump")){
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;

        pawn.Move((input * speed + Vector3.down * velocityY) * Time.deltaTime);

        if(pawn.isGrounded){
            velocityY = 0;
        } else {
            mode = Mode.InAir;
        }

        Animate();
    }
    void Animate(){

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
        // TODO:

        // lift legs?
        // lift hands?
        // adjust spikes / hair?
        // use vertical velocity
    }
    void AnimateIdle(){

        footLeft.SetPositionHome();
        footRight.SetPositionHome();

    }
    delegate void MoveFoot(float time, FootRaycast foot);
    void AnimateWalk(){

        MoveFoot moveFoot = (t, foot) => {
            
            float y = Mathf.Cos(t) * walkSpreadY; // vertical movement
            float lateral = Mathf.Sin(t) * walkSpreadZ; // lateral movement

            Vector3 localDir = foot.transform.parent.InverseTransformDirection(input);

            float x = lateral * localDir.x;
            float z = lateral * localDir.z;

            float alignment = Mathf.Abs(Vector3.Dot(localDir, Vector3.forward));
            // 1 = foreward
            // 1 = backwards
            // 0 = strafing

            if(y < 0) y = 0;

            foot.SetPositionOffset(new Vector3(x, y, z), footSeparateAmount * alignment);
        };

        walkTime += Time.deltaTime * input.sqrMagnitude * walkFootSpeed;

        moveFoot.Invoke(walkTime, footLeft);
        moveFoot.Invoke(walkTime + Mathf.PI, footRight);
    }


}
