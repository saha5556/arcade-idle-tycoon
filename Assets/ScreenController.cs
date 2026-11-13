using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public GameObject shopScreen;
    public GameObject secondPlaceScreen;
    public GameObject unlockScreen;

    private void Start()
    {
        ShowShop();
    }

    private void HideAllScreens()
    {
        if (shopScreen != null) shopScreen.SetActive(false);
        if (secondPlaceScreen != null) secondPlaceScreen.SetActive(false);
        if (unlockScreen != null) unlockScreen.SetActive(false);
    }

    public void ShowShop()
    {
        HideAllScreens();
        if (shopScreen != null) shopScreen.SetActive(true);
    }

    public void ShowSecondPlace()
    {
        HideAllScreens();
        if (secondPlaceScreen != null) secondPlaceScreen.SetActive(true);
    }

    public void ShowUnlockMenu()
    {
        HideAllScreens();
        if (unlockScreen != null) unlockScreen.SetActive(true);
    }
}