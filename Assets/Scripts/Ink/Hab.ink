INCLUDE exports.ink

// Program entry point
-> Commands


=== Commands ===
{
    - GetEvent("has_seen_epic_cold") || GetTodayIsSafe():
        You are inside the emergency Hab.
    - GetEvent("can_heat_hab"):
        <color=red>WARNING!</color>
        <br/>
        The temperature today is going to drop to extreme levels. The heater is going to pull power from your batteries to keep it warm.
        ~ SetEvent("has_seen_epic_cold", true)
    - else:
        The temperature today is going to drop to extreme levels. You must upgrade the heater or you will freeze to death!
        ~ SetEvent("has_seen_epic_cold", true)
}

-> command_loop
-> DONE

= command_loop
* [<cmd>Read ship logs</cmd> - Maybe there is some useful information here.]
    You turn on the computer and load up the last automated log entry from your ship.
    <br/>
    Unknown ship failure. Category: <color=red>catastrophic</color>. Emergency landers deployed, two successful landings.
    <b>Landing one:</b> cargo, including rover and miscellaneous supplies.
    <b>Landing two:</b> emergency habitat, one crew member.
    ~ SetEvent("knows_about_rover", true)

* { GetEvent("is_crystal_expert") } [<cmd>Crystal study results</cmd> - Read the computer analysis of the crystal.]
    The crystal posesses strange conductivity powers. When an electrical current runs through it it's almost a superconducter, but without a current it's a fantastic insulator.
    <br/>
    You should build a crystal extractor for your rover.
    ~ SetEvent("crystal_is_unlocked", true)

* { GetEvent("has_seen_crystal") } [<cmd>Study the crystal</cmd> - Learn what you can about this mysterious crystal.]
    You set the computer to analyze the crystal. When you're done the computer will give you a complete analysis.
    ~ DoPurchase("study_crystal")

* { GetEvent("knows_about_rover") } [<cmd>Summon rover</cmd> - Send message to rover to return to the Hab via autopilot.]
    ~ SetEvent("has_rover", true)
    You open the computer console and scan for the rover. Sure enough, it's on the planet, very nearby, and reports no damage!
    <br/>
    Finally some good news.
    <br/>
    You tell the rover to drive to Hab and within minutes it's parked out front.

+ { GetEvent("has_rover") } [<cmd>Go outside</cmd> - Explore with the rover.]
    You head outside.
    ~ DoGoOutside()
    ~ AdvanceTime(1)

+ [<cmd>Work on the Hab</cmd> - The Hab could use an upgrade.]
    -> Build_Stuff

+ { GetEvent("can_heat_hab") } [<cmd>Work on the Rover</cmd> - The rover could use an upgrade.]
    -> Build_Rover_Stuff

+ [<cmd>Take a break</cmd> - I'll take an hour off to relax.]
    ~ AdvanceTime(1)

+ [<cmd>Go to bed</cmd> - I've done enough today.]
    ~ DoGoToBed()
    You go to sleep.

- -> command_loop ->
-> DONE

=== Sleep ===
You're getting very tired.
+ [<cmd>Go to bed</cmd>  - Time to sleep.]
    ~ DoGoToBed()

- -> Commands.command_loop
-> DONE

=== Gameover
{ GetDeathText() }

+ [<cmd>Play again</cmd> - Next time I'll live longer.]
    ~ DoRestartGame()
-> DONE

=== Build_Stuff ===
Improving the Hab requires scrap, and sometimes other materials.
{ HasBuildProject():
    -> continue_build_project ->
- else:
    -> build_stuff_loop ->
}
-> DONE

= build_stuff_loop

+ { !GetEvent("can_heat_hab") } [<cmd>Improve heater ({ GetCost("first_heater_upgrade") }) </cmd> - Add a battery interface to the heater so it can draw power from the spare batteries when it gets really cold.]
    { CanPurchase("first_heater_upgrade"):
        ~ DoPurchase("first_heater_upgrade")
        The current heater and insulation is good enough for some weather, but this planet gets <i>really</i> cold.
        <br/>
        Having the batteries attached to the heater will enable the heaters to draw power if the temperature falls below a certain point.
    - else:
        -> fail_build ->
    }

+ { !GetEvent("has_efficient_heater") && GetEvent("can_heat_hab") } [<cmd>High efficiency heater ({ GetCost("efficient_heater")})</cmd> - The heater works but can be made a lot better.]
    { CanPurchase("efficient_heater"):
        ~ DoPurchase("efficient_heater")
        This will produce twice as much heat per unit of battery power.
    - else:
        -> fail_build ->
    }

+ { !GetEvent("has_insulation") && GetEvent("crystal_is_unlocked") } [<cmd>Crystal insulation ({ GetCost("crystal_insulation") })</cmd> - This will decrease the amount of cold you'll have to combat.]
    { CanPurchase("crystal_insulation"):
        ~ DoPurchase("crystal_insulation")
        This insulation means less heat will escape the Hab, so you'll live a bit longer.
        <br/>
        Probably.
    - else:
        -> fail_build ->
    }

+ { !GetEvent("has_epic_insulation") && GetEvent("has_insulation") } [<cmd>Crystal insulation <i>everywhere</i> ({ GetCost("epic_crystal_insulation") })</cmd> - This will decrease the amount of cold you'll have to combat.]
    { CanPurchase("epic_crystal_insulation"):
        ~ DoPurchase("epic_crystal_insulation")
        The insulation worked really well so let's build some more.
    - else:
        -> fail_build ->
    }

+ [<cmd>Done</cmd> - I'm finished working on the Hab now.] -> Commands.command_loop

- -> build_stuff_loop ->
-> DONE

= fail_build
You don't have enough resources.
- ->->

= continue_build_project
{ GetBuildProjectText() }
+ [<cmd>Resume work</cmd> - This will be useful when complete.]
    ~ GetToWork()
+ [<cmd>Done</cmd> - I'm finished working on the Hab now.] -> Commands.command_loop
- ->->

=== Build_Rover_Stuff ===
Improving the Hab requires scrap, and sometimes other materials.
{ HasBuildProject():
    -> continue_build_project ->
- else:
    -> build_stuff_loop ->
}
-> DONE

= build_stuff_loop

+ { !GetEvent("rover_upgrade_rough_terrain") } [<cmd>Rough terrain wheels ({ GetCost("rover_rough_terrain_upgrade") })</cmd> - Toughen up the wheels to enable the rover to drive over rough terrain.]
    { CanPurchase("rover_rough_terrain_upgrade"):
        ~ DoPurchase("rover_rough_terrain_upgrade")
        The rover's wheels are lacking a lifted suspension and a whole lot of metal armor, that's what they need!
        <br/>
        Soon I'll be driving over rough terrain.
    - else:
        -> fail_build ->
    }

+ { !GetEvent("rover_upgrade_silt_terrain") } [<cmd>Silt terrain upgrade ({ GetCost("rover_silt_terrain_upgrade") })</cmd> - Improved traction and <i>anti-sinking technology</i>.]
    { CanPurchase("rover_silt_terrain_upgrade"):
        ~ DoPurchase("rover_silt_terrain_upgrade")
        The rover cannot travel over some extremely fine ground. It needs better traction and <i>anti-sinking technology</i>.
    - else:
        -> fail_build ->
    }

+ { !GetEvent("faster_engine_1") } [<cmd>A faster engine ({ GetCost("faster_engine_1") })</cmd> - This will improve the rover's daily range.]
    { CanPurchase("faster_engine_1"):
        ~ DoPurchase("faster_engine_1")
        It's dangerous to fall asleep outside the Hab, so clearly the solution is to <i>drive faster</i> so I can see more!
    - else:
        -> fail_build ->
    }

+ { !GetEvent("faster_engine_2") && GetEvent("faster_engine_1")} [<cmd>An even faster engine ({ GetCost("faster_engine_2") })</cmd> - This will improve the rover's daily range.]
    { CanPurchase("faster_engine_2"):
        ~ DoPurchase("faster_engine_2")
        The rover is a lot of fun now, but it could be more fun!
    - else:
        -> fail_build ->
    }

+ { !GetEvent("faster_engine_3") && GetEvent("faster_engine_2") } [<cmd>The <i>fastest</i> engine ({ GetCost("faster_engine_3") })</cmd> - This will improve the rover's daily range.]
    { CanPurchase("faster_engine_3"):
        ~ DoPurchase("faster_engine_3")
        Let's see just how fast this guy can go!
    - else:
        -> fail_build ->
    }

+ { !GetEvent("has_crystal_extractor") && GetEvent("crystal_is_unlocked") } [<cmd>Crystal extractor ({ GetCost("crystal_extractor") })</cmd> - With this you can bring the crystal back to base and build fun stuff with it.]
    { CanPurchase("crystal_extractor"):
        ~ DoPurchase("crystal_extractor")
        The rover cannot travel over some extremely fine ground. It needs better traction and <i>anti-sinking technology</i>.
    - else:
        -> fail_build ->
    }

+ [<cmd>Done</cmd> - I'm finished working on the rover now.] -> Commands.command_loop

- -> build_stuff_loop ->
-> DONE

= fail_build
You don't have enough resources.
- ->->

= continue_build_project
{ GetBuildProjectText() }
+ [<cmd>Resume work</cmd> - This will be useful when complete.]
    ~ GetToWork()
+ [<cmd>Done</cmd> - I'm finished working on the rover now.] -> Commands.command_loop
- ->->
