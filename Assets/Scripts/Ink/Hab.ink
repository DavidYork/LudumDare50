INCLUDE exports.ink

// Program entry point
-> Commands


=== Commands ===
{
    - GetEvent("has_seen_epic_cold") || GetTodayIsSafe():
        You are inside the emergency Hab.
    - GetEvent("can_heat_hab"):
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

+ [<cmd>Take a break</cmd> - I'll take an hour off to relax.]
    ~ AdvanceTime(1)

// + [<cmd>Take a short break</cmd> - I'll stop working for 15 min.]
//     ~ AdvanceTime(0.25)

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
        // ~ SetEvent("can_heat_hab", true)
        // ~ GainResource("EnergyToHeat", 1)
        The current heater and insulation is good enough for some weather, but this planet gets <i>really</i> cold.
        <br/>
        Having the batteries attached to the heater will enable the heaters to draw power if the temperature falls below a certain point.
    - else:
        -> fail_build ->

    }
    // { DoPurchase("first_heater_upgrade"):
    //     ~ SetEvent("can_heat_hab", true)
    //     ~ GainResource("EnergyToHeat", 1)
    //     The current heater and insulation is good enough for some weather, but this planet gets <i>really</i> cold.
    //     <br/>
    //     Having the batteries attached to the heater will enable the heaters to draw power if the temperature falls below a certain point.
    // - else:
    //     -> fail_build ->
    // }

+ [<cmd>Done</cmd> - I'm finished working on the Hab now.] -> Commands.command_loop

- -> build_stuff_loop ->
-> DONE

= fail_build
You don't have enough resources.
- ->->

= continue_build_project
~ GetToWork()
- ->->