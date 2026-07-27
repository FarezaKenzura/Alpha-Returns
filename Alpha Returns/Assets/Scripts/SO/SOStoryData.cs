using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChapterData
{
    public string chapterID;
    public string chapterName;
    public TextAsset inkJSON;
}

[CreateAssetMenu(fileName = "StoryDatabase", menuName = "Dialog/Story Database")]
public class SOStoryData : ScriptableObject
{
    public List<ChapterData> Chapters;

    public ChapterData GetChapter(string id)
    {
        return Chapters.Find(c => c.chapterID == id);
    }
}
