using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFrozzenCatapult : TowerBase
{
    [SerializeField] private float _freezeTime;

    public override void OnAtack()
    {
        base.OnAtack();

        if (pt == null || pt.GetComponent<Frozzer>() == null)
        {
            return;
        }

        pt.GetComponent<Frozzer>().SetFreezTime(_freezeTime);

    }
}