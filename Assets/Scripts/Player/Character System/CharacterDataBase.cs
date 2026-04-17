using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBase", menuName = "Settings/CharacterDataBase")]
public class CharacterDataBase : ScriptableObject
{
    public Character[] characters;
    
    public int CharacterCount
    {
        get { return characters.Length; }
    }
    
    public Character GetCharacter(int index)
    {
        return characters[index];
    }
}
