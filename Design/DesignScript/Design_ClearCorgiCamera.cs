using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ClearCorgiCamera : MonoBehaviour
{
    [SerializeField]
    private Animator CorgiAnimator = null;
    public void SetFinishAnim()
    {
        var CurStarCount = CBiscuitManager.Instance.GetCurrentStar();
        CorgiAnimator.SetInteger("ClearStarCount", CurStarCount);
    }
}
