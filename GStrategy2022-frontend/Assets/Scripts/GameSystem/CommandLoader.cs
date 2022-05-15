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
    public float waitTime = 1f;
    private float playSpeed = 1f;

    // private IEnumerator coroutine; 
    void Start()
    {
        index = 0;
    }

    private void Update()
    {
        waitTime = 1f / playSpeed;
        if(gameController != null) playSpeed = gameController.playSpeed;
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
            //StartCoroutine(gameController.UI.updateCurrentPlayer(status, runningState.CurrentEvent));
            yield return updateAllUI();

            int[] playerPos = status.pos;

            if(runningState.CurrentEvent == "DIED" || runningState.CurrentEvent == "ENDGAME")
            {
                break;
            }

            if(currentRound != runningState.Round)
            {
                gameController.overlayUI.updateRound(runningState.Round);
                gameController.record.updateRound(runningState.Round);
                currentRound = runningState.Round;
            }

            int[] pos = runningState.ActivePos;
            if(runningState.CurrentEvent == "BOUNDARYHURT")
            {
                // effect needed
                gameController.map.highRole(pos[0], pos[2]);
                yield return StartCoroutine(playerAction.Damaged(currentPlayer, runningState.BoundaryHurt));
                yield return updateAllUI();
                yield return new WaitForSeconds(waitTime);
                gameController.map.clearState();
                continue;
            }

            switch (runningState.CurrentEvent)
            {
                case "MOVE":
                    gameController.map.setFow(playerPos[0], playerPos[2], status.move_range);
                    yield return new WaitForSeconds(waitTime/4);
                    // gameController.map.clearState();
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return StartCoroutine(playerAction.MoveTo(currentPlayer, pos[0], pos[1], pos[2]));
                    break;

                case "ATTACK":
                    GameObject[] victim = new GameObject[runningState.VictimId.Length];
                    for (int i = 0; i < runningState.VictimId.Length; i++)
                    {
                        gameController.map.setFow(playerPos[0], playerPos[2], status.move_range);
                        yield return new WaitForSeconds(waitTime/4);
                        // gameController.map.clearState();
                        victim[i] = gameController.players[runningState.VictimId[i]];
                        int[] victimPos = victim[i].GetComponent<PlayerStatus>().pos;
                        gameController.map.highRole(victimPos[0], victimPos[2]);
                        yield return StartCoroutine(playerAction.Attack(currentPlayer, victim));
                    }
                    break;

                case "GATHER":
                    gameController.map.highRole(pos[0], pos[2]);
                    gameController.map.checkres(pos[0], pos[2], runningState.MinesLeft);
                    yield return StartCoroutine(playerAction.Gather(currentPlayer, runningState.Exp));
                    break;

                case "UPGRADE":
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return StartCoroutine(playerAction.LevelUp(currentPlayer, runningState.UpgradeType));
                    yield return StartCoroutine(gameController.overlayUI.updateBloodline(currentPlayer.GetComponent<PlayerStatus>()));
                    updateAllUI();
                    break;

                case "DONOTHING":
                case "ERROR":
                    gameController.map.highRole(pos[0], pos[2]);
                    yield return StartCoroutine(playerAction.DoNothing(currentPlayer));
                    break;
            }

            gameController.map.clearState();
            gameController.map.highRole(pos[0], pos[2]);
            yield return updateAllUI();
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
        StartCoroutine(quitGame.QuitGame(gameController.GetPlayerStatus(0), gameController.GetPlayerStatus(1)));
        yield break;
    }

    IEnumerator updateAllUI()
    {
        GameObject currentPlayer = gameController.players[runningState.ActivePlayerId];
        PlayerStatus status = currentPlayer.GetComponent<PlayerStatus>();
        if (runningState.ActivePlayerId == 0)
        {
            yield return StartCoroutine(gameController.redUI.updateCurrentPlayer(status, runningState.CurrentEvent));
            yield return StartCoroutine(gameController.allUI.updateRedPlayer(status, runningState.CurrentEvent));
            yield return StartCoroutine(gameController.UI.updateRedPlayer(status, runningState.CurrentEvent));
        }
        else
        {
            yield return StartCoroutine(gameController.blueUI.updateCurrentPlayer(status, runningState.CurrentEvent));
            yield return StartCoroutine(gameController.allUI.updateBluePlayer(status, runningState.CurrentEvent));
            yield return StartCoroutine(gameController.UI.updateBluePlayer(status, runningState.CurrentEvent));
        }
        yield break;
    }

}
