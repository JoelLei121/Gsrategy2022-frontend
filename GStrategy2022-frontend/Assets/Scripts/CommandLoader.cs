using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows;

[Serializable]
class GameState
{
    public int ActivePlayerId;
    public int[] ActivePos;
    public String CurrentEvent;
    public int[] MapSize = { 10, 10, 10 };
    public int Round;
    public int[] VictimId;
    public int? WinnerId;
    public float exp;

}

public class Response<T>
{
    public List<T> list;
}


public class CommandLoader : MonoBehaviour
{
    private int index;
    private int gameLength;
    public GameController gameController;
    public PlayerAction playerAction;
    private List<GameState> gameStates;
    private GameState runningState;
    // private IEnumerator coroutine; 
    void Start()
    {
        index = 0;
    }
    public void GetCommandFromDocument()
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
        dialog.title = "Choose game history file";
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
            else break;

        }

        String gameStateJson = File.ReadAllText(dialog.file);
        Response<GameState> response = JsonUtility.FromJson<Response<GameState>>(gameStateJson);
        gameStates = response.list;

        if (gameStates != null)
            Debug.Log("Game process read successfully.");
        gameLength = gameStates.Count;
    }
    void NextCommand()
    {
        // start from 0
        runningState = gameStates[index];
        index++;
    }

    public IEnumerator LoadCommand()
    {
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(2f);
        while(true)
        {
            if(index >= gameLength)
            {
                gameController.commandIsDone = true;
                Debug.Log("Stop running game");
                yield break;
            }
            NextCommand();
            // PlayerStatus target = gameController.GetPlayerStatus(runningState.ActivePlayerId);
            gameController.map.checkWidth(runningState.MapSize[0]);
            yield return new WaitForSeconds(2f);
            GameObject target = gameController.players[runningState.ActivePlayerId];
            int[] pos = runningState.ActivePos;
            PlayerStatus status = gameController.players[runningState.ActivePlayerId].GetComponent<PlayerStatus>();
            gameController.map.setFow(pos[0], pos[2], status.visibility);
            yield return new WaitForSeconds(2f);
            gameController.map.highRole(pos[0], pos[2]);
            yield return new WaitForSeconds(2f);
            gameController.map.clearState();
            yield return new WaitForSeconds(2f);

            switch (runningState.CurrentEvent)
            {
                case "MOVE":
                    // int[] pos = runningState.ActivePos;
                    yield return StartCoroutine(playerAction.MoveTo(target, pos[0], pos[1], pos[2]));
                    break;

                case "ATTACK":
                    GameObject[] victim = new GameObject[runningState.VictimId.Length];
                    for (int i = 0; i < runningState.VictimId.Length; i++)
                    {
                        victim[i] = gameController.players[runningState.VictimId[i]];
                    }
                    yield return StartCoroutine(playerAction.Attack(target, victim));
                    break;

                case "GATHER":
                    yield return StartCoroutine(playerAction.Gather(target, runningState.exp));
                    break;

                case "ERROR":
                    yield return StartCoroutine(playerAction.DoNothing(target));
                    break;

                case "DIED":
                    yield return StartCoroutine(playerAction.Died(target));
                    break;
            }

            if (runningState.WinnerId != null)
            {
                gameController.commandIsDone = true;
                Debug.Log("Game is Finish!");
                PlayerStatus winner = gameController.GetPlayerStatus((int)runningState.WinnerId);
                Debug.Log("Player " + winner.ordering + ": " + winner.teamName + " is the winner!");
                // end game
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

}
