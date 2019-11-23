using UnityEngine;

public class CUI_ViewChagneTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui1 = null, _ui2 = null, _ui3 = null;

    private void Update()
    {
        if (CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.ViewChangeInit) || CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.ViewChangeIdle))
        {
            _ui1.SetActive(false);
            _ui2.SetActive(true);
            _ui3.SetActive(false);
        }
        else if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
        {
            _ui1.SetActive(true);
            _ui2.SetActive(false);
            _ui3.SetActive(false);
        }
        else if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
        {
            _ui1.SetActive(false);
            _ui2.SetActive(false);
            _ui3.SetActive(false);
        }
        else if(CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
        {
            _ui1.SetActive(false);
            _ui2.SetActive(false);
            _ui3.SetActive(true);
        }
    }
}
