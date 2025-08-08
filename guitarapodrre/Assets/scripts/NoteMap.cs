using System;

[Serializable]
public class Note
{
    public float time; // Quando aparece
    public int line;   // Em qual lane
}

[Serializable]
public class NoteMap
{
    public Note[] notas; // Array de notas
}
