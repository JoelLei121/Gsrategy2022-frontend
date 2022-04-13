using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Quit : MonoBehaviour
{
    public IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForSeconds(10f);
        // System.Threading.Thread.Sleep(5000);
        Application.Quit();
        yield break;
    }
}
