using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public enum EBGMType { None, };
public enum ESFXType 
{ 
                        None,
                        Corgi_Climb, Corgi_PutEnd,
                        Soop_Move_0, Soop_Move_1, Soop_PutEnd_0, Soop_PutEnd_1,
                        ViewChange_Cast, ViewChange_ChangStart, ViewChange_ChangeEnd, ViewChange_Block,
                        BombFire_0, Boom_0, Boom_1, Boom_2, Boom_3, Boom_4, Boom_5,
                        BrokenTile_0, BrokenTile_1,
                        MagicStone_Activate, MagicStone_Idle,
                        Pipe_1,
                        StageClear_0, StageClear_1, StageClear_2,
                        TileButton_0, TileButton_1, TileButton_2,
                        UI_MouseEnter_1, UI_MouseEnter_2, UI_MouseEnter_3, UI_MouseEnter_4, UI_MouseEnter_5,
                        UI_MouseClick_1, UI_MouseClick_2, UI_MouseClick_3,
};

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            if(null == _instance)
            {
                GameObject soundManagerObject = Instantiate(Resources.Load<GameObject>("Prefabs/SoundManager"), null);
                _instance = soundManagerObject.GetComponent<SoundManager>();
            }

            return _instance;
        }
    }

    private readonly string _BGMPath = "Sounds/BGM/";
    private readonly string _SFXPath = "Sounds/SFX/";

    public AudioSource _audioSource_BGM = null;
    public AudioSource _audioSource_SFX = null;

    private Dictionary<EBGMType, AudioClip> _BGMClipDic = new Dictionary<EBGMType, AudioClip>();
    private Dictionary<ESFXType, AudioClip> _SFXClipDic = new Dictionary<ESFXType, AudioClip>();

    private List<AudioSource> _audioSourceList_BGM = new List<AudioSource>();
    private List<AudioSource> _audioSourceList_SFX = new List<AudioSource>();

    private bool _isInitialize = false;

    private void Awake()
    {
        Initialize();
    }

    #region Initialize

    private void Initialize()
    {
        InitBGM();
        InitSFX();
        InitAudioSource();

        _instance = this;
        _isInitialize = true;

        DontDestroyOnLoad(this.gameObject);
    }

    private void InitBGM()
    {
        EBGMType[] enumValues = (EBGMType[])Enum.GetValues(typeof(EBGMType));

        foreach (EBGMType enumValue in enumValues)
        {
            if (EBGMType.None == enumValue)
                continue;

            string sourcePath = string.Format("{0}{1}", _BGMPath, enumValue.ToString("G"));
            AudioClip audioClip = Resources.Load<AudioClip>(sourcePath);

            if (null == audioClip)
                continue;

            _BGMClipDic.Add(enumValue, audioClip);
        }
    }

    private void InitSFX()
    {
        ESFXType[] enumValues = (ESFXType[])Enum.GetValues(typeof(ESFXType));

        foreach(ESFXType enumValue in enumValues)
        {
            if (ESFXType.None == enumValue)
                continue;

            string sourcePath = string.Format("{0}{1}", _SFXPath, enumValue.ToString("G"));
            AudioClip audioClip = Resources.Load<AudioClip>(sourcePath);

            if (null == audioClip)
                continue;

            _SFXClipDic.Add(enumValue, audioClip);
        }
    }

    private void InitAudioSource()
    {
        _audioSourceList_BGM.Add(_audioSource_BGM);
        _audioSourceList_SFX.Add(_audioSource_SFX);
    }

    #endregion

    #region AudioSource

    private AudioSource GetCanPlayBGMAudioSource()
    {
        foreach(AudioSource audioSource in _audioSourceList_BGM)
        {
            if(false == audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        return CreateBGMAudioSource();
    }

    private AudioSource CreateBGMAudioSource()
    {
        GameObject newAudioSourceObject = Instantiate(_audioSource_BGM.gameObject, this.transform);
        AudioSource newAudioSource = newAudioSourceObject.GetComponent<AudioSource>();
        Stop(newAudioSource);
        _audioSourceList_BGM.Add(newAudioSource);

        return newAudioSource;
    }

    private AudioSource GetCanPlaySFXAudioSource()
    {
        foreach (AudioSource audioSource in _audioSourceList_SFX)
        {
            if (false == audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        return CreateSFXAudioSource();
    }

    private AudioSource CreateSFXAudioSource()
    {
        GameObject newAudioSourceObject = Instantiate(_audioSource_SFX.gameObject, this.transform);
        AudioSource newAudioSource = newAudioSourceObject.GetComponent<AudioSource>();
        Stop(newAudioSource);
        _audioSourceList_SFX.Add(newAudioSource);

        return newAudioSource;
    }

    #endregion

    #region Play

    /// <summary>외부에서 Stop 할 수 있도록 AudioSource를 반환</summary>
    public AudioSource PlayBGM(EBGMType bgmType, bool useLoop = false, float delay = 0f)
    {
        if (false == _isInitialize || EBGMType.None == bgmType)
            return null;

        AudioSource audioSource = GetCanPlaySFXAudioSource();

        audioSource.Stop();
        audioSource.clip = _BGMClipDic[bgmType];
        audioSource.loop = useLoop;

        if (delay.Equals(0f))
            audioSource.Play();
        else
            audioSource.PlayDelayed(delay);

        return audioSource;
    }

    /// <summary>외부에서 Stop 할 수 있도록 AudioSource를 반환</summary>
    public AudioSource PlaySFX(ESFXType sfxType, bool useLoop = false, float delay = 0f)
    {
        if (false == _isInitialize || ESFXType.None == sfxType)
            return null;

        AudioSource audioSource = GetCanPlaySFXAudioSource();

        audioSource.Stop();
        audioSource.clip = _SFXClipDic[sfxType];
        audioSource.loop = useLoop;

        if (delay.Equals(0f))
            audioSource.Play();
        else
            audioSource.PlayDelayed(delay);

        return audioSource;
    }

    #endregion

    #region Stop

    public void Stop(AudioSource audioSource)
    {
        if (null == audioSource)
            return;

        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = null;
    }

    public void StopAll()
    {
        StopAll_BGM();
        StopAll_SFX();
    }

    public void StopAll_BGM()
    {
        foreach(AudioSource audioSource in _audioSourceList_BGM)
        {
            if (true == audioSource.isPlaying)
                Stop(audioSource);
        }
    }

    public void StopAll_SFX()
    {
        foreach(AudioSource audioSource in _audioSourceList_SFX)
        {
            if (true == audioSource.isPlaying)
                Stop(audioSource);
        }
    }

    #endregion
}
