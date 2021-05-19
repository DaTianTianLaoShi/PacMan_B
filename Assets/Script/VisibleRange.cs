using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleRange : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("找到玩家");
            transform.parent.GetComponent<Moster>().CanSeePlayer = true;
            transform.parent.GetComponent<Moster>().findPlayerPositionStart = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent.GetComponent<Moster>().CanSeePlayer = false;

    }
}
