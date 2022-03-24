using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    // player datatype needed
    void Start()
    {
        getCommand(call);
        commandIsDone = false;
        i = 0;
    }

    void Update()
    {
        
    }

    // MoveTo(x, y, z)
    // Attack(Vector3 direction)
    // Damaged(Vector3 direction)
    // Died()
    // Grab(Vector3 setPoint)

}