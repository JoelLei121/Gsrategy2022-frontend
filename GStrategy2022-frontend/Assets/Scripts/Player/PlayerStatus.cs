using UnityEngine;
using System.IO;
using System;

[Serializable]
public class PlayerInitialize
{
    public int id;
    public string name;
    public int[] Position;           //λ��
    public int mine_speed; //�ɼ��ٶ�
    public int at; //������
    public int hp; //Ѫ��
    public int attack_range;   //������Χ
    public int sight_range; //��Ұ��Χ
    public int move_range; //�ƶ���Χ
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
    public int attack_range;   //������Χ
    public int sight_range; //��Ұ��Χ
    public int move_range; //�ƶ���Χ
    public int mine_speed;
    public float gatheringSkill;

    public int cure;
    public bool OnProtection;

    public void init(PlayerInitialize p)
    {
        teamName = p.name;
        pos = new int[3];
        for(int i = 0; i < 3; i++)
            pos[i] = p.Position[i];
        id = p.id;
        exp = 0;
        hp = p.hp;
        atk = p.at;
        mine_speed = p.mine_speed;
        attack_range = p.attack_range;
        sight_range = p.sight_range;
        move_range = p.move_range;
    }

    void Start()
    {
        exp = 0;
    }

    public bool isDead()
    {
        return hp <= 0;
    }


}
