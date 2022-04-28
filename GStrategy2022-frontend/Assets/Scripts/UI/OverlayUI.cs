using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class OverlayUI : MonoBehaviour
{
    public Text currentRound;
    public Image playerImage1;
    public Image playerImage2;
    public Slider playerBoodline1;
    public Slider playerBoodline2;


    public void updateRound(int numRound)
    {
        currentRound.text = numRound.ToString();
    }

    public IEnumerator updateBloodline(PlayerStatus player)
    {
        //playerBoodline1.value = player.hp / 100f;
        Slider bloodline;
        if (player.id == 0) bloodline = playerBoodline1;
        else bloodline = playerBoodline2;
        bloodline.value = player.hp / 100f;
        // if (player.hp <= 0)
        // {
        //     bloodline.gameObject.SetActive(false);
        // } 
        yield break;
    }
}
