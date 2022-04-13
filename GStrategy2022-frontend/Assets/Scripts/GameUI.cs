using UnityEngine.UI;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public Text currentPlayerName;
    public Text currentRound;
    public Text currentPlayerHp;
    public Text currentPlayerExp;
    public Text currentPlayerAction;
    public Image playerImage1;
    public Image playerImage2;
    public Slider playerBoodline1;
    public Slider playerBoodline2;


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
}
