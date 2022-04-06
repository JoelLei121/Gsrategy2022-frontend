using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator quit()
    {
        yield return new WaitForSeconds(10f);
        Application.Quit();
        Debug.Log("Quit");
        yield break;
    }
}
