using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfCameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera shelfCamera;

    private float lastClickTime = 0f;
    private float doubleClickTime = 0.3f;

    private bool isShelfView = false;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickTime)
        {
            ToggleCamera();
        }

        lastClickTime = Time.time;
    }

    void ToggleCamera()
    {
        isShelfView = !isShelfView;

        mainCamera.gameObject.SetActive(!isShelfView);
        shelfCamera.gameObject.SetActive(isShelfView);

        Debug.Log(isShelfView ? "Switch to Shelf Camera" : "Back to Main Camera");
    }
}
