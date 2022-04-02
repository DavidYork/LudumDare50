using UnityEngine;

public class RoverManager: MonoBehaviour {
    [field: SerializeField]
    public Vector2Int Position { get; private set; }

    [SerializeField] Camera _camera;

    float debounceTime;

    public void OnEnterMapState() => debounceTime = Time.time;

    public void SetPosition(Vector2Int pos) {
        Position = pos;
        repositionMap();
    }

    public void TryMove(Direction dir) {
        var newPos = Position.Move(dir);
        var canMove = Ludum.Dare.World.GetHexType(newPos) != HexType.Blocked;
        if (canMove) {
            Position = newPos;
            repositionMap();
        } else {
            Ludum.Dare.Computer.SetText("You can't move there");
            // TODO: Sound
        }
    }

    // Private

    void repositionMap() {
        Ludum.Dare.World.SpawnMissingMaps();
        var tile = Ludum.Dare.World.GetHexTileAt(Position);
        _camera.transform.localPosition = tile.transform.position;
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
    }
}
