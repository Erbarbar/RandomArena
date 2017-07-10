using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    public int tearDamage;
    public int tearSpeed;
    public int tearDistance;
    public int tearRate;

    [SyncVar]
    public float tearTimer;

    public float inputMargin;
    public GameObject tear;

	
    void FixedUpdate(){
        if(tearTimer > 0)
            tearTimer -= Time.deltaTime;
    }

	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer)
            return;


        int direction = getDirection(Input.GetAxisRaw("Horizontal Shoot"),Input.GetAxisRaw("Vertical Shoot"));

        if(direction != 5 && tearTimer <= 0){
            CmdShoot(direction);
        }
	}

    [Command]
    void CmdShoot(int direction){
        if(tearTimer>0 || !isServer)
            return;
        Vector2 tearPosition = this.transform.position;
        GameObject spawnedTear = Instantiate(tear, tearPosition, Quaternion.identity);

        Rigidbody2D tearBody = spawnedTear.GetComponent("Rigidbody2D") as Rigidbody2D;
        Rigidbody2D playerBody = GetComponent("Rigidbody2D") as Rigidbody2D;

        Vector2 tearVector = getVector(direction);
        tearVector *= tearSpeed;
        tearVector += addPlayerVelocity(direction, playerBody.velocity);
        tearBody.velocity = tearVector;
        tearTimer = (float)1.0f/tearRate;

        spawnedTear.SendMessage("setDamage",tearDamage);

        Destroy(spawnedTear,tearDistance);
        NetworkServer.Spawn(spawnedTear);
    }

    int getDirection(float x, float y){
        if(x>inputMargin)
            return 6;
        if(x<-inputMargin)
            return 4;
        if(y>inputMargin)
            return 8;
        if(y<-inputMargin)
            return 2;
        return 5;
    }

    Vector2 getVector(int direction){
        switch(direction){
            case 8: // North
                return new Vector2(0,1);
            case 2: // South
                return new Vector2(0,-1);
            case 6: // East
                return new Vector2(1,0);
            case 4: // West
                return new Vector2(-1,0);
            default: break;
        }
        Debug.Log("[ERROR]: getVector(direction="+direction+") direction not supported!");
        return new Vector2(0,0);
    }


    Vector2 addPlayerVelocity(int direction, Vector2 player){
        switch(direction){
            case 8:
                if(player.y < 0)
                    return (player - new Vector2(0,player.y));
                break;
            case 2:
                if(player.y > 0)
                    return (player - new Vector2(0,player.y));
                break;
            case 6:
                if(player.x < 0)
                    return (player - new Vector2(player.x,0));
                break;
            case 4:
                if(player.x > 0)
                    return (player - new Vector2(player.x,0));
                break;
            default: break;
        }
        return player;
    }
}
