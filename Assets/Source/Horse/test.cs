using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.EnemyLogic;
using Dreamteck.Splines;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private SplineFollower _horse;
    private SplineFollower _me;
    public event Action<float> SetSpearRotationX;
    void Start()
    {
        _me = GetComponent<SplineFollower>();
        _me.startPosition = _horse.startPosition + 0.01;
        _me.spline = _horse.spline;
    }

    void Update()
    {
        _me.followSpeed = _horse.followSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log("Триггер попал в энеми");
            SetSpearRotationX?.Invoke(enemy.XoffsetSpear);
        }
    }
}
