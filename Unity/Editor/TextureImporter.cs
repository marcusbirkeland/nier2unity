using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public enum OPTIONS
{
    ALBEDO = 0,
    NORMAL = 1,
    MASKMAP = 2
}

public class TextureImporter : EditorWindow
{
    public OPTIONS op;
    //string textureFolderPath = "Assets/Textures";
    string materialsJsonPath = "Assets/materials.json";
    string textureName = "g_AlbedoMap";
    Shader shaderToReplace;
    Shader shaderNew;

    [MenuItem("MC / Texture Importer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TextureImporter)); // Inheritedd from EditorWindow
    }

    public void OnEnable()
    {
        shaderToReplace = Shader.Find("Standard");
    }

    private void OnGUI()
    {
        GUILayout.Label("MC's Texture Importer!");
        //textureFolderPath = EditorGUILayout.TextField("Path to textures folder", textureFolderPath);
        materialsJsonPath = EditorGUILayout.TextField("Path to materials.json", materialsJsonPath);
        op = (OPTIONS)EditorGUILayout.EnumPopup("Map to replace in shader:", op);
        textureName = EditorGUILayout.TextField("Tex name", textureName);
        shaderToReplace = EditorGUILayout.ObjectField("Shader to replace", shaderToReplace, typeof(Shader), false) as Shader;
        shaderNew = EditorGUILayout.ObjectField("New shader", shaderNew, typeof(Shader), false) as Shader;
        if (GUILayout.Button("Set textures (specified map)"))
        {
            setTextures();
        }
        if (GUILayout.Button("Set textures (all maps)"))
        {
            setAllTextures();
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

    private string getTextureOption()
    {
        switch (op)
        {
            case OPTIONS.ALBEDO:
                return "_MainTex";
            case OPTIONS.NORMAL:
                return "_BumpMap";
            case OPTIONS.MASKMAP:
                return "_MetallicGlossMap";
            default:
                Debug.LogError("Unrecognized Option");
                return "";
        }
    }

    private void setTextures()
    {
        var materialData = SimpleJSON.JSON.Parse(getJSONString());
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        //Debug.Log("Num game objects" + obj.Length);
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            //Fetch the GameObject's Renderer component
            if (g.GetComponent<Renderer>() == null)
            {
                break;
            }
            Renderer r = g.GetComponent<Renderer>();
            Material material = r.sharedMaterial;
            Debug.Log("Current GameObject: " + g.name + "  Current material: " + material.name);
            // Iterate over the materials in the json file, and match with a material in game
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> dict in materialData)
            {
                //Debug.Log("dict key: " + dict.Key + "  materialname: " + material.name);
                if (material.name.Contains(dict.Key) && dict.Value[textureName] != null && (material.shader.Equals(shaderToReplace) || material.shader.Equals(shaderNew)))
                {
                    // Get the texture identifier from JSON
                    string identifier = dict.Value[textureName];
                    //Debug.Log("Identifier for " + material.name + "  =  " + identifier);
                    // Get the GUID(s) of the texture from the assetDatabase
                    string[] guids = AssetDatabase.FindAssets(identifier);
                    foreach (string guid in guids)
                    {
                        Debug.Log("GUID: " + guid + "\n");
                    }
                    // Set the new shader
                    material.shader = shaderNew;
                    string textureSelector = getTextureOption();
                    // Get and set the maskmap texture
                    setTextureInMaterial(material, guids, textureSelector, textureSelector == "_MetallicGlossMap");
                }
            }
        }
    }


    private void setTextureInMaterial(Material material, String [] guids, String textureSelector, bool isMaskMap = false)
    {
        try
        {
            if (guids.Length > 0)
            {
                Debug.Log("TEXTURE PATH: " + AssetDatabase.GUIDToAssetPath(guids[0]));
                material.SetTexture(textureSelector, (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(Texture2D)));
                Debug.Log("Setting texture for " + material.name);
                if (isMaskMap)
                {
                    material.EnableKeyword("_METALLICGLOSSMAP");
                    material.EnableKeyword("_OCCLUSIONMAP");
                }

                material.EnableKeyword("_BUMPMAP");
                material.EnableKeyword("_NORMALMAP");
            }
            else
            {
                Debug.Log("Texture not found, skipping");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("COULD NOT IMPORT TEXTURE " + material.name + "\n" + e.ToString());
        }
    }

    private void setAllTextures()
    {
        var materialData = SimpleJSON.JSON.Parse(getJSONString());
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        //Debug.Log("Num game objects" + obj.Length);
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            //Fetch the GameObject's Renderer component
            if (g.GetComponent<Renderer>() == null)
            {
                break;
            }
            Renderer r = g.GetComponent<Renderer>();
            Material material = r.sharedMaterial;
            Debug.Log("Current GameObject: " + g.name + "  Current material: " + material.name);
            // Iterate over the materials in the json file, and match with a material in game
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> dict in materialData)
            {
                //Debug.Log("dict key: " + dict.Key + "  materialname: " + material.name);
                if (material.name.Contains(dict.Key) && dict.Value[textureName] != null && (material.shader.Equals(shaderToReplace) || material.shader.Equals(shaderNew)))
                {
                    // Get the texture identifier from JSON
                    string identifierAlbedo = dict.Value["g_AlbedoMap"];
                    string identifierNormal = dict.Value["g_NormalMap"];
                    string identifierMaskMap = dict.Value["g_MaskMap"];
                    //Debug.Log("Identifier for " + material.name + "  =  " + identifier);
                    // Get the GUID(s) of the texture from the assetDatabase
                    string[] guidsAlbedo = AssetDatabase.FindAssets(identifierAlbedo);
                    string[] guidsNormal = AssetDatabase.FindAssets(identifierNormal);
                    string[] guidsMaskMap = AssetDatabase.FindAssets(identifierMaskMap);

                    // Set the new shader
                    material.shader = shaderNew;

                    setTextureInMaterial(material, guidsAlbedo, "_MainTex");
                    setTextureInMaterial(material, guidsNormal, "_BumpMap");
                    setTextureInMaterial(material, guidsMaskMap, "_MetallicGlossMap", true);

                }
            }
        }
    }
}
