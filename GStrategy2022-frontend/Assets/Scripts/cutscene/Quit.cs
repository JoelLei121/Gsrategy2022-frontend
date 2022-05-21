using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Quit : MonoBehaviour
{
    public IEnumerator QuitGame(PlayerStatus red,PlayerStatus blue)
    {
        //StartCoroutine(fadeOut());
        GameData.Instance.red = red;
        GameData.Instance.blue = blue;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //yield return new WaitForSeconds(2f);
        //yield return new WaitForSeconds(2f);
        yield break;
    }
}
