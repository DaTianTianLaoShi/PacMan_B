using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    float Speed=5f;//移动速度;
    Vector2 playerto;//移动方向;
    void Start()
    {
        playerto = gameObject.transform.position;//初始位置;
    }
    void FixedUpdate()
    {
        if (GameController.instance.isGamerover == true)
        {
            gameObject.GetComponent<Animator>().SetFloat("DirX",0f);//播放相应的动画;
            gameObject.GetComponent<Animator>().SetFloat("DirY",0f);
            this.GetComponent<Animator>().SetBool("IsDie",true);
            return;
        };
        //移动方法;
        Vector2 a = Vector2.MoveTowards(this.gameObject.transform.position, playerto, Speed * Time.fixedDeltaTime);//移动方法
        this.gameObject.GetComponent<Rigidbody2D>().MovePosition(a);//利用rigidoby2D移动
        if ((Vector2)gameObject.transform.position == playerto)
        {
            Vector2 s=Vector2.zero;
            if (Input.GetKey(KeyCode.A)&&!CanGo(Vector2.left))
            {
                s = Vector2.left;
            }
             else if (Input.GetKey(KeyCode.S)&&!CanGo(Vector2.down))
            {
                s = Vector2.down;
            }
             else if (Input.GetKey(KeyCode.D)&&!CanGo(Vector2.right))
            {
                s = Vector2.right;
            }
             else if (Input.GetKey(KeyCode.W)&&!CanGo(Vector2.up))
            {
                s = Vector2.up;
            }
            playerto += s;//改变移动坐标;
            gameObject.GetComponent<Animator>().SetFloat("DirX",s.x);//播放相应的动画;
            gameObject.GetComponent<Animator>().SetFloat("DirY",s.y);
        }
    }
    void Update()
    {

    }
    bool CanGo(Vector2 ss)//检测方法
    {
        //Debug.Log("检测到障碍物");
       RaycastHit2D raycastHit2=Physics2D.Linecast(this.transform.position,(Vector2)this.transform.position+ss,1<<LayerMask.NameToLayer("map"));
        if (raycastHit2==true)
        {
            Debug.Log("检测到障碍物");
        }
        return raycastHit2;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Moster")
        {
            GameController.instance.GameOverText.enabled = true;
            GameController.instance.isGamerover = true;
            GameController.instance.GameOverText.text = "lost";   
        }
    }
}
