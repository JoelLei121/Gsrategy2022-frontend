using UnityEngine;
using System.Collections;
enum Types { nor,bar,res };//0:normal 1:barrier 2:resource TODO:缩圈,更改资源
public class MapUnit : MonoBehaviour 
{
    private int x = 0;
    private int y = 0;
    private int z = 0;
    private GameObject cell;
    private GameObject resource;
    private int type = (int)Types.nor; 
    private int resource_num = 0;
    
    public MapUnit()
    {

    }
    public void init(int t_x,int t_z,float length, int t_type, GameObject hexPrefab,GameObject resPrefab, int t_res_num = 0)
    {
        x = t_x - t_z/2;
        z = t_z;
        y = -x - z;
        type = t_type;
        if (type == (int)Types.res)
            resource_num = t_res_num;

        //生成地图块
        float innerRadius = 1.73205081f * length * 0.5f;
        float outerRadius = length;
        Vector3 position;
        position.x = (t_x + (t_z % 2) * 0.5f) * (innerRadius * 2f);
        position.y = 0f;
        position.z = t_z * (outerRadius * 1.5f);

        GameObject cell = Instantiate<GameObject>(hexPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;


        switch(type)
        {
            case 1://障碍
                {
                    Vector3 pos_obs = cell.transform.localPosition;
                    pos_obs.y = 100f;
                    cell.transform.localPosition = pos_obs;
                    Vector3 scale_obs = cell.transform.localScale;
                    scale_obs.y = 1000f;
                    cell.transform.localScale = scale_obs;
                    break;
                }
            case 2://资源 TODO：根据资源数修改模型
                {
                    Vector3 pos_res = Vector3.zero;
                    pos_res.y = 1f;
                    GameObject resource = Instantiate<GameObject>(resPrefab);
                    resource.transform.SetParent(cell.transform, false);
                    resource.transform.localPosition = pos_res;
                    break;
                }
            default:
                break;
        }
    }

    public int[] GetHexCoor() //六边形坐标位置
    {
        int[] res = { x, y, z };  
        return res;
    }
    
    public Vector3 GetGloPos() //全局坐标
    {
        return transform.TransformPoint(cell.transform.position);
    }
    public Vector3 GetResPos() //相对坐标
    {
        return cell.transform.position;
    }
    public GameObject GetCell()//返回地图块
    {
        return cell;
    }

    /*public GameObject GetMonster()
    {
        return monster;
    }*/

    public GameObject GetRes()//返回资源
    {
        return resource;
    }
}

public class HexGrid : MonoBehaviour //TODO:与后端文件联动
{
    public int width = 10;//边长
    private int w = 20;
    public float length = 1;//一块的边长

    public GameObject hexPrefab;
    //public GameObject monsterPrefab;
    public GameObject resPrefab;

    ArrayList units = new ArrayList();

    void Awake()
    {
        if (width % 2 == 1)
        {
            width++;
        }
        if (width <= 2)
            return;
        w = 2 * width;


        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < w; j++)
            {
                units.Add(gameObject.AddComponent<MapUnit>());
            }
        }

        for (int z = 0; z < w - 1; z++)
        {
            int width_tmp = Mathf.Abs((w - 2) / 2 - z);
            int tmp_s = (width_tmp + 1) / 2;
            int tmp_e = w - 2 - width_tmp  / 2;
            for (int x = tmp_s; x <= tmp_e; x++)
            {
                ((MapUnit)units[x + w * z]).init(x, z, length, (int)Types.nor, hexPrefab, resPrefab);
            }
        }
    }

    public int[] hexToNormal(int h_x, int h_z)
    {
        int[] normal = { h_x + h_z / 2, h_z };
        return normal;
    }

    public MapUnit GetMapUnit(int h_x, int h_z)
    {
        int[] n = hexToNormal(h_x, h_z);
        int n_x = n[0];
        int n_z = n[1];
        return (MapUnit)units[n_z * w + n_x];
    }
    
}
