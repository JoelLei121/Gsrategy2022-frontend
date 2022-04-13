using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesForPlayer : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform shootPoint;
    public Transform enemyTransform;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void releaseFireball()
    {
        Debug.Log("fireball!");
        GameObject fireball = Instantiate<GameObject>(fireballPrefab, shootPoint.position, shootPoint.rotation);
        Fireball fireballScript = fireball.GetComponent<Fireball>();
        fireballScript?.Seek(enemyTransform);
    }
}
