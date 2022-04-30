using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public Text currentPlayerName;
    public Text currentPlayerHp;
    public Text currentPlayerExp;
    public Text currentPlayerAction;
    public Text currentPlayerATK;
    public Text currentPlayerVision;
    public Text currentPlayerPosition;
    public Text currentPlayerAttackRange;
    public Text currentPlayerMoveRange;
    public Text currentPlayerGatheringSkill;


    public void init()
    {

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
        currentPlayerGatheringSkill.text = player.mine_speed.ToString();
        int [] pos = player.pos;
        string posS = "(" + pos[0] + ", " + pos[1] + ", " + pos[2] + ")";
        currentPlayerPosition.text = posS;
        yield break;
    }

}
