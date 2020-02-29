using UnityEngine;

public class CPlayerState2D_PutEnd : CPlayerState2D
{
    private bool _bIsHit = false;
    private float _currentStateTime = 0f;

    public override void InitState()
    {
        base.InitState();

        _currentStateTime = 0f;
        _bIsHit = false;
        Controller2D.RigidBody2D.isKinematic = false;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (false == _bIsHit && currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 0.3f)
        {
            CPlayerManager.Instance.Stat.Hp -= 1;
            _bIsHit = true;
        }


        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
        {
            _currentStateTime += Time.deltaTime;
        }
        
        if(_currentStateTime >= CPlayerManager.Instance.Stat.PutEndAndChange3DDelay)
        {
            Controller2D.ChangeState(EPlayerState2D.Idle);
            CWorldManager.Instance.ChangeWorld();
        }
    }
}
