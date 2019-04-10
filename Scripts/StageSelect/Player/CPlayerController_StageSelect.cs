using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerController_StageSelect : MonoBehaviour
{
    /// <summary>현재 스테이지</summary>
    [SerializeField]
    private CStage _currentStage = null;

    /// <summary>스텟</summary>
    private CPlayerStat_StageSelect _stat = null;

    /// <summary>애니메이터</summary>
    private Animator _animator;

    /// <summary>Idle상태에서 기본 오일러 회전값</summary>
    private Vector3 _idleEulerRotation = new Vector3(0, 180f, 0);

    private void Awake()
    {
        _stat = GetComponent<CPlayerStat_StageSelect>();

        _animator = GetComponentInChildren<Animator>();

        StartCoroutine(IdleLogic());
    }

    /// <summary>대기 로직</summary>
    private IEnumerator IdleLogic()
    {
        transform.eulerAngles = _idleEulerRotation;

        CStage nextStage = null;

        while(true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Right);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Up);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Down);

            if (nextStage != null)
                break;

            yield return null;
        }

        _currentStage = nextStage;

        StartCoroutine(MoveLogic());
    }

    /// <summary>이동 로직</summary>
    private IEnumerator MoveLogic()
    {
        // 목적지
        Vector3 destination = _currentStage.transform.position;
        destination.y = transform.position.y;

        // 회전
        Vector3 direction = destination - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        _animator.SetBool("IsMove", true);
        
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, _stat.MoveSpeed * Time.deltaTime);

            if (transform.position.Equals(destination))
                break;

            yield return null;
        }

        _animator.SetBool("IsMove", false);

        StartCoroutine(IdleLogic());
    }
}
