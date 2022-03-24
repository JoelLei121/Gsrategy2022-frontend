using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    private int i;
    public bool commandIsDone;
    private string[] call;

    public GameObject[] player;
    public int playerNum = 4;

    //public Map map; // waiting for map data development
    public int mapSize = 20;

    void Start()
    {
        getCommand(call);
        commandIsDone = false;
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //unnecessary
    }

    void doAction() // for external call
    {
        doCommand(call[i]);
        i++;
    }

    string getCommand(string[] command)
    {
        // split text and return
    }

    void doCommand(string s)
    {
        // if..else..
        WaitForCommand();
        return;
    }

    void WaitForCommand()
    {
        while(!commandIsDone);
        return;
    }
}