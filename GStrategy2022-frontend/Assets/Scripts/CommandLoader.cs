using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;

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
    public string path = "Assets/Resources/Play.json";
    // private IEnumerator coroutine; 
    void Start()
    {
        index = 0;
    }
    public void GetCommandFromDocument()
    {
        String gameStateJson = File.ReadAllText(path);
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
            gameController.map.highRole(pos[0], pos[2]);
            yield return new WaitForSeconds(2f);
            PlayerStatus status = gameController.players[runningState.ActivePlayerId].GetComponent<PlayerStatus>();
            gameController.map.setFow(pos[0], pos[2], status.visibility);
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
