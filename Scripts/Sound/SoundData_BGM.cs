[System.Serializable]
public class SoundData_BGM
{
    public EBGMType BGMType = EBGMType.None;
    public float Delay = 0f;
    // MinPercent ~ MaxPercent 숫자가 나올 경우 사운드 재생 (0 ~ 0은 재생되지 않음)
    public int MinPercent = 1;
    public int MaxPercent = 100;
}
