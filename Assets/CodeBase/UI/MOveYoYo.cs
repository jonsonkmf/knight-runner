using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MOveYoYo : MonoBehaviour
{
   
    
    void Start()
    {
        TweenParams tParms = new TweenParams().SetLoops(-1, LoopType.Yoyo);
        // Apply them to a couple of tweens
        transform.DOLocalMoveX(-200, 3).From(250).SetAs(tParms);
    
        //transform.DOLocalMoveX(-200, 4).SetLoops(-1, LoopType.Yoyo);
    }
}
