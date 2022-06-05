using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    private float fadeTime = 0.01f;

    public CanvasGroup cgRef;
    // Start is called before the first frame update
    void Start()
    {

        Invoke("StartFunct", 2);
        
    }

    void StartFunct()
    {
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeIn()
    {
        float waitTime = 0;
        while (waitTime < 1)
        {
            waitTime += fadeTime;
            yield return new WaitForFixedUpdate();

            CanvasGroup c = cgRef;
            c.alpha = waitTime;
            cgRef = c;
        }
        yield return FadeOut();
    }

    IEnumerator FadeOut()
    {
        float waitTime = 1;
        while (waitTime > 0)
        {
            waitTime -= fadeTime;
            yield return new WaitForFixedUpdate();

            CanvasGroup c = cgRef;
            c.alpha = waitTime;
            cgRef = c;
        }
        //yield return FadeIn();
    }


    public void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
