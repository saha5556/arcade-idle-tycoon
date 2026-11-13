using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class SplashController : MonoBehaviour
{
    public float delayTime = 3f;

    public string mainGameSceneName = "SampleScene";

    void Start()
    {
        StartCoroutine(LoadMainGameAfterDelay());
    }

    IEnumerator LoadMainGameAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(mainGameSceneName);
    }
}