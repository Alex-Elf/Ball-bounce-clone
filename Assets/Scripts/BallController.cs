using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    private Vector2 position;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}
    
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Square")
        {
            col.gameObject.SendMessage("Hit");
            
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bottom")
        {
            other.SendMessage("CountBall");
            Destroy(gameObject);
        }
    }
}
