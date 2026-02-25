using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TreeNodeUI : MonoBehaviour,
    IDropHandler, IPointerClickHandler
{
    private int? value = null;
    private NumberButton placedButton = null;

    public TMP_Text valueText;

    public void OnDrop(PointerEventData eventData)
    {
        NumberButton button =
            eventData.pointerDrag.GetComponent<NumberButton>();

        if (button == null) return;

        // ❌ ถ้ามีค่าอยู่แล้ว → ไม่ให้วาง
        if (value.HasValue) return;

        value = button.value;
        valueText.text = value.ToString();

        placedButton = button;

        button.placedSuccessfully = true;
        button.HideAfterPlaced();
    }

    // 🔁 คลิก Node เพื่อคืนเลข
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!value.HasValue) return;

        if (placedButton != null)
        {
            placedButton.ReturnToOriginal();
        }

        value = null;
        valueText.text = "";
        placedButton = null;
    }


    public bool HasValue() => value.HasValue;
    public int? GetValue() => value;

    public void SetValue(int v)
    {
        value = v;
        valueText.text = v.ToString();
    }

    public void Clear()
    {
        if (placedButton != null)
        {
            placedButton.ReturnToOriginal();
            placedButton = null;
        }

        value = null;
        valueText.text = "";
    }



}
