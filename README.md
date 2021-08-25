# NieR2Unity
A suite of tools to port maps from Nier/Astral Chain to Unity / VRCHAT. 

## Contents

- A custom version of Nier2Blender that creates a JSON file for the materials
- MaskmapImporter: A tool for copying the maskmaps from the blender export to Unity.
- A Unity Editor plugin that automatically replaces shaders and assigns maskmaps
- Custom Rero-based shaders that support MSO(Automata/Astral Chain) or ORM (Replicant)maskmaps.

![image smol](https://user-images.githubusercontent.com/36818485/130872143-1e5cf304-6b4e-41c1-84c1-e388934536ba.png)

## How to use

1. Download this repository as a zip.
2.  Install the version of Nier2Blender plugin bundled here.
3. Import the files you want in Unity with Blender. When importing files with this plugin, it will create a materials.json file. 
- It is advised to have all the files you want to export to Unity in the same folder before importing with Blender. This way, all the identifiers are present in the materials.json file!
4. After you have imported all the models you are going to use, open the MaskmapImporter program included here. You do this by either:
- Running the exe bundled inside the "MaskmapImporter" folder.
or
- Running ```python main.py``` in a command line window inside the MaskmapImporter folder.
5. Now with the program, select your materials.json file (located in the same folder as the files you imported to blender). Select the texture directory where the maskmaps are, and finally select the directory inside your Unity project where you want them copied over to. Press the button!

![python_k4X2lZOMuk](https://user-images.githubusercontent.com/36818485/130873691-6b5d6474-e394-4598-a113-40ef4e0eab7e.png)


7. Open Unity, and copy over the contents of the "Unity" folder included in this repository.
8. Copy over the materials.json file into the root of the "Assets" folder.
9. In the toolbar, click MC, and then MaskmapImporter

![bilde](https://user-images.githubusercontent.com/36818485/130873243-291c3cd1-4abf-4d4b-9737-ff28b88733f2.png)

9. An editor window should open. Normally, leave the first two values alone. Then select the shader you want to replace with a maskmap-compatible one. This should be the Standard shader most of the time. Then select the maskmap-compatible shader to replace with. Bundled with this zip are ReroStandardAutomata, and ReroStandardReplicant.

![M88WuikK2s](https://user-images.githubusercontent.com/36818485/130873720-6134cda0-cb90-49cb-a8b5-b524fbf63810.png)

11. Press "Set maskmaps" ! 
12. DONE :D




