using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteSpa : MonoBehaviour
{
    [Header("Notas e Lanes")]
    public List<GameObject> noteprefab;
    public List<Transform> spawnPositions;

    [Header("Configurações de Tempo")]
    public float bpm = 126f;
    public float noteTravelTime = 2f;
    public float minNoteSpacing = 0.1f;

    [Header("Controle de Lanes")]
    public int maxNotesPerLine = 3;

    [Header("Música")]
    public AudioSource musica; // <-- arraste o AudioSource aqui no Inspector

    private float beatInterval;
    private int notesSpawned = 0;
    private float lastNoteSpawnTime = -999f;
    private int[] notesInLane;
    private bool hasStarted = false;

    void Start()
    {
        if (noteprefab == null || noteprefab.Count == 0)
        {
            Debug.LogError("A lista de prefabs está vazia!");
            return;
        }

        if (spawnPositions == null || spawnPositions.Count != noteprefab.Count)
        {
            Debug.LogError("A quantidade de posições deve ser igual à de prefabs!");
            return;
        }

        if (musica == null)
        {
            Debug.LogError("A música (AudioSource) não está atribuída!");
            return;
        }

        notesInLane = new int[spawnPositions.Count];
        beatInterval = 60f / bpm;
        musica.Play();
        hasStarted = true;
    }

    void Update()
    {
        if (!hasStarted || !musica.isPlaying)
            return;

        float songElapsedTime = musica.time;
        int totalNotesShouldBeSpawned = Mathf.FloorToInt(songElapsedTime / beatInterval);

        if (notesSpawned < totalNotesShouldBeSpawned && Time.time - lastNoteSpawnTime >= minNoteSpacing)
        {
            TrySpawnNote();
            notesSpawned++;
            lastNoteSpawnTime = Time.time;
        }
    }

    void TrySpawnNote()
    {
        int attempts = 0;
        const int maxAttempts = 10;

        while (attempts < maxAttempts)
        {
            int laneIndex = Random.Range(0, spawnPositions.Count);

            if (notesInLane[laneIndex] < maxNotesPerLine)
            {
                SpawnNoteInLane(laneIndex);
                break;
            }

            attempts++;
        }
    }

    void SpawnNoteInLane(int laneIndex)
    {
        Transform spawnPoint = spawnPositions[laneIndex];
        GameObject note = Instantiate(noteprefab[laneIndex], spawnPoint.position, Quaternion.identity);

        beatScroller bs = note.GetComponent<beatScroller>();
        if (bs != null)
        {
            bs.SetTravelTime(noteTravelTime);
        }

        KeyCode[] possibleKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };
        noteObject noteObj = note.GetComponent<noteObject>();
        if (noteObj != null && laneIndex < possibleKeys.Length)
        {
            noteObj.keyToPress = possibleKeys[laneIndex];
            noteObj.laneIndex = laneIndex;
            noteObj.spawner = this;
        }

        notesInLane[laneIndex]++;
    }

    public void OnNoteDestroyed(int laneIndex)
    {
        if (laneIndex >= 0 && laneIndex < notesInLane.Length)
        {
            notesInLane[laneIndex] = Mathf.Max(0, notesInLane[laneIndex] - 1);
        }
    }
}
