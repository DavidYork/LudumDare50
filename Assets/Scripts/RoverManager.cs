using UnityEngine;

public class RoverManager: MonoBehaviour {
    [field: SerializeField]
    public Vector2Int Position { get; private set; }

    public Vector2Int StartPosition;

    [SerializeField] Camera _camera;
    [SerializeField] SpriteRenderer _rover;

    float debounceTime;

    public void OnEnterMapState() => debounceTime = Time.time;

    public void SetPosition(Vector2Int pos) {
        Position = pos;
        repositionMap();
    }

    public void TryMove(Direction dir) {
        var newPos = Position.Move(dir);
        var targetHex = Ludum.Dare.World.GetHexTileAt(newPos);
        var hexType = targetHex.ToHexType();

        switch (hexType) {
        case HexType.Open:
            moveTo(newPos);
            break;
        case HexType.Rough:
            if (Ludum.Dare.Events.GetEvent(Event.rover_upgrade_rough_terrain)) {
                moveTo(newPos);
            } else {
                Ludum.Dare.Computer.SetText("That ground is too rough, your rover cannot travel there.");
            }
            break;
        case HexType.Silt:
            if (Ludum.Dare.Events.GetEvent(Event.rover_upgrade_silt_terrain)) {
                moveTo(newPos);
            } else {
                Ludum.Dare.Computer.SetText("That ground is like quicksand! Your rover cannot travel there.");
            }
            break;
        case HexType.Blocked:
            Ludum.Dare.Computer.SetText("You can't move there");
            // TODO: Play sound
            break;
        case HexType.Fungus:
            if (Ludum.Dare.Events.GetEvent(Event.can_harvest_crystal) || Ludum.Dare.Events.GetEvent(Event.has_crystal_extractor)) {
                targetHex.ChangeHexType(targetHex.TileID + 1);
                Ludum.Dare.Computer.SetText("More mysterious purple crystal. This is fascinating stuff.");
                Ludum.Dare.Resources.Fungus.Amount += Ludum.Dare.Data.Map.FungusAmount;
            } else {
                Ludum.Dare.Commands.DoInteract(hexType);
            }
            break;
        case HexType.Scrap:
            targetHex.ChangeHexType(targetHex.TileID + 1);
            Ludum.Dare.Computer.SetText("Some scrap! This will be useful.");
            Ludum.Dare.Resources.Scrap.Amount += Ludum.Dare.Data.Map.ScrapAmount;
            break;
        case HexType.Hab:
            Ludum.Dare.State.Current = State.Hab;
            break;
        case HexType.Battery:
            targetHex.ChangeHexType(targetHex.TileID + 1);
            Ludum.Dare.Computer.SetText("A fresh battery! I can never have too many of these.");
            Ludum.Dare.Resources.Energy.Max += Ludum.Dare.Data.Map.BatteryAmount;
            Ludum.Dare.Resources.Energy.Amount += Ludum.Dare.Data.Map.BatteryEnergyAmount;
            break;
        }
    }

    // Private

    void moveTo(Vector2Int newPos) {
        Position = newPos;
        if (Ludum.Dare.Events.GetEvent(Event.faster_engine_3)) {
            Ludum.Dare.Temperature.Hour += Ludum.Dare.Data.Rover.FastestSpeed;
        } else if (Ludum.Dare.Events.GetEvent(Event.faster_engine_2)) {
            Ludum.Dare.Temperature.Hour += Ludum.Dare.Data.Rover.FasterSpeed;
        } else if (Ludum.Dare.Events.GetEvent(Event.faster_engine_1)) {
            Ludum.Dare.Temperature.Hour += Ludum.Dare.Data.Rover.FastSpeed;
        } else {
            Ludum.Dare.Temperature.Hour += Ludum.Dare.Data.Rover.SlowSpeed;
        }
        repositionMap();
    }

    void repositionMap() {
        Ludum.Dare.World.SpawnMissingMaps();
        var tile = Ludum.Dare.World.GetHexTileAt(Position);
        _camera.transform.localPosition = tile.transform.position;
        var roverPos = _rover.transform.position;
        roverPos.z = tile.transform.position.z - .001f;
        _rover.transform.position = roverPos;
    }

    void Update() {
        if (Ludum.Dare.State.Current != State.Map) {
            return;
        }

        if (Kbd.NorthPressed(debounceTime)) { TryMove(Direction.North); }
        if (Kbd.SouthPressed(debounceTime)) { TryMove(Direction.South); }
        if (Kbd.NorthEastPressed(debounceTime)) { TryMove(Direction.NorthEast); }
        if (Kbd.NorthWestPressed(debounceTime)) { TryMove(Direction.NorthWest); }
        if (Kbd.SouthEastPressed(debounceTime)) { TryMove(Direction.SouthEast); }
        if (Kbd.SouthWestPressed(debounceTime)) { TryMove(Direction.SouthWest); }
        if (Kbd.NextPressed(debounceTime)) { Ludum.Dare.Commands.GainFocus(); }
    }
}
