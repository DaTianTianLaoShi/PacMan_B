using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridType
{
	Normal,//正常
	Obstacle,//障碍物
	Start,//起点
	End//终点
}

//为了格子排序 需要继承IComparable接口实现排序
public class MapGrid : IComparable//排序接口
{
	public int y;//记录坐标
	public int x;

	public int f;//总消耗
	public int g;//当前点到起点的消耗
	public int h;//当前点到终点的消耗


	public GridType type;//格子类型
	public MapGrid fatherNode;//父节点


	//排序
	public int CompareTo(object obj)	 //排序比较方法 ICloneable的方法
	{
		//升序排序
		MapGrid other = (MapGrid)obj;
		if (this.f < other.f)
		{
			return -1;					//升序
		}
		if (this.f > other.f)
		{
			return 1;					//降序
		}
		return 0;
	}


	internal void Reset()
	{
		f = g = h = 0;
		fatherNode = null;
	}
}

public class AStar2
{
	//格子大小
	private int row = 5;
	private int col = 10;

	private MapGrid[,] grids;			//格子数组

	private List<MapGrid> openList;			//开启列表
	private List<MapGrid> closeList;			//结束列表

	//开始,结束点位置
	private int yStart;
	private int xStart;

	private int yEnd;
	private int xEnd;

	//private List<Vector2> obstacles;

	// 从终点回溯路径到起点（寻路成功后）
	private Stack<string> fatherNodeLocation;

	public AStar2(List<Vector2> obstacles, bool isObstacle, int row, int col)
	{
		this.grids = new MapGrid[row, col];	//初始化数组
		this.row = row;
		this.col = col;
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				grids[i, j] = new MapGrid();
				grids[i, j].y = i;
				grids[i, j].x = j;		//初始化格子,记录格子坐标
				grids[i, j].type = isObstacle ? GridType.Normal : GridType.Obstacle;
			}
		}

		//生成障碍物
		foreach (var pos in obstacles)
		{
			Debug.Log(pos);
			grids[(int)pos.y, (int)pos.x].type = isObstacle ? GridType.Obstacle : GridType.Normal;
		}

		openList = new List<MapGrid>();
		closeList = new List<MapGrid>();
	}

	public List<Vector2> Find(Vector2 start, Vector2 end)
	{
		// 如果起点终点就是障碍则返回空
		if (grids[(int)start.y, (int)start.x].type == GridType.Obstacle
			|| grids[(int)end.y, (int)end.x].type == GridType.Obstacle)
			return null;

		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				grids[i, j].Reset();		//初始化格子,记录格子坐标
			}
		}

		// 上次寻路的起点终点设置为普通可通过点
		grids[yStart, xStart].type = GridType.Normal;
		grids[yEnd, xEnd].type = GridType.Normal;

		// 设置新的起点终点
		xStart = (int)start.x;
		yStart = (int)start.y;
		xEnd = (int)end.x;
		yEnd = (int)end.y;

		grids[yStart, xStart].type = GridType.Start;
		grids[yStart, xStart].h = Manhattan(yStart, xStart);	//起点的 h 值

		grids[yEnd, xEnd].type = GridType.End;					//结束点
		fatherNodeLocation = new Stack<string>();

		openList.Clear();
		openList.Add(grids[yStart, xStart]);
		closeList.Clear();

		int result;
		do
		{
			result = NextStep();
		} while (result == 0);
		if (result == -1)
			return null;
		else
			return GetPath(openList[0]);
	}

	int Manhattan(int x, int y)					//计算算法中的 h
	{
		return (int)(Mathf.Abs(yEnd - x) + Mathf.Abs(xEnd - y)) * 10;
	}

	//每个格子显示的内容
	string FGH(MapGrid grid)
	{
		string str = "F" + grid.f + "\n";
		str += "G" + grid.g + "\n";
		str += "H" + grid.h + "\n";
		str += "(" + grid.y + "," + grid.x + ")";
		return str;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>-1=not found,0=finding,1=found</returns>
	int NextStep()
	{
		if (openList.Count == 0)				//没有可走的点
		{
			Debug.Log("Over !");
			return -1;
		}
		// 注意：因为openList每一步都Sort，
		// 又因为其中的每个元素都是MapGrid，并且MapGrid按照F值升序实现了IComparable接口，
		// 所以第0个元素一定是F值最小那个！
		MapGrid grid = openList[0];	//取出openList数组中的第一个点
		if (grid.type == GridType.End)			//找到终点
		{
			Debug.Log("Find");
			//ShowFatherNode(grid);		//找节点//打印路线
			return 1;
		}

		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (!(i == 0 && j == 0) && Mathf.Abs(i)+Mathf.Abs(j)!=2)
				{
					// 计算周围格在地图上的坐标
					int y = grid.y + i;
					int x = grid.x + j;

					//x,y不超过边界,不是障碍物,不在closList里面
					if (y >= 0 && y < row && x >= 0 && x < col && grids[y, x].type != GridType.Obstacle && !closeList.Contains(grids[y, x]))
					{


						//从中心格（当前格）到周围格(x=j,y=i)的路径消耗
						int g = grid.g + (int)(Mathf.Sqrt((Mathf.Abs(i) + Mathf.Abs(j))) * 10);
						if (grids[y, x].g == 0 || grids[y, x].g > g)
						{
							grids[y, x].g = g;
							grids[y, x].fatherNode = grid;		//更新父节点
						}
						//到终点的消耗
						grids[y, x].h = Manhattan(y, x);
						grids[y, x].f = grids[y, x].g + grids[y, x].h;
						if (!openList.Contains(grids[y, x]))
						{
							openList.Add(grids[y, x]);			//如果没有则加入到openlist
						}
						openList.Sort();						//排序
					}
				}
			}
		}
		//添加到关闭数组
		closeList.Add(grid);
		//从open数组删除
		openList.Remove(grid);

		return 0;
	}


	//回溯法 递归父节点
	void ShowFatherNode(MapGrid grid)
	{
		if (grid.fatherNode != null)
		{
			//print(grid.fatherNode.x + "," + grid.fatherNode.y);
			string str = grid.fatherNode.y + "," + grid.fatherNode.x;
			fatherNodeLocation.Push(str);
			ShowFatherNode(grid.fatherNode);
		}
		if (fatherNodeLocation.Count != 0)
		{
			Debug.Log(fatherNodeLocation.Pop());
		}
	}

	public List<Vector2> GetPath(MapGrid grid)
	{
		List<Vector2> ret = new List<Vector2>();
		do
		{
			ret.Insert(0,new Vector2(grid.x, grid.y));
			grid = grid.fatherNode;
		} while (grid != null);
		//ret.Insert(0,new Vector2(grid.x, grid.y)); // 加入开始节点
		return ret;
	}


}
