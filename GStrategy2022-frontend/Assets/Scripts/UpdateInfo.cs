using UnityEngine.UI;
using UnityEngine;

public class UpdateInfo : MonoBehaviour
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

    public void updateRound(int numRound)
    {
        currentRound.text = numRound.ToString();
    }
    public void updateCurrentPlayer(PlayerStatus player,PlayerAction action)
    {
        currentPlayerName.text = player.name;
        currentPlayerAction.text = action.name;
        currentPlayerExp.text = player.exp.ToString();
        currentPlayerHp.text = player.hp.ToString();

    }

    public void updateBloodline(PlayerStatus p1,PlayerStatus p2)
    {
        playerBoodline1.value = p1.hp / 100f;
        playerBoodline2.value = p2.hp / 100f;
    }
}
