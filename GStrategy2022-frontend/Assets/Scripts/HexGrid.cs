using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour //TODO:�����ļ�����
{
    public int width = 10;//�߳�
    private int w = 20;
    public float length = 1;//һ��ı߳�

    public GameObject hexPrefab;
    //public GameObject monsterPrefab;
    public GameObject resPrefab;
    public Text textPrefab;

    GameObject[] units;

    void Awake()
    {
    }

    public void init()
    {
        if (width % 2 == 1)
        {
            width++;
        }
        if (width <= 2)
            return;
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
            int tmp_s = (width_tmp + 1) / 2;
            int tmp_e = w - 2 - width_tmp  / 2;
            for (int x = tmp_s; x <= tmp_e; x++)
            {
                GameObject tmp_hex = units[z * w + x] = createHex(x, z, length);
                tmp_hex.AddComponent<MapUnit>();
                tmp_hex.GetComponent<MapUnit>().init(x, z,w, (int)Types.nor, tmp_hex, resPrefab,0,textPrefab);
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

    public MapUnit GetMapUnit(int h_x, int h_z)
    {
        int[] n = hexToNormal(h_x, h_z);
        int n_x = n[0];
        int n_z = n[1];
        return (units[n_z * w + n_x].GetComponent<MapUnit>());
    }

    public Vector3 GetUnitPosition(int h_x, int h_z)
    {
        int[] n = hexToNormal(h_x, h_z);
        int n_x = n[0];
        int n_z = n[1];
        GameObject unit = units[n_z * w + n_x];
        if (unit == null) Debug.Log("ERROR");
        Vector3 point = new Vector3(unit.transform.position.x, 0, unit.transform.position.z);
        return point;
    }
    public GameObject createHex(int t_x, int t_z, float length)
    {
        //���ɵ�ͼ��
        float innerRadius = 1.73205081f * length * 0.5f;
        float outerRadius = length;
        Vector3 position;
        position.x = (t_x + (t_z % 2) * 0.5f) * (innerRadius * 2f);
        position.y = 0f;
        position.z = t_z * (outerRadius * 1.5f);
        GameObject cell = Instantiate<GameObject>(hexPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        return cell;
    }

}
