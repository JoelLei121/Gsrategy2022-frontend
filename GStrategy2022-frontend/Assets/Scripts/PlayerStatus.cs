using UnityEngine;
using System.IO;
using System;

[Serializable]
public class PlayerInitialize
{
    public int id;
    public string name;
    public int[] Position;           //位置
    public int attack_range;   //攻击范围
    public int sight_range; //视野范围
    public int move_range; //移动范围
    public int mine_speed; //采集速度
    public int at; //攻击力
    public int hp; //血量
}


public class PlayerStatus : MonoBehaviour
{
    // Start is called before the first frame update
    public string teamName;
    public int id;
    public int[] pos;
    public float exp;
    public float hp;
    public int atk;
    public int visibility;

    public int speed;
    public int cure;
    public float gatheringSkill;
    public int ordering;
    public bool OnProtection;

    public void init(PlayerInitialize p)
    {
        pos = new int[3];
        for(int i = 0; i < 3; i++)
            pos[i] = p.Position[i];
        id = p.id;
        exp = 0;
        hp = p.hp;
        atk = p.at;
        visibility = 1;  
    }

    void Start()
    {
        exp = 0;
    }


}
