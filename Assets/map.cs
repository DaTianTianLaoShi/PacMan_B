using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour {
    public static map instance;
    //坐标组件;
    public GameObject Map_HstartPulse;//生成豆子地图起始点
    public GameObject Map_HendPulse;//生成豆子竖向结束点
    public GameObject Map_WendPulse;//生成豆子横向结束点
    const int x= 1;
    //预制体
    public GameObject Pulses;//生成的豆子（普通）
    //地图状态器
    // Use this for initialization
    //public bool isbeigover = false;//豆子是否生成完成
    //public bool bensbehaveto=false;//豆子是否销毁
    //public GameObject Beans;

    void Awake()
    {
        instance = this;
    }
    void Start () {

        IsPulse();
    }

    // Update is called once per frame
    void Update ()
    {

    }
    public void IsPulse()//生成豆子的方法
    {
        
            Debug.Log("制造完了");
            for (float y = Map_HstartPulse.transform.position.y-1; y > Map_HendPulse.transform.position.y; y--)
            {
                for (float x = Map_HstartPulse.transform.position.x+1; x < Map_WendPulse.transform.position.x; x++)
                {
                    Instantiate(Pulses, new Vector2(x, y), Quaternion.identity);   
                }            
            }     
    }
}
