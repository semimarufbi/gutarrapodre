using System;

[Serializable]
public class NoteData
{
    public float tempo; // tempo da nota, em segundos
    public int lane;    // qual trilha (0 = esquerda, 1 = meio, etc)
}
