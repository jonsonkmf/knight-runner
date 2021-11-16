using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syncWithHourse : MonoBehaviour
{
    [SerializeField] private Transform _boneTransform;
    private Transform _startTransform;
    private float timer = 0;

    private void Start()
    {
        _startTransform = _boneTransform;
    }

    private void Update()
    {
        Debug.Log(_boneTransform.rotation);
        transform.position = _boneTransform.position;
        /*    if (_boneTransform.rotation.x>0.05 )
            {
                timer += Time.deltaTime / 5;
            }
            else
            {
                timer -= Time.deltaTime / 5;
            }
            transform.rotation = new Quaternion(timer, 0, 0, 1);
        }/**/
    }
}
