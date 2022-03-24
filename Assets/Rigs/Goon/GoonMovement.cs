using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour {
    
    public float speed = 2;
    private CharacterController pawn;

    void Start() {
        pawn = GetComponent<CharacterController>();
    }

    
    void Update() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v + transform.right * h;
        if(move.sqrMagnitude > 1) move.Normalize();

        pawn.SimpleMove(move * speed);

    }
}
