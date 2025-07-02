extends Node
class_name WiimoteBridge

signal button_pressed(id, button)
signal nunchuk_button_pressed(id, button)
signal nunchuk_joystick_moved(id, stick_x, stick_y)

func _ready():
	if Engine.has_singleton("WiimoteService"):
		var wm = Engine.get_singleton("WiimoteService")
		wm.connect("ButtonPressed", _on_button_pressed)
		wm.connect("NunchukButtonPressed", _on_nunchuk_button_pressed)
		wm.connect("NunchukJoystickMoved", _on_nunchuk_joystick_moved)
	else:
		push_error("WiimoteService not found!")

func _on_button_pressed(id, button):
	emit_signal("button_pressed", id, button)

func _on_nunchuk_button_pressed(id, button):
	emit_signal("nunchuk_button_pressed", id, button)

func _on_nunchuk_joystick_moved(id, x, y):
	emit_signal("nunchuk_joystick_moved", id, x, y)

func set_rumble(id: int, enabled: bool):
	Engine.get_singleton("WiimoteService").set_rumble(id, enabled)

func set_led(id: int, led_index: int, enabled: bool):
	Engine.get_singleton("WiimoteService").set_led(id, led_index, enabled)
