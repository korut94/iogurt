#Command
The command "package" keeps any humand command.

# Constraints
* Any command has to implement a method `apply(target : CVmObject)` where `target` is the subject of the command
* Any command has to set the value of the transformation before to apply it.
