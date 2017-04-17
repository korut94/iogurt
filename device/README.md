# Device Translation
For each devices is neccessary to provide a function `deviceTranslation` that maps the user input in human command.

# Constraints
* The function has to return a tuple (status, id_target, command):

### status
The `status` flag indicates that the event doesn't have a correspective command and therefore it was ignored. 

### id_target
The `id_target` indicates what body parts will be to subject at the action of the command.

### command
The `command` value, if `status` is setted to true, rappresents the correct command to apply.
