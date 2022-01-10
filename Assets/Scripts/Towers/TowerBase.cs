using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class TowerBase : MonoBehaviour
{
    // Таймеры для КД
    [SerializeField] protected float coldownAtack;
    [SerializeField] protected float timer;

    // Цена покупки здания
    public int price;

    // Состояние, построено здание или нет
    public bool isBuild;

    // Сфера для слежения вошедших в нее врагов
    [SerializeField] protected SphereCollider AtackRange;

    // Скин здания
    public MeshRenderer skin;
    public Material baseskin;

    // Сфера вокруг здания, визуализация рейнджа атаки
    public GameObject RangeSphere;

    // Анимация катапульты
    protected Animator animationArm;

    // Точка откуда стрелять
    [SerializeField] protected Transform pointAtack;

    // Враги вошедшие в поле зрения пушки
    [SerializeField] protected List<EnemyBase> collisionEnemys = new List<EnemyBase>();

    // Снаряд ы
    [SerializeField] private ProjectileBase projectile;
    protected ProjectileBase pt; // Текущий снаряд

    protected virtual void Awake()
    {
        animationArm = GetComponent<Animator>();
        baseskin = skin.materials[0];
    }
    
    /// <summary>
    /// При установке пушки каждупо КД будет срабатывать стрельба или не будет если не соблюдено услвоие
    /// </summary>

    IEnumerator EnemyChecker() {

        while (true)
        {
            for (int i = collisionEnemys.Count - 1; i >= 0; i--)
            {
                if (collisionEnemys[i] == null)
                {
                    collisionEnemys.Remove(collisionEnemys[i]);
                    continue;
                }
            }

            if (collisionEnemys.Count == 0 || timer > 0)
            {
                timer = timer - Time.deltaTime;
                yield return null;
            } else {
                Atack();
                timer = coldownAtack;
                yield return null;
            }
        }

    }

    /// <summary>
    /// Добавтьб иди убраь из массива врагов
    /// </summary>

    public void AddEnemys(GameObject t)
    {
        EnemyBase enemy = t.GetComponent<EnemyBase>();
        collisionEnemys.Add(enemy);
    }
    public void RemoveEnemys(GameObject t)
    {
        EnemyBase enemy = t.GetComponent<EnemyBase>();
        collisionEnemys.Remove(enemy);
    }


    /// <summary>
    /// Улушает здание
    /// </summary>
    public void UpTower()
    {
        if (GameMannager.Instance.gameController.money - 50 < 0)
        {
            GameMannager.Instance.gameController.messageShower.ShowMessage("Не хватает денег");
        } else {
            GameMannager.Instance.gameController.messageShower.ShowMessage("Скорость атаки увеличина");
            GameMannager.Instance.gameController.ChangeMoney(-50);
            coldownAtack *= 0.8f;
        }
    }

    /// <summary>
    /// Установить здание на землю
    /// </summary>
    public void Build()
    {
        RangeSphere.SetActive(false);
        isBuild = true;
        StartCoroutine(EnemyChecker());
    }
    

    // Запуск анимации атаки
    public virtual void Atack() {
        animationArm.SetTrigger("atack");
    }

    // Запускается снаряд. ( Функция запускается через анимацию)
    public virtual void OnAtack()
    {
        EnemyBase enemy = null;

        int minHealth = -1;
        float minDist = -1;

        foreach (var item in collisionEnemys)
        {
            if (item == null)
            {

                continue;
            }


            if (GameMannager.Instance.gameController.LowHpToggle)
            {
                if (minHealth < 0 || item.health < minHealth)
                {
                    enemy = item;
                    minHealth = enemy.health;
                }
            }
            else
            {
                if (minDist < 0 || Vector3.Distance(item.transform.position, transform.position) < minDist)
                {
                    enemy = item;
                    minDist = Vector3.Distance(item.transform.position, transform.position);

                }
            }
        }

        if (enemy != null)
        {
            pt = Instantiate(projectile, pointAtack.transform.position, Quaternion.identity);
            pt.SetTarget(enemy.transform);
        }
    }

    // Установить материал другого цвета
    public void SetMaterial(Material mat)
    {
        Material newMaterials = new Material(mat); // или просто = skinnedRenderer.materials;
        skin.material = newMaterials;
    }

    // Установить материал базовый
    public void ResetMaterial()
    {
        SetMaterial(baseskin);
    }



}

