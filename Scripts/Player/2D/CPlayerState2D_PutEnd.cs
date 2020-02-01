using UnityEngine;

public class CPlayerState2D_PutEnd : CPlayerState2D
{
    private float _currentStateTime = 0f;

    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Stat.Hp -= 1;
        _currentStateTime = 0f;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
        {
            Controller2D.ChangeState(EPlayerState2D.Idle);
        }

        _currentStateTime += Time.deltaTime;

        if(_currentStateTime >= CPlayerManager.Instance.Stat.PutEndAndChange3DDelay)
            CWorldManager.Instance.ChangeWorld();
    }
}
