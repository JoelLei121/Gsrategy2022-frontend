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
        foreach (GameObject player in gameController.players)
        {
            PlayerStatus redPlayer = gameController.players[0].GetComponent<PlayerStatus>();
            redPlayer.init(state.players[0]);
            PlayerStatus bluePlayer = gameController.players[1].GetComponent<PlayerStatus>();
            bluePlayer.init(state.players[1]);
        }
        Debug.Log("Players are ready.");
    }
}
