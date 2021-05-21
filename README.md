# AR Desk Mysteries
<img src="https://raw.githubusercontent.com/giodestone/Desk-Mysteries-AR/master/Images/GIF.gif" height="500">

3D augmented reality point and click game about finding a pen in a desk. Made in Unity with Google ARCore (not XR Foundation) as coursework for a module in University. As a result, a lot of functionality was implemented in code, rather than using the interface.

## Running
[Download](TODO)

Only Android is supported. You must have enabled installing apps from untrusted sources.


Instructions provided on screen.


Scroll down for walkthrough!

## Implementation
<img src="https://raw.githubusercontent.com/giodestone/Desk-Mysteries-AR/master/Images/Image 1.png" height="500">
This project mostly uses ARCore's spatial tracking features. First, the player is asked to map out a scene, where ARCore will (hopefully) find a plane. The user can then pick out where they wish to place the desk, as represented by a cube. The player can go back to this stage at any time without losing their progress.

### Summary
* Event based structure.
* Reusable components.
* Specific functionality in managers.
* Use of C# interfaces to provide reusable functionality (such as GameplayEventSender).
* Queue of events is kept to restore state if the player remaps the scene.
* Has sound effects.
* Interface made to support one handed operation and existing design patterns.

The UML diagram is located in the source folder.

### Interaction
The interaction technique was modelled after the camera app, along with a reticle in the middle to indicate what the player will interact with. This was done to enable one handed operation, as finding the buttons requires the player to kneel down.

The raycasting was implemented using ARCore's Frame.

### Gameplay Logic Implementation
<img src="https://raw.githubusercontent.com/giodestone/Desk-Mysteries-AR/master/Images/Image 3.png" height="500">
Specialized logic was implemented in 'manager' classes, which handle the game state. In turn, they interact with reusable components, such as ones which detect presses. These components use a custom event sender and receiver interface to communicate with each other.

In order to allow the player to resume after remapping the scene, a queue of gameplay events is kept constant between scenes. These events are then re-executed after the scene is loaded back in.

## Walkthrough
<img src="https://raw.githubusercontent.com/giodestone/Desk-Mysteries-AR/master/Images/Image 2.png" height="500">
1. Find the key in the large right drawer and click on it.

2. Click on the lock which is on the top left of the screen.

3. Find the pattern in the small bottom drawer.

4. Enter the code “XXOXOXOO” using the buttons on the back of the desk.

## Asset Credits
Icon was adapted. Pen compartment sounds were cut together. Sounds are all public domain.

Material.io. Available at: https://material.io/resources/icons/?style=baseline

https://freesound.org/people/Nox_Sound/sounds/486081/ - Drawer Sound

https://freesound.org/people/Breviceps/sounds/448080/ - Button Sound

https://freesound.org/people/dkiller2204/sounds/422971/ - Key Sound

Pen compartment sounds:

https://freesound.org/people/giddster/sounds/360519/

https://freesound.org/people/leytos/sounds/251003/

### Notes
My phone struggles with recording while playing AR. You can fix this by employing me though.