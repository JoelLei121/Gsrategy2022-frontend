using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // public Map map;
    void Start()
    {

    }

    // MoveTo(x, y, z)
    public void MoveTo(PlayerStatus player, int x, int y, int z)
    {
        //go to specify point
        //play moving animation
        Debug.Log("Player " + player.ordering + ": moving to (" + x + ", " + y + ", " + z + ")");
    }

    // Attack(Vector3 direction)
    public void Attack(PlayerStatus player, PlayerStatus[] list)
    {
        //attack all the target could attack 
        Debug.Log("Player " + player.ordering + " is going crazy!");
        foreach (PlayerStatus target in list)
        {
            AttackTo(player, target);
        }
    }
    public void AttackTo(PlayerStatus player, PlayerStatus target)
    {
        //move to front of target
        //MoveTo(0, 0, 0);
        //play attack animation
        Debug.Log("Player " + player.ordering + " attack to Player " + target.ordering);
        Damaged(target, player.atk);
        //go back
        //MoveTo(0, 0, 0);
    }

    // Damaged(Vector3 direction)
    public void Damaged(PlayerStatus player, int atk)
    {
        player.hp -= atk;
        //play damaged animation
        Debug.Log("Player " + player.ordering + " is damaged. HP left: " + player.hp);
    }

    // Died()
    public void Died(PlayerStatus player)
    {
        Debug.Log("Player " + player.ordering + " is killed.");
        // Destroy player's gameobject
    }

    // Grab(Vector3 setPoint)
    public void Gather(PlayerStatus player, float exp)
    {
        Debug.Log("Player " + player.ordering + " is gathering. exp + " + exp);
        // play gathering animation
    }

    public void Upgrade(PlayerStatus player)
    {
        Debug.Log("Player " + player.ordering + "upgrade level!");
    }
    public void DoNothing(PlayerStatus player)
    {
        Debug.Log("Player " + player.ordering + " are hesitating");
    }

}
