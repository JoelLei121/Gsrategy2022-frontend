using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public Text currentPlayerName;
    public Text currentRound;
    public Text currentPlayerHp;
    public Text currentPlayerExp;
    public Text currentPlayerAction;
    public Text currentPlayerATK;
    public Text currentPlayerVision;
    public Text currentPlayerPosition;
    public Text currentPlayerAttackRange;
    public Text currentPlayerMoveRange;
    public Text currentPlayerGatheringSkill;
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
    public IEnumerator updateCurrentPlayer(PlayerStatus player, string action)
    {
        currentPlayerName.text = player.teamName;
        currentPlayerAction.text = action;
        currentPlayerExp.text = player.exp.ToString();
        currentPlayerHp.text = player.hp.ToString();
        currentPlayerATK.text = player.atk.ToString();
        currentPlayerVision.text = player.sight_range.ToString();
        currentPlayerMoveRange.text = player.move_range.ToString();
        currentPlayerAttackRange.text = player.attack_range.ToString();
        currentPlayerGatheringSkill.text = player.gatheringSkill.ToString();
        int [] pos = player.pos;
        string posS = "(" + pos[0] + ", " + pos[1] + ", " + pos[2] + ")";
        currentPlayerPosition.text = posS;
        yield break;
    }

    public IEnumerator updateBloodline(PlayerStatus player)
    {
        Slider bloodline;
        if (player.id == 0) bloodline = playerBoodline1;
        else bloodline = playerBoodline2;
        if (player.hp <= 0) bloodline.enabled = false;
        else bloodline.value = player.hp / 100f;
        yield break;
    }
}
