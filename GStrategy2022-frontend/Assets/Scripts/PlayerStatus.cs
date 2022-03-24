using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // Start is called before the first frame update
    public string teamName;
    public float exp;
    public float hp;
    public int atk;
    public int visibility;

    public int speed;
    public int cure;
    public float gatheringSkill;
    public int ordering;
    public bool OnProtection;

    void Start()
    {
        exp = 0;
    }


}
