using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Quit : MonoBehaviour
{
    public Image FadeImage;
    private float alpha = 0f;

    private IEnumerator fadeOut()
    {
        alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 2 * Time.deltaTime;
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
        StartCoroutine(fadeIn());
    }
    private IEnumerator fadeIn()
    {
        alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= 2 * Time.deltaTime;
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public IEnumerator QuitGame()
    {
        StartCoroutine(fadeOut());
        
        //yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(10f);
        // System.Threading.Thread.Sleep(5000);
        Application.Quit();
        yield break;
    }
}
