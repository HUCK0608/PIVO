using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTutorialSetActiveTrigger_3D : MonoBehaviour
{
    [SerializeField]
    private eTutorialType _tutorialType = eTutorialType.Move2D;

    [SerializeField]
    private bool _value = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
        {
            switch (_tutorialType)
            {
                case eTutorialType.Move2D:
                    CUIManager.Instance.SetActiveMove2DTutorialUI(_value);
                    break;
                case eTutorialType.Move3D:
                    CUIManager.Instance.SetActiveMove3DTutorialUI(_value);
                    break;
                case eTutorialType.Climb:
                    CUIManager.Instance.SetActiveClimbTutorialUI(_value);
                    break;
                case eTutorialType.ViewChange:
                    CUIManager.Instance.SetActiveViewChangeTutorialUI(_value);
                    break;
            }
        }
    }
}
