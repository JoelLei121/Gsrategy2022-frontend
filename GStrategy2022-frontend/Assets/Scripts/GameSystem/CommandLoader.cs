using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows;



public class CommandLoader : MonoBehaviour
{
    private int index;
    private int gameLength;
    public GameController gameController;
    public PlayerActions playerAction;
    private List<GameState> gameStates;
    private GameState runningState;
    private String gameHistory;
    public Quit quitGame;
    private float waitTime = 1f;
    [DllImport("__Internal")]
    private static extern String ReadGameHistory();

    // private IEnumerator coroutine; 
    void Start()
    {
        index = 0;
    }
    public void GetCommandFromDocument(List<GameState> states)
    {
        Debug.Log("waiting for init");

        gameStates = states;
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
            // Debug.Log("checking boundary " + runningState.MapSize[0]);

            GameObject currentPlayer = gameController.players[runningState.ActivePlayerId];
            gameController.UI.updateCurrentPlayer(currentPlayer.GetComponent<PlayerStatus>(), runningState.CurrentEvent);

            int[] playerPos = currentPlayer.GetComponent<PlayerStatus>().pos;
            PlayerStatus status = gameController.players[runningState.ActivePlayerId].GetComponent<PlayerStatus>();
            gameController.map.setFow(playerPos[0], playerPos[2], status.visibility);
            yield return new WaitForSeconds(waitTime);
            gameController.map.clearState();

            int[] pos;
            switch (runningState.CurrentEvent)
            {
                case "MOVE":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return new WaitForSeconds(waitTime);
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
                    yield return new WaitForSeconds(waitTime);
                    yield return StartCoroutine(playerAction.Attack(currentPlayer, victim));
                    break;

                case "GATHER":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    gameController.map.checkres(pos[0], pos[2]);
                    yield return new WaitForSeconds(waitTime);
                    yield return StartCoroutine(playerAction.Gather(currentPlayer, runningState.Exp));
                    break;

                case "UPGRADE":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return new WaitForSeconds(waitTime);
                    yield return StartCoroutine(playerAction.LevelUp(currentPlayer));
                    break;

                case "ERROR":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return new WaitForSeconds(waitTime);
                    yield return StartCoroutine(playerAction.DoNothing(currentPlayer));
                    break;

                case "DIED":
                    yield return StartCoroutine(playerAction.Died(currentPlayer));
                    break;

                case "ENDGAME":
                    
                    break;
            }

            if (runningState.WinnerId != -1)
            {
                gameController.commandIsDone = true;
                Debug.Log("Game is Finish!");
                if ((int)runningState.WinnerId != 2)
                {
                    PlayerStatus winner = gameController.GetPlayerStatus((int)runningState.WinnerId);
                    Debug.Log("Player " + winner.ordering + ": " + winner.teamName + " is the winner!");
                }
                else
                {
                    Debug.Log("Tie");
                }
                break;
                // end game
            }
            gameController.map.clearState();
            // yield return new WaitForSeconds(0.5f);
        }
        // game is end
        // call function to end scene
        yield return new WaitForSeconds(10f);
        Debug.Log("Quit!");
        StartCoroutine(quitGame.QuitGame());
        yield break;
    }

}
