using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthChanger : MonoBehaviour
{
    public int value;

    public void ChangeHP(GameObject target)
    {
        var coinComponent = target.GetComponent<EnemyBase>();
        coinComponent.ChangeHealth(value);

    }

    public void DestroyBullet() {
        Destroy(gameObject);
    }
}


