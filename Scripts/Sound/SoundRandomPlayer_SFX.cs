using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomPlayer_SFX : MonoBehaviour
{
    [SerializeField]
    private List<SoundData_SFX> _soundDataList = null;

    [SerializeField]
    private bool _useLoop = false;

    private AudioSource _currentAudioSource = null;
    private bool _isPlayLoop = false;

    public void Play()
    {
        if(false == _useLoop)
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
        SoundData_SFX playSoundData = GetPlaySoundData();
        ESFXType sfxType = playSoundData.SFXType;
        float delay = playSoundData.Delay;

        return SoundManager.Instance.PlaySFX(sfxType, false, delay);
    }

    private SoundData_SFX GetPlaySoundData()
    {
        SoundData_SFX playSoundData = null;

        int randomValue = Random.Range(1, 101);

        for(int i = 0; i < _soundDataList.Count; i++)
        {
            SoundData_SFX currentSoundData = _soundDataList[i];

            if(null == currentSoundData)
                return null;

            if(_soundDataList[i].MinPercent <= randomValue && randomValue <= _soundDataList[i].MaxPercent)
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
