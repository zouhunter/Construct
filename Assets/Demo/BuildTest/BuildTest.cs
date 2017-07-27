using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class BuildTest : MonoBehaviour {
    public SelectablePlane selectPanel;
    public SelectDrawer drawer;
    private BuildingItem activeItem;
	// Use this for initialization
	void Start () {
        drawer.InitSelectDrawer<BuildingItem>();
        selectPanel.onMoveStateChanged = (x) => {
            drawer.enabled = !x;
            activeItem.SetBuildState(BuildState.Normal);
        };
        drawer.onGetRootObjs = (x) =>
        {
            if(x != null && x.Length > 0)
            {
                activeItem = x[0].GetComponent<BuildingItem>();
                activeItem.SetBuildState(BuildState.Inbuild);
                selectPanel.SetTarget(x[0]);
                drawer.enabled = false;
            }
            else
            {
                selectPanel.SetTarget(null);
            }
        };

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
