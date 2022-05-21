using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ShowResult : MonoBehaviour
{
    public AllUI ui;
    public PlayerStatus red, blue;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        red = GameData.Instance.red;
        blue = GameData.Instance.blue;
        StartCoroutine(ui.updateRedPlayer(red, ""));
        StartCoroutine(ui.updateBluePlayer(blue, ""));
    }

}
