using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SureStatic<T> : MonoBehaviour
{
    //被用于继承
    public virtual void Exit(T a)//状态退出
    {

    }
	public virtual void Enter(T a)//状态进入
    {

    }
    public virtual void Update(T a)//状态更新
    {

    }
}
