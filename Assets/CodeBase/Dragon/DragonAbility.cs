using System;
using CodeBase.ScriptableObjects.Events;
using CodeBase.ScriptableObjects.GameVariables;
using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using UnityEngine;

[RequireComponent(typeof(SplineFollower))]
public class DragonAbility : MonoBehaviour
{
    [SerializeField] private SplineFollower _splineFollower;

    [SerializeField] private SplineVariable _spline;

    [SerializeField] private GameEvent _onEnemyKilled;

    [SerializeField] private DragonData _data;
    private float _fillHitAttack = 0;

    [SerializeField] private BoolGameEvent _onGameState;

    [SerializeField] private FloatGameEvent _onFillState;

    [SerializeField] private GameEvent _onDragonSpawned;
    [SerializeField] private SplineComputer _splineComputerDragonWay;
    [SerializeField] private SplineFollower _road;
    
    private bool _isActive = false;

    private void Awake() => _splineFollower.spline = _spline.Value;

    private void OnEnable()
    {
        _onEnemyKilled.AddAction(AddHit);
        _onGameState.AddAction(SetActive);
    }

    private void Start()
    {
        _road.gameObject.transform.parent = _spline.Value.transform;
        _road.spline = _spline.Value;
        _road.startPosition = _spline.Value.Travel(0, 10 * (15+6), Spline.Direction.Forward);
        _road.motion.offset = new Vector2(-1,0);
    }

    private void OnDisable()
    {
        _onEnemyKilled.RemoveAction(AddHit);
        _onGameState.RemoveAction(SetActive);
    }
        
    private void SetActive(bool isActive) => _isActive = isActive;

    private void Update()
    {
        if (!_isActive) return;
            //   AutoFilling();
        if (IsAttackReady())
        {
            SpawnDragon();
            _onDragonSpawned.Invoke();
        }
    }

    private void SpawnDragon()
    {
        Instantiate(_data.template,_spline.Value.transform).TryGetComponent(out SplineFollower dragon);
        dragon.spline = _splineComputerDragonWay;
       // dragon.startPosition = _splineFollower.result.percent;
       dragon.startPosition = 0;
        dragon.follow = true;
        _isActive = false;
    //    _splineComputerDragonWay.gameObject.transform.parent = null;
      //  this.InvokeDelegate(() => { Destroy(_splineComputerDragonWay.gameObject); }, 17);
    }

    private bool IsAttackReady()
    {
        if (!(_fillHitAttack >= 1)) return false;
        _fillHitAttack = 0;
        return true;
    }

    private void AutoFilling()
    {
        if (_splineFollower.result.percent < 0.9f)
        {
            _fillHitAttack += Time.deltaTime / 15;
            _onFillState.Invoke(_fillHitAttack);
        }
    }


    private void AddHit()
    {
        _fillHitAttack += 0.085f;
        _onFillState.Invoke(_fillHitAttack);
    } 
}