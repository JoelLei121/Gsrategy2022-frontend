using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerAction : MonoBehaviour
{
    public HexGrid map;
    public float moveSpeed = 0.1f;
    public bool isMoving;
    public bool isAttacking;
    float margin = 0.5f;
    void Start()
    {
        isMoving = false;
        isAttacking = false;
    }

    bool isArrived(Vector3 start, Vector3 end)
    {
        if(Mathf.Abs(start.x - end.x) <= margin && Mathf.Abs(start.z - end.z) <= margin)
        {
            return true;
        }
        return false;
    }

    // MoveTo(x, y, z)
    public IEnumerator MoveTo(GameObject player, int x, int y, int z)
    {
        //go to specify point
        //play moving animation
        float turnSpeed = 10f;
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = map.GetUnitPosition(x, z);

        // turning to target
        Vector3 dir = endPosition - startPosition;
        Debug.Log("facing");
        while(true)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            player.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            yield return null;
            if(Mathf.Abs(player.transform.rotation.y - lookRotation.y) < 0.05f) break;
        }

        // moving
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("isRunning", true);
        // adjust animation
        while(true)
        {
            // animation
            startPosition = player.transform.position;
            if(isArrived(startPosition, endPosition)) break;
            yield return null;
        }
        animator.SetBool("isRunning", false);

        // update position
        status.pos = new int[] {x, y, z};
        Debug.Log("Player " + status.id + ": moving to (" + x + ", " + y + ", " + z + ")");
        yield return new WaitForSeconds(3f);
        yield break;
    }

    // Attack(Vector3 direction)
    public IEnumerator Attack(GameObject player, GameObject[] victims)
    {
        //attack all the target could attack 
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " is going crazy!");
        foreach (GameObject target in victims)
        {
            yield return StartCoroutine(AttackTo(player, target));
        }
        yield break;
    }
    public IEnumerator AttackTo(GameObject player, GameObject target)
    {
        //move to front of target
        //MoveTo(0, 0, 0);
        //play attacking animation
        Animator animator = player.GetComponent<Animator>();
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        PlayerStatus targetStatus = target.GetComponent<PlayerStatus>();

        Debug.Log("Player " + playerStatus.id + " attack Player " + targetStatus.id);
        yield return new WaitForSeconds(0.5f);

        player.transform.Rotate(0f, 30f, 0f);
        animator.SetBool("isAttacking", true);
        while(true)
        {
            // do some animation
            yield return null;
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Cross Punch"))
            {
                animator.SetBool("isAttacking", false);
                break;
            }
        }
        //go back
        player.transform.Rotate(0f, -30f, 0f);
        // play too early
        yield return StartCoroutine(Damaged(target, playerStatus.atk));
        yield return new WaitForSeconds(0.5f);
        //MoveTo(0, 0, 0);
        yield break;
    }

    // Damaged(Vector3 direction)
    public IEnumerator Damaged(GameObject player, int atk)
    {
        Animator animator = player.GetComponent<Animator>();
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        status.hp -= atk;
        animator.SetBool("isDamaged", true);
        while(true)
        {
            // do some animation
            yield return null;
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            {
                animator.SetBool("isDamaged", false);
                break;
            }
        }
        Debug.Log("Player " + status.id + " is damaged. HP left: " + status.hp);
        yield return new WaitForSeconds(2f);
        if(status.hp <= 0)
        {
            StartCoroutine(Died(player));
        }
        yield break;
    }

    // Died()
    public IEnumerator Died(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " is killed.");
        while(true)
        {
            //play die animation
            yield return null;
            if(true) break;
        }
        
        // Destroy player's gameobject?
        yield break;
    }

    // Grab(Vector3 setPoint)
    public IEnumerator Gather(GameObject player, float exp)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " is gathering. exp + " + exp);
        while(true)
        {
            // play gathering animation
            yield return null;
            if(true) break;
        }
        yield break;
    }

    public IEnumerator LevelUp(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + "upgrade level!");
        yield break;
    }
    public IEnumerator DoNothing(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Debug.Log("Player " + status.id + " are hesitating");
        yield break;
    }

    IEnumerator DestroyPlayer(GameObject player)
    {
        // play destroy animation
        yield return new WaitForSeconds(3f);
        // destroy gameObject
        yield break;
    }

}
