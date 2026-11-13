using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SimplePopupManager : MonoBehaviour
{
    public GameObject openButton;
    public GameObject whitePanel;

    void Start()
    {
        if (whitePanel != null)
        {
            whitePanel.SetActive(false);
        }
    }

    public void OpenPopup()
    {
        if (whitePanel != null)
        {
            whitePanel.SetActive(true);
        }
    }

    public void ClosePopup()
    {
        if (whitePanel != null)
        {
            whitePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (whitePanel != null && whitePanel.activeSelf)
        {
            bool isClicked = false;
            Vector2 mousePos = Vector2.zero;

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                mousePos = Mouse.current.position.ReadValue();
                isClicked = true;
            }
            else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                mousePos = Touchscreen.current.primaryTouch.position.ReadValue();
                isClicked = true;
            }

            if (isClicked)
            {
                RectTransform rectTransform = whitePanel.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos, null))
                    {
                        ClosePopup();
                    }
                }
            }
        }
    }
}