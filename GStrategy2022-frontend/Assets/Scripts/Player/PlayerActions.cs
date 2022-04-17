using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class PlayerActions : MonoBehaviour
{

    public GameUI UI;
    public HexGrid map;
    public float moveSpeed = 0.1f;
    public bool isMoving;
    public bool isAttacking;
    public GameObject LevelUpPrefab;

    private int[] expReduce = {30, 30, 5, 40, 20, 30};
    float rotationMargin = 0.7f;
    float runningMargin = 0.5f;
    float turnSpeed = 10f;
    void Start()
    {
        isMoving = false;
        isAttacking = false;
    }

    bool isArrived(Vector3 start, Vector3 end)
    {
        Vector3 a = new Vector3(start.x, 0, start.z);
        Vector3 b = new Vector3(end.x, 0, end.z);
        return Vector3.Distance(a, b) < runningMargin;
    }

    // MoveTo(x, y, z)
    public IEnumerator MoveTo(GameObject player, int x, int y, int z)
    {
        //go to specify point
        //play moving animation
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = map.GetUnitPosition(x, z);

        // turning to target
        Vector3 dir = endPosition - startPosition;

        float counter = 0f;

        while (counter < rotationMargin)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            player.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            counter += Time.deltaTime;
            yield return null;
            if (Mathf.Abs(player.transform.rotation.y - lookRotation.y) < rotationMargin) break;
        }

        // moving
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("isRunning", true);
        // adjust animation
        while (true)
        {
            // animation
            startPosition = player.transform.position;
            dir = endPosition - startPosition;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            player.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            if (isArrived(startPosition, endPosition)) break;
            yield return null;
        }
        animator.SetBool("isRunning", false);

        // update position
        status.pos = new int[] { x, y, z };
        Debug.Log("Player " + status.id + ": moving to (" + x + ", " + y + ", " + z + ")");
        UI.updateFightRecord("Player " + status.id + ": moving to (" + x + ", " + y + ", " + z + ")");
        yield return new WaitForSeconds(0.5f);
        yield break;
    }

    // Attack(Vector3 direction)
    public IEnumerator Attack(GameObject player, GameObject[] victims)
    {
        //attack all the target could attack 
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        //UI.updateFightRecord("Player " + status.id + " is going crazy!");
        //Debug.Log("Player " + status.id + " is going crazy!");
        foreach (GameObject target in victims)
        {
            yield return StartCoroutine(AttackTo(player, target));
        }
        yield break;
    }
    public IEnumerator AttackTo(GameObject player, GameObject target)
    {
        //play attacking animation
        Animator animator = player.GetComponent<Animator>();
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        PlayerStatus targetStatus = target.GetComponent<PlayerStatus>();

        UI.updateFightRecord("Player " + playerStatus.id + " attack Player " + targetStatus.id);
        Debug.Log("Player " + playerStatus.id + " attack Player " + targetStatus.id);
        yield return new WaitForSeconds(0.5f);
        Vector3 dir = target.transform.position - player.transform.position;
        // rotate y 60
        float turn = 500f;
        float counter = 0f;

        // look at target
        while (counter < rotationMargin)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            player.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            counter += Time.deltaTime;
            yield return null;
        }

        // adjust
        counter = 0;
        while (counter < 60f)
        {
            player.transform.Rotate(0f, turn * Time.deltaTime, 0f);
            counter += turn * Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(1.2f);

        counter = 0;
        while (counter < rotationMargin)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, Time.deltaTime * turnSpeed * 0.5f).eulerAngles;
            player.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            counter += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.8f);
        yield break;
    }

    // Damaged(Vector3 direction)
    public IEnumerator Damaged(GameObject player, int atk)
    {
        Animator animator = player.GetComponent<Animator>();
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        status.hp -= atk;
        status.hp = (status.hp > 0 ? status.hp : 0);
        yield return StartCoroutine(UI.updateBloodline(status));
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isDamaged", false);
        //Debug.Log("Player " + status.id + " is damaged. HP left: " + status.hp);
        if (status.isDead())
        {
            animator.SetBool("isDamaged", true);
            animator.SetBool("isDying", true);
            StartCoroutine(Died(player));
            yield break;
        }
        //yield return new WaitForSeconds(1f);
        //animator.SetBool("isDamaged", false);
        UI.updateFightRecord("Player " + status.id + " is damaged. HP left: " + status.hp);
        Debug.Log("Player " + status.id + " is damaged. HP left: " + status.hp);
        yield break;
    }

    // Died()
    public IEnumerator Died(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        UI.updateFightRecord("Player " + status.id + " is killed.");
        UI.updateBloodline(status);
        Debug.Log("Player " + status.id + " is killed.");

        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("isDamaged", true);
        animator.SetBool("isDying", true);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(DestroyPlayer(player));
        UI.updateFightRecord("Player " + (status.id == 1 ? 0 : 1) + " is the winner!");
        yield break;
    }

    // Grab(Vector3 setPoint)
    public IEnumerator Gather(GameObject player, float exp)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        UI.updateFightRecord("Player " + status.id + " is gathering. exp + " + exp);
        Debug.Log("Player " + status.id + " is gathering. exp + " + exp);
        status.exp += exp;
        UI.updateCurrentPlayer(status, "GATHER");
        yield return new WaitForSeconds(1f);
        yield break;
    }

    public IEnumerator LevelUp(GameObject player, string upgradeType)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        switch (upgradeType)
        {
            case "move_range":
                status.move_range++;
                status.exp -= expReduce[0];
                break;

            case "attack_range":
                status.attack_range++;
                status.exp -= expReduce[1];
                break;

            case "mine_speed":
                status.mine_speed++;
                status.exp -= expReduce[2];
                break;

            case "hp":
                status.hp = (status.hp + 50 > 100 ? 100 : status.hp + 50);
                status.exp -= expReduce[3];
                break;

            case "sight_range":
                status.sight_range++;
                status.exp -= expReduce[4];
                break;

            case "atk":
                status.atk++;
                status.exp -= expReduce[5];
                break;
        }

        GameObject particle = Instantiate<GameObject>(LevelUpPrefab);
        particle.transform.position = player.transform.position;
        Destroy(particle, 3f);
        UI.updateFightRecord("Player " + status.id + " level up!");
        UI.updateCurrentPlayer(status, "UPGRADE");
        Debug.Log("Player " + status.id + " level up!");
        yield return new WaitForSeconds(1f);
        yield break;
    }
    public IEnumerator DoNothing(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        UI.updateFightRecord("Player " + status.id + " are hesitating");
        Debug.Log("Player " + status.id + " are hesitating");
        yield break;
    }

    IEnumerator DestroyPlayer(GameObject player)
    {
        // play destroy animation
        yield return new WaitForSeconds(5f);
        player.SetActive(false);
        // destroy gameObject
        yield break;
    }

}
