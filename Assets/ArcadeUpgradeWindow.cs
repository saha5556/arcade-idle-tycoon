using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ArcadeUpgradeWindow : MonoBehaviour
{
    [Header("Player Progress")]
    public int currentLevel = 0;
    public double baseCost = 100;
    public double costMultiplier = 1.07;

    [Header("Income Settings")]
    public double baseCustomerMoney = 10;
    public double customerMoneyMultiplier = 1.18;
    public double baseFixedIncome = 5;
    public float baseBonusChance = 0f;

    [Header("UI References (Drag & Drop)")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI bonusChanceText;
    public TextMeshProUGUI fixedIncomeText;
    public TextMeshProUGUI customerMoneyText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;
    public TextMeshProUGUI milestoneTitleText;
    public TextMeshProUGUI milestoneDescText;
    public Slider progressBar;

    private double currentCost;
    private double currentCustomerMoney;
    private double currentFixedIncome;
    private float currentBonusChance;

    void Start()
    {
        CalculateValues();
        UpdateUI();

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(BuyUpgrade);
        }
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            bool isClicked = false;

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                isClicked = true;
            }
            else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                isClicked = true;
            }

            if (isClicked)
            {
                RectTransform rectTransform = GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector2 pointerPos = Vector2.zero;
                    if (Mouse.current != null)
                        pointerPos = Mouse.current.position.ReadValue();
                    else if (Touchscreen.current != null)
                        pointerPos = Touchscreen.current.primaryTouch.position.ReadValue();

                    if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pointerPos, null))
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void CalculateValues()
    {
        currentCost = baseCost * System.Math.Pow(costMultiplier, currentLevel);
        currentCustomerMoney = baseCustomerMoney * System.Math.Pow(customerMoneyMultiplier, currentLevel);
        currentFixedIncome = baseFixedIncome * currentLevel;

        currentBonusChance = (currentLevel / 10) * 5f;
        if (currentBonusChance > 100f) currentBonusChance = 100f;
    }

    public void BuyUpgrade()
    {
        if (EconomyManager.Instance != null)
        {
            if (EconomyManager.Instance.SpendCoins(currentCost))
            {
                currentLevel++;
                CalculateValues();
                UpdateUI();
            }
            else
            {
                Debug.Log("Not enough money to upgrade!");
            }
        }
    }

    void UpdateUI()
    {
        if (levelText != null) levelText.text = "Level " + currentLevel;

        if (customerMoneyText != null)
        {
            if (EconomyManager.Instance != null)
                customerMoneyText.text = EconomyManager.Instance.FormatMoney(currentCustomerMoney);
            else
                customerMoneyText.text = currentCustomerMoney.ToString("F0");
        }

        if (bonusChanceText != null) bonusChanceText.text = $"Bonus Chance: {currentBonusChance:F0}%";
        if (fixedIncomeText != null) fixedIncomeText.text = $"Fixed Income: +{currentFixedIncome:F0}/5s";

        if (costText != null)
        {
            if (EconomyManager.Instance != null)
                costText.text = EconomyManager.Instance.FormatMoney(currentCost);
            else
                costText.text = currentCost.ToString("F0");
        }

        UpdateMilestonesAndSlider();
    }

    void UpdateMilestonesAndSlider()
    {
        int previousTarget = 0;
        int nextTarget = 30;
        string rewardDesc = "Arcade Boost (+1 Gem)";

        if (currentLevel < 30)
        {
            previousTarget = 0;
            nextTarget = 30;
            rewardDesc = "Arcade Boost (+1 Gem)";
        }
        else if (currentLevel < 50)
        {
            previousTarget = 30;
            nextTarget = 50;
            rewardDesc = "10x Income Multiplier!";
        }
        else if (currentLevel < 100)
        {
            previousTarget = 50;
            nextTarget = 100;
            rewardDesc = "Lucky Find (+2 Gems)";
        }
        else if (currentLevel < 150)
        {
            previousTarget = 100;
            nextTarget = 150;
            rewardDesc = "Arcade Empire (25x Multiplier)";
        }
        else if (currentLevel < 200)
        {
            previousTarget = 150;
            nextTarget = 200;
            rewardDesc = "Rare Crystal (+3 Gems)";
        }
        else if (currentLevel < 250)
        {
            previousTarget = 200;
            nextTarget = 250;
            rewardDesc = "Mega Cash Bonus (50x Multiplier)";
        }
        else if (currentLevel < 300)
        {
            previousTarget = 250;
            nextTarget = 300;
            rewardDesc = "Secret Vault (+5 Gems)";
        }
        else
        {
            int bracket = (currentLevel - 300) / 50;
            previousTarget = 300 + (bracket * 50);
            nextTarget = previousTarget + 50;
            rewardDesc = "Endless Tycoon Bonus";
        }

        if (milestoneTitleText != null) milestoneTitleText.text = $"Next at {nextTarget} Upgrades:";
        if (milestoneDescText != null) milestoneDescText.text = rewardDesc;

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = 1;

            int range = nextTarget - previousTarget;
            if (range <= 0) range = 1;

            float progress = (float)(currentLevel - previousTarget) / range;
            progressBar.value = Mathf.Clamp01(progress);
        }
    }
}