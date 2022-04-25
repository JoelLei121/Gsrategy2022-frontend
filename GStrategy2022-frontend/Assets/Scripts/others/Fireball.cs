using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    
    private GameObject target;
    public float speed = 10f;
    public float time = 0.8f;
    public int atk;
    private PlayerActions action;
    private float playSpeed = 1f;


    public void Seek(GameObject _target, int _atk, PlayerActions _action)
    {
        action = _action;
        atk = _atk;
        target = _target;
    }


    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }
        playSpeed = action.playSpeed;

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = (speed * playSpeed) * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            target = null;
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target.transform);
    }

    void HitTarget()
    {
        Debug.Log("Hit!");
        StartCoroutine(action.Damaged(target, atk));
        Destroy(gameObject, time / playSpeed);
        return;
    }

}
