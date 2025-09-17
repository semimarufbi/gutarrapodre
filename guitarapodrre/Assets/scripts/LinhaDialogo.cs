using UnityEngine;

[System.Serializable]
public class LinhaDialogo
{
    public string nome;
    [TextArea(2, 5)]
    public string texto;
}

[CreateAssetMenu(fileName = "NovoDialogo", menuName = "Dialogo/Novo")]
public class Dialogo : ScriptableObject
{
    public LinhaDialogo[] falas;
}
