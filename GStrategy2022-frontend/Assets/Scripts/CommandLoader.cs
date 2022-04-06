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
    public Quit quitGame;
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
        dialog.initialDir = "C:";  //Ĭ��·��
        dialog.title = "Choose game history file";
        dialog.defExt = "json";//��ʾ�ļ�������
        //ע��һ����Ŀ��һ��Ҫȫѡ ����0x00000008�Ҫȱ��
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        while (true)
        {
            if (!DialogShow.GetOpenFileName(dialog))
            {
                Messagebox.MessageBox(IntPtr.Zero, "Choose correct game file��", "Warning", 0);
                continue;
            }
            else if (!dialog.file.Contains(".json"))
            {
                Messagebox.MessageBox(IntPtr.Zero, "Choose correct game file��", "Warning", 0);
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
                break;
            }
            NextCommand();
            gameController.UI.updateRound(runningState.Round);
            // PlayerStatus target = gameController.GetPlayerStatus(runningState.ActivePlayerId);
            gameController.map.checkWidth(runningState.MapSize[0]);
            Debug.Log("checking boundary " + runningState.MapSize[0]);
            GameObject currentPlayer = gameController.players[runningState.ActivePlayerId];
            gameController.UI.updateCurrentPlayer(currentPlayer.GetComponent<PlayerStatus>(), runningState.CurrentEvent);
            int[] playerPos = currentPlayer.GetComponent<PlayerStatus>().pos;
            PlayerStatus status = gameController.players[runningState.ActivePlayerId].GetComponent<PlayerStatus>();
            gameController.map.setFow(playerPos[0], playerPos[2], status.visibility);
            yield return new WaitForSeconds(1f);
            gameController.map.clearState();

            int[] pos;
            switch (runningState.CurrentEvent)
            {
                case "MOVE":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return new WaitForSeconds(1f);
                    yield return StartCoroutine(playerAction.MoveTo(currentPlayer, pos[0], pos[1], pos[2]));
                    break;

                case "ATTACK":
                    GameObject[] victim = new GameObject[runningState.VictimId.Length];
                    for (int i = 0; i < runningState.VictimId.Length; i++)
                    {
                        victim[i] = gameController.players[runningState.VictimId[i]];
                        int[] victimPos = victim[i].GetComponent<PlayerStatus>().pos;
                        gameController.map.highRole(victimPos[0], victimPos[2]);
                    }
                    yield return new WaitForSeconds(1f);
                    yield return StartCoroutine(playerAction.Attack(currentPlayer, victim));
                    break;

                case "GATHER":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return new WaitForSeconds(1f);
                    yield return StartCoroutine(playerAction.Gather(currentPlayer, runningState.exp));
                    break;

                case "ERROR":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return new WaitForSeconds(1f);
                    yield return StartCoroutine(playerAction.DoNothing(currentPlayer));
                    break;

                case "DIED":
                    yield return StartCoroutine(playerAction.Died(currentPlayer));
                    break;
            }

            if (runningState.WinnerId != null)
            {
                gameController.commandIsDone = true;
                Debug.Log("Game is Finish!");
                PlayerStatus winner = gameController.GetPlayerStatus((int)runningState.WinnerId);
                Debug.Log("Player " + winner.ordering + ": " + winner.teamName + " is the winner!");
                break;
                // end game
            }
            gameController.map.clearState();
            yield return new WaitForSeconds(0.5f);
        }
        // game is end
        // call function to end scene
        yield return new WaitForSeconds(10f);
        Debug.Log("Quit!");
        StartCoroutine(quitGame.QuitGame());
        yield break;
    }

}
