using UnityEngine;
using System.Collections;

public class HexGrid : MonoBehaviour
{

    public int width = 6;
    public int height = 6;
    public int innerRadius = 1;
    public int outerRadius = 1;

    public GameObject hexPrefab;

    GameObject[] cells;

    void Awake()
    {
        cells = new GameObject[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x +(z % 2)*0.5f) *(innerRadius * 2f);
        position.y = 0f;
        position.z = z * (outerRadius * 1.5f);

        GameObject cell = cells[i] = Instantiate<GameObject>(hexPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }
}