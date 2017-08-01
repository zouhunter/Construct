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
    public BuildingItem target;
    public CreateStepRecord(BuildingItem target)
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
                UnDoUtility.ReDoOneStep();//刚创建出来并没有坐标
            }
            target.buildState = BuildState.normal;//.installAble = true;
        }
    }
}
public class DestroyStepRecord : IUndoAbleStep
{
    BuildingItem target;
    DBDeviceRecord record;
    public DestroyStepRecord(BuildingItem target)
    {
        this.target = target;
        record = new global::DBDeviceRecord();
        record.Record(target);
        target.gameObject.SetActive(false);
    }
    public void Restore()
    {
        if (target != null)
        {
            target.gameObject.SetActive(true);
            record.UnRecord(target);
            target.buildState = BuildState.normal;//.installAble = true;
        }
    }
}
public class TransformStepRecord : IUndoAbleStep
{
    BuildingItem target;
    DBDeviceRecord record;
    public TransformStepRecord(BuildingItem target)
    {
        this.target = target;
        record = new global::DBDeviceRecord();
        record.Record(target);
    }

    public void Restore()
    {
        if (target != null)
        {
            record.UnRecord(target);
        }
    }
}

public static class UnDoUtility
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
