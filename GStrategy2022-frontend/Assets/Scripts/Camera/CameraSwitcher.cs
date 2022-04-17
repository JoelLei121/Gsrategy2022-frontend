using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
public Camera[] c = new Camera[4];

    void start()
    {
        c[0].enabled = true;
        c[1].enabled = false;
        c[2].enabled = false;
        c[3].enabled = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            c[0].enabled = true;
            c[1].enabled = false;
            c[2].enabled = false;
            c[3].enabled = false;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            c[0].enabled = false;
            c[1].enabled = true;
            c[2].enabled = false;
            c[3].enabled = false;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            c[0].enabled = false;
            c[1].enabled = false;
            c[2].enabled = true;
            c[3].enabled = false;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            c[0].enabled = false;
            c[1].enabled = false;
            c[2].enabled = false;
            c[3].enabled = true;
        }
    }
}
