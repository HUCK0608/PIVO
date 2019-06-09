using UnityEngine;
using UnityEngine.UI;

public class CUIManager_Title : MonoBehaviour
{
    /// <summary>애니메이터</summary>
    private Animator _animator = null;

    /// <summary>기본 메뉴 및 글로우 메뉴</summary>
    [SerializeField]
    private GameObject[] _defaultMenu = null, _selectMenu = null;

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

        ChangeSelectMenu(_currentSelect);
    }

    private void Start()
    {
        // 플레이어 조작 막기
        CPlayerManager.Instance.IsCanOperation = false;

        if (CUIManager.Instance != null)
            CUIManager.Instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangeSelectMenu(Mathf.Clamp(_currentSelect - 1, 0, _maxMenuCount - 1));
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangeSelectMenu(Mathf.Clamp(_currentSelect + 1, 0, _maxMenuCount - 1));
        else if (Input.GetKeyDown(KeyCode.Space))
            ExcutionSelectMenu(_currentSelect);
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
}
