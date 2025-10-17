using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField]
    public TextAsset mapaJson;
    public GameObject[] laneSpawners;   // Posição inicial da nota para cada lane
    public GameObject[] notePrefabs;    // Prefab por lane

    public float leadTime = 0.2f; // Segundos antes do tempo da nota para spawnar

    private NoteMap mapa;
    private int proximaNotaIndex = 0;

    void Start()
    {
        if (mapaJson == null)
        {
            Debug.LogError("Mapa JSON não foi atribuído no Inspector!");
            return;
        }

        mapa = JsonUtility.FromJson<NoteMap>(mapaJson.text);

        if (mapa == null)
        {
            Debug.LogError("Falha ao carregar o mapa JSON!");
        }
    }

    void Update()
    {
        if (mapa == null || !gameManager.instance.startPlaying) return;

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
            GameObject go = Instantiate(notePrefabs[nota.line], pos, Quaternion.identity);

            // Se a nota tiver o componente NoteObject, passamos se ela é especial ou não
            NoteObject noteObj = go.GetComponent<NoteObject>();
            if (noteObj != null)
            {
                noteObj.Setup(nota.especial);
            }
        }
        else
        {
            Debug.LogError("Linha inválida ou prefab não atribuído: " + nota.line);
        }
    }

    // Marca as próximas N notas como super (especial)
    public void MarkNextNotesAsSuper(int count)
    {
        if (mapa == null || mapa.notas == null) return;

        int marked = 0;
        for (int i = proximaNotaIndex; i < mapa.notas.Length && marked < count; i++)
        {
            mapa.notas[i].especial = true;
            marked++;
        }
    }

    // Spawna uma nota imediatamente em uma lane específica
    public void SpawnInstantNoteOnLane(int lane, bool isSuper)
    {
        if (lane >= 0 && lane < laneSpawners.Length && lane < notePrefabs.Length)
        {
            Vector3 pos = laneSpawners[lane].transform.position;
            GameObject go = Instantiate(notePrefabs[lane], pos, Quaternion.identity);

            NoteObject noteObj = go.GetComponent<NoteObject>();
            if (noteObj != null)
            {
                noteObj.Setup(isSuper);
            }
        }
        else
        {
            Debug.LogError("Linha inválida para spawn instantâneo: " + lane);
        }
    }
}