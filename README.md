# NieR2Unity
A suite of tools to port maps from Nier/Astral Chain to Unity / VRCHAT. 

**NOTE: THIS DOES NOT WORK WITH REPLICANT ATM, BECAUSE THE BLENDER PLUGIN DOES NOT YET OUTPUT JSON FILES**

![banner](https://user-images.githubusercontent.com/36818485/130874731-4a7987f6-4522-44cb-9def-1076a375df32.png)



## Contents

- A custom version of Nier2Blender that creates a JSON file for the materials
- MaskmapImporter: A tool for copying the maskmaps from the blender export to Unity.
- A Unity Editor plugin that replaces shaders and assigns maskmaps to all materials
- Custom Rero-based shaders that support MSO(Automata/Astral Chain) or ORM (Replicant)maskmaps.

## How to use

1. Download this repository as a zip.
2.  Install the version of Nier2Blender plugin bundled here. **This version creates JSON files, which the original currently does not**
3. Import the files you want in Unity with Blender. When importing files with this plugin, it will create a materials.json file. 
- It is advised to have all the files you want to export to Unity in the same folder before importing with Blender. This way, all the identifiers are present in the materials.json file!
4. After you have imported all the models you are going to use, open the importer program included here. You do this by either:
- Running the exe bundled inside the "MaskmapImporter" folder.
or
- Running ```python main.py``` in a command line window inside the MaskmapImporter folder.

5. Now with the program, select your materials.json file (located in the same folder as the files you imported to blender). Select the texture directory where the maskmaps are, and finally select the directory inside your Unity project where you want them copied over to. Press the button!

![smoler py](https://user-images.githubusercontent.com/36818485/130875055-2dcc9e02-c52b-4edf-95e6-7633aa6fa69c.png)

6. Open Unity, and copy over the contents of the "Unity" folder included in this repository to the "Assets" folder.
7. Copy over the materials.json file into the root of the "Assets" folder.
8. In the toolbar, click MC, and then MaskmapImporter

![bilde](https://user-images.githubusercontent.com/36818485/130873243-291c3cd1-4abf-4d4b-9737-ff28b88733f2.png)

9. An editor window should open. Normally, leave the first two values alone. Then select the shader you want to replace with a maskmap-compatible one. This should be the Standard shader most of the time. Then select the maskmap-compatible shader to replace with. Bundled with this zip are ReroStandardAutomata, and ReroStandardReplicant.
10. Press "Set maskmaps" ! 

![bilde](https://user-images.githubusercontent.com/36818485/130875382-01f30b5c-7fa5-40c1-b748-68af1b6b2b8f.png)


11. DONE :D


## KNOWN ISSUES

- When using the maskmap tool in Unity, sometimes it doesn't detect the gameobjects in the scene. Mask maps will not be applied when this happens. To fix this, try reimporting the gameobject in the scene, or removing the object from the scene, and adding it again. 

## CREDITS

- WoefulWolf for making [Nier2Blender_2.8](https://github.com/Zprite/NieR2Blender_2_8/tree/master)
- Modified shaders from RetroGeo's [Rero Standard](https://github.com/RetroGEO/reroStandard)
