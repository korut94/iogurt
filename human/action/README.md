#Actions
The action "package" keeps any humand action.

# Constraints
* Any action has to implement a method `apply(target : Object)` where `target` is the subject of the command
* Any action has to set the value of the transformation (in the constructor) before to apply it.
