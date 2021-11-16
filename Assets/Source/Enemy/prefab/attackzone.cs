using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.EnemyLogic;
using UnityEngine;

public class attackzone : MonoBehaviour
{
    public event Action<IDamagable> _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out IDamagable player))
        {
            _player?.Invoke(player);
        }
    }
}
