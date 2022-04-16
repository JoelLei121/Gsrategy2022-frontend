using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesForPlayer : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform shootPoint;
    public GameObject enemy;
    public PlayerActions action;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void releaseFireball()
    {
        PlayerStatus status = transform.GetComponent<PlayerStatus>();
        Debug.Log("fireball!");
        GameObject fireball = Instantiate<GameObject>(fireballPrefab, shootPoint.position, shootPoint.rotation);
        Fireball fireballScript = fireball.GetComponent<Fireball>();
        fireballScript.Seek(enemy, status.atk, action);
    }
}
