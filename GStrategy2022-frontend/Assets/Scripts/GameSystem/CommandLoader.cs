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
        int currentRound = 0;
        while(true)
        {
            if(index >= gameLength)
            {
                gameController.commandIsDone = true;
                Debug.Log("Stop running game");
                break;
            }
            NextCommand();
            
            gameController.map.checkWidth(runningState.MapSize[0]);
            
            GameObject currentPlayer = gameController.players[runningState.ActivePlayerId];
            PlayerStatus status = currentPlayer.GetComponent<PlayerStatus>();
            StartCoroutine(gameController.UI.updateCurrentPlayer(status, runningState.CurrentEvent));

            int[] playerPos = status.pos;

            if(runningState.CurrentEvent == "DIED" || runningState.CurrentEvent == "ENDGAME")
            {
                break;
            }

            if(currentRound != runningState.Round)
            {
                gameController.UI.updateRound(runningState.Round);
                currentRound = runningState.Round;
            }

            if(runningState.CurrentEvent == "BOUNDARYHURT")
            {
                // effect needed
                yield return StartCoroutine(playerAction.Damaged(currentPlayer, runningState.BoundaryHurt));
                StartCoroutine(gameController.UI.updateCurrentPlayer(status, runningState.CurrentEvent));
                yield return new WaitForSeconds(waitTime);
                continue;
            }


            gameController.map.setFow(playerPos[0], playerPos[2], status.move_range);
            yield return new WaitForSeconds(waitTime);
            gameController.map.clearState();

            int[] pos;
            switch (runningState.CurrentEvent)
            {
                case "MOVE":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return StartCoroutine(playerAction.MoveTo(currentPlayer, pos[0], pos[1], pos[2]));
                    break;

                case "ATTACK":
                    GameObject[] victim = new GameObject[runningState.VictimId.Length];
                    for (int i = 0; i < runningState.VictimId.Length; i++)
                    {
                        victim[i] = gameController.players[runningState.VictimId[i]];
                        int[] victimPos = victim[i].GetComponent<PlayerStatus>().pos;
                        gameController.map.highRole(victimPos[0], victimPos[2]);
                        yield return StartCoroutine(playerAction.Attack(currentPlayer, victim));
                    }
                    break;

                case "GATHER":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    gameController.map.checkres(pos[0], pos[2], runningState.MinesLeft);
                    yield return StartCoroutine(playerAction.Gather(currentPlayer, runningState.Exp));
                    break;

                case "UPGRADE":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return StartCoroutine(playerAction.LevelUp(currentPlayer, runningState.UpgradeType));
                    yield return StartCoroutine(gameController.UI.updateCurrentPlayer(currentPlayer.GetComponent<PlayerStatus>(), runningState.CurrentEvent));
                    yield return StartCoroutine(gameController.UI.updateBloodline(currentPlayer.GetComponent<PlayerStatus>()));
                    break;

                case "ERROR":
                    pos = runningState.ActivePos;
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return StartCoroutine(playerAction.DoNothing(currentPlayer));
                    break;
            }

            gameController.map.clearState();
            // yield return new WaitForSeconds(0.5f);
        }
        // game is end
        // call function to end scened

        if (runningState.WinnerId != -1)
        {
            gameController.commandIsDone = true;
            Debug.Log("Game is Finish!");
            if ((int)runningState.WinnerId != 2)
            {
                PlayerStatus winner = gameController.GetPlayerStatus((int)runningState.WinnerId);
                Debug.Log("Player " + winner.id + ": " + winner.teamName + " is the winner!");
            }
            else
            {
                Debug.Log("Tie");
            }
            // end game
        }

        yield return new WaitForSeconds(10f);
        Debug.Log("Quit!");
        StartCoroutine(quitGame.QuitGame());
        yield break;
    }

}
