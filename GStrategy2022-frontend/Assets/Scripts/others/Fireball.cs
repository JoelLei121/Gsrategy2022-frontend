using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Transform target;
    public float speed = 5f;
    // public int damage = 40;


    public void Seek(Transform _target)
    {
        target = _target;
    }


    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // Damage(target);
        Destroy(gameObject, 1f);
        return;
    }

    // void Damage(Transform enemy)
    // {
    //     Enemy e = enemy.GetComponent<Enemy>();
    //     if(e != null)
    //     {
    //         e.TakeDamage(damage);
    //     }

    // }

}
