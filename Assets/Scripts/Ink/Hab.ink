INCLUDE exports.ink

// Program entry point
-> Commands


=== Commands ===
You are inside the emergency Hab.

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

+ [<cmd>Work on the Hab</cmd> - The Hab could use an upgrade.]
    -> Build_Stuff

+ [<cmd>Go to bed</cmd> - I've done enough today.]
    ~ DoGoToBed()
    Text and more text!

- -> command_loop ->
-> DONE


=== Build_Stuff ===
Improving the Hab requires scrap, and sometimes other materials. -> build_stuff_loop -> DONE

= build_stuff_loop

+ { !GetEvent("can_heat_hab") } [<cmd>Improve heater ({ GetCost("first_heater_upgrade") }) </cmd> - Add a battery interface to the heater so it can draw power from the spare batteries when it gets really cold.]
    { TryPurchase("first_heater_upgrade"):
        ~ SetEvent("can_heat_hab", true)
        ~ GainResource("EnergyToHeat", 1)
        The current heater and insulation is good enough for some weather, but this planet gets <i>really</i> cold.
        <br/>
        Having the batteries attached to the heater will enable the heaters to draw power if the temperature falls below a certain point.
    - else:
        -> fail_build ->
    }

+ [<cmd>Done</cmd> - I'm finished working on the Hab now'.] -> Commands.command_loop

- -> build_stuff_loop ->
-> DONE

= fail_build
You don't have enough resources.
- ->->