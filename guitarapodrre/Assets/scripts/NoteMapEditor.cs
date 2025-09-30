using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class NoteMapEditor : EditorWindow
{
    [System.Serializable]
    public class Note
    {
        public float time;
        public int line;
        public bool especial; // NOVO: marca nota especial
    }

    [System.Serializable]
    public class NoteMap
    {
        public List<Note> notas = new List<Note>();
        public float bpm = 120f;
    }

    private NoteMap mapa = new NoteMap();

    private string saveFileName = "mapa";

    private AudioClip audioClip;
    private AudioSource audioSource;

    private float playTime = 0f;
    private bool isPlaying = false;

    private KeyCode[] laneKeys = new KeyCode[4] { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    private Vector2 scrollPos;

    // Controle do modo especial
    private bool modoEspecialAtivo = false;
    private int notasEspeciaisContadas = 0;
    private int limiteNotasEspeciais = 10;

    [MenuItem("Tools/Note Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<NoteMapEditor>("Note Map Editor").Show();
    }

    private void OnEnable()
    {
        GameObject go = new GameObject("AudioPlayer_Temp");
        go.hideFlags = HideFlags.HideAndDontSave;
        audioSource = go.AddComponent<AudioSource>();
        EditorApplication.update += UpdateEditor;

        this.Focus();
    }

    private void OnDisable()
    {
        if (audioSource != null)
        {
            DestroyImmediate(audioSource.gameObject);
        }
        EditorApplication.update -= UpdateEditor;
    }

    void UpdateEditor()
    {
        if (isPlaying && audioSource != null)
        {
            playTime = audioSource.time;
            Repaint();

            if (!audioSource.isPlaying)
            {
                isPlaying = false;
                playTime = 0f;
                Repaint();
            }
        }
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // Campo BPM
        mapa.bpm = EditorGUILayout.FloatField("BPM da música", mapa.bpm);

        GUILayout.Label("Audio para mapear", EditorStyles.boldLabel);
        audioClip = (AudioClip)EditorGUILayout.ObjectField(audioClip, typeof(AudioClip), false);

        if (audioClip != null && audioSource.clip != audioClip)
        {
            audioSource.clip = audioClip;
        }

        GUILayout.Space(10);

        // Controles play/pause/stop/reiniciar
        GUILayout.BeginHorizontal();

        if (!isPlaying)
        {
            if (GUILayout.Button("Play"))
            {
                if (audioClip != null)
                {
                    audioSource.Play();
                    isPlaying = true;
                }
            }
        }
        else
        {
            if (GUILayout.Button("Pause"))
            {
                audioSource.Pause();
                isPlaying = false;
            }
            if (GUILayout.Button("Stop"))
            {
                audioSource.Stop();
                isPlaying = false;
                playTime = 0f;
            }
        }

        if (GUILayout.Button("Reiniciar Música"))
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.time = 0f;
                playTime = 0f;
                isPlaying = false;
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Barra de progresso
        if (audioClip != null)
        {
            float progress = playTime / audioClip.length;
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), progress, $"Tempo: {playTime:F2} / {audioClip.length:F2}");
        }

        GUILayout.Space(10);

        GUILayout.Label("Teclas associadas às lanes:", EditorStyles.boldLabel);
        for (int i = 0; i < laneKeys.Length; i++)
        {
            GUILayout.Label($"Linha {i} : {laneKeys[i]}");
        }

        GUILayout.Space(20);

        // Status do modo especial
        if (modoEspecialAtivo)
        {
            EditorGUILayout.HelpBox($"[MODO ESPECIAL ATIVO] - Notas restantes: {limiteNotasEspeciais - notasEspeciaisContadas}", MessageType.Info);
        }

        GUILayout.Label("Notas atuais:", EditorStyles.boldLabel);
        if (mapa.notas.Count == 0)
        {
            GUILayout.Label("Nenhuma nota adicionada.");
        }
        else
        {
            for (int i = 0; i < mapa.notas.Count; i++)
            {
                GUILayout.BeginHorizontal();
                string especialTag = mapa.notas[i].especial ? " [especial]" : "";
                GUILayout.Label($"[{i}] Tempo: {mapa.notas[i].time:F2}s | Linha: {mapa.notas[i].line} (Tecla: {laneKeys[mapa.notas[i].line]}){especialTag}");
                if (GUILayout.Button("Remover"))
                {
                    mapa.notas.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(20);

        GUILayout.Label("Salvar arquivo JSON", EditorStyles.boldLabel);
        saveFileName = EditorGUILayout.TextField("Nome do arquivo", saveFileName);

        if (GUILayout.Button("Salvar"))
        {
            SalvarJson();
        }

        // Captura tecla para adicionar nota
        Event e = Event.current;
        if (e != null && e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.Space) // Ativa modo especial
            {
                modoEspecialAtivo = true;
                notasEspeciaisContadas = 0;
                Debug.Log("Modo especial ativado! As próximas 10 notas serão especiais.");
                e.Use();
            }
            else
            {
                for (int i = 0; i < laneKeys.Length; i++)
                {
                    if (e.keyCode == laneKeys[i])
                    {
                        AdicionarNota(playTime, i);
                        e.Use();
                        break;
                    }
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    void AdicionarNota(float time, int line)
    {
        Note nova = new Note { time = time, line = line };

        if (modoEspecialAtivo)
        {
            nova.especial = true;
            notasEspeciaisContadas++;

            if (notasEspeciaisContadas >= limiteNotasEspeciais)
            {
                modoEspecialAtivo = false;
                Debug.Log("Modo especial desativado após 10 notas.");
            }
        }

        mapa.notas.Add(nova);
        mapa.notas.Sort((a, b) => a.time.CompareTo(b.time));
        Debug.Log($"Nota adicionada: lane {line}, tempo {time:F2}s, especial: {nova.especial}");
        Repaint();
    }

    void SalvarJson()
    {
        string json = JsonUtility.ToJson(mapa, true);
        string path = Path.Combine(Application.dataPath, saveFileName + ".json");
        File.WriteAllText(path, json);
        Debug.Log($"Mapa salvo em: {path}");
        AssetDatabase.Refresh();
    }
}
