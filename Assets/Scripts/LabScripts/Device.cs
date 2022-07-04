using Model;
using UIScripts;
using UnityEngine;

public enum DeviceStatus : int
{
    working = 0, 
    nonWorking, 
    blocked
}

public class Device : MonoBehaviour
{
    [SerializeField] private DeviceInfo _deviceInfo;
    [SerializeField] private Material _material;
    [SerializeField] private Scaler _scaler;
    [SerializeField] private SpriteRenderer renderer;

    private Color[] _colors = { Color.green, Color.yellow, Color.red };

    public LevelInfo LevelInfo 
    { 
        get => _deviceInfo.levelInfo; 
        set
        {
            _deviceInfo.levelInfo = value;
        }
    }
    public DeviceStatus DeviceStatus
    {
        get => _deviceInfo.deviceStatus;
        set
        {
            _deviceInfo.deviceStatus = value;
            _material.SetColor("_SolidOutline", _colors[((int)_deviceInfo.deviceStatus)]);
        }
    }

    private void Awake()
    {
        _material.enableInstancing = true;
        _material = new Material(_material);
        renderer.material = _material;
    }

    private void Start()
    {
        _material.SetColor("_SolidOutline", _colors[((int)_deviceInfo.deviceStatus)]);
    }

    private void OnMouseDown()
    {
        if (DeviceStatus != DeviceStatus.nonWorking)
            return;
        _scaler.scale = false;
        GameController.SetDevise = this;
        UIController.levelLoadPanel.gameObject.SetActive(true); 
        UIController.levelLoadPanel.close += () => _scaler.scale = true;
    }

    public void SetDeviceInfo(DeviceInfo deviceInfo)
    {
        _deviceInfo = deviceInfo;
    }
}
