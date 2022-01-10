using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [Serializable]
    public struct WaveScript
    {
        public EnemyName[] waveScript;
    }

    public enum EnemyName
    {
        Human = 1,
        Fast = 2,
        Power = 3
    }
}
