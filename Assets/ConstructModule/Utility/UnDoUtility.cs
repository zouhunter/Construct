using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor;
using System;

public interface IUndoAbleStep
{
    void Restore();
}
public class CreateStepRecord : IUndoAbleStep
{
    public ISelectable target;
    public CreateStepRecord(ISelectable target)
    {
        this.target = target;
    }
    public void Restore()
    {
        if (target != null)
        {
            target.gameObject.SetActive(!target.gameObject.activeSelf);
            if (target.gameObject.activeSelf)
            {
                UnDoUtil.ReDoOneStep();//刚创建出来并没有坐标
            }
            target.BuildState = BuildState.normal;//.installAble = true;
        }
    }
}
public class DestroyStepRecord : IUndoAbleStep
{
    ISelectable target;
    DBDeviceRecord record;
    public DestroyStepRecord(ISelectable target)
    {
        this.target = target;
        if (target is BuildingItem)
        {
            record = new global::DBDeviceRecord();
            record.Record(target as BuildingItem);
        }
       
        target.gameObject.SetActive(false);
    }
    public void Restore()
    {
        if (target != null)
        {
            target.gameObject.SetActive(true);
            if (target is BuildingItem)
            {
                record.UnRecord(target as BuildingItem);
            }
            target.BuildState = BuildState.normal;//.installAble = true;
        }
    }
}
public class TransformStepRecord : IUndoAbleStep
{
    ISelectable target;
    DBDeviceRecord record;
    public TransformStepRecord(ISelectable target)
    {
        this.target = target;
        if (target is BuildingItem)
        {
            record = new global::DBDeviceRecord();
            record.Record(target as BuildingItem);
        }
    }

    public void Restore()
    {
        if (target != null)
        {
            if (target is BuildingItem)
            {
                record.UnRecord(target as BuildingItem);
            }
        }
    }
}

public static class UnDoUtil
{
    public static Stack<IUndoAbleStep> records = new Stack<IUndoAbleStep>();
    public static Stack<IUndoAbleStep> temprecords = new Stack<IUndoAbleStep>();

    public static void RecordStep(IUndoAbleStep step)
    {
        records.Push(step);
        TryDiscardCreated();
    }
    private static void TryDiscardCreated()
    {
        var newItems = temprecords.ToArray();
        foreach (var item in newItems)
        {
            if (item is CreateStepRecord)
            {
                GameObject.Destroy((item as CreateStepRecord).target.gameObject);
            }
        }
        temprecords.Clear();
    }
    public static void UnDoOneStep()
    {
        if (records.Count > 0)
        {
            var step = records.Pop();
            temprecords.Push(step);
            step.Restore();

            if (!(temprecords.Peek() is TransformStepRecord))
            {
                UnDoOneStep();
            }
        }
    }
    public static void ReDoOneStep()
    {
        if (temprecords.Count > 0)
        {
            var step = temprecords.Pop();
            records.Push(step);
            step.Restore();
        }
    }

}
