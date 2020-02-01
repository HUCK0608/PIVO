using UnityEngine;

public class CSoopState3D_Surprise : CSoopState3D
{
    [SerializeField]
    private GameObject _surpriseObject = null;

    public override void InitState()
    {
        base.InitState();

        Controller3D.LookDirection((CPlayerManager.Instance.RootObject3D.transform.position - transform.position).normalized);
        _surpriseObject.SetActive(true);

        CPlayerManager.Instance.IsOnSoopDetection = true;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("Surprise") && currentAnimatorStateInfo.normalizedTime >= 1f)
        {
            if (Controller3D.IsDetectionPlayer())
                Controller3D.ChangeState(ESoopState.Chase);
            else
            {
                Controller3D.ChangeState(ESoopState.Idle);
                CPlayerManager.Instance.IsOnSoopDetection = false;
            }
        }
    }

    public override void EndState()
    {
        base.EndState();
        _surpriseObject.SetActive(false);
    }
}
