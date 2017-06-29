using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

    public float speed;
    public bool blocked;
    public bool flying;
    public bool spirit;

    public float speedX;
    public float speedY;

    public Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		blocked = false;
        flying = false;
        spirit = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // only local player can use this script
        if(!isLocalPlayer)
            return;

        // so that running diagonally doesnt increase overall speed
        normalizeMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // adds movement to the player Rigidbody2D
        rb2d.AddForce(new Vector2(speedX,speedY), ForceMode2D.Impulse);

	}

    void normalizeMovement(float x, float y){
        Vector2 movement = new Vector2(x,y);
        movement.Normalize();
        speedX = movement.x * speed;
        speedY = movement.y * speed;
    }

}
