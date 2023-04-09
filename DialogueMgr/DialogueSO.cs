using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialouge", menuName = "Dialouge")]
public class DialogueSO : ScriptableObject
{
    public List<DialougePiece> dialouge;
}
