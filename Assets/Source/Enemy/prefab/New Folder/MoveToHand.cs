using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToHand : MonoBehaviour
{
    [SerializeField] private Transform _hand;
    [SerializeField] private Animator _animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.enabled == true)
        {
            transform.position = _hand.position;
        }
    }
}
