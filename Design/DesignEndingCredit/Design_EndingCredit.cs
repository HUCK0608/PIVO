using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum EndingCreditType { None, First }

public class Design_EndingCredit : MonoBehaviour
{
    private RawImage EndingLogo = null;
    private Text TargetText = null;

    [SerializeField]
    private EndingCreditType CreditType = EndingCreditType.None;

    float ShowLogoValue = 0;
    float ShowTextValue = 0;
    bool bShowTextCoroutine = false;

    void Start()
    {
        if (CreditType == EndingCreditType.First)
        {
            EndingLogo = transform.Find("Canvas_1").Find("LogoImage").GetComponent<RawImage>();
            TargetText = transform.Find("Canvas_1").Find("Text").GetComponent<Text>();

            EndingLogo.color = new Color(1, 1, 1, 0);
            TargetText.color = new Color(1, 1, 1, 0);

            StartCoroutine(EndingCreditCoroutine());
        }
    }

    IEnumerator EndingCreditCoroutine()
    {

        yield return new WaitForSeconds(1);

        while (ShowLogoValue < 1)
        {
            ShowLogoValue += 0.01f;
            EndingLogo.color = new Color(1, 1, 1, ShowLogoValue);
            yield return new WaitForFixedUpdate();
        }


        float WaitTerm = 1f;

        StartCoroutine(ShowTextCoroutine("GameDesign : KimMyungJun"));//KimMyungJun
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        StartCoroutine(ShowTextCoroutine("GameDesign : KimHeonJu"));//KimHeonJu
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        StartCoroutine(ShowTextCoroutine("Programming : K.Huck"));//KimSangHyeok
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        StartCoroutine(ShowTextCoroutine("Animation : KimDoYeon"));//KimDoYeon
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        StartCoroutine(ShowTextCoroutine("ConceptArt/Voxel : KimJuYoung"));//KimJuYoung
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        StartCoroutine(ShowTextCoroutine("UI/Pixel : HanEunHye"));//HanEunHye
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        StartCoroutine(ShowTextCoroutine("Special Thanks : JungJongPil"));//JungJongPil
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        StartCoroutine(ShowTextCoroutine("END"));
        yield return new WaitUntil(() => !bShowTextCoroutine);
        yield return new WaitForSeconds(WaitTerm);

        while (ShowLogoValue > 0)
        {
            ShowLogoValue -= 0.01f;
            EndingLogo.color = new Color(1, 1, 1, ShowLogoValue);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(WaitTerm);
        FinishCredit();
    }

    IEnumerator ShowTextCoroutine(string TText)
    {
        bShowTextCoroutine = true;
        ShowTextValue = 0;
        float ShowSpeed = 0.02f;
        float HideSpeed = 0.02f;

        while (ShowTextValue < 1)
        {
            ShowTextValue += ShowSpeed;
            TargetText.color = new Color(1, 1, 1, ShowTextValue);
            TargetText.text = TText;
            yield return new WaitForFixedUpdate();
        }

        ShowTextValue = 1;

        while (ShowTextValue > 0)
        {
            ShowTextValue -= HideSpeed;
            TargetText.color = new Color(1, 1, 1, ShowTextValue);
            yield return new WaitForFixedUpdate();
        }
        bShowTextCoroutine = false;
    }

    void FinishCredit()
    {
        SceneManager.LoadScene("GrassStage_Stage1");
    }
    
}
