using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoolingData))]
public class PoolingDataEditor : Editor
{
    private SerializedProperty pooledObejctReferences;
    private SerializedProperty pooledObjectCounts;
    private void OnEnable()
    {
        this.pooledObejctReferences = serializedObject.FindProperty("toBePooledAtStart"); ;
        this.pooledObjectCounts = serializedObject.FindProperty("counts");
    }



    private bool shouldFoldOut = true;
    public override void OnInspectorGUI()
    {
        this.shouldFoldOut = EditorGUILayout.Foldout(this.shouldFoldOut, "To Be Pooled At Beginning");
        if (shouldFoldOut)
            this.DrawPooledObjectsList();

        this.AddElementButton();
        this.RemoveLastElementButton();
        this.RemoveElementAtButton();
    }


    private void DrawPooledObjectsList()
    {
        int count = pooledObejctReferences.arraySize;

        for (int i = 0; i < count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.indentLevel = 1;
            SerializedProperty pooledReference = pooledObejctReferences.GetArrayElementAtIndex(i);
            EditorGUIUtility.labelWidth = 75f;
            EditorGUILayout.PropertyField(pooledReference);

            EditorGUI.indentLevel = 0;
            SerializedProperty pooledCount = pooledObjectCounts.GetArrayElementAtIndex(i);
            EditorGUIUtility.labelWidth = 45f;
            EditorGUILayout.PropertyField(pooledCount, new GUIContent("Count"), GUILayout.Width(100f));

            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }


    private void AddElementButton()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button("Add Element")) {
            this.pooledObejctReferences.arraySize++;
            this.pooledObjectCounts.arraySize++;
        }
    }


    private void RemoveLastElementButton()
    {
        if (GUILayout.Button("Remove Element")) {
            this.pooledObejctReferences.arraySize--;
            this.pooledObjectCounts.arraySize--;
        }
    }


    private int removeIndex;
    private void RemoveElementAtButton()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Remove Element At")) {
            this.pooledObejctReferences.DeleteArrayElementAtIndex(this.removeIndex);
            this.pooledObjectCounts.DeleteArrayElementAtIndex(this.removeIndex);
        }

        this.removeIndex = EditorGUILayout.IntField(removeIndex, GUILayout.Width(45f));

        EditorGUILayout.EndHorizontal();
    }
}
