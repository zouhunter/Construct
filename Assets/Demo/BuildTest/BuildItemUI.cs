using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;
using System;
using UnityEngine.EventSystems;
public class BuildItemUI : MonoBehaviour, IListItem,IPointerDownHandler
{
    internal Action<BuildItemHold> onButtonClicked;
    private Image img;
    private Text txt;
    public int Id { get; set; }
    private BuildItemHold hold;
    private void Awake()
    {
        img = GetComponent<Image>();
        txt = GetComponentInChildren<Text>();
    }

    internal void InitData(BuildItemHold buildItemHold)
    {
        this.img.sprite = buildItemHold.pic;
        this.txt.text = buildItemHold.itemName;
        this.hold = buildItemHold;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onButtonClicked != null)
            onButtonClicked(hold);
    }
}
