using System;
using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using UnityEngine;

public class AddSpeedByEvent : MonoBehaviour
{
 [SerializeField] private GameEvent _addSpeed;
 [SerializeField] private float _speed = 10f;
 private SplineFollower _splineFollower;

 private void Awake() => _splineFollower = GetComponent<SplineFollower>();

 private void Start()
 {
  var offset = _splineFollower.motion.offset;
  _splineFollower.motion.offset = new Vector2(0.3f, offset.y);
 }

 private void OnEnable() => _addSpeed.AddAction(ChangeSpeed);

 private void OnDisable() => _addSpeed.RemoveAction(ChangeSpeed);

 private void ChangeSpeed() => _splineFollower.followSpeed = _speed;
}
