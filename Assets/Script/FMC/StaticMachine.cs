using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMachine<T> : MonoBehaviour {
    // 状态机控制器;
    public SureStatic<T> SureStatic = null;//当前的状态;
    public T owner;//状态机拥有者;
    public void Init(T owner,SureStatic<T> initalState)
    {
        this.owner = owner;
        //SureStatic = initalState;
        ChangeState(initalState);//状态机变化方法
    }
	public void ChangeState(SureStatic<T> NewState)
    {
        if (SureStatic!=null)
        {
            SureStatic.Exit(owner);
        }
        SureStatic = NewState;
        SureStatic.Enter(owner);
    }
    public void Update()
    {
        SureStatic.Update(owner);
    }
}
