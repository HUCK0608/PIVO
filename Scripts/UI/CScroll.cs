using UnityEngine;
using UnityEngine.UI;

public class CScroll : MonoBehaviour
{
    /// <summary>스크롤 바</summary>
    [SerializeField]
    private Image _scrollDefault = null;

    /// <summary>버튼</summary>
    [SerializeField]
    private Transform _button = null;

    /// <summary>최소, 최대 로컬 X 위치</summary>
    [SerializeField]
    private float _minLocalX = 0f, _maxLocalX = 0f;

    private float _normalizedValue = 1f;
    /// <summary>정규화 값</summary>
    public float NormalizedValue { get { return _normalizedValue; } }

    public void DragEvent()
    {
        Vector3 newLocalPosition = Vector3.zero;
        newLocalPosition.x = (Input.mousePosition.x - Screen.width * 0.5f) - (transform.position.x - Screen.width * 0.5f);
        newLocalPosition.x *= 1920f / Screen.width;
        newLocalPosition.x = Mathf.Clamp(newLocalPosition.x, _minLocalX, _maxLocalX);

        _button.localPosition = newLocalPosition;

        float tempValue = _button.localPosition.x + _maxLocalX;
        _normalizedValue = tempValue.Equals(0f) ? 0f : tempValue / (_maxLocalX - _minLocalX);

        _scrollDefault.fillAmount = _normalizedValue;
    }
}
