using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class RecordUI : MonoBehaviour
{
    public Text fightRecord;

    public void updateFightRecord(string action)
    {
        fightRecord.text = action + "\n" + fightRecord.text;
    }

    public void updateRound(int numRound)
    {
        fightRecord.text = "Round " + numRound.ToString() + " start" + "\n" + fightRecord.text;
    }

}
