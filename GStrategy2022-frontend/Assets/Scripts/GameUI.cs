using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public Image FadeImage;
    private float alpha = 1f;
    public Text currentPlayerName;
    public Text currentRound;
    public Text currentPlayerHp;
    public Text currentPlayerExp;
    public Text currentPlayerAction;
    public Text fightRecord;
    public Image playerImage1;
    public Image playerImage2;
    public Slider playerBoodline1;
    public Slider playerBoodline2;

    private IEnumerator fade()
    {
        alpha = 1f;
        while (alpha > 0f)
        {
            alpha -=  2*Time.deltaTime;
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
    }

    public void Start()
    {
        StartCoroutine(fade());
    }
    public void init()
    {

    }
    public void updateRound(int numRound)
    {
        currentRound.text = numRound.ToString();
    }
    public void updateCurrentPlayer(PlayerStatus player,string action)
    {
        currentPlayerName.text = player.teamName;
        currentPlayerAction.text = action;
        currentPlayerExp.text = player.exp.ToString();
        currentPlayerHp.text = player.hp.ToString();

    }

    public void updateBloodline(PlayerStatus player)
    {
        playerBoodline1.value = player.hp / 100f;
    }
    public void updateFightRecord(string action)
    {
        fightRecord.text = action+"\n"+ fightRecord.text;
    }
}
