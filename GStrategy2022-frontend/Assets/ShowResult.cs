using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ShowResult : MonoBehaviour
{
    public AllUI ui;
    public PlayerStatus red, blue;
    public Text winner;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        red = GameData.Instance.red;
        blue = GameData.Instance.blue;
        if (red.hp == 0) winner.text = "Blue";
        else winner.text = "Red";
        StartCoroutine(ui.updateRedPlayer(red, ""));
        StartCoroutine(ui.updateBluePlayer(blue, ""));
    }

}
