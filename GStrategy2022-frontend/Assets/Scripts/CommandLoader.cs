using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class CommandLoader : MonoBehaviour
{
    private int index;
    private string[] Commands;
    private string runningCommand;
    public string path = "Assets/Resources/test.txt";
    public GameController gameController;
    public PlayerAction playerAction;
    void Start()
    {
        index = 0;
    }
    public void GetCommandFromDocument()
    {
        Commands = File.ReadAllLines(path);
        if(Commands != null) Debug.Log("Read successful.");
    }
    void NextCommand()
    {
        // start from 0
        runningCommand = Commands[index];
        index++;
    }

    public void LoadCommand()
    {
        NextCommand();
        char[] seperator = { '-' };
        string[] s = runningCommand.Split(seperator);
        string token = s[0];
        // no checking
        if (token == "game")
        {
            DoGameRunning(s.Skip<string>(1).ToArray<string>());
        }
        else if (token == "player")
        {
            int x = int.Parse(s[1]);
            DoAction(gameController.players[x - 1], s.Skip<string>(2).ToArray<string>());
        }
        else
        {
            Debug.Log("missing command type: " + token);
        }
        return;
    }

    void DoAction(PlayerStatus player, string[] _action)
    {
        string token = _action[0];
        if (token == "move")
        {
            int x = int.Parse(_action[1]), y = int.Parse(_action[2]), z = int.Parse(_action[3]);
            playerAction.MoveTo(player, x, y, z);
            // move to
        }
        else if (token == "attack")
        {
            string[] s = _action.Skip<string>(1).ToArray<string>();
            List<PlayerStatus> enemys = new List<PlayerStatus>();
            foreach(string x in s)
            {
                enemys.Add(gameController.players[int.Parse(x)-1]);
            }
            playerAction.Attack(player, enemys.ToArray());
            // attack
        }
        else if (token == "gather")
        {
            playerAction.Gather(player, float.Parse(_action[1]));
            // gathering
        }
        else if(token == "died")
        {
            playerAction.Died(player);
        }
        else if (token == "ERROR")
        {
            playerAction.DoNothing(player);
            // do nothing
        }
        else
        {
            Debug.Log("missing action: " + token);
        }
        return;
    }

    void DoGameRunning(string[] _command)
    {
        if(_command[0] == "start")
        {
            Debug.Log("Game started!");
            // run game
        }
        else if(_command[0] == "init")
        {
            string[] s = _command.Skip<string>(1).ToArray<string>();
            for(int i = 0; i < 4; i++)
            {
                gameController.players[i].teamName = s[i];
                gameController.players[i].ordering = i;
            }
        }
        else if(_command[0] == "end")
        {
            gameController.commandIsDone = true;
            Debug.Log("Game is Finish!");
            PlayerStatus winner = gameController.players[int.Parse(_command[1])-1];
            Debug.Log("Player " + winner.ordering + ": " + winner.teamName + " is the winner!");
            // end game
        }
    }

}
