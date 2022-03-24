using UnityEngine;
using System;
using System.Collections;

public class HexGrid : MonoBehaviour
{

    public int width = 20;
    public float length = 1;
    public int monster_num = 0;
    public int gift_num = 0;
    public int obs_num = 0;
    private int monster_index = 0;
    private int gift_index = 0;
    private int obs_index = 0;

    public GameObject hexPrefab;
    public GameObject monsterPrefab;
    public GameObject giftPrefab;

    GameObject[] cells;
    GameObject[] monsters;
    GameObject[] gifts;
    GameObject[] obss;
    bool[] has_item;

    void Awake()
    {
        if(width%2==1)
        {
            width++;
        }
        if (width <= 2)
            return;

        cells = new GameObject[width * width];
        monsters = new GameObject[monster_num];
        gifts = new GameObject[gift_num];
        obss = new GameObject[obs_num];

        int unit_num = 0;
        for (int z = 0; z < width - 1; z++)
        {
            int width_tmp = Mathf.Abs((width - 2) / 2 - z);
            int tmp_s = (width_tmp + 1) / 2;
            int tmp_e = width - 2 - width_tmp  / 2;
            for (int x = tmp_s; x <= tmp_e; x++)
            {
                CreateCell(x, z, unit_num++);
            }
        }

        has_item = new bool[unit_num];
        Array.Clear(has_item, 0, unit_num);

        CreateCentre(unit_num);
        CreateMonster(unit_num);
        CreateGift(unit_num);
        CreateObstacle(unit_num);
    }

    void CreateCell(int x, int z, int i)
    {
        float innerRadius = 1.73205081f * length * 0.5f;
        float outerRadius = length;
        Vector3 position;
        position.x = (x +(z % 2)*0.5f) *(innerRadius * 2f);
        position.y = 0f;
        position.z = z * (outerRadius * 1.5f);

        GameObject cell = cells[i] = Instantiate<GameObject>(hexPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        
    }

    void CreateCentre(int unit_num)
    {
        GameObject cell = Instantiate<GameObject>(hexPrefab);
        Vector3 position = Vector3.zero;
        position.y = 5f;
        cell.transform.SetParent(cells[unit_num/2].transform);
        cell.transform.localPosition = position;
    }

    //随机生成的
    void CreateMonster(int unit_num)
    {
        while (monster_index < monster_num)
        {
            int unit_index = 0;
            do
            {
                unit_index = UnityEngine.Random.Range(0,unit_num-1); 
            } while (has_item[unit_index]);
            GameObject monster = monsters[monster_index++] = Instantiate<GameObject>(monsterPrefab);
            Vector3 pos_monster = Vector3.zero;
            pos_monster.y = 0.1f;
            monster.transform.SetParent(cells[unit_index].transform, false);
            monster.transform.localPosition = pos_monster;
        }
    }

    void CreateGift(int unit_num)
    {
        while (gift_index < gift_num)
        {
            int unit_index = 0;
            do
            {
                unit_index = UnityEngine.Random.Range(0, unit_num - 1);
            } while (has_item[unit_index]);
            GameObject gift = gifts[gift_index++] = Instantiate<GameObject>(giftPrefab);
            Vector3 pos_gift = Vector3.zero;
            pos_gift.y = 1f;
            gift.transform.SetParent(cells[unit_index].transform, false);
            gift.transform.localPosition = pos_gift;
        }
    }

    void CreateObstacle(int unit_num)
    {
        while (obs_index < obs_num)
        {
            int unit_index = 0;
            do
            {
                unit_index = UnityEngine.Random.Range(width-1, unit_num - width);
            } while (has_item[unit_index]);
            GameObject obs = obss[obs_index++] = Instantiate<GameObject>(hexPrefab);
            Vector3 pos_obs = Vector3.zero;
            Vector3 scale_obs = Vector3.one;
            scale_obs.y = 100f;
            pos_obs.y = 10f;
            obs.transform.SetParent(cells[unit_index].transform, false);
            obs.transform.localPosition = pos_obs;
            obs.transform.localScale = scale_obs;
        }
    }
}
