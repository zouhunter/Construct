using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public enum BuildState
{
	Normal,
	Inbuild,
}
public class BuildingItem : MonoBehaviour
{
    public const string movePosTag = "MovePos";
	//public BuildingInfo buildingInfo;
    private BuildingCtrl ctrl;

    private bool canBuild;
	private bool reactive;
	private string gikey;
    private Vector3 nomalizePos;

	public void Init(BuildingCtrl ctrl)
    {
     
    }
    void OnMouseOver()
    {
		
    }

	//修建建筑物************************************
	void Update()
	{
	}

	private void FallowMouse()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hitinfos = Physics.RaycastAll(ray);
		for (int i = 0; i < hitinfos.Length; i++)
		{
			RaycastHit hitinfo = hitinfos[i];
			if (hitinfo.collider.CompareTag(movePosTag))
			{
				//更新创建出来对象的坐标
				transform.position = hitinfo.point;
                return;
			}
		}
	}
	public void ShowPos()
	{
     
    }
	private void BulidOrNot()
	{
		
	}

}

