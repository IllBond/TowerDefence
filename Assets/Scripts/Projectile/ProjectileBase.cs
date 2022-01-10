using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected Transform target;
    private bool _start;

    [SerializeField] protected float speed;


    public void SetTarget(Transform t) {
        target = t;
        _start = true;
    }

    void Update()
    {
        if (_start)
        {
            if (!target) {
                Destroy(gameObject);
                return;
            }

            transform.Translate(Vector3.Normalize(target.position - transform.position) * speed * Time.deltaTime);
        }
    }

}
