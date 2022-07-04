using System.Collections.Generic;
using KnotGameController;
using Model;

public static class GameController
{
    public static Connection[] startConnections;
    public static Connection[] connections;
    public static List<Conductor> conductors = new List<Conductor>();
    public static LevelLoader levelLoader;
    private static Device _device;
    private static Conductor _activeConductor;

    public static Device SetDevise
    {
        set
        {
            _device = value;
        }
    }
    public static List<ConductorInfo> Conductors => _device.LevelInfo.conductors;
    public static Conductor ActiveConductor
    {
        get => _activeConductor;
        set
        {
            if (_activeConductor != null)
            {
                _activeConductor.Connect();
                if (_activeConductor == value)
                    _activeConductor = null;
                else
                {
                    _activeConductor = value;
                    _activeConductor.Unconnect();
                }
            }
            else
            {
                _activeConductor = value;
                _activeConductor.Unconnect();
            }
        }
    }
    public static List<Knot> Nodes
    {
        get
        {
            return _device.LevelInfo.knots;
        }
    }

    public static void Check()
    {
        if (_device.LevelInfo.knots.Count == 0)
            Win();
    }

    public static void Start() => levelLoader.LoadLevel(_device.LevelInfo);

    public static Conductor GetConductor(int startConnection)
    {
        foreach (var conductor in conductors)
            if (conductor.ConductorInfo.startConnectionPoint == startConnection)
                return conductor;

        return null;
    }

    private static void Win()
    {
        _device.DeviceStatus = DeviceStatus.working;
        Serializator.Serialize(GameInfo.key, GameInfo.room.roomState);
        GameUIController.Win();
    }
}

