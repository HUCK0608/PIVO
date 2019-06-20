using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;

public class CUIManager_Title : MonoBehaviour
{
    /// <summary>애니메이터</summary>
    private Animator _animator = null;

    /// <summary>기본 메뉴 및 글로우 메뉴</summary>
    [SerializeField]
    private GameObject[] _defaultMenu = null, _selectMenu = null;

    /// <summary>인트로 Playable Director</summary>
    [SerializeField]
    private PlayableDirector _introPlayerDirector = null;

    /// <summary>현재 선택하고 있는 메뉴</summary>
    private int _currentSelect = 0;
    /// <summary>최대 메뉴 개수</summary>
    private int _maxMenuCount = 0;

    /// <summary>무언가를 실행하고 있는지 여부</summary>
    private bool _isExcutionAnything = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _maxMenuCount = _defaultMenu.Length;
    }

    private void Start()
    {
        // 플레이어 조작 막기
        CPlayerManager.Instance.IsCanOperation = false;

        // 메인카메라와 메인 UI의 목표 디스플레이 변경(화면에서 안보이게)
        CCameraController.Instance.SetTargetDisplay(1);
        CUIManager.Instance.SetTargetDisplay(1);

        // 모든 오브젝트를 2D 상태로 변경
        CWorldManager.Instance.AllObjectsCanChange2D();
        CWorldManager.Instance.ChangeWorld();

        StartCoroutine(SelectMenuInputLogic());
    }

    /// <summary>메뉴 선택 로직</summary>
    private IEnumerator SelectMenuInputLogic()
    {
        ChangeSelectMenu(_currentSelect);

        while(!_isExcutionAnything)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ChangeSelectMenu(Mathf.Clamp(_currentSelect - 1, 0, _maxMenuCount - 1));
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                ChangeSelectMenu(Mathf.Clamp(_currentSelect + 1, 0, _maxMenuCount - 1));
            else if (Input.GetKeyDown(KeyCode.Space))
                ExcutionSelectMenu(_currentSelect);

            yield return null;
        }
    }

    /// <summary>선택 메뉴 변경</summary>
    public void ChangeSelectMenu(int selectMenu)
    {
        if (_isExcutionAnything)
            return;

        _defaultMenu[_currentSelect].SetActive(true);
        _selectMenu[_currentSelect].SetActive(false);
        _currentSelect = selectMenu;
        _defaultMenu[_currentSelect].SetActive(false);
        _selectMenu[_currentSelect].SetActive(true);
    }

    /// <summary>선택 메뉴 실행</summary>
    public void ExcutionSelectMenu(int selectMenu)
    {
        if (_isExcutionAnything)
            return;

        _isExcutionAnything = true;

        switch (selectMenu)
        {
            case 0:
                _animator.SetBool("IsFadeOut", true);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    
    /// <summary>인트로 타임라인 시작</summary>
    public void StartIntroTimeline()
    {
        _introPlayerDirector.Play();

        StartCoroutine(IntroTimeLineEndCheck());
    }

    /// <summary>인트로 타임라인 끝남 체크</summary>
    private IEnumerator IntroTimeLineEndCheck()
    {
        yield return new WaitUntil(() => _introPlayerDirector.time >= 35f);
        
        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.DownIdle);
        CCameraController.Instance.IsFollowTarget = true;
        CPlayerManager.Instance.Controller2D.IsUseGravity = false;
        Vector3 startPosition = CPlayerManager.Instance.Controller2D.transform.position;
        startPosition.x = -46.72f;
        startPosition.y = 1f;
        CPlayerManager.Instance.Controller2D.transform.position = startPosition;
        _introPlayerDirector.gameObject.SetActive(false);

        CCameraController.Instance.SetTargetDisplay(0);
        CUIManager.Instance.SetTargetDisplay(0);
        CPlayerManager.Instance.IsCanOperation = true;

        //gameObject.SetActive(false);
    }
}
