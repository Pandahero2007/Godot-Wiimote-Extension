# Godot Wiimote Extension

A Godot 4 plugin that adds support for Nintendo Wii Remotes, including:
- Button input (A/B/Plus/Minus/Home/etc.)
- Nunchuk joystick and buttons
- Rumble control
- LED control
- Multiple Wiimotes support

Powered by [WiimoteLib](https://github.com/benthor/wiiuse/tree/master/managed/WiimoteLib) and usable entirely from GDScript.

---

## 🚀 Installation

1. Copy the `wiimote/` folder into your Godot project under the `addons/` folder or create one if you don't have one.
2. In **Project > Project Settings > Autoload**, add the following:
   - **Name**: `WiimoteService` → `res://addons/wiimote/WiimoteService.cs`
   - **Name**: `WiimoteBridge` → `res://addons/wiimote/WiimoteBridge.gd`
3. Ensure C# support is enabled in your project.
4. Make sure your wii remotes are connected to your computer via bluetooth.
    - [This tutorial](https://www.youtube.com/watch?v=J-s9gZJNp8o) is good if you are having issues. **Dolphin is not needed here.**
6. Run your game and press buttons on your wii remotes(s) to connect automatically.

---

## 🧩 Usage (GDScript)

```gdscript
func _ready():
    WiimoteBridge.connect("button_pressed", _on_button_pressed)
    WiimoteBridge.connect("nunchuck_joystick_moved", _on_nunchuk_joystick_moved)

    WiimoteBridge.set_led(0, 1, true)    # turn on LED 1 on Wiimote 0
    WiimoteBridge.set_rumble(1, true)    # rumble Wiimote 1

func _on_button_pressed(id, button):
    print("Wiimote %d pressed: %s" % [id, button])

func _on_nunchuk_joystick_moved(id, x, y):
    print("Wiimote %d nunchuk: %s,%s)
