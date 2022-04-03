using UnityEngine;

public enum Cost {
    first_heater_upgrade,
    rover_rough_terrain_upgrade,
    rover_silt_terrain_upgrade,
    study_crystal,
    faster_engine_1,
    faster_engine_2,
    faster_engine_3,
    crystal_extractor,
    crystal_insulation,
    epic_crystal_insulation,
    efficient_heater,
}

public enum Event {
    can_heat_hab,
    has_rover,
    knows_about_rover,
    heating_is_on,
    alarm_is_on,
    rover_upgrade_rough_terrain,
    rover_upgrade_silt_terrain,
    can_harvest_crystal, // Do not use
    has_seen_crystal,
    faster_rover,   // Do not use
    has_seen_epic_cold,
    has_been_outside,
    is_crystal_expert,
    crystal_is_unlocked,
    has_crystal_extractor,
    faster_engine_1,
    faster_engine_2,
    faster_engine_3,
    has_insulation,
    has_epic_insulation,
    has_efficient_heater,
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
    Open, Rough, Blocked, Fungus, Scrap, Hab, Battery, Silt
}