    using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// This Class is use for creating the horse layer for the horses and set ignore collision with the water
/// </summary>
namespace MalbersAnimations
{
    [InitializeOnLoad]
    public class AnimalLayer : Editor
    {
        #if UNITY_EDITOR

        static AnimalLayer()
        {
            CreateLayer();
            CreateTag("Fly");
            CreateTag("Climb");
           // CreateInputAxe();
        }

        static void CreateLayer()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layers = tagManager.FindProperty("layers");

            if (layers == null || !layers.isArray)
            {
                Debug.LogWarning("Can't set up the layers.  It's possible the format of the layers and tags data has changed in this version of Unity.");
                Debug.LogWarning("Layers is null: " + (layers == null));
                return;
            }

            if (LayerMask.GetMask("Animal") == 0)
            {
                var layerSP = layers.GetArrayElementAtIndex(20);

                if (layerSP.stringValue == string.Empty)
                {
                    Debug.Log("Setting up layers.  Layer " + "[20]" + " is now called " + "[Animal]");
                    layerSP.stringValue = "Animal";

                    Physics.IgnoreLayerCollision(4, 20);
                    tagManager.ApplyModifiedProperties();
                }
            }

            if (LayerMask.GetMask("Enemy") == 0) 
            {
                var layerEnemy = layers.GetArrayElementAtIndex(23);

                if (layerEnemy.stringValue == string.Empty)
                {
                    Debug.Log("Setting up layers.  Layer " + "[23]" + " is now called " + "[Enemy]");
                    layerEnemy.stringValue = "Enemy";

                    Physics.IgnoreLayerCollision(4, 20);
                    tagManager.ApplyModifiedProperties();
                }
            }
        }


        static void CreateTag(string s)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // First check if it is not already present
            bool found = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(s)) { found = true; break; }
            }

            // if not found, add it
            if (!found)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                n.stringValue = s;
                Debug.Log("Tag: <B>"+s+"</B> Added");
                tagManager.ApplyModifiedProperties();
            }
        }
#endif
    }
}