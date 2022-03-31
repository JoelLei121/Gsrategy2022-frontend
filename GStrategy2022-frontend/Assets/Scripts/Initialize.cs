using UnityEngine;
using System.IO;
using System;

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
    public string path = "Assets/Resources/Init.json";
    public GameController gameController;

    void Awake()
    {
        String initialJson = File.ReadAllText(path);
        InitialState state = JsonUtility.FromJson<InitialState>(initialJson);
        //Initialize Map

        //Initialize Players
        gameController.playerNum = state.players.Length;
        gameController.players = new GameObject[gameController.playerNum];

        // Have to create instances, not data only.
        for(int i = 0; i < gameController.playerNum; i++)
        {
            GameObject player = Instantiate<GameObject>(gameController.playerPrefab);
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            playerStatus.init(state.players[i]);
            // is it works?
            gameController.players[i] = player;
            gameController.players[i].transform.Translate(10, 0, 0);
        }
        // foreach (PlayerInitialize p in state.players)
        // {
        //     gameController.players[p.id].set(p);
        // }
        Debug.Log("Players are ready.");
        gameController.isInstantiated = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
