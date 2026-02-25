using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class NumberButton : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int value;

    //private Vector3 originalLocalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private int originalSiblingIndex;


    [HideInInspector]
    public bool placedSuccessfully = false;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
    }




    public void SetValue(int v, TreeUIManager manager)
    {
        value = v;
        GetComponentInChildren<TMP_Text>().text = v.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
        placedSuccessfully = false;
    }



    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!placedSuccessfully)
        {
            transform.SetParent(originalParent, false);
        }
    }


    public void HideAfterPlaced()
    { 
        gameObject.SetActive(false);
    }

    public void ReturnToOriginal()
    {
        gameObject.SetActive(true);

        transform.SetParent(originalParent, false);

        transform.SetSiblingIndex(originalSiblingIndex);  

        canvasGroup.blocksRaycasts = true;
        placedSuccessfully = false;
    }






}
