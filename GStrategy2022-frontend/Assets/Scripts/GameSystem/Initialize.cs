using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
class Map
{
    public int[] size;
    public int[,] barriers;
    public int[,] resources;

}


[Serializable]
class InitialState
{
    public Map map;
    public PlayerInitialize[] players;
}

public class Initialize : MonoBehaviour
{
    public GameController gameController;

    void Awake()
    {

    }

    public void Run()
    {
        Debug.Log("waiting for init");
        FileOpenDialog dialog = new FileOpenDialog();
        dialog.structSize = Marshal.SizeOf(dialog);
        dialog.filter = "json files\0*.json\0All Files\0*.*\0\0";
        dialog.file = new string(new char[256]);
        dialog.maxFile = dialog.file.Length;
        dialog.fileTitle = new string(new char[64]);
        dialog.maxFileTitle = dialog.fileTitle.Length;
        dialog.initialDir = "C:";  //默认路径
        dialog.title = "Choose Initialization data file";
        dialog.defExt = "json";//显示文件的类型
        //注意一下项目不一定要全选 但是0x00000008项不要缺少
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        while (true)
        {
            if (!DialogShow.GetOpenFileName(dialog))
            {
                Messagebox.MessageBox(IntPtr.Zero, "Choose correct game file！", "Warning", 0);
                continue;
            }
            else if (!dialog.file.Contains(".json"))
            {
                Messagebox.MessageBox(IntPtr.Zero, "Choose correct game file！", "Warning", 0);
                continue;
            }
            else
                break;
        }

        String initialJson = File.ReadAllText(dialog.file);
        InitialState state = JsonUtility.FromJson<InitialState>(initialJson);
        //Initialize Map

        //Initialize Players
        gameController.playerNum = state.players.Length;
        gameController.players = new GameObject[gameController.playerNum];

        for(int i = 0; i < gameController.playerNum; i++)
        {
            GameObject player = Instantiate<GameObject>(gameController.playerPrefab);
            gameController.players[i] = player;
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            playerStatus.init(state.players[i]);
        }
        Debug.Log("Players are ready.");
        gameController.isInstantiated = true;
        Response<GameState> tmp = new Response<GameState>();
        tmp.list = new List<GameState>();
        tmp.list.Add(new GameState());
        Debug.Log(JsonUtility.ToJson(tmp));
    }
}
