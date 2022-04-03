using UnityEngine;

public enum Cost {
    first_heater_upgrade,
}

public enum Event {
    can_heat_hab,
    has_rover,
    knows_about_rover,
    heating_is_on,
    alarm_is_on,
    rover_upgrade_rough_terrain,
    can_harvest_crystal,
    has_seen_crystal,
    faster_rover,
    has_seen_epic_cold,
    has_been_outside,
}

public enum State {
    Uninitialized, Hab, Map, Dream, Dead
}

public enum Direction {
    North,
    NorthEast,
    SouthEast,
    South,
    SouthWest,
    NorthWest
}

public enum HexType {
    Open, Rough, Blocked, Fungus, Scrap, Hab, Battery,
}