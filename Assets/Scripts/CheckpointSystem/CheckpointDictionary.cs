using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class RespawnPointDictionary : MonoBehaviour
{
    [SerializedDictionary("Checkpoints", "RespawnPoints")]
    [SerializeField] private SerializedDictionary<GameObject, GameObject> respawnPoints;

    public void UpdateDictionary()
    {
        Checkpoint[] respawnPointsArray = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in respawnPointsArray)
        {
            if (checkpoint != null && checkpoint.respawnPoint != null && respawnPoints.ContainsKey(checkpoint.gameObject))
            {
                checkpoint.respawnPoint = respawnPoints[checkpoint.gameObject];
            }
        }

        respawnPoints.Clear();

        foreach (Checkpoint Checkpoint in respawnPointsArray)
        {
            if (Checkpoint != null)
            {
                if (Checkpoint.respawnPoint != null)
                {
                    respawnPoints.Add(Checkpoint.gameObject, Checkpoint.respawnPoint);
                }
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(RespawnPointDictionary))]
public class RespawnPointDictionaryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RespawnPointDictionary script = (RespawnPointDictionary)target;
        if (GUILayout.Button("Update Dictionary"))
        {
            script.UpdateDictionary();
        }
    }
}
#endif