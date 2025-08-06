using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteSpa : MonoBehaviour
{
    public GameObject[] notePrefabs; // 0, 1, 2...
    public Transform[] spawnPositions; // posições da nota nas 3 linhas
    public string jsonFileName = "notemap"; // Nome do arquivo sem extensão

    private Note loadedData;
    private int nextNoteIndex = 0;

    private void Start()
    {
        LoadNoteMap();
    }

    void LoadNoteMap()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null)
        {
            Debug.LogError("Arquivo JSON não encontrado em Resources!");
            return;
        }

        loadedData = JsonUtility.FromJson<Note>(jsonFile.text);
    }

    void Update()
    {
        if (!gameManager.instance.startPlaying || loadedData == null) return;

        float songTime = gameManager.instance.theMusic.time;

        while (nextNoteIndex < loadedData.notes.Count &&
               loadedData.notes[nextNoteIndex].time <= songTime)
        {
            SpawnNote(loadedData.notes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    void SpawnNote(Note note)
    {
        if (note.line < 0 || note.line >= spawnPositions.Length)
        {
            Debug.LogError("Linha inválida: " + note.line);
            return;
        }

        Instantiate(notePrefabs[note.line], spawnPositions[note.line].position, Quaternion.identity);
    }
}
