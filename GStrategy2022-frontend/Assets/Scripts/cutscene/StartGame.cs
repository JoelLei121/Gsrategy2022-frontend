using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Image FadeImage;
    private float alpha = 0f;

    private IEnumerator fade() 
    {
        alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 2*Time.deltaTime;
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void startGame()
    {


        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(fade());
        }
    }
}
