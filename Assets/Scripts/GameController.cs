using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Wave;

public class GameController : MonoBehaviour
{
    public bool LowHpToggle;
    [Space]
    [Space]
    public int wave = 1;
    public int health = 10;
    public int money = 100;
    [Space]
    [Space]
    public WaveScript[] waveScrips;
    public List<EnemyBase> enemys = new List<EnemyBase>();
    [Space]
    [Space]
    [SerializeField] private Text _valWave;
    [SerializeField] private Text _valHealth;
    [SerializeField] private Text _valMoney;
    [Space]
    [SerializeField] private GameObject _loseWindow;
    [SerializeField] private EnemySpawner _enemySpawner;
    public MessageShow messageShower;

    public event Action OnLose; 

    private void Start()
    {
        // Вставть базовые значения 
        _valWave.text = wave.ToString();
        _valHealth.text = health.ToString();
        _valMoney.text = money.ToString() + " $";
    }

    // Следующая волна
    public void NextWave() {
        wave++;
        _valWave.text = wave.ToString();
    }

    // Проверка попеды если враги закончились
    public void CheckWin() {
        if (enemys.Count == 0)
        {
            NextWave();
            _enemySpawner.WinWave();
        }
    }

    // Изменить деньги
    public bool ChangeMoney(int value) {
        
        if (money + value < 0)
        {
            return false;

        } else {
            if (_valMoney)
            {
                money += value;
                _valMoney.text = money.ToString() + " $";
                return true;
            }
            else {
                return false;
            }
        }
    }    

    // Изменить жизни
    public void ChangeHealth() {
        Debug.Log("HP");
        if (health - 1 <= 0)
        {
            health = 0;
            _valHealth.text = health.ToString();
            GameOver();
        } else {
            health--;
            _valHealth.text = health.ToString();
        }
    }

    // Проигрышь
    public void GameOver() {
        OnLose?.Invoke();
        _loseWindow.SetActive(true);
    }
}
