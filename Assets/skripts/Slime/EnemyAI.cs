using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using KnigtAdventure.Utils;
using UnityEditor.Experimental.GraphView;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;
    
    [SerializeField] private bool _isChasingEnemy = false;

    [SerializeField] private float _chasingDistance=4f;
    [SerializeField] private float _chasingSpeedMultiplayer = 2f;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _attackRate = 2f;
    private float _nextAttackTime = 0;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    public event EventHandler OnEnemyAttack;
    private enum State
    {

        Roaming,
        Chasing,
        Attacking,
        Death
    }


    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed* _chasingSpeedMultiplayer;
    }
    private void Update()
    {

        StateHandler();
    }
    private void StateHandler()
    {
        switch (_currentState)
        {
            default:

            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                 CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                break;
            case State.Death:
                break;
        }
    }
    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }
    
    private void CheckCurrentState()
    {
        float distanceToPLayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;
        if (_isChasingEnemy)
        {
            if (distanceToPLayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }
        if (_isAttackingEnemy)
        {
            if (distanceToPLayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }

            if (newState != _currentState)
            {
                if (newState == State.Chasing)
                {
                    _navMeshAgent.ResetPath();
                    _navMeshAgent.speed = _chasingSpeed;
                }
                else if (newState == State.Roaming)
                {
                    _roamingTimer = 0f;
                    _navMeshAgent.speed = _roamingSpeed;
                }
                else if (newState == State.Attacking)
                {
                    _navMeshAgent.ResetPath();
                }
                _currentState = newState;
            }

        }
    }
    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {

            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }
    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        changeFacingDirection(_startingPosition, _roamPosition);
        _navMeshAgent.SetDestination(_roamPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }
    private void changeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}


 