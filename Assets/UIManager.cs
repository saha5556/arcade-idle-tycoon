using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject editNamePanel;
    public GameObject inputBlocker;

    [Header("Name Elements")]
    public TMP_InputField nameInputField;
    public TMP_Text shopNameDisplay;
    public Button saveButton;

    [Header("Buttons")]
    public Button topSettingsButton;
    public Button menuSettingsButton;
    public Button openNameButton;

    [Header("Close Buttons")]
    public Button closeSettingsButton;
    public Button closeNameButton;

    [Header("Settings Close on Click Outside")]
    public RectTransform menuRectTransform;

    [Header("Character Limit")]
    public int maxCharacterLimit = 15;

    void Start()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (editNamePanel != null) editNamePanel.SetActive(false);
        if (inputBlocker != null) inputBlocker.SetActive(false);

        if (nameInputField != null)
        {
            nameInputField.characterLimit = maxCharacterLimit;
        }

        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveShopName);
        }

        if (topSettingsButton != null)
        {
            topSettingsButton.onClick.AddListener(ToggleMenuPanel);
        }

        if (menuSettingsButton != null)
        {
            menuSettingsButton.onClick.AddListener(OpenSettingsPanel);
        }

        if (openNameButton != null)
        {
            openNameButton.onClick.AddListener(OpenEditNamePanel);
        }

        if (closeSettingsButton != null)
        {
            closeSettingsButton.onClick.AddListener(CloseSettingsPanel);
        }

        if (closeNameButton != null)
        {
            closeNameButton.onClick.AddListener(CloseEditNamePanel);
        }
    }

    void Update()
    {
        bool isSubPanelOpen = (settingsPanel != null && settingsPanel.activeSelf) || (editNamePanel != null && editNamePanel.activeSelf);

        if (inputBlocker != null)
        {
            inputBlocker.SetActive(isSubPanelOpen);
        }

        if (isSubPanelOpen)
        {
            return;
        }

        if (menuPanel != null && menuPanel.activeSelf && UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

            bool clickedOutsideMenu = !RectTransformUtility.RectangleContainsScreenPoint(
                menuPanel.GetComponent<RectTransform>(),
                mousePosition,
                null);

            bool clickedOnTopButton = false;
            if (topSettingsButton != null)
            {
                RectTransform topRect = topSettingsButton.GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(topRect, mousePosition, null))
                {
                    clickedOnTopButton = true;
                }
            }

            if (clickedOutsideMenu && !clickedOnTopButton)
            {
                menuPanel.SetActive(false);
            }
        }
    }

    public void ToggleMenuPanel()
    {
        if (menuPanel != null)
        {
            bool isActive = menuPanel.activeSelf;
            menuPanel.SetActive(!isActive);

            if (isActive)
            {
                if (settingsPanel != null) settingsPanel.SetActive(false);
                if (editNamePanel != null) editNamePanel.SetActive(false);
                if (inputBlocker != null) inputBlocker.SetActive(false);
            }
        }
    }

    public void OpenSettingsPanel()
    {
        if (menuPanel != null) menuPanel.SetActive(false);

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            if (editNamePanel != null) editNamePanel.SetActive(false);
        }

        if (inputBlocker != null) inputBlocker.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (inputBlocker != null) inputBlocker.SetActive(false);
    }

    public void OpenEditNamePanel()
    {
        if (menuPanel != null) menuPanel.SetActive(false);

        if (editNamePanel != null)
        {
            editNamePanel.SetActive(true);
            if (settingsPanel != null) settingsPanel.SetActive(false);
        }

        if (inputBlocker != null) inputBlocker.SetActive(true);
    }

    public void CloseEditNamePanel()
    {
        if (editNamePanel != null)
        {
            editNamePanel.SetActive(false);
        }

        if (inputBlocker != null) inputBlocker.SetActive(false);
    }

    public void SaveShopName()
    {
        if (nameInputField != null && shopNameDisplay != null)
        {
            string enteredText = nameInputField.text;

            if (!string.IsNullOrEmpty(enteredText))
            {
                if (enteredText.Length > maxCharacterLimit)
                {
                    enteredText = enteredText.Substring(0, maxCharacterLimit);
                }

                shopNameDisplay.text = enteredText;
            }

            if (editNamePanel != null) editNamePanel.SetActive(false);
            if (menuPanel != null) menuPanel.SetActive(false);
            if (inputBlocker != null) inputBlocker.SetActive(false);
        }
    }
}