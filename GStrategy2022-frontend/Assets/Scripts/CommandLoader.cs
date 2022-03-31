using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;


[Serializable]
enum GameEvent
{
    MOVE, ATTACK, GATHER, DIED, ERROR
}

[Serializable]
class GameState
{
    public int Round = 0;
    public GameEvent CurrentEvent = GameEvent.MOVE;
    public int ActivePlayerId = 0;
    public int[] VictimId = { 0 };
    public int[] ActivePos = { 0, 0, 0 };
    public int? WinnerId = null;
    public float? exp = null;

    public int[] MapSize = { 10, 10, 10 };

}

public class CommandLoader : MonoBehaviour
{
    private int index;
    public string path = "Assets/Resources/Play.json";
    public GameController gameController;
    public PlayerAction playerAction;
    private GameState[] gameStates;
    private GameState runningState;
    void Start()
    {
        index = 0;
    }
    public void GetCommandFromDocument()
    {
        String gameStateJson = File.ReadAllText(path);
        gameStates = JsonUtility.FromJson<GameState[]>(gameStateJson);
        if (gameStates != null) 
            Debug.Log("Game process read successfully.");
    }
    void NextCommand()
    {
        // start from 0
        runningState = gameStates[index];
        index++;
    }

    public void testCommand()
    {

    }
    public void LoadCommand()
    {
        NextCommand();
        // PlayerStatus target = gameController.GetPlayerStatus(runningState.ActivePlayerId);
        GameObject target = gameController.players[runningState.ActivePlayerId];
        int[] pos = runningState.ActivePos;

        switch (runningState.CurrentEvent)
        {
            case GameEvent.MOVE:

                playerAction.MoveTo(target, pos[0], pos[1], pos[2]);
                break;

            case GameEvent.ATTACK:
                GameObject[] victim = new GameObject[runningState.VictimId.Length];
                for(int i = 0; i < runningState.VictimId.Length; i++)
                {
                    victim[i] = gameController.players[runningState.VictimId[i]];
                }
                playerAction.Attack(target, victim);
                break;
            case GameEvent.GATHER:
                playerAction.Gather(target, (float)runningState.exp);
                break;
            case GameEvent.ERROR:
                playerAction.DoNothing(target);
                break;
            case GameEvent.DIED:
                playerAction.Died(target);
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
        return;
    }

}
