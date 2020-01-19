using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum EndingCreditType { None, First, Second }

public class Design_EndingCredit : MonoBehaviour
{
    private RawImage EndingLogo = null;
    private Text TargetText = null;
    private GameObject TargetTextGroup = null;
    private List<GameObject> CanvasList = new List<GameObject>();

    [SerializeField]
    private EndingCreditType CreditType = EndingCreditType.None;

    float ShowLogoValue = 0;
    float ShowTextValue = 0;
    bool bShowTextCoroutine = false;

    void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            CanvasList.Add(transform.Find("Canvas_" + i.ToString()).gameObject);
        }

        if (CreditType == EndingCreditType.First)
        {
            SetCanvasList(1);
            EndingLogo = transform.Find("Canvas_1").Find("LogoImage").GetComponent<RawImage>();
            TargetText = transform.Find("Canvas_2").Find("Text").GetComponent<Text>();

            EndingLogo.color = new Color(1, 1, 1, 0);
            TargetText.color = new Color(1, 1, 1, 0);

            StartCoroutine(EndingCreditCoroutine_1());
        }
        else if (CreditType == EndingCreditType.Second)
        {
            SetCanvasList(2);
            EndingLogo = transform.Find("Canvas_2").Find("LogoImage").GetComponent<RawImage>();
            TargetTextGroup = transform.Find("Canvas_2").Find("TextGroup").gameObject;

            EndingLogo.color = new Color(1, 1, 1, 0);

            StartCoroutine(EndingCreditCoroutine_2());
        }
    }







    //-------------------------
    //CommonMethod
    //-------------------------

    void SetCanvasList(int CanvasNum)
    {
        foreach (var v in CanvasList)
        {
            v.SetActive(false);
        }
        CanvasList[CanvasNum-1].SetActive(true);
    }
    void FinishCredit()
    {
        SceneManager.LoadScene("GrassStage_Stage1");
    }








    //-------------------------
    //CreditCoroutine
    //-------------------------

    IEnumerator EndingCreditCoroutine_1()
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

    IEnumerator EndingCreditCoroutine_2()
    {
        yield return new WaitForSeconds(1);

        while (ShowLogoValue < 1)
        {
            ShowLogoValue += 0.01f;
            EndingLogo.color = new Color(1, 1, 1, ShowLogoValue);
            yield return new WaitForFixedUpdate();
        }

        float UpSpeed = 0.5f;
        while (true)
        {
            TargetTextGroup.transform.position += Vector3.up * UpSpeed;

            yield return new WaitForFixedUpdate();

            if (CanvasList[1].transform.Find("TextGroup").Find("END").position.y > CanvasList[1].transform.Find("GoalPos").position.y)
                break;
        }

        while (ShowLogoValue > 0)
        {
            ShowLogoValue -= 0.01f;
            EndingLogo.color = new Color(1, 1, 1, ShowLogoValue);
            yield return new WaitForFixedUpdate();
        }

        FinishCredit();
    }


}
