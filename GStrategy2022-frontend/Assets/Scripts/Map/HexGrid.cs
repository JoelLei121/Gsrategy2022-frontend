using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class HexGrid : MonoBehaviour
{
    public int width = 10;//地图边长
    private int w = 20;
    private int diff = 0;
    public float length = 1;//元件长度

    public GameObject hexPrefab;
    //装饰
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject flowPrefab1;
    public GameObject flowPrefab2;  
    public GameObject flowPrefab3;  
    public GameObject flowPrefab4;
    public GameObject flowPrefab5;
    public GameObject beautifulPrefab;

    public Material normalMaterial;
    public Material highlightMaterial;
    public Material deleteMaterial;
    public Material fowMaterial;

    //public GameObject monsterPrefab;
    public GameObject resPrefab;
    public GameObject gemPrefab;
    //public Text textPrefab;

    GameObject[] units;

    void Awake()
    {            
    }

    public void init(Map map)
    {
        int[] size = map.size;
        List<int[]> barriers = new List<int[]>(map.barriers);
        List<int[]> resources = new List<int[]>(map.resources);
        int t_width = size[0];
        width = t_width;
        if (width < 2)
            return;
        w = 2 * width;
        w = 2 * width;
        units = new GameObject[w * w];

        //for (int i = 0; i < w; i++)
        //{
        //    for (int j = 0; j < w; j++)
        //    {
        //        //GameObject tmp_gO = new GameObject();
        //        //tmp_gO.AddComponent<MapUnit>();
        //        //units.Add(tmp_gO);
        //        //units.Add(gameObject.AddComponent<MapUnit>());
        //    }
        //}

        for (int z = 0; z < w - 1; z++)
        {
            int width_tmp = Mathf.Abs((w - 2) / 2 - z);
            int tmp_s = (width_tmp + ((width+1)%2)) / 2;
            int tmp_e = w - 2 - (width_tmp + (width % 2)) / 2;
            for (int x = tmp_s; x <= tmp_e; x++)
            {
                GameObject tmp_hex = units[z * w + x] = createHex(x, z, length);
                tmp_hex.AddComponent<MapUnit>();
                tmp_hex.GetComponent<MapUnit>().init(x, z, w, tmp_hex, treePrefab, rockPrefab);
            }
        }

        int b_R = barriers.Count;
        for (int i = 0; i < b_R; i++)
        {
            int a_x = barriers[i][0];
            int a_z = barriers[i][2];
            int[] n = hexToOur(a_x, a_z);
            int h_x = n[0];
            int h_z = n[1];
            int[] t = hexToNormal(h_x, h_z);
            int t_x = t[0];
            int t_z = t[1];
            GameObject tmp_hex = units[t_z * w + t_x];
            if (tmp_hex != null)
            {
                int ran = Random.Range(0, 100);
                if(ran<50)
                    tmp_hex.GetComponent<MapUnit>().setType((int)Types.bar, rockPrefab);
                else
                    tmp_hex.GetComponent<MapUnit>().setType((int)Types.bar, treePrefab);
            }
            else
                Debug.Log("initERROR");
        }

        int r_R = resources.Count;
        for (int i = 0; i < r_R; i++)
        {
            int a_x = resources[i][0];
            int a_z = resources[i][2];
            int[] n = hexToOur(a_x, a_z);
            int h_x = n[0];
            int h_z = n[1];
            int[] t = hexToNormal(h_x, h_z);
            int t_x = t[0];
            int t_z = t[1];
            GameObject tmp_hex = units[t_z * w + t_x];
            if (tmp_hex != null)
                tmp_hex.GetComponent<MapUnit>().setType((int)Types.res, resPrefab, resources[i][3]);
            else
                Debug.Log("initERROR");
        }

        for (int z = 0; z < w - 1; z++)
        {
            int width_tmp = Mathf.Abs((w - 2) / 2 - z);
            int tmp_s = (width_tmp + ((width+1)%2)) / 2;
            int tmp_e = w - 2 - (width_tmp + (width % 2)) / 2;
            for (int x = tmp_s; x <= tmp_e; x++)
            {
                GameObject tmp_hex = units[z * w + x];
                if(tmp_hex.GetComponent<MapUnit>().getUnitType()== (int)Types.nor)
                {
                    int ran = Random.Range(0, 100);
                    if (ran < 20)
                        tmp_hex.GetComponent<MapUnit>().setType((int)Types.nor, flowPrefab1);
                    else if (ran<40)
                        tmp_hex.GetComponent<MapUnit>().setType((int)Types.nor, flowPrefab2);
                    else if (ran < 60)
                        tmp_hex.GetComponent<MapUnit>().setType((int)Types.nor, flowPrefab3);
                    else if (ran < 80)
                        tmp_hex.GetComponent<MapUnit>().setType((int)Types.nor, flowPrefab4);
                    else
                        tmp_hex.GetComponent<MapUnit>().setType((int)Types.nor, flowPrefab5);


                }
            }
        }

    }

    public int[] hexToNormal(int h_x, int h_z)
    {
        int t_z = h_z + (2 * width - 1) / 2;
        int t_x = h_x + width / 2 + t_z / 2;
        int[] normal = { t_x, t_z };
        return normal;
    }

    public int[] hexToOur(int h_x, int h_z)
    {
        int t_z = h_z - width + 1;
        int t_x = h_x - width + 1;
        int[] normal = { t_x, t_z };
        return normal;
    }

    public MapUnit GetMapUnit(int h_x, int h_z)
    {
        int[] n = hexToNormal(h_x, h_z);
        int n_x = n[0];
        int n_z = n[1];
        if (units[n_z * w + n_x] == null)
            Debug.Log("HighLightError");
        return (units[n_z * w + n_x].GetComponent<MapUnit>());
    }

    public Vector3 GetUnitPosition(int h_x, int h_z)
    {
        int[] n = hexToNormal(hexToOur(h_x, h_z)[0], hexToOur(h_x, h_z)[1]);
        int n_x = n[0];
        int n_z = n[1];
        // Debug.Log(n_z * w + n_x);
        GameObject unit = units[n_z * w + n_x];
        if (unit == null) Debug.Log("GetUnitPositionERROR");
        Vector3 point = new Vector3(unit.transform.position.x, 0, unit.transform.position.z);
        return point;
    }
    public GameObject createHex(int t_x, int t_z, float length)
    {
        //生成六边形
        float innerRadius = 1.73205081f * length * 0.5f;
        float outerRadius = length;
        Vector3 position;
       // if(width%2==0)
        position.x = (t_x + (t_z % 2) * 0.5f) * (innerRadius * 2f);
        //else
        //    position.x = (t_x + ((t_z+1) % 2) * 0.5f) * (innerRadius * 2f);
        position.y = 0f;
        position.z = t_z * (outerRadius * 1.5f);
        GameObject cell = Instantiate<GameObject>(hexPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        return cell;
    }

    public void checkWidth(int t_width)
    {
        if (diff == width - t_width)
            return;
        else
        {
            Debug.Log("Boundary is changed.");
            while (diff < width - t_width)
            {
                for (int z = diff; z < w - 1 - diff; z++)
                {
                    if (z == diff || z == w - 2 - diff)
                    {
                        int width_tmp = Mathf.Abs((w - 2) / 2 - z);
                        int tmp_s = (width_tmp + ((width + 1) % 2)) / 2 + diff;
                        int tmp_e = w - 2 - (width_tmp + (width % 2)) / 2 - diff;
                        for (int x = tmp_s; x <= tmp_e; x++)
                        {
                            units[z * w + x].GetComponent<MapUnit>().state = (int)States.del;
                            units[z * w + x].GetComponent<Renderer>().material = deleteMaterial;
                        }
                    }
                    else
                    {
                        int width_tmp = Mathf.Abs((w - 2) / 2 - z);
                        int tmp_s = (width_tmp + ((width + 1) % 2)) / 2 + diff;
                        int tmp_e = w - 2 - (width_tmp + (width % 2)) / 2 - diff;
                        units[z * w + tmp_s].GetComponent<MapUnit>().state = (int)States.del;
                        units[z * w + tmp_s].GetComponent<Renderer>().material = deleteMaterial;
                        units[z * w + tmp_e].GetComponent<MapUnit>().state = (int)States.del;
                        units[z * w + tmp_e].GetComponent<Renderer>().material = deleteMaterial;
                    }

                }
                diff++;
            }
        }
    }

    public void setFow(int a_x, int a_z, int range)
    {
        int[] n = hexToOur(a_x, a_z);
        int h_x = n[0];
        int h_z = n[1];
        for (int z = 0; z < w - 1; z++)
        {
            int width_tmp = Mathf.Abs((w - 2) / 2 - z);
            int tmp_s = (width_tmp + ((width + 1) % 2)) / 2;
            int tmp_e = w - 2 - (width_tmp + (width % 2)) / 2;
            for (int x = tmp_s; x <= tmp_e; x++)
            {
                if (units[z * w + x] == null)
                {
                    Debug.Log("setFowERROR");
                    return;
                }
                int[] u_coor = units[z * w + x].GetComponent<MapUnit>().GetHexCoor();
                int h_y = -h_x - h_z;
                if (units[z * w + x].GetComponent<MapUnit>().type != (int)Types.bar && ((Mathf.Sqrt(Mathf.Pow((h_x - u_coor[0]), 2) + Mathf.Pow(h_y - u_coor[1], 2) + Mathf.Pow(h_z - u_coor[2], 2)) - ((float)range * Mathf.Sqrt(2))) < 0.1))
                {
                    if (units[z * w + x].GetComponent<MapUnit>().state != (int)States.del)
                    {
                        units[z * w + x].GetComponent<MapUnit>().state = (int)States.fow;
                    }
                    units[z * w + x].GetComponent<Renderer>().material = fowMaterial;
                }
            }
        }
    }

    public void clearState()
    {
        for (int z = 0; z < w - 1; z++)
        {
            int width_tmp = Mathf.Abs((w - 2) / 2 - z);
            int tmp_s = (width_tmp + ((width + 1) % 2)) / 2;
            int tmp_e = w - 2 - (width_tmp + (width % 2)) / 2;
            for (int x = tmp_s; x <= tmp_e; x++)
            {
                if (units[z * w + x] == null)
                {
                    Debug.Log("clearERROR");
                    return;
                }
                if(units[z * w + x].GetComponent<MapUnit>().state != (int)States.del)
                {
                    units[z * w + x].GetComponent<MapUnit>().state = (int)States.nor;
                    units[z * w + x].GetComponent<Renderer>().material = normalMaterial;
                }
                else
                {
                    units[z * w + x].GetComponent<Renderer>().material = deleteMaterial;
                }
            }
        }
    }

    public void highRole(int a_x, int a_z)
    {
        int[] n = hexToOur(a_x, a_z);
        int h_x = n[0];
        int h_z = n[1];

        GetMapUnit(h_x, h_z).GetCell().GetComponent<Renderer>().material = highlightMaterial;
        if (GetMapUnit(h_x, h_z).state != (int)States.del)
            GetMapUnit(h_x, h_z).state = (int)States.high;
    }

    public void setBeautifulUnits(int a_x, int a_z)
    {
        int[] n = hexToOur(a_x, a_z);
        int h_x = n[0];
        int h_z = n[1];
        Vector3 pos_dec = Vector3.zero;
        GameObject decoration = Instantiate<GameObject>(beautifulPrefab);
        GameObject cell = GetMapUnit(h_x, h_z).GetCell();
        decoration.transform.SetParent(cell.transform, false);
        decoration.transform.localPosition = pos_dec;
    }

    public void checkres(int a_x, int a_z, int MinesLeft)
    {
        int[] n = hexToOur(a_x, a_z);
        int h_x = n[0];
        int h_z = n[1];
        MapUnit unit = GetMapUnit(h_x, h_z).GetComponent<MapUnit>();
        if (unit.getUnitType() != (int)Types.res)
        {
            Debug.Log("Error: Not resource type!");
            return;
        }
        StartCoroutine(unit.Mining(gemPrefab, MinesLeft));
    }


}
