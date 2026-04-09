using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Chasing,
        Waiting,
        Assaulting
    }

    public State currentState = State.Patrolling;

    [Header("Configuration")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private Transform _player;
    [SerializeField] private float _patrolSpeed = 5f;
    [SerializeField] private float _chaseSpeed = 6f;
    [SerializeField] private float _assaultSpeed = 7f;
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _assaultRange = 3f;
    [SerializeField] private float _waitTime = 2f;

    private int _currentWaypointIndex = 0;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer < _detectionRange)
        {
            //Paramos corrutinas
            StopAllCoroutines();
            currentState = State.Chasing;

            if(distanceToPlayer < _assaultRange)
            {
                currentState = State.Assaulting;
            }
        }
        else if (currentState == State.Chasing)
        {
            currentState = State.Patrolling;
        }

            HandleBehaviour();
    }

    private void Patrol()
    {
        Transform target = _waypoints[_currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, _patrolSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(Wait());       
        }
    }

    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.position, _chaseSpeed * Time.deltaTime);
    }

    private void Assault()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.position, _assaultSpeed * Time.deltaTime);
    }
    
    private IEnumerator Wait()
    {
        currentState = State.Waiting;
        yield return new WaitForSeconds(_waitTime);
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        currentState = State.Patrolling;
    }

    private void HandleBehaviour()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                _renderer.material.color = Color.green;
                break;
            case State.Chasing:
                Chase();
                _renderer.material.color = Color.red;
                break;
            case State.Waiting:
                _renderer.material.color = Color.blue;
                // No hacemos nada, solo esperamos
                break;
            case State.Assaulting:
                Assault();
                _renderer.material.color = Color.black;
                break;
        }
    }
}
