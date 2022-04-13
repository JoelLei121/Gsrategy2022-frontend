using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Initialize : MonoBehaviour
{
    public GameController gameController;

    void Awake()
    {

    }

    public void Run(InitialState state)
    {
        Debug.Log("waiting for init");
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
    }
}
