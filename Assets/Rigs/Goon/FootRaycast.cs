using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour {
    
    public float raycastLength = 2;

    ///<summary>The local-space position of where the IK spawned</summary>
    private Vector3 startingPosition;
    ///<summary>The local-space rotation of where the IK spawned</summary>
    private Quaternion startingRotation;
    ///<summary>The world-space position of the ground above/below the foot IK.</summary>
    private Vector3 groundPosition;
    ///<summary>The world-space rotation for the foot to be aligned w/ ground.</summary>
    private Quaternion groundRotation;

    ///<summary>The local-space position to ease towards. This allows us to animate the position!</summary>
    private Vector3 targetPosition;
    private Vector3 footSeparateDir;

    void Start() {

        startingRotation = transform.localRotation;
        startingPosition = transform.localPosition;

        footSeparateDir = (startingPosition.x > 0) ? Vector3.right : Vector3.left;

    }

    void Update() {

        //FindGround();

        // ease towards target:
        transform.localPosition = AnimMath.Ease(transform.localPosition, targetPosition, .01f);
    }
    public void SetPositionLocal(Vector3 p){
        targetPosition = p;
    }
    public void SetPositionHome(){
        targetPosition = startingPosition;
    }
    public void SetPositionOffset(Vector3 p, float separateAmount = 0){
        targetPosition = startingPosition + p + separateAmount * footSeparateDir;
    }

    private void FindGround() {

        Vector3 origin = transform.position + Vector3.up * raycastLength / 2;
        Vector3 direction = Vector3.down;

        // draw the ray in the scene:
        Debug.DrawRay(origin, direction * raycastLength, Color.blue);

        // check for collision with ray:
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {

            // finds ground position
            groundPosition = hitInfo.point + Vector3.up * startingPosition.y;

            // convert starting rotation into world-space
            Quaternion worldNeutral = transform.parent.rotation * startingRotation;

            // finds ground rotation
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;

        }
    }
}
