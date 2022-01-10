using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Frozzer : MonoBehaviour
{
    [SerializeField] public float timeFrozzen;

    public void Freeze(GameObject target)
    {
        var coinComponent = target.GetComponent<EnemyBase>();
        coinComponent.FrozzenCharacter(timeFrozzen);
    }

    public void SetFreezTime(float val) {
        timeFrozzen = val;
    }

    public void DestroyBullet() {
        Destroy(gameObject);
    }
}


