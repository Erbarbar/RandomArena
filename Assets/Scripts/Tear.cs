using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour {

    public int damage;
	
	void OnTriggerEnter2D(Collider2D coll){
        
    }

    void setDamage(int damage){
        this.damage = damage;
    }
}
