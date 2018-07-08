using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    private Vector2 position;
    private bool returnBack;
	// Use this for initialization
	void Start () {
        returnBack = false;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (returnBack)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position,
                new Vector2(0, -5), Time.deltaTime*5));
        }
	}
    
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Square")
        {
            col.transform.parent.SendMessage("Hit");
            
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bottom")
        {
            other.SendMessage("CountBall");
            Destroy(gameObject);
        }
        else if(other.tag == "Bonus")
        {
            other.SendMessage("Hitted");
        }
    }

    public void ReturnBack()
    {
        returnBack = true;
    }
}
