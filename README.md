# Immersive Third Person Camera
This overhaul

### Building
Learn more about vintage story modding in [Linux](https://github.com/LeandroTheDev/arch_linux/wiki/Games#vintage-story-modding) or [Windows](https://wiki.vintagestory.at/index.php/Modding:Setting_up_your_Development_Environment)

> Not necessary but recomendend, if you dont do that you need to get the all the files in Releases/immersivetpscamera/ and add into Mods folder from vintage story every time you build for test

Make a symbolic link for fast tests
- ln -s /path/to/project/Releases/immersivetpscamera/ImmersiveTPSCamera.dll /path/to/game/Mods/ImmersiveTPSCamera/Immersivetpscamera.dll
- ln -s /path/to/project/Releases/immersivetpscamera/ImmersiveTPSCamera.pdb /path/to/game/Mods/ImmersiveTPSCamera/Immersivetpscamera.pdb
- ln -s /path/to/project/Releases/immersivetpscamera/ImmersiveTPSCamera.deps.json /path/to/game/Mods/ImmersiveTPSCamera/ImmersiveTPSCamera.deps.json
- ln -s /path/to/project/Releases/immersivetpscamera/modinfo.json /path/to/game/Mods/ImmersiveTPSCamera/modinfo.json

Execute the comamnd ./build.sh, consider having setup everthing from vintage story ide before

FTM License