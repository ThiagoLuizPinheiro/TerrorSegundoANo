using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [SerializeField]private Transform[] _patrolPoints;
    private int _currentPointIndex;//Ponto atual da patrulha
    public Vector3 GetRandomPoint()
    {
        int randomIndex = Random.Range(0, _patrolPoints.Length);
        return _patrolPoints[randomIndex].position;
    }
    public Vector3 MoveToNextPoint()
    {
        if (_patrolPoints.Length == 0)
            return Vector3.zero;
        Vector3 nextPoint = _patrolPoints[_currentPointIndex].localPosition;
        _currentPointIndex++;
        if (_currentPointIndex >= _patrolPoints.Length)
            _currentPointIndex = 0;//Volta para o primeiro ponto de patrulha
        /*
         Se fosse fazer como o mecanismo de bosster do pokemon tcg
         para montar um esquema de "carrosel", teria que adicionar uma verificação do valor mínimo
        if (_currentPointIndex < 0)
            _currentPointIndex = _patrolPoints.Length - 1;
         */
        return nextPoint;
    }
    //Criar método para retornar o ponto de patrulha mais próximo do inimigo
    //Criar método para retornar um ponto aleatório dentro do NavMesh
}
