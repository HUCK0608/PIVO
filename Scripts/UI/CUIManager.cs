using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUIManager : MonoBehaviour
{
    private static CUIManager _instance = null;
    public static CUIManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>체력 이미지 리스트</summary>
    [SerializeField]
    private List<GameObject> _hpImages = null;

    /// <summary>
    /// 체력 UI 설정
    /// </summary>
    /// <param name="currentHp">현재 체력</param>
    public void SetHpUI(int currentHp)
    {
        _hpImages[currentHp].SetActive(false);
    }

    /// <summary>비스킷 개수 텍스트</summary>
    [SerializeField]
    private Text _biscuitCountText = null;

    /// <summary>
    /// 비스킷 UI 설정
    /// </summary>
    /// <param name="currentBiscuitCount">현재 비스킷 개수</param>
    public void SetBiscuitUI(int currentBiscuitCount)
    {
        _biscuitCountText.text = "X " + currentBiscuitCount.ToString();
    }
}
