using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomPlayer_BGM : MonoBehaviour
{
    [SerializeField]
    private List<SoundData_BGM> _soundDataList = null;

    [SerializeField]
    private bool _useLoop = false;

    private AudioSource _currentAudioSource = null;
    private bool _isPlayLoop = false;

    public void Play()
    {
        if (false == _useLoop)
            PlayAndGetAudioSource();
        else
            PlayLoop();
    }

    public void StopLoop()
    {
        StopAllCoroutines();
        _isPlayLoop = false;
    }

    private AudioSource PlayAndGetAudioSource()
    {
        SoundData_BGM playSoundData = GetPlaySoundData();
        EBGMType BGMType = playSoundData.BGMType;
        float delay = playSoundData.Delay;

        return SoundManager.Instance.PlayBGM(BGMType, false, delay);
    }

    private SoundData_BGM GetPlaySoundData()
    {
        SoundData_BGM playSoundData = null;

        int randomValue = Random.Range(1, 101);

        for (int i = 0; i < _soundDataList.Count; i++)
        {
            SoundData_BGM currentSoundData = _soundDataList[i];

            if (null == currentSoundData)
                return null;

            if (_soundDataList[i].MinPercent <= randomValue && randomValue <= _soundDataList[i].MaxPercent)
            {
                playSoundData = _soundDataList[i];
                break;
            }
        }

        return playSoundData;
    }

    private void PlayLoop()
    {
        if (false == _isPlayLoop)
            StartCoroutine("PlayLoopLogic");
    }

    private IEnumerator PlayLoopLogic()
    {
        _isPlayLoop = true;

        while (true)
        {
            _currentAudioSource = PlayAndGetAudioSource();

            yield return new WaitUntil(() => false == _currentAudioSource.isPlaying);
        }
    }
}
