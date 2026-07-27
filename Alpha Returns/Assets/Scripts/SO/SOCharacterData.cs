using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string CharacterName;
    public CharacterChat[] Chats;
    public CharacterPortrait[] Portraits;

    public Sprite GetPortrait(string tag) =>
        System.Array.Find(Portraits, x => x.Expression == tag)?.Portrait;

    public Sprite GetChat(string tag) =>
        System.Array.Find(Chats, x => x.Pose == tag)?.Chat;
}

[System.Serializable]
public class CharacterPortrait
{
    public string Expression;
    public Sprite Portrait;
}

[System.Serializable]
public class CharacterChat
{
    public string Pose;
    public Sprite Chat;
}

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Dialog/Character Database")]
public class SOCharacterData : ScriptableObject
{
    public List<CharacterData> Characters;

    public CharacterData GetCharacter(string name)
    {
        return Characters.Find(c => c.CharacterName == name);
    }
}
