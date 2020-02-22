using UnityEngine;
using UnityEngine.SceneManagement;

public class CSoopState3D_Idle : CSoopState3D
{
    [SerializeField]
    private Transform _emoticonPoint = null;
    [SerializeField]
    private Transform _sleepEmoticon = null;

    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer = null;
    [SerializeField]
    private Material _sleepMaterial_Snow = null;
    [SerializeField]
    private Material _sleepMaterial_Grass = null;
    private Material _defaultMaterial = null;

    public override void InitState()
    {
        base.InitState();

        Controller3D.LookDirection(Controller3D.Manager.Stat.IsSoopDirectionRight ? Vector3.right : Vector3.left);

        _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
        _sleepEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        _sleepEmoticon.gameObject.SetActive(true);

        _defaultMaterial = _skinnedMeshRenderer.material;

        string currentSceneSeason = SceneManager.GetActiveScene().name.Split('_')[0];
        if(currentSceneSeason != null)
        {
            if (currentSceneSeason.Equals("GrassStage"))
                _skinnedMeshRenderer.material = _sleepMaterial_Grass;
            else if (currentSceneSeason.Equals("SnowStage"))
                _skinnedMeshRenderer.material = _sleepMaterial_Snow;
        }
    }

    private void Update()
    {
        if (CUIManager.Instance.IsOnStageClearUI)
        {
            if (_sleepEmoticon.gameObject.activeSelf)
                _sleepEmoticon.gameObject.SetActive(false);
        }
        else
        {
            if (!_sleepEmoticon.gameObject.activeSelf)
                _sleepEmoticon.gameObject.SetActive(true);
        }

        Vector3 startPoint = Controller3D.Manager.transform.position;

        _sleepEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);

        if (Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Surprise);
        if (!transform.position.Equals(startPoint))
            Controller3D.ChangeState(ESoopState.Return);
    }

    public override void EndState()
    {
        base.EndState();

        _sleepEmoticon.gameObject.SetActive(false);

        _skinnedMeshRenderer.material = _defaultMaterial;
    }
}
