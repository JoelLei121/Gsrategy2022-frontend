using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class AllUI : MonoBehaviour
{
    public Text rPlayerName;
    public Text rPlayerHp;
    public Text rPlayerExp;
    public Text rPlayerAction;
    public Text rPlayerATK;
    public Text rPlayerVision;
    public Text rPlayerPosition;
    public Text rPlayerAttackRange;
    public Text rPlayerMoveRange;
    public Text rPlayerGatheringSkill;

    public Text bPlayerName;
    public Text bPlayerHp;
    public Text bPlayerExp;
    public Text bPlayerAction;
    public Text bPlayerATK;
    public Text bPlayerVision;
    public Text bPlayerPosition;
    public Text bPlayerAttackRange;
    public Text bPlayerMoveRange;
    public Text bPlayerGatheringSkill;

    public IEnumerator updateRedPlayer(PlayerStatus player, string action)
    {
        rPlayerName.text = player.teamName;
        rPlayerAction.text = action;
        rPlayerExp.text = player.exp.ToString();
        rPlayerHp.text = player.hp.ToString();
        rPlayerATK.text = player.atk.ToString();
        rPlayerVision.text = player.sight_range.ToString();
        rPlayerMoveRange.text = player.move_range.ToString();
        rPlayerAttackRange.text = player.attack_range.ToString();
        rPlayerGatheringSkill.text = player.gatheringSkill.ToString();
        int[] pos = player.pos;
        string posS = "(" + pos[0] + ", " + pos[1] + ", " + pos[2] + ")";
        rPlayerPosition.text = posS;
        yield break;
    }

    public IEnumerator updateBluePlayer(PlayerStatus player, string action)
    {
        bPlayerName.text = player.teamName;
        bPlayerAction.text = action;
        bPlayerExp.text = player.exp.ToString();
        bPlayerHp.text = player.hp.ToString();
        bPlayerATK.text = player.atk.ToString();
        bPlayerVision.text = player.sight_range.ToString();
        bPlayerMoveRange.text = player.move_range.ToString();
        bPlayerAttackRange.text = player.attack_range.ToString();
        bPlayerGatheringSkill.text = player.gatheringSkill.ToString();
        int[] pos = player.pos;
        string posS = "(" + pos[0] + ", " + pos[1] + ", " + pos[2] + ")";
        bPlayerPosition.text = posS;
        yield break;
    }

}
