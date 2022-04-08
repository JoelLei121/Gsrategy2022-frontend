using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Types { nor, bar, res };//0:normal 1:barrier 2:resource TODO:类型
enum States { nor,del,high,fow};//0:normal 1:deleted 2:highlighted 3:fowed
public class MapUnit : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public int z = 0;
    private GameObject cell;
    private GameObject resource;
    public int type = (int)Types.nor;
    private int resource_num = 0;
    public int state = (int) States.nor;
    public MapUnit()
    {

    }
    public void init(int t_x,int t_z,int width, GameObject hexParent,GameObject treePrefab,GameObject rockPrefab)
    {
        z = t_z - (width - 1) / 2;
        x = t_x - t_z / 2 - width  / 4;
        y = -x - z;
            

        this.transform.SetParent(hexParent.transform,false);
        cell = hexParent;
        //文本
        // if(textPrefab !=null)
        // {
        //     Text label = Instantiate<Text>(textPrefab);
        //     label.rectTransform.SetParent(hexParent.transform, false);
        //     label.rectTransform.anchoredPosition = new Vector2(hexParent.transform.position.x, hexParent.transform.position.z);
        //     label.text = x.ToString() + "\n" + z.ToString();
        // }
        //装饰
        int ifDecoration = Random.Range(0, 100);
        if (ifDecoration<5)//生成树
        {
            Vector3 pos_dec = Vector3.zero;
            Vector3 scale_dec = Vector3.one;
            scale_dec.x = 0.5f;
            scale_dec.y = 0.5f;
            scale_dec.z = 0.5f;
            GameObject decoration = Instantiate<GameObject>(treePrefab);
            decoration.transform.SetParent(cell.transform, false);
            decoration.transform.localPosition = pos_dec;
            decoration.transform.transform.localScale = scale_dec;
        }
        else if(ifDecoration<10)//生成石头
        {
            Vector3 pos_dec = Vector3.zero;
            pos_dec.y = 0.1f;
            Vector3 scale_dec = Vector3.one;
            scale_dec.x = 0.5f;
            scale_dec.y = 0.5f;
            scale_dec.z = 0.5f;
            GameObject decoration = Instantiate<GameObject>(rockPrefab);
            decoration.transform.SetParent(cell.transform, false);
            decoration.transform.localPosition = pos_dec;
            decoration.transform.transform.localScale = scale_dec;
        }
    }
        

   public void setType(int t_type, GameObject resPrefab=null, int t_res_num = 0)
   {
       type = t_type;
       switch (type)
        {
            case 1://障碍
                {
                    Vector3 pos_obs = cell.transform.localPosition;
                    pos_obs.y = 100f;
                    GetCell().transform.localPosition = pos_obs;
                    Vector3 scale_obs = cell.transform.localScale;
                    scale_obs.y = 1000f;
                    GetCell().transform.localScale = scale_obs;
                    break;
                }
            case 2://资源
                {
                    resource_num = t_res_num;
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

    public int[] GetHexCoor() //六边形坐标
    {
        int[] res = { x, y, z };
        return res;
    }

    public Vector3 GetGloPos() //全局位置
    {
        return transform.TransformPoint(GetCell().transform.position);
    }
    public Vector3 GetResPos() //相对位置
    {
        return GetCell().transform.position;
    }
    public GameObject GetCell()//返回六边形
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
