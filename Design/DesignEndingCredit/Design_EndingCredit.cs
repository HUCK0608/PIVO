﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public enum EndingCreditType { None, First, Second }

public class Design_EndingCredit : MonoBehaviour
{
    private RawImage EndingLogo = null;
    private Text TargetText = null;
    private GameObject TargetTextGroup = null;
    private List<GameObject> CanvasList = new List<GameObject>();

    private VideoPlayer DanceVideo = null;
    private Transform DanceRenderTexture = null;

    [SerializeField]
    private EndingCreditType CreditType = EndingCreditType.None;
    private AudioSource endingBGM;

    float ShowLogoValue = 0;
    float ShowTextValue = 0;
    bool bShowTextCoroutine = false;

    void Start()
    {
        endingBGM = GetComponent<AudioSource>();

        if (CreditType == EndingCreditType.None)
            Debug.LogError("EndingCreditType을 설정해주어야 합니다.");

        for (int i = 1; i < transform.childCount; i++)
        {
            CanvasList.Add(transform.Find("Canvas_" + i.ToString()).gameObject);
        }

        if (CreditType == EndingCreditType.First)
        {
            SetCanvasList(1);
            EndingLogo = transform.Find("Canvas_1").Find("LogoImage").GetComponent<RawImage>();
            TargetText = transform.Find("Canvas_1").Find("Text").GetComponent<Text>();

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

            DanceVideo = transform.Find("Canvas_2").transform.Find("DanceVideo").GetComponent<VideoPlayer>();
            DanceRenderTexture = transform.Find("Canvas_2").transform.Find("DanceRenderTexture");

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
        CUIManager_Title._isUseTitle = true;
        SceneManager.LoadScene("GrassStage_Stage1");
    }








    //-------------------------
    //CreditCoroutine_1
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






    //-------------------------
    //CreditCoroutine_2
    //-------------------------

    IEnumerator EndingCreditCoroutine_2()
    {
        CanvasList[1].transform.Find("loading").gameObject.SetActive(false);
        DanceRenderTexture.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(1);

        while (ShowLogoValue < 1)
        {
            ShowLogoValue += 0.01f;
            EndingLogo.color = new Color(1, 1, 1, ShowLogoValue);
            yield return new WaitForFixedUpdate();
        }

        float UpSpeed = 0.0006f * Screen.height;

        float curTime = Time.time;
        float snapShotTime = Time.time;
        float showValue = 0;
        float hideValue = 1;
        var defaultColorValue = DanceRenderTexture.GetComponent<RawImage>().color;

        float waitBeginVideo = 14f;
        float fadeOutVideo = 14f;

        float loopTime = fadeOutVideo / Time.deltaTime;
        int loopCount = 0;
        
        while (true)
        {
            TargetTextGroup.transform.position += Vector3.up * UpSpeed;

            yield return new WaitForFixedUpdate();

            if (CanvasList[1].transform.Find("TextGroup").Find("END").position.y > CanvasList[1].transform.Find("GoalPos").position.y)
                break;

            curTime += Time.deltaTime;
            if (snapShotTime + waitBeginVideo < curTime && !DanceVideo.isPlaying)
            {
                DanceVideo.Play();
            }
            else if (snapShotTime + waitBeginVideo + 4f < curTime && showValue < 1)
            {
                showValue = Mathf.Clamp(showValue + (Time.deltaTime * 0.15f), 0, 1);
                DanceRenderTexture.GetComponent<RawImage>().color = new Color(defaultColorValue.r, defaultColorValue.g, defaultColorValue.b, showValue);
            }
            else if (showValue == 1 && hideValue > 0)
            {
                if (DanceVideo.length - fadeOutVideo - 1f < DanceVideo.time)
                {
                    loopCount++;
                    hideValue = Mathf.Clamp(hideValue - (1/loopTime), 0, 1);
                    DanceRenderTexture.GetComponent<RawImage>().color = new Color(defaultColorValue.r, defaultColorValue.g, defaultColorValue.b, hideValue);
                }
            }
        }

        while (ShowLogoValue > 0)
        {
            if (endingBGM != null)
                endingBGM.volume -= Time.deltaTime * 0.5f;
            ShowLogoValue -= 0.01f;
            EndingLogo.color = new Color(1, 1, 1, ShowLogoValue);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);
        
        CanvasList[1].transform.Find("loading").gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);


        FinishCredit();
    }


}
