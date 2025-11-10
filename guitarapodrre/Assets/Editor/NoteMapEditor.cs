using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class NoteMapEditor : EditorWindow
{
    [MenuItem("Tools/Note Map Editor")] // 🔹 ESSA LINHA É O QUE FAZ APARECER NO MENU
    public static void ShowWindow()
    {
        GetWindow<NoteMapEditor>("Note Map Editor").Show();
    }

    [System.Serializable]
    public class Note
    {
        public float time;
        public int line;
        public bool especial;
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

    // Delay antes de tocar (só no editor)
    private float startDelay = 0f;

    // 🔹 Snap (subdivisão por beat)
    private int subdivisao = 4;
    private readonly int[] subdivOptions = new int[] { 1, 2, 4, 8, 16 };
    private readonly string[] subdivLabels = new string[] { "1/1", "1/2", "1/4", "1/8", "1/16" };

    // 🔹 Waveform
    private Texture2D waveformTexture;
    private int waveformWidth = 1024;
    private int waveformHeight = 120;


    

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
        if (audioSource == null) return;

        // Atualiza playTime com o tempo atual do AudioSource
        if (isPlaying)
        {
            playTime = audioSource.time;
            Repaint();
        }

        // Se o áudio acabou fora do editor (quando não estamos em playDelayed), estabiliza flags
        if (!audioSource.isPlaying && isPlaying && audioClip != null && audioSource.time >= audioClip.length - 0.01f)
        {
            isPlaying = false;
            playTime = 0f;
            Repaint();
        }
    }

    void OnGUI()
    {
        // Captura eventos de teclado ANTES da GUI
        Event e = Event.current;
        if (e != null && e.type == EventType.KeyDown)
        {
            // Ativa modo especial com Space (mantive sua lógica)
            if (e.keyCode == KeyCode.Space)
            {
                modoEspecialAtivo = true;
                notasEspeciaisContadas = 0;
                Debug.Log("Modo especial ativado! As próximas " + limiteNotasEspeciais + " notas serão especiais.");
                e.Use();
            }
            else if (e.keyCode == KeyCode.LeftArrow) // retrocede
            {
                float delta = e.shift ? 5f : 1f;
                playTime = Mathf.Max(0f, playTime - delta);
                if (audioSource.clip != null) audioSource.time = playTime;
                e.Use();
            }
            else if (e.keyCode == KeyCode.RightArrow) // avança
            {
                float delta = e.shift ? 5f : 1f;
                if (audioSource.clip != null)
                {
                    playTime = Mathf.Min(audioSource.clip.length, playTime + delta);
                    audioSource.time = playTime;
                }
                e.Use();
            }
            else
            {
                // Captura as teclas das lanes e adiciona nota com snap
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

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // BPM
        mapa.bpm = EditorGUILayout.FloatField("BPM da música", mapa.bpm);

        // Subdivisão configurável (snap)
        subdivisao = EditorGUILayout.IntPopup("Subdivisão (snap)", subdivisao, subdivLabels, subdivOptions);

        // Áudio
        GUILayout.Label("Áudio para mapear", EditorStyles.boldLabel);
        AudioClip newClip = (AudioClip)EditorGUILayout.ObjectField(audioClip, typeof(AudioClip), false);
        if (newClip != audioClip)
        {
            audioClip = newClip;
            if (audioSource != null) audioSource.clip = audioClip;
            GenerateWaveformTexture();
        }

        GUILayout.Space(8);

        // Delay inicial (só editor)
        startDelay = EditorGUILayout.FloatField("Delay inicial (só editor)", startDelay);

        GUILayout.Space(10);

        // Controles de play/pause/stop
        GUILayout.BeginHorizontal();
        if (!isPlaying)
        {
            if (GUILayout.Button("Play"))
            {
                if (audioClip != null)
                {
                    audioSource.Stop();
                    audioSource.time = playTime;
                    audioSource.PlayDelayed(startDelay);
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

        // Timeline slider
        if (audioClip != null)
        {
            float progress = audioClip.length > 0f ? playTime / audioClip.length : 0f;
            float newProgress = EditorGUILayout.Slider("Timeline", progress, 0f, 1f);
            if (Mathf.Abs(newProgress - progress) > 0.0001f)
            {
                playTime = newProgress * audioClip.length;
                audioSource.time = playTime;
                Repaint();
            }

            GUILayout.Label($"Tempo atual: {FormatTime(playTime)} / {FormatTime(audioClip.length)}");
        }

        GUILayout.Space(12);

        GUILayout.Label("Teclas associadas às lanes:", EditorStyles.boldLabel);
        for (int i = 0; i < laneKeys.Length; i++)
        {
            GUILayout.Label($"Linha {i} : {laneKeys[i]}");
        }

        GUILayout.Space(12);

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
                GUILayout.Label($"[{i}] Tempo: {mapa.notas[i].time:F3}s | Linha: {mapa.notas[i].line} (Tecla: {laneKeys[mapa.notas[i].line]}){especialTag}");
                if (GUILayout.Button("Remover"))
                {
                    mapa.notas.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Apagar todas as notas"))
        {
            if (EditorUtility.DisplayDialog("Confirmação", "Deseja realmente apagar todas as notas?", "Sim", "Cancelar"))
            {
                mapa.notas.Clear();
            }
        }

        GUILayout.Space(14);

        // Waveform display
        if (waveformTexture != null)
        {
            GUILayout.Label("Waveform:");
            Rect rect = GUILayoutUtility.GetRect(waveformWidth, waveformHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawPreviewTexture(rect, waveformTexture);

            // Linha indicadora do tempo atual
            if (audioClip != null && audioClip.length > 0f)
            {
                float progress = Mathf.Clamp01(playTime / audioClip.length);
                float x = rect.x + progress * rect.width;
                EditorGUI.DrawRect(new Rect(x - 1, rect.y, 2, rect.height), Color.red);
            }
        }

        GUILayout.Space(18);

        GUILayout.Label("Salvar arquivo JSON", EditorStyles.boldLabel);
        saveFileName = EditorGUILayout.TextField("Nome do arquivo", saveFileName);
        if (GUILayout.Button("Salvar"))
        {
            SalvarJson();
        }

        EditorGUILayout.EndScrollView();
    }

    void AdicionarNota(float time, int line)
    {
        // Snap baseado no BPM + subdivisão
        float snapTime = time;

        if (mapa.bpm > 0f)
        {
            float segundosPorBeat = 60f / mapa.bpm;
            float segundosPorStep = segundosPorBeat / (float)subdivisao;
            if (segundosPorStep > 0f)
            {
                snapTime = Mathf.Round(time / segundosPorStep) * segundosPorStep;
            }
        }

        // Garante que não ultrapasse a duração do áudio (se houver)
        if (audioClip != null)
        {
            snapTime = Mathf.Clamp(snapTime, 0f, audioClip.length);
        }
        else
        {
            snapTime = Mathf.Max(0f, snapTime);
        }

        Note nova = new Note { time = snapTime, line = line };

        if (modoEspecialAtivo)
        {
            nova.especial = true;
            notasEspeciaisContadas++;

            if (notasEspeciaisContadas >= limiteNotasEspeciais)
            {
                modoEspecialAtivo = false;
                Debug.Log("Modo especial desativado após " + limiteNotasEspeciais + " notas.");
            }
        }

        mapa.notas.Add(nova);
        mapa.notas.Sort((a, b) => a.time.CompareTo(b.time));
        Debug.Log($"Nota adicionada: lane {line}, tempo {snapTime:F3}s, especial: {nova.especial}");
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

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int cent = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100f);
        return $"{minutes:D2}:{seconds:D2}.{cent:D2}";
    }

    // Gera a textura da waveform (simplificada)
    void GenerateWaveformTexture()
    {
        waveformTexture = null;

        if (audioClip == null) return;

        int samplesPerChannel = audioClip.samples;
        int channels = audioClip.channels;
        if (samplesPerChannel <= 0) return;

        float[] samples = new float[samplesPerChannel * channels];
        audioClip.GetData(samples, 0);

        int width = Mathf.Max(256, Mathf.Min(waveformWidth, (int)position.width - 20));
        int height = waveformHeight;

        Color[] pixels = new Color[width * height];

        int step = Mathf.Max(1, samples.Length / width);

        for (int x = 0; x < width; x++)
        {
            float sum = 0f;
            int start = x * step;
            int end = Mathf.Min(start + step, samples.Length);
            for (int i = start; i < end; i++)
            {
                sum += Mathf.Abs(samples[i]);
            }
            float avg = sum / (end - start);
            int bar = Mathf.RoundToInt(avg * (height / 2f) * 2f); // amplifica visualmente

            for (int y = 0; y < height; y++)
            {
                int mid = height / 2;
                if (y >= mid - bar && y <= mid + bar)
                    pixels[y * width + x] = Color.green;
                else
                    pixels[y * width + x] = new Color(0f, 0f, 0f, 0.5f);
            }
        }

        waveformTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        waveformTexture.SetPixels(pixels);
        waveformTexture.Apply();
    }
}
