using UnityEngine;

public class NoteMapLoader : MonoBehaviour
{
    public TextAsset mapaJson;
    public GameObject[] laneSpawners; // 1 para cada lane
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
        if (!gameManager.instance.startPlaying || mapa == null) return;

        float tempoAtual = musica.time;

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
            GameObject novaNota = Instantiate(notePrefab, laneSpawners[nota.line].transform.position, Quaternion.identity);
            noteObject noteComp = novaNota.GetComponent<noteObject>();
            noteComp.laneIndex = nota.line;
        }
        else
        {
            Debug.LogError($"Linha inválida: {nota.line}");
        }
    }
}
