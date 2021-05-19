using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController instance;
    //public enum PacDot
    //{
    //    mapfalse,
    //    mapTure,
    //    mapOver
    //}
    //public PacDot PacDots=PacDot.mapfalse;
    public bool isGamerover=false;
    //UI组件
    public Text ScoreTexture;
    public Text GameOverText;
    public int ScoreNumber;
    public List<Vector2> ob = new List<Vector2>();
    public AStar2 FindMoster;
    void Awake()
    {
        instance = this;
    }
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (GameObject.FindGameObjectsWithTag("ass")==null&&PacDots==PacDot.mapfalse)
        //{
        //    FindBens();
        //    FindMoster = new AStar2(GameController.instance.ob, false, 29, 26);
        //    PacDots = PacDot.mapOver;
        //}
        ScoreTexture.text="Score"+ScoreNumber;
	}

    private void LateUpdate()
    {
        if (GameObject.FindGameObjectWithTag("pac")==false)
        {
            GameOverText.enabled = true;
            isGamerover = true;
            GameOverText.text ="win" ;
            
            Debug.Log("游戏结束;");
        };
    }
    //public void FindBens()
    //{
    //        GameObject[] gameObjects;
    //        gameObjects = GameObject.FindGameObjectsWithTag("pac");
    //        for (int i = 0; i < gameObjects.Length; i++)
    //        {
    //            if (gameObjects[i].gameObject.tag == "pac")
    //            {
    //                Vector2 d = (Vector2)gameObjects[i].GetComponent<Transform>().position + new Vector2(12.5f, 14f);
    //                ob.Add(d);
    //            }
    //        }
    //        Debug.Log("加入数组完成");
    //}
}
