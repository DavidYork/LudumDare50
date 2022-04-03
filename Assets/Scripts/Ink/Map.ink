INCLUDE exports.ink

// Program entry point
-> Commands


=== Commands ===
You head outside.

{ GetEvent("has_been_outside") == false:
    <br/>
    You take your heater and the batteries with you, just in case.
    ~ SetEvent("has_been_outside", true)
}
-> command_loop
-> DONE

= command_loop

+ [<cmd>Cancel</cmd> - nevermind, back to driving the rover.]
    ~ DoReturnToRover()
+ [<cmd>Return home</cmd> - I've seen enough today.]
    ~ DoGoBackToHab()
    ~ AdvanceTime(1)
- -> command_loop ->
-> DONE

=== Interact_Fungus ===
There is a strange purple crystal here.
{ GetEvent("has_seen_crystal"):
    -> boring_crystal
- else:
    -> new_crystal
}
-> Commands.command_loop ->
-> DONE

= new_crystal
<br/>
What a bizarre structure this is. Maybe it's useful? It's too hard for me to break off a piece to take with me.
<br/>
There is a tiny amount lose on the ground, I'll take that for further study back at the Hab.

* [<cmd>Take a sample</cmd> - A small sample can be studied back at the lab.]
    ~ SetEvent("has_seen_crystal", true)

~ DoReturnToRover()
- -> Commands.command_loop ->
-> DONE

= boring_crystal
<br/>
I should study my sample at the lab, maybe I can learn something useful.
~ DoReturnToRover()
- -> Commands.command_loop ->
-> DONE

=== Interact_Rough ===
Rough terrain!
-> DONE

=== Sleep ===
You're getting very tired.
+ [<cmd>Return to the Hab</cmd>  - Time to sleep.]
    ~ DoGoBackToHab()
    ~ DoGoToBed()

- -> Commands
-> DONE

=== Gameover
{ GetDeathText() }

+ [<cmd>Play again</cmd> - Next time I'll live longer.]
    ~ DoRestartGame()
-> DONE
