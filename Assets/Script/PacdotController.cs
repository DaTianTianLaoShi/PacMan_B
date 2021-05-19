using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacdotController : MonoBehaviour {
	void Start () {
        
	}
	void Update () {
		
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "map")
        //{
        //    this.gameObject.tag = "ass";
        //    gameObject.SetActive(false);
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameController.instance.ScoreNumber++;
            gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }
}
