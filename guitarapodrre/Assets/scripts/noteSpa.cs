using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteSpa : MonoBehaviour
{
    [Header("Configurações")]
    public List<GameObject> noteprefab;
    public Transform spawnPoint;
    public float bpm = 126f;
    public float noteTravelTime = 2f;

    private float beatInterval;
    private float songElapsedTime = 0f;
    private int notesSpawned = 0;
    private bool hasStarted = false;

    void Start()
    {
        if (noteprefab == null || noteprefab.Count == 0)
        {
            Debug.LogError("A lista de prefabs está vazia!");
            return;
        }

        beatInterval = 60f / bpm;
        hasStarted = true; // Se quiser sincronizar com música, altere isso
    }

    void Update()
    {
        if (!hasStarted) return;

        songElapsedTime += Time.deltaTime;

        // Quantas notas deveriam ter sido spawnadas até agora
        int totalNotesShouldBeSpawned = Mathf.FloorToInt(songElapsedTime / beatInterval);

        // Spawn de notas atrasadas
        while (notesSpawned < totalNotesShouldBeSpawned)
        {
            SpawnNote();
            notesSpawned++;
        }
    }

    void SpawnNote()
    {
        int index = Random.Range(0, noteprefab.Count);
        GameObject note = Instantiate(noteprefab[index], spawnPoint.position, Quaternion.identity);

        // Define tempo de viagem da nota
        beatScroller bs = note.GetComponent<beatScroller>();
        if (bs != null)
        {
            bs.SetTravelTime(noteTravelTime);
        }

        // Define tecla aleatória para pressionar
        KeyCode[] possibleKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };
        noteObject noteObj = note.GetComponent<noteObject>();
        if (noteObj != null)
        {
            noteObj.keyToPress = possibleKeys[Random.Range(0, possibleKeys.Length)];
        }
    }
}
