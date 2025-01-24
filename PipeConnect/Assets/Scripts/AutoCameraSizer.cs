using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AutoCameraSizer : MonoBehaviour, IInilizable
{
    [SerializeField]private float _worldSize=5 ;

    [SerializeField] private Mode _mode;

    private Camera _camera;


    public bool Initialized { get; private set; }

    public void Init()
    {
        if (Initialized)
        {
            return;
        }

        _camera = GetComponent<Camera>();
        if (_mode == Mode.VerticalFit)
        {
            _camera.orthographicSize = _worldSize / 2;
        }
        else
        {
            var aspectRatio = (Screen.height + 0f) / Screen.width;
            _camera.orthographicSize = aspectRatio * _worldSize * 0.5f;
        }

        Initialized = true;
    }

    private void Awake()
    {
        Init();
    }

    public enum Mode
    {
        VerticalFit,
        HorizontalFit
    }
}


public interface IInilizable
{
    bool Initialized { get; }
    void Init();
}