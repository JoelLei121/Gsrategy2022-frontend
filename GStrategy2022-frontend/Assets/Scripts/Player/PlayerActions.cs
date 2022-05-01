using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class PlayerActions : MonoBehaviour
{

    public GameUI UI,redUI,blueUI;
    public RecordUI record;
    public OverlayUI overlayUI;
    public AllUI allUI;
    public HexGrid map;
    public float moveSpeed = 0.1f;
    public bool isMoving;
    public bool isAttacking;
    public GameObject LevelUpPrefab;
    private Animator animator;

    private int[] expReduce = { 30, 30, 5, 40, 30, 20 };
    private int[] levelUpData = { 1, 1, 5, 50, 1, 5 };
    private int[] levelUpBoundary = { 100, 100, 30, 100, 100, 50 };
    private float rotationTimer = 0.35f;
    private float runningMargin = 0.05f;
    public float turnSpeed = 20f;
    private float particleLivetime = 3f;
    public float playSpeed = 1f;
    void Start()
    {
        isMoving = false;
        isAttacking = false;
    }

    private void Update()
    {
        moveSpeed = 0.1f * playSpeed;
        turnSpeed = 20f * playSpeed;
        runningMargin = 0.25f * Mathf.Pow(2, playSpeed);
        rotationTimer = 0.5f / playSpeed;
        if(animator != null) animator.SetFloat("playSpeed", playSpeed);
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

        while (counter < rotationTimer)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            player.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            counter += Time.deltaTime;
            yield return null;
        }

        // moving
        animator = player.GetComponent<Animator>();
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
        Debug.Log("Player " + status.id + ": Move to (" + x + ", " + y + ", " + z + ")");
        record.updateFightRecord("Player " + status.id + ": Move to (" + x + ", " + y + ", " + z + ")");
        yield return new WaitForSeconds(0.2f);
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
        animator = player.GetComponent<Animator>();
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        PlayerStatus targetStatus = target.GetComponent<PlayerStatus>();

        record.updateFightRecord("Player " + playerStatus.id + " attack Player " + targetStatus.id);
        Debug.Log("Player " + playerStatus.id + " attack Player " + targetStatus.id);
        yield return new WaitForSeconds(0.5f / playSpeed);
        Vector3 dir = target.transform.position - player.transform.position;
        // rotate y 60
        float turn = 500f;
        float counter = 0f;

        // look at target
        while (counter < rotationTimer)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            player.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            counter += Time.deltaTime;
            yield return null;
        }

        // adjust angle to 60
        counter = 0;
        while (counter < 60f)
        {
            player.transform.Rotate(0f, turn * Time.deltaTime, 0f);
            counter += turn * Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.5f / playSpeed);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(1.2f / playSpeed);

        counter = 0;
        while (counter < rotationTimer)
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
        animator = player.GetComponent<Animator>();
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        status.hp -= atk;
        status.hp = (status.hp > 0 ? status.hp : 0);
        yield return StartCoroutine(overlayUI.updateBloodline(status));
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.1f / playSpeed);
        animator.SetBool("isDamaged", false);
        //Debug.Log("Player " + status.id + " is damaged. HP left: " + status.hp);
        if (status.isDead())
        {
            animator.SetBool("isDamaged", true);
            animator.SetBool("isDying", true);
            StartCoroutine(Died(player));
            yield break;
        }
        record.updateFightRecord("Player " + status.id + " is damaged. HP left: " + status.hp);
        Debug.Log("Player " + status.id + " is damaged. HP left: " + status.hp);
        yield break;
    }

    // Died()
    public IEnumerator Died(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        record.updateFightRecord("Player " + status.id + " is killed.");
        overlayUI.updateBloodline(status);
        Debug.Log("Player " + status.id + " is killed.");

        animator = player.GetComponent<Animator>();
        animator.SetBool("isDamaged", true);
        animator.SetBool("isDying", true);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(DestroyPlayer(player));
        record.updateFightRecord("Player " + (status.id == 1 ? 0 : 1) + " is the winner!");
        yield break;
    }

    // unused
    public IEnumerator Gather(GameObject player, float exp)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        record.updateFightRecord("Player " + status.id + " is gathering. exp + " + exp);
        Debug.Log("Player " + status.id + " is gathering. exp + " + exp);
        status.exp += exp;
        yield return StartCoroutine(UI.updateCurrentPlayer(status, "GATHER"));
        if (status.id == 0)
        {
            yield return StartCoroutine(redUI.updateCurrentPlayer(status, "GATHER"));
            yield return StartCoroutine(allUI.updateRedPlayer(status, "GATHER"));
        }
        else
        {
            yield return StartCoroutine(blueUI.updateCurrentPlayer(status, "GATHER"));
            yield return StartCoroutine(allUI.updateBluePlayer(status, "GATHER"));
        }
        yield return new WaitForSeconds(1f / playSpeed);
        yield break;
    }

    public IEnumerator LevelUp(GameObject player, string upgradeType)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        switch (upgradeType)
        {
            case "move_range":
                status.move_range += levelUpData[0];
                status.exp -= expReduce[0];
                break;

            case "attack_range":
                status.attack_range += levelUpData[1];
                status.exp -= expReduce[1];
                break;

            case "mine_speed":
                status.mine_speed = (status.mine_speed + levelUpData[2] > levelUpBoundary[2] ? levelUpBoundary[2] : status.mine_speed + levelUpData[2]);
                status.exp -= expReduce[2];
                upgradeType = "Gathering_skill";
                break;

            case "hp":
                status.hp = (status.hp + levelUpData[3] > levelUpBoundary[3] ? levelUpBoundary[3] : status.hp + levelUpData[3]);
                status.exp -= expReduce[3];
                break;

            case "sight_range":
                status.sight_range += levelUpData[4];
                status.exp -= expReduce[4];
                break;

            case "atk":
                status.atk = (status.atk + levelUpData[5] > levelUpBoundary[5] ? levelUpBoundary[5] : status.atk + levelUpData[5]);
                status.exp -= expReduce[5];
                break;
        }

        record.updateFightRecord("Player " + status.id + " level up: " + upgradeType.ToUpper() + "!");
        yield return StartCoroutine(UI.updateCurrentPlayer(status, "UPGRADE"));
        if (status.id == 0)
        {
            yield return StartCoroutine(redUI.updateCurrentPlayer(status, "UPGRADE"));
            yield return StartCoroutine(allUI.updateRedPlayer(status, "UPGRADE"));
        }
        else
        {
            yield return StartCoroutine(blueUI.updateCurrentPlayer(status, "UPGRADE"));
            yield return StartCoroutine(allUI.updateBluePlayer(status, "UPGRADE"));
        }

        GameObject particle = Instantiate<GameObject>(LevelUpPrefab);
        particle.transform.position = player.transform.position;
        Destroy(particle, particleLivetime);
        Debug.Log("Player " + status.id + " level up!");
        yield return new WaitForSeconds(1f / playSpeed);
        yield break;
    }
    public IEnumerator DoNothing(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        record.updateFightRecord("Player " + status.id + " are hesitating");
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
    public void SpeedUp(bool flag)
    {
        playSpeed = flag ? 1.5f : 1f;
        return;
    }

}
