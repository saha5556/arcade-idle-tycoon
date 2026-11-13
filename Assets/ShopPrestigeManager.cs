using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPrestigeManager : MonoBehaviour
{
    [Header("Prestige Requirements")]
    public double prestigeCost = 1000;
    public double costMultiplier = 2.5f;

    [Header("Shop Screen & Visuals")]
    public GameObject shopScreenPanel;
    public Image shopFloorImage;
    public Color[] prestigeColors;

    [Header("UI References")]
    public Button prestigeButton;
    public TextMeshProUGUI prestigeButtonText;

    private void Start()
    {
        if (prestigeButton != null)
        {
            prestigeButton.onClick.RemoveAllListeners();
            prestigeButton.onClick.AddListener(AttemptPrestige);
        }

        if (EconomyManager.Instance != null)
        {
            prestigeCost = GetPrestigeCostForLevel(EconomyManager.Instance.prestigeNumber);
        }

        ApplyShopVisuals();
        UpdatePrestigeUI();
    }

    private void Update()
    {
        if (EconomyManager.Instance != null)
        {
            prestigeCost = GetPrestigeCostForLevel(EconomyManager.Instance.prestigeNumber);
        }

        UpdatePrestigeUI();
    }

    public void AttemptPrestige()
    {
        if (EconomyManager.Instance != null)
        {
            prestigeCost = GetPrestigeCostForLevel(EconomyManager.Instance.prestigeNumber);

            if (EconomyManager.Instance.coins >= prestigeCost)
            {
                EconomyManager.Instance.SpendCoins(prestigeCost);
                EconomyManager.Instance.prestigeNumber++;
                EconomyManager.Instance.prestigeCoins += 10;

                prestigeCost = GetPrestigeCostForLevel(EconomyManager.Instance.prestigeNumber);

                ApplyShopVisuals();

                EconomyManager.Instance.SaveGameData();
                UpdatePrestigeUI();

                Debug.Log("Prestige completed successfully! Level: " + EconomyManager.Instance.prestigeNumber);
            }
            else
            {
                Debug.Log("Insufficient balance for Prestige!");
            }
        }
    }

    private double GetPrestigeCostForLevel(int currentLevel)
    {
        switch (currentLevel)
        {
            case 0: return 10000;
            case 1: return 50000;
            case 2: return 100000;
            case 3: return 100000;
            case 4: return 500000;
            case 5: return 500000;
            case 6: return 900000;
            default: return 900000;
        }
    }

    public void ApplyShopVisuals()
    {
        if (shopFloorImage == null || prestigeColors == null || prestigeColors.Length == 0) return;

        int currentLevel = (EconomyManager.Instance != null) ? EconomyManager.Instance.prestigeNumber : 0;
        int colorIndex = currentLevel % prestigeColors.Length;

        Color selectedColor = prestigeColors[colorIndex];
        selectedColor.a = 1f;

        shopFloorImage.color = selectedColor;
    }

    private void UpdatePrestigeUI()
    {
        if (prestigeButtonText != null)
        {
            prestigeButtonText.text = $"Prestige\nCost: {prestigeCost:N0}";
        }
    }
}