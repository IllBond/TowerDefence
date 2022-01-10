using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Wave;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase _enemyHuman;
    [SerializeField] private EnemyBase _enemyPower;
    [SerializeField] private EnemyBase _enemyFast;

    private GameMannager _gameManager;

    [SerializeField] private Transform _pointStart;
    private Coroutine _currentWave;

    private bool _scriptMode; // Спавн врагов по заранее спланированному режиму
    private int _waveInterval = 5; // Задержка перед спавном врагов
    [SerializeField] private int _enemyCount = 3; // Количество врагов за раз

    private void Start()
    {
        _gameManager = GameMannager.Instance;
        StartWave();
    }

    // Запустить новую волну
    private void StartWave() {
        GameMannager.Instance.gameController.messageShower.ShowMessage($"Новая атака через {_waveInterval} секунд");
        _currentWave = StartCoroutine(WaveController());
    }


    IEnumerator WaveController()
    {
        yield return new WaitForSeconds(_waveInterval);

        if (_scriptMode)
        {
            for (int i = 0; i < _gameManager.gameController.waveScrips[_gameManager.gameController.wave-1].waveScript.Length; i++)
            {
                var original = CompareEnemy(_gameManager.gameController.waveScrips[_gameManager.gameController.wave-1].waveScript[i]);
                if (!original) continue;

                var obj = Instantiate(original, _pointStart.transform.position, Quaternion.identity);
                GameMannager.Instance.gameController?.enemys.Add(obj);
                yield return new WaitForSeconds(1);
            }
        }
        else {
            for (int i = 0; i < _enemyCount; i++)
            {
                List<EnemyBase> cha = new List<EnemyBase>() { _enemyHuman, _enemyPower, _enemyFast };
                int rnd = Random.Range(0, cha.Count);

                var obj = Instantiate(cha[rnd], _pointStart.transform.position, Quaternion.identity);
                GameMannager.Instance.gameController?.enemys.Add(obj);
                yield return new WaitForSeconds(1);
            }
        }

        _enemyCount++;
        StopCoroutine(_currentWave);

    }

    // Перебор врагов и поиск правильного
    private EnemyBase CompareEnemy(EnemyName enemyNum) {

        switch (enemyNum)
        {
            case EnemyName.Human:
                return _enemyHuman;
                break;
            case EnemyName.Fast:
                return _enemyFast;
                break;
            case EnemyName.Power:
                return _enemyPower;
                break;
            default: return null;
        }
    }

    // Следующая волна
    public void WinWave() {
        _gameManager.gameController.wave++;
        StartWave();
    }


}
