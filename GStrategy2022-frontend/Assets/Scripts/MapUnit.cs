using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Types { nor, bar, res };//0:normal 1:barrier 2:resource TODO:缩圈,更改资源
public class MapUnit : MonoBehaviour
{
    private int x = 0;
    private int y = 0;
    private int z = 0;
    //private GameObject cell;
    private GameObject resource;
    private int type = (int)Types.nor;
    private int resource_num = 0;

    public MapUnit()
    {

    }
    public void init(int t_x,int t_z,int width, int t_type, GameObject hexParent, GameObject resPrefab, int t_res_num = 0)
    {
        z = t_z - (width - 1) / 2;
        x = t_x - t_z / 2 - width  / 4;
        y = -x - z;
        type = t_type;
        if (type == (int)Types.res)
            resource_num = t_res_num;

        this.transform.SetParent(hexParent.transform,false);

        switch (type)
        {
            case 1://障碍
                {
                    Vector3 pos_obs = GetCell().transform.localPosition;
                    pos_obs.y = 100f;
                    GetCell().transform.localPosition = pos_obs;
                    Vector3 scale_obs = GetCell().transform.localScale;
                    scale_obs.y = 1000f;
                    GetCell().transform.localScale = scale_obs;
                    break;
                }
            case 2://资源 TODO：根据资源数修改模型
                {
                    Vector3 pos_res = Vector3.zero;
                    pos_res.y = 1f;
                    GameObject resource = Instantiate<GameObject>(resPrefab);
                    resource.transform.SetParent(GetCell().transform, false);
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
        return transform.TransformPoint(GetCell().transform.position);
    }
    public Vector3 GetResPos() //相对坐标
    {
        return GetCell().transform.position;
    }
    public GameObject GetCell()//返回地图块
    {
        return this.transform.parent.gameObject;
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
