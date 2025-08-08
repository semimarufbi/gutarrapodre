using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteMapLoader : MonoBehaviour
{
    [Header("Configuração")]
    public TextAsset mapaJson;
    public GameObject[] laneSpawners; // Um para cada lane
    public GameObject notePrefab;
    public AudioSource musica;

    private NoteMap mapa;
    private int proximaNotaIndex = 0;

    void Start()
    {
        mapa = JsonUtility.FromJson<NoteMap>(mapaJson.text);
    }

    void Update()
    {
        if (mapa == null || !gameManager.instance.startPlaying) return;

        float tempoAtual = musica.time;

        // Verifica se está na hora de instanciar a próxima nota
        while (proximaNotaIndex < mapa.notas.Length && tempoAtual >= mapa.notas[proximaNotaIndex].time)
        {
            SpawnNota(mapa.notas[proximaNotaIndex]);
            proximaNotaIndex++;
        }
    }

    void SpawnNota(Note nota)
    {
        if (nota.line >= 0 && nota.line < laneSpawners.Length)
        {
            Vector3 posicao = laneSpawners[nota.line].transform.position;
            Instantiate(notePrefab, posicao, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Linha inválida: {nota.line}");
        }
    }
}
