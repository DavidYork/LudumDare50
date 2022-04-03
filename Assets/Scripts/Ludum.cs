using UnityEngine;

public class Ludum: MonoBehaviour {
    public static Ludum Dare {
        get {
            if (_dare != null) {
                return _dare;
            }
            return Object.FindObjectOfType<Ludum>();
        }
    }

    public ResourcesManager Resources;
    public TemperatureManager Temperature;
    public HabManager Hab;
    public MapManager Map;
    public CommandManager Commands;
    public StateManager State;
    public Factory Factory;
    public ComputerManager Computer;
    public DreamManager Dream;
    public GameMaster GM;
    public Database Data;
    public World World;
    public RoverManager Rover;
    public EventManager Events;

    // Private

    static Ludum _dare;

    void Awake() => _dare = this;
}