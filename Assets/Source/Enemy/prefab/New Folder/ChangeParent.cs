using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    [SerializeField] private SplineFollower _horseSplineFollower;
    [SerializeField] private Animator _animator;


    // Update is called once per frame
    void Update()
    {
        if (_animator.enabled == false)
        {
            transform.SetParent(_horseSplineFollower.spline.gameObject.transform);
        }
    }
}
