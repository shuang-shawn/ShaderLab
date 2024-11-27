using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MyButton : MonoBehaviour, IPointerClickHandler
{
    public Action<string> callback;
    public TMP_Text text;
    public void OnPointerClick(PointerEventData eventData)
    {
        callback?.Invoke(text.text);
    }

    public void SetText(string s)
    {
        text.SetText(s);
    }
}
