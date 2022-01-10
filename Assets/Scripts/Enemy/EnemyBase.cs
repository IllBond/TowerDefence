using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    // Params
    public int health; // Жихни врага
    protected int maxHealth; // Максимальные жизни врага 
    public float _speed = 1; // Скорость пережвижения 

    [SerializeField] private int _price = 100; // За убийство врага столько денег дадут игроку

    // Frozzen 
    public SkinnedMeshRenderer skin; 
    [HideInInspector] public Material baseMaterial;
    public Material frozzenMaterial;
    private Coroutine _frozzenCoroutine;
    
    // Path
    protected Coroutine pathMoveCouratine;
    protected Transform[] paths; // Массив точек запланированного маршрута

    // States
    protected StateMachine stateMachine;
    protected IdleState idleState;
    protected MovingState movingState;
    protected ReactionState frozzenState;
    protected VictoryState victoryState;

    // Links
    [HideInInspector] public Animator animator;
    protected NavMeshAgent navMeshAgent;

    protected GameController gameController;


    #region BaseMethods
    protected virtual void Awake()
    {
        gameController = GameMannager.Instance.gameController;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        baseMaterial = skin.materials[0];
        maxHealth = health;
    }

    // При исчезновении врага срабтает
    protected virtual void OnDestroy()
    {
        EndLifeTime();
    }

    public void EndLifeTime() {
        Unsubscribe();
        if (gameController)
        {
            gameController.enemys.Remove(gameObject.GetComponent<EnemyBase>());
            gameController.ChangeMoney(_price);
            gameController.CheckWin();
        }
    }

    protected virtual void Start()
    {
        Subscribe();
        stateMachine = new StateMachine();

        idleState = new IdleState(this, stateMachine);
        movingState = new MovingState(this, stateMachine);
        frozzenState = new ReactionState(this, stateMachine);
        victoryState = new VictoryState(this, stateMachine);

        stateMachine.Initialize(idleState);

        paths = GameMannager.Instance.pathPoints; // Масив точек взят из singletone обекта GameMannager

        MovingEnemy();
        PathScript();
    }

    // Подписаться / Отписаться на событие проигрыша
    public void Unsubscribe() {
        GameMannager.Instance.gameController.OnLose -= VictoryEnemy;
    }

    public void Subscribe()
    {
        GameMannager.Instance.gameController.OnLose += VictoryEnemy;
    }    
    



    protected virtual void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    protected virtual void Update()
    {
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
    }
    #endregion



    #region Methods states
    public void VictoryEnemy()
    {
        StopMove();
        stateMachine.ChangeState(victoryState);
    }

    public void FrozzenEnemy()
    {
        StopMove();
        stateMachine.ChangeState(frozzenState);
    }

    public void MovingEnemy()
    {
        StartMove();
        stateMachine.ChangeState(movingState);
    }

    public void StayEnemy()
    {
        StopMove();
        stateMachine.ChangeState(idleState);
    }
    #endregion

    #region AnyMehods

    /// <summary>
    /// Изменить жизни
    /// </summary>
    public void ChangeHealth(int val)
    {
        if (health + val <= 0)
        {
            DestroyEnemy();
        }

        if (health + val > maxHealth)
        {
            health = maxHealth;
            return;
        }

        health += val;
    }

    /// <summary>
    /// Скорость перемещения 0
    /// </summary>
    protected void StopMove()
    {
        if (navMeshAgent)
        {
            navMeshAgent.speed = 0;
        }
        
    }

    /// <summary>
    /// Скорость перемещения стандартная
    /// </summary>
    protected void StartMove()
    {
        navMeshAgent.speed = _speed;
    }

    private void PathScript()
    {
        pathMoveCouratine = StartCoroutine(PathCriptCouratine());
    }

    /// <summary>
    /// Идет к первой точке массива, затем ко второй и тд. В конце пути останавливается
    /// Запускается в PathScript() при старте
    /// </summary>
    IEnumerator PathCriptCouratine()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            navMeshAgent.destination = paths[i].position;
            while (Vector3.Distance(transform.position, paths[i].position) > 1f)
                yield return null;

            if (i == paths.Length - 1)
            {
                StopMove();
                StopCoroutine(pathMoveCouratine);
                EndPath();
            }
        }
    }

    /// <summary>
    /// В конце пути срабатывает функция
    /// </summary>
    protected void EndPath() {
        DestroyEnemy();
    }

    protected void DestroyEnemy() {
        Destroy(gameObject);
    }
    #endregion

    /// <summary>
    /// Меняем материал врагу
    /// </summary>
    public void SetMaterial(Material mat)
    {
        Material newMaterials = new Material(mat); // или просто = skinnedRenderer.materials;
        skin.material = newMaterials;
    }

    /// <summary>
    ///Заморозка на определенное к-во секунд
    /// </summary>
    public void FrozzenCharacter(float time) {
        if (_frozzenCoroutine != null)
            StopCoroutine(_frozzenCoroutine);

        _frozzenCoroutine = StartCoroutine(Frozzen(time));
    }

    IEnumerator Frozzen(float time) {
        FrozzenEnemy();
        yield return new WaitForSeconds(time);
        MovingEnemy();
        StopCoroutine(_frozzenCoroutine);
    }
}
