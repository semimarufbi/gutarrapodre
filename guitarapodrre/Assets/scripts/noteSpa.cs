using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public TextAsset mapaJson;
    public GameObject[] laneSpawners;   // posição inicial da nota para cada lane
    public GameObject[] notePrefabs;    // prefab por lane

    public float leadTime = 1f; // segundos antes do tempo da nota para spawnar

    private NoteMap mapa;
    private int proximaNotaIndex = 0;

    void Start()
    {
        mapa = JsonUtility.FromJson<NoteMap>(mapaJson.text);
    }

    void Update()
    {
        if (mapa == null || !gameManager.instance.startPlaying) return;

        // Pega o tempo atual da música direto do gameManager
        float tempoAtual = gameManager.instance.theMusic.time;

        while (proximaNotaIndex < mapa.notas.Length &&
               tempoAtual >= mapa.notas[proximaNotaIndex].time - leadTime)
        {
            SpawnNota(mapa.notas[proximaNotaIndex]);
            proximaNotaIndex++;
        }
    }

    void SpawnNota(Note nota)
    {
        if (nota.line >= 0 && nota.line < laneSpawners.Length && nota.line < notePrefabs.Length)
        {
            Vector3 pos = laneSpawners[nota.line].transform.position;
            Instantiate(notePrefabs[nota.line], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Linha inválida ou prefab não atribuído: " + nota.line);
        }
    }
}

