using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionStickyFeet : MonoBehaviour
{
    
    public Transform raycastSource;
    public AnimationCurve curveStepVertical;

    public float raycastLength = 2;

    public float maxDistanceBeforeMove = 1;


    private Quaternion startingRotation;


    private Vector3 previousGroundPosition;
    private Vector3 groundPosition;

    private Quaternion previousGroundRotation;
    private Quaternion groundRotation;

    private float animationLength = .25f;
    private float animationCurrentTime = 0;

    private bool isAnimating {
        get {
            return (animationCurrentTime < animationLength);
        }
    }


    void Start()
    {
        startingRotation = transform.localRotation;
    }

    
    void Update()
    {
        if(isAnimating){

            animationCurrentTime += Time.deltaTime;
            float p = Mathf.Clamp(animationCurrentTime / animationLength, 0, 1);

            float y = curveStepVertical.Evaluate(p);
            
            // move position
            transform.position = AnimMath.Lerp(previousGroundPosition, groundPosition, p) + new Vector3(0, y, 0);

            // move rotation
            transform.rotation = AnimMath.Lerp(previousGroundRotation, groundRotation, p);

        } else {
            // keep feet planted:

            transform.position = groundPosition;
            transform.rotation = groundRotation;

            // check distance to starting position, trigger animation
            Vector3 vToStarting = transform.position - raycastSource.position;
            if(vToStarting.sqrMagnitude > maxDistanceBeforeMove * maxDistanceBeforeMove){
                FindGround();
            }
        }
    }
    private void FindGround() {

        Vector3 origin = raycastSource.position + Vector3.up * raycastLength / 2;
        Vector3 direction = Vector3.down;

        // draw the ray in the scene:
        Debug.DrawRay(origin, direction * raycastLength, Color.blue);

        // check for collision with ray:
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {

            // prepare animation values:
            animationCurrentTime = 0;
            previousGroundPosition = groundPosition;
            previousGroundRotation = groundRotation;

            // finds ground position
            groundPosition = hitInfo.point;

            // convert starting rotation into world-space
            Quaternion worldNeutral = transform.parent.rotation * startingRotation;

            // finds ground rotation
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;

        }
    }
}
