import tkinter as tk
from tkinter import filedialog, messagebox
from tkinter.ttk import *
import os
import json
from shutil import copyfile

def copyFiles(infile, outfile):
  print("copying ", infile, "to", outfile)
  copyfile(infile, outfile)

def importTextures(jsonPath, indir, outdir):
  
  completedTasks = 1
  print("Moving textures!")
  
  f = open(jsonPath, 'r')
  jsonData = json.load(f)
  f.close()
  
  files = [f for f in os.listdir(indir) if os.path.isfile(os.path.join(indir,f))]
  for material in jsonData:
    for f in files:
      if not textureName.get() in jsonData[material]:
        break
      maskmapIdentifier = jsonData[material][textureName.get()]
      if f.__contains__(maskmapIdentifier):
        copyFiles(indir+'//'+f, outdir+'//'+f)
        completedTasks += 1
        updateProgress(len(jsonData), completedTasks, str(material))
        break
  updateProgress(1, 1, "Done")

def loadJson():
  filename = tk.filedialog.askopenfilename(initialdir="/", title="Select file", filetypes=(("json files", "*.json"),("all files", "*.*")))

  print(filename)
  jsonFilename.set(filename)

def loadTextureDir():
  directory = filedialog.askdirectory(initialdir="/",title="Select texture directory")
  print(directory)
  textureDir.set(directory)

def loadUnityDir ():
  directory = filedialog.askdirectory(initialdir="/",title="Select Unity texture directory")
  print(directory)
  unityDir.set(directory)

def executeImport ():
  print("import clicked")
  errorMessage.set("")
  if jsonFilename.get() == "":
    print("Please provide a Materials.JSON file! It can be found in the nier2blender extracted folder!")
    errorMessage.set("Missing materials.json! It can be found in the nier2blender extracted folder")
    return
  elif textureDir.get() == "" :
    print("Please provide a input texture directory")
    errorMessage.set("Please provide a input texture directory")
    return
  elif unityDir.get() == "" :
    print("Please provide a output texture directory")
    errorMessage.set("Please provide a output texture directory")
    return
  
  #Import!
  importTextures(jsonFilename.get(),textureDir.get(), unityDir.get())

  messagebox.showinfo("DONE!","Files copied successfully!")
  

def updateProgress(numTasks, completedTasks, currentTask):
  bar['value'] = completedTasks/numTasks * 100
  progressPercent.set(str(completedTasks/numTasks * 100)+"%")
  progressCurrentTask.set("Copying " + str(currentTask))
  root.update_idletasks()


root = tk.Tk()
root.resizable(False, False)
root["bg"] = "black"
root.title('MCs texture importer')

jsonFilename = tk.StringVar(root)
textureDir = tk.StringVar(root)
textureName = tk.StringVar(root)
textureName.set("g_MaskMap")
unityDir = tk.StringVar(root)
errorMessage = tk.StringVar(root)
progressPercent = tk.StringVar(root)
progressCurrentTask = tk.StringVar(root)


main = tk.Canvas(root, height = 900, width = 900, bg="#333", highlightthickness=0)
main.pack()

titleFrame = tk.Frame(main, bg="#333", width=1000)
titleFrame.pack()

titleLabel = tk.Label(titleFrame, text="MC's texture importer!",fg="white", bg="#333", pady=32, font=("Arial", 24), width="20")
titleLabel.pack()

mainFrame = tk.Frame(main, bg="#333", padx=24, pady=8)
mainFrame.pack()

# JSON Section
jsonLabel = tk.Label(mainFrame, text="Materials.json :",fg="white", bg="#444", padx=2)
jsonLabel.grid(row=1,column= 0)
jsonEntry = tk.Entry(mainFrame, fg="white", bg="#222", width=48 , textvariable=jsonFilename)
jsonEntry.grid(row=1,column= 1, pady=4, )
openJsonFile = tk.Button(mainFrame, text="Browse...", fg="white", bg="#222" ,command=loadJson, width=10)
openJsonFile.grid(row=1, column= 2 , padx= 6, pady=4)

# Texture Type Section
typeLabel = tk.Label(mainFrame, text="Texture name:",fg="white", bg="#444", padx=2)
typeLabel.grid(row=2,column= 0)
typeEntry = tk.Entry(mainFrame, fg="white", bg="#222", width=48 , textvariable=textureName)
typeEntry.grid(row=2,column= 1, pady=4, )

# Texture Section
textureLabel = tk.Label(mainFrame, text="Texture folder :",fg="white", bg="#444", padx=2)
textureLabel.grid(row=3,column= 0)
textureEntry = tk.Entry(mainFrame, fg="white", bg="#222", width=48, textvariable=textureDir)
textureEntry.grid(row=3,column= 1, pady=4, )
openTextureFolder = tk.Button(mainFrame, text="Browse...", fg="white", bg="#222" ,command=loadTextureDir, width=10,)
openTextureFolder.grid(row=3, column= 2 , padx= 6, pady=4)

# Unity Section
unityLabel = tk.Label(mainFrame, text="Unity Texture Folder:",fg="white", bg="#444", padx=2)
unityLabel.grid(row=4,column= 0)
unityEntry = tk.Entry(mainFrame, fg="white", bg="#222", width=48, textvariable=unityDir)
unityEntry.grid(row=4,column= 1, pady=4, )
openUnityFolder = tk.Button(mainFrame, text="Browse...", fg="white", bg="#222" ,command=loadUnityDir, width=10)
openUnityFolder.grid(row=4, column= 2 , padx= 6, pady=4)

#Execute section
execFrame = tk.Frame(main, bg="#333", width=1000)
execFrame.pack()
executeButton = tk.Button(execFrame, text="Import textures to Unity!",fg="white", bg="#222", width=64, command=executeImport, pady=4 )
executeButton.pack()
progressFrame = tk.Frame(main, bg="#333", width=1000, pady=12)
progressFrame.pack()
errorLabel = tk.Label(progressFrame,textvariable=errorMessage, fg="white", bg="#333", pady=4)
errorLabel.pack()
bar = Progressbar(progressFrame, length=450)
bar.pack(pady=2)
percentLabel = tk.Label(progressFrame,textvariable=progressPercent, fg="white", bg="#333", pady=4)
percentLabel.pack()
taskLabel = tk.Label(progressFrame,textvariable=progressCurrentTask, fg="white", bg="#333", pady=4)
taskLabel.pack()

root.mainloop()