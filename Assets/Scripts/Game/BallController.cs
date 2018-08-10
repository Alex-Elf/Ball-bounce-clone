using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    private Vector2 _position;
    private bool _returnBack;
	// Use this for initialization
	void Start () {
        _returnBack = false;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (_returnBack)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position,
                new Vector2(0, -5), Time.deltaTime*5));
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
        _returnBack = true;
    }
}
