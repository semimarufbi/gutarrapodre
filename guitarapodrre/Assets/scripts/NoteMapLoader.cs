using UnityEngine;
using System.Collections.Generic;

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

        while (proximaNotaIndex < mapa.notas.Length && tempoAtual >= mapa.notas[proximaNotaIndex].tempo)
        {
            SpawnNota(mapa.notas[proximaNotaIndex]);
            proximaNotaIndex++;
        }
    }

    void SpawnNota(NoteData nota)
    {
        if (nota.lane >= 0 && nota.lane < laneSpawners.Length)
        {
            GameObject novaNota = Instantiate(notePrefab, laneSpawners[nota.lane].transform.position, Quaternion.identity);
            noteObject noteComp = novaNota.GetComponent<noteObject>();
            noteComp.laneIndex = nota.lane;
        }
    }
}
