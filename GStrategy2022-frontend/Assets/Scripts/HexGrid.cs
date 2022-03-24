using UnityEngine;
using System;
using System.Collections;

public class HexGrid : MonoBehaviour
{

    public int width = 20;
    public float length = 1;
    public int monster_num = 0;
    private int monster_index = 0;

    public GameObject hexPrefab;
    public GameObject monsterPrefab;

    GameObject[] cells;
    GameObject[] monsters;

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

        CreateMonster(unit_num);
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

    //随机生成的
    void CreateMonster(int unit_num)
    {
        bool[] has_monster;
        has_monster = new bool[unit_num];
        Array.Clear(has_monster,0,unit_num);
        while (monster_index < monster_num)
        {
            int unit_index = 0;
            do
            {
                unit_index = UnityEngine.Random.Range(0,unit_num-1); 
            } while (has_monster[unit_index]);
            GameObject monster = monsters[monster_index++] = Instantiate<GameObject>(monsterPrefab);
            Vector3 pos_monster = Vector3.zero;
            pos_monster.y = 0.1f;
            monster.transform.SetParent(cells[unit_index].transform, false);
            monster.transform.localPosition = pos_monster;
        }
    }
}
