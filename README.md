# Version to choose
| Game Version         | Mod Version          |
|----------------------|----------------------|
| 1.20-rc and upper    | 1.0.7 and above      |
| 1.20-pre             | 1.0.6                |
| 1.19 and below       | 1.0.5 and below      |

# Immersive Third Person Camera
Smoothly changes third-person camera positions, improving player immersion in the world

### Observations
This mod modifies the native camera function from vintage story, and probably might break in new updates.

### Considerations
Calculation for the camera position is quietly simple so no performance can be impacted.

# About Immersive Third Person Camera
Immersive Third Person Camera is open source project and can easily be accessed on the github, all contents from this mod is completly free.

If you want to contribute into the project you can access the project github and make your pull request.

You are free to fork the project and make your own version of Immersive Third Person Camera, as long the name is changed.

# Building
- Install .NET in your system, open terminal type: ``dotnet new install VintageStory.Mod.Templates``
- Create a template with the name ``ImmersiveTPSCamera``: ``dotnet new vsmod --AddSolutionFile -o ImmersiveTPSCamera``
- [Clone the repository](https://github.com/LeandroTheDev/immersive_tps_camera/archive/refs/heads/main.zip)
- Copy the ``CakeBuild`` and ``build.ps1`` or ``build.sh`` and paste inside the repository

Now you can build using the ``build.ps1`` or ``build.sh`` file

FTM License
