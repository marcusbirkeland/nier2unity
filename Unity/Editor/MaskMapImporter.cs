using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;


public class MaskMapImporter : EditorWindow
{
    //string textureFolderPath = "Assets/Textures";
    string materialsJsonPath = "Assets/materials.json";
    string textureName = "g_MaskMap";
    Shader shaderToReplace;
    Shader shaderNew;

    [MenuItem("MC / MaskmapImporter")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MaskMapImporter)); // Inheritedd from EditorWindow
    }

    public void OnEnable()
    {
        shaderToReplace = Shader.Find("Standard");
    }

    private void OnGUI()
    {
        GUILayout.Label("MC's Maskmap Importer!");
        //textureFolderPath = EditorGUILayout.TextField("Path to textures folder", textureFolderPath);
        materialsJsonPath = EditorGUILayout.TextField("Path to materials.json", materialsJsonPath);
        textureName = EditorGUILayout.TextField("Tex name", textureName);
        shaderToReplace = EditorGUILayout.ObjectField("Shader to replace", shaderToReplace, typeof(Shader), false) as Shader;
        shaderNew = EditorGUILayout.ObjectField("New shader", shaderNew, typeof(Shader), false) as Shader;

        if(GUILayout.Button("Set maskmaps!"))
        {
            setMaskmaps();
        }
    }

    private string getJSONString()
    {
        StreamReader reader = new StreamReader(materialsJsonPath);
        string json = reader.ReadToEnd();
        reader.Close();
        Debug.Log(json);
        return json;
    }

    private void setMaskmaps()
    {
        var materialData = SimpleJSON.JSON.Parse(getJSONString());
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach(object o in obj)
        {
            GameObject g = (GameObject) o;
            Debug.Log("Current GameObject: " + g.name);

            //Fetch the GameObject's Renderer component
            if(g.GetComponent<Renderer>() == null)
            {
                break;
            }
            Renderer r = g.GetComponent<Renderer>();
            Material material = r.sharedMaterial;

            // Iterate over the materials in the json file, and match with a material in game
            foreach (KeyValuePair<string,SimpleJSON.JSONNode> dict in materialData)
            {
                //Debug.Log("dict key: " + dict.Key + "  materialname: " + material.name);
                if (dict.Key.Equals(material.name) && dict.Value[textureName] != null && (material.shader.Equals(shaderToReplace) || material.shader.Equals(shaderNew)))
                {
                    // Get the texture identifier from JSON
                    string identifier = dict.Value[textureName];
                    Debug.Log("Identifier for " + dict.Key + "  =  " + identifier);
                    // Get the GUID(s) of the texture from the assetDatabase
                    string[] guids = AssetDatabase.FindAssets(identifier);
                    foreach(string guid in guids)
                    {
                        Debug.Log("GUID: " + guid + "\n");
                    }
                    // Set the new shader
                    material.shader = shaderNew;
                    material.EnableKeyword("_METALLICGLOSSMAP");
                    material.EnableKeyword("_OCCLUSIONMAP");
                    // Get and set the maskmap texture
                    try
                    {
                        Debug.Log("TEXTURE PATH: " + AssetDatabase.GUIDToAssetPath(guids[0]));
                        material.SetTexture("_MetallicGlossMap", (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(Texture2D)));
                        Debug.Log("Setting texture for " + material.name);
                    } catch (Exception e)
                    {
                        Debug.LogError("COULD NOT IMPORT TEXTURE:" + e.ToString());
                    }

                }
            }
        }
    }
}


