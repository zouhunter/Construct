using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragSlider : MonoBehaviour,IPointerDownHandler,IPointerUpHandler {
    public float speed = 30;
    public InputField target;
    public CanvasGroup canvasGroup;
    private bool enable;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (canvasGroup == null|| canvasGroup.interactable == true){
            enable = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        enable = false;
    }
    private void Update()
    {
        if (enable && target != null && target.contentType == InputField.ContentType.DecimalNumber && Input.GetMouseButton(0))
        {
            var data = target.text ==""?0: float.Parse(target.text);
            data += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            target.text = data.ToString("0.00");
            target.onEndEdit.Invoke(target.text);
        }
    }
}
