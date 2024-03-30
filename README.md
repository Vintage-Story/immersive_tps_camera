# Immersive Third Person Camera
Smoothly changes third-person camera positions, improving player immersion in the world

### Before
![image](https://github.com/LeandroTheDev/immersive_tps_camera/assets/106118473/0f1aacd3-8878-4ebe-b38c-7795cf29001c)
### After
![image](https://github.com/LeandroTheDev/immersive_tps_camera/assets/106118473/b504f6e5-a18d-43a8-9326-80474ecb75c5)
![image](https://github.com/LeandroTheDev/immersive_tps_camera/assets/106118473/7620e77c-dfa8-461d-bac2-fd2527f8faae)

### Using
Download the latest version from [releases](https://github.com/LeandroTheDev/immersive_tps_camera/releases)

Place the mod in Mods folder vintage story


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
