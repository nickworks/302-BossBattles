using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour {
    
    public float raycastLength = 2;
    private float distanceBetweenGroundAndIK = 0;

    private Quaternion startingRot;
    ///<summary>The world-space position of the ground above/below the foot IK.</summary>
    private Vector3 groundPosition;
    ///<summary>The world-space rotation for the foot to be aligned w/ ground.</summary>
    private Quaternion groundRotation;

    void Start() {

        startingRot = transform.localRotation;
        distanceBetweenGroundAndIK = transform.localPosition.y;
    }

    void Update() {

        //FindGround();

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
            groundPosition = hitInfo.point + Vector3.up * distanceBetweenGroundAndIK;

            // convert starting rotation into world-space
            Quaternion worldNeutral = transform.parent.rotation * startingRot;

            // finds ground rotation
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;

        }
    }
}
