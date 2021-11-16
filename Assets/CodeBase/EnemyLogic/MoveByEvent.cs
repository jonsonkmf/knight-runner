using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using UnityEngine;


[RequireComponent(typeof(SplineFollower))]
public class MoveByEvent : MonoBehaviour
{
    [SerializeField] private float _speed=5;
    [SerializeField] private GameEvent _onFinishReached;
    private SplineFollower _splineFollower;

    private void Awake() => _splineFollower = GetComponent<SplineFollower>();

    private void OnEnable() => _onFinishReached.AddAction(StartMove);

    private void OnDisable() => _onFinishReached.RemoveAction(StartMove);

    private void StartMove() => _splineFollower.followSpeed = _speed;
}
