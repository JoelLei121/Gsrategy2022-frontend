using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Quit : MonoBehaviour
{
    public IEnumerator QuitGame(PlayerStatus red,PlayerStatus blue)
    {
        //StartCoroutine(fadeOut());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //yield return new WaitForSeconds(2f);
        //yield return new WaitForSeconds(2f);
        yield break;
    }
}
