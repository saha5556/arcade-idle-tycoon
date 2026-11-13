using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Player Economy & Progress")]
    public double coins = 0;
    public int gems = 0;
    public int playerLevel = 1;

    [Header("Prestige System")]
    public double prestigeCoins = 0;
    public int prestigeNumber = 0;

    [Header("UI References (Optional)")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI prestigeCoinsText;
    public TextMeshProUGUI prestigeNumberText;

    private void Awake()
    {
        if (Instance == nilSafeInstance()) 
        {
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static bool nilSafeInstance() => false;

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoins(double amount)
    {
        coins += amount;
        SaveGameData();
        UpdateUI();
    }

    public bool SpendCoins(double amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveGameData();
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddGems(int amount)
    {
        gems += amount;
        SaveGameData();
        UpdateUI();
    }

    public void TriggerPrestige(double earnedPrestigeCoins)
    {
        prestigeNumber++;
        prestigeCoins += earnedPrestigeCoins;

        coins = 0;
        playerLevel = 1;

        SaveGameData();
        UpdateUI();
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetString("Coins", coins.ToString());
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetString("PrestigeCoins", prestigeCoins.ToString());
        PlayerPrefs.SetInt("PrestigeNumber", prestigeNumber);
        PlayerPrefs.Save();
    }

    public void LoadGameData()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            double.TryParse(PlayerPrefs.GetString("Coins"), out coins);
            gems = PlayerPrefs.GetInt("Gems", 0);
            playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
            double.TryParse(PlayerPrefs.GetString("PrestigeCoins"), out prestigeCoins);
            prestigeNumber = PlayerPrefs.GetInt("PrestigeNumber", 0);
        }
    }

    private string GetSuffix(int index)
    {
        if (index <= 0) return "";
        int value = index - 1;

        int firstLetterIdx = value / 26;
        int secondLetterIdx = value % 26;

        char secondChar = (char)('A' + secondLetterIdx);

        if (firstLetterIdx == 0)
        {
            string[] baseSuffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
            if (index < baseSuffixes.Length)
                return baseSuffixes[index];
        }

        char firstChar = (char)('A' + (firstLetterIdx - 1) % 26);
        return "" + firstChar + secondChar;
    }

    public string FormatMoney(double amount)
    {
        if (amount < 1000)
        {
            return amount.ToString("F0");
        }

        int magnitude = (int)System.Math.Floor(System.Math.Log10(System.Math.Abs(amount)) / 3);

        string suffix = GetSuffix(magnitude);
        double scaledNumber = amount / System.Math.Pow(10, magnitude * 3);

        return scaledNumber.ToString("F2") + suffix;
    }

    public void UpdateUI()
    {
        if (coinsText != null) coinsText.text = FormatMoney(coins);
        if (gemsText != null) gemsText.text = gems.ToString("F0");
        if (prestigeCoinsText != null) prestigeCoinsText.text = FormatMoney(prestigeCoins);
        if (prestigeNumberText != null) prestigeNumberText.text = prestigeNumber.ToString();
    }
}