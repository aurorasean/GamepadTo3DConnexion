
### How to use
* 1 Select your joystick/space mouse

![JoyStick](https://github.com/aurorasean/GamepadTo3DConnexion/blob/master/Documentation/SelectJoyStick.PNG)

* 2 Add an application partial title 'Application Window Title'

![Application](https://github.com/aurorasean/GamepadTo3DConnexion/blob/master/Documentation/CaptureNewApplication.PNG)

* 3 Fill out the settings to match your Application

![ApplicationSettings](https://github.com/aurorasean/GamepadTo3DConnexion/blob/master/Documentation/ApplicationSettings.PNG)

* 4 Press Save Settings

* 5 Go to the application you have specified in the application window name textbox


##### Example Settings File

If you don't want to go through the hassle of setting up the setting file then use the one in documentation [Setting.json](Documentation/SettingParent.json) and paste it in the Bin\Debug folder

##### Additional Setup in 3D Connexion

Disable the axis input for the application you are targeting, or your application will get more than one input sent to it

##### Application Settings Description

Application command timer: How much to seperate the mouse commands being sent to the application \(Milliseconds\)

Vector Lock: Send only the biggest vector\rotation value to the application, Best to leave checked, applications don't like many mouse movements

Grid Name: Axis name from gamepad

Grid Disabled: Check to disable the axis output

Grid Modifier: What modifier key to be sent with the mouse action

Grid Mouse Axis: What mouse axis to send to application

Grid Mouse Button: What button to send for the axis

Gird Multiplier: The multiplier to change the gamepad axis output by