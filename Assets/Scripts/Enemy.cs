using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.iOS;

public enum EnemyState
{
    Idle,
    Chasing,
    Patrolling
}
public class Enemy : MonoBehaviour
{
    private PatrolController _patrolController;
    private GameObject _nape;
    private NavMeshAgent _agent;//Responsável por calcular rotas e mover
    private Transform _player;
    private EnemyState _currentState;
    [SerializeField][Range(0.5f, 5)]private float _waitTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        _nape = transform.GetChild(0).gameObject;//Pega o primeiro filho do inimigo, que é o pescoço
        _player = GameController.Instance.PlayerTransform;
        _patrolController = GameController.Instance.PatrolController;
        _agent = GetComponent<NavMeshAgent>();
        yield return new WaitForSeconds(1);
        SetState(EnemyState.Patrolling);//Só para testar
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ativar depois
        Vision();
    }
    public void Vision()
    {
        bool playerInSight = Physics.Linecast(transform.position, _player.position, out RaycastHit hit);
        if (playerInSight)//Não vejo o player
        {
            //Aqui o enemy para
            if (_currentState.Equals(EnemyState.Chasing))//Se eu estiver caçando
            {
                SetState(EnemyState.Idle);//Muda para o modo parado
            }

        }
        else//Aqui eu vejo o player
        {
            //Aqui ele persegue o jogador
            if (_currentState.Equals(EnemyState.Chasing))
                return;
            StopAllCoroutines();//Caso ele esteja esperando para patrulhar, ele para a contagem
            SetState(EnemyState.Chasing);
        }
    }
    public void SetState(EnemyState newState)
    {
        //O primeiro switch é para simular um OnTriggerExit, onde o inimigo para de fazer algo relacionado ao estado anterior
        Vector3 lastPlayerPos = _player.position;//Pegar a última posição vista
        switch (_currentState)
        {
            case EnemyState.Idle:
                
                break;
            case EnemyState.Chasing:
                //Inimigo segue até o último local que ele viu o player
                _agent.SetDestination(lastPlayerPos);
                _nape.SetActive(true);//Ativa a nuca quando ele para de perseguir
                break;
            case EnemyState.Patrolling:
                // Implementar lógica de patrulha aqui
                print("Inimigo parou de patrulhar");
                break;
        }
        _currentState = newState;//Aqui altera o estado do inimigo
        //O segundo switch é para simular um OnTriggerEnter, onde o inimigo começa a fazer algo relacionado ao novo estado
        switch (_currentState)
        {
            case EnemyState.Idle:
                StartCoroutine(Wait());//Inicia a contagem para começar a patrulha
                break;
            case EnemyState.Chasing:
                _nape.SetActive(false);//Desativa a nuca na perseguição
                _agent.SetDestination(_player.position);
                break;
            case EnemyState.Patrolling:
                // Implementar lógica de patrulha aqui
                print("Inimigo começou a patrulhar");
                _agent.SetDestination(_patrolController.MoveToNextPoint());
                StartCoroutine(Patrolling());
                break;
        }
    }
    IEnumerator Wait()
    {
        //Ainda temos que adicionar uma verificação para ver se ele finalizou a última rota
        Debug.LogError("Temporário");
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
        yield return new WaitForSeconds(_waitTime);
        SetState(EnemyState.Patrolling);
    }
    IEnumerator Patrolling()
    {
        //yield return new WaitForSeconds(_waitTime);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
        //Quando chegar aqui, ele terá chego ao ponto de patrulha
        SetState(EnemyState.Idle);
    }
}
