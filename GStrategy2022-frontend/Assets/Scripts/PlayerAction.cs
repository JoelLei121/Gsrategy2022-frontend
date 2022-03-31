using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerAction : MonoBehaviour
{
    // public Map map;
    void Start()
    {

    }

    // MoveTo(x, y, z)
    public async void MoveTo(GameObject player, int x, int y, int z)
    {
        //go to specify point
        //play moving animation
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        await Task.Run(()=>{
            // moving
            // animation
        });
        Debug.Log("Player " + status.id + ": moving to (" + x + ", " + y + ", " + z + ")");
    }

    // Attack(Vector3 direction)
    public void Attack(GameObject player, GameObject[] victims)
    {
        //attack all the target could attack 
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " is going crazy!");
        foreach (GameObject target in victims)
        {
            AttackTo(player, target);
        }
    }
    public void AttackTo(GameObject player, GameObject target)
    {
        //move to front of target
        //MoveTo(0, 0, 0);
        //play attack animation
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        PlayerStatus targetStatus = target.GetComponent<PlayerStatus>();

        Debug.Log("Player " + playerStatus.id + " attack to Player " + targetStatus.id);
        Damaged(target, playerStatus.atk);
        //go back
        //MoveTo(0, 0, 0);
    }

    // Damaged(Vector3 direction)
    public void Damaged(GameObject player, int atk)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        status.hp -= atk;
        //play damaged animation
        Debug.Log("Player " + status.id + " is damaged. HP left: " + status.hp);
    }

    // Died()
    public void Died(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " is killed.");
        // Destroy player's gameobject
    }

    // Grab(Vector3 setPoint)
    public void Gather(GameObject player, float exp)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " is gathering. exp + " + exp);
        // play gathering animation
    }

    public void LevelUp(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + "upgrade level!");
    }
    public void DoNothing(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " are hesitating");
    }

}
