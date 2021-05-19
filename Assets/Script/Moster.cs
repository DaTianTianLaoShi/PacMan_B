using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Moster : MonoBehaviour {
    StaticMachine<Moster> machine = new StaticMachine<Moster>();
    public List<Vector2> LiveHomePath;
    public float speed = 4f;
    private bool isGoStart=false;//出发状态是否完成;
    //状态;
    //以下是搜寻找玩家;
    public bool CanSeePlayer=false;
    public Vector2 findPlayerPositionStart=Vector2.zero;
    public Transform beans;
    AStar2 FindMoster;
    class WayPointSetect:SureStatic<Moster>//一种状态，出发状态;
    {
        private List<Vector2> path=new List<Vector2>();
        private int index;//当前走的路径点个数;
         public  WayPointSetect(List<Vector2> path)
        {
            this.path = path;
            this.index = 0;
        }
        public override void Update(Moster c)
        {
            Vector2 a = Vector2.MoveTowards(c.transform.position,path[index],c.speed*Time.fixedDeltaTime);
            c.GetComponent<Rigidbody2D>().MovePosition(a);
            if ((Vector2)c.transform.position==a)
            {
                index++;
                if (index>=path.Count)//.count属性判断元素真实个数;
                {
                    c.isGoStart = true;
                    c.machine.ChangeState(new XunluoPoint());
                    //走完路点后，开始巡逻状态;
                    Debug.Log("出发路点完成");
                    return;
                }else 
                {
                    Vector2 b = path[index] - path[index - 1];
                    c.GetComponent<Animator>().SetFloat("MirX",b.x);
                    c.GetComponent<Animator>().SetFloat("MirY",b.y);
                }
            }
        }
    }
    class XunluoPoint:SureStatic<Moster>//巡逻状态;
    {

        private Vector2 dir;//目标点;
        private Vector2 Edir;//当前方向向量;
        private Vector2[] EdirTo = new Vector2[] { Vector2.left, Vector2.up,Vector2.right,Vector2.down };
        public override void Enter(Moster c)
        {
            dir = c.gameObject.transform.position;
            Edir = c.gameObject.transform.position;
        }
        public override void Update(Moster c)
        {
            
            //Edir = c.transform.position;
            Vector2 b = Vector2.MoveTowards(c.transform.position,dir,c.speed*Time.fixedDeltaTime);
            c.gameObject.GetComponent<Rigidbody2D>().MovePosition(b);
            if ((Vector2)c.transform.position==dir)
            {
                
                if (c.CanSeePlayer==true&&c.isGoStart==true)
                {
                    Vector2 endp = c.findPlayerPositionStart + new Vector2(12.5f, 14f);
                    Vector2 Ss = dir;
                    Ss += new Vector2(12.5f,14f);
                    //string aas = "" + Ss + endp+c.name;
                    //Debug.Log(aas);
                    List<Vector2> paths=c. FindMoster.Find(Ss,endp);
                    if (paths!=null)
                    {
                        for (int i = 0; i < paths.Count; i++)
                        {
                            paths[i] -= new Vector2(12.5f, 14f);
                        }
                        for (int i = 0; i < paths.Count - 1; i++)
                        {
                            Debug.DrawLine(paths[i], paths[i + 1], Color.red);
                        }
                        c.machine.ChangeState( new WayPointSetect(paths)); 
                        return;
                    }
                   
                }
                List<Vector2> Averation = new List<Vector2>();
                for (int i=0;i<EdirTo.Length;i++)
                {

                    if (EdirTo[i]==-Edir)
                    {
                        Debug.Log("相反，跳出路径;");
                        continue;
                    }
                    if (c.CanGo(EdirTo[i]) == false)
                    {
                        Averation.Add(EdirTo[i]);
                    }    
                    
                }
                int a = Random.Range(0,Averation.Count);
                Edir = Averation[a];
                dir += Averation[a];
                c.GetComponent<Animator>().SetFloat("MirX", Edir.x);
                c.GetComponent<Animator>().SetFloat("MirY", Edir.y);
            }
        }
    }
    private bool CanGo(Vector2 dir)
    {
        RaycastHit2D a = Physics2D.Linecast(this.transform.position,(Vector2)this.transform.position+dir,1<<LayerMask.NameToLayer("map"));
        return a;
    }
	void Start ()
    {
        List<Vector2> walkables = new List<Vector2>();
        for (int row = 0; row < beans.childCount; row++)
        {
            Transform transRow = beans.GetChild(row);

            for (int col = 0; col < transRow.childCount; col++)
            {
                Transform dot = transRow.GetChild(col);

                Vector2 p = dot.position; // 取得豆子的坐标
                p = p + new Vector2(12.5f, 14f); // 将其偏移到以（0,0）为原点
                walkables.Add(p);
            }
        }
        FindMoster = new AStar2(walkables, false, 29, 26);
        this.machine.Init(this, new WayPointSetect(LiveHomePath));//初始化;

    }
    void FixedUpdate()
     {
        
        if (GameController.instance.isGamerover == true) return;
        this.machine.Update();
    }   
}
