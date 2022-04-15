using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThanksScene : MonoBehaviour
{
    private float counter;
    // Start is called before the first frame update
    public Image FadeImage;
    private float alpha = 1f;

    private IEnumerator fadeIn()
    {
        alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= 2 * Time.deltaTime;
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
        //StartCoroutine(fadeOut());
    }
    void Start()
    {
        StartCoroutine(fadeIn());
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > 6)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
