using UnityEngine;

public class CSoopState3D_Move : CSoopState3D
{
    private void Update()
    {
        Vector3 playerPosition = CPlayerManager.Instance.RootObject3D.transform.position;
        playerPosition.y = transform.position.y;

        // 플레이어 방향으로 이동
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, Controller.Manager.Stat.MoveSpeed * Time.deltaTime);

        if (!Controller.IsDetectionPlayer())
            Controller.ChangeState(ESoopState.Idle);
    }
}
