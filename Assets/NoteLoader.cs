using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

[System.Serializable]
public class Note
{
    public float time;
    public int note;
    public int column;
    public string color;
}

[System.Serializable]
public class NoteList
{
    public List<Note> notes;
}

public class NoteLoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/song_notes.json"; // JSON dosyasýnýn yolu
    public List<Note> notes; // JSON'dan yüklenecek notalar

    public GameObject notePrefab; // Nota için prefab
    public Transform[] spawnPoints; // Her sütun için spawn noktalarý
    public AudioSource audioSource; // Ana müzik için AudioSource
    public AudioSource audioSource2;

    public float beforeTime = 0;

    private float startTime; // Müziðin baþlama zamaný

    void Start()
    {
         // Notalarý yükle

       
         // Notalarý spawn etmek için coroutine baþlat
    }

    public void StartFight()
    {
        LoadNotes();
        StartCoroutine(SpawnNotes());
        StartCoroutine(StartSong());
    }
    void LoadNotes()
    {
        string jsonText = File.ReadAllText(jsonFilePath);
        NoteList noteList = JsonUtility.FromJson<NoteList>(jsonText);

        notes = noteList.notes;

        // Notlarý renklerine göre sütunlara ata
        foreach (var note in notes)
        {
            switch (note.color)
            {
                case "red":
                    note.column = 0;
                    break;
                case "blue":
                    note.column = 1;
                    break;
                case "yellow":
                    note.column = 2;
                    break;
                case "green":
                    note.column = 3;
                    break;
                default:
                    Debug.LogWarning("Unknown color: " + note.color);
                    break;
            }
        }
    }


    IEnumerator StartSong()
    {

        yield return new WaitForSecondsRealtime(2.25f);
        startTime = (float)AudioSettings.dspTime; // Müziðin baþlama zamanýný kaydet
        audioSource.PlayScheduled(startTime); // Müziði baþlat (planlanmýþ zamanla)
        audioSource2.PlayScheduled(startTime);

    }
    IEnumerator SpawnNotes()
    {
        
        foreach (Note note in notes)
        {
            // Müziðin baþladýðý zaman ile notanýn spawn zamanýný senkronize et
            float TimetoWait = note.time - beforeTime;
            beforeTime = note.time;
            yield return new WaitForSecondsRealtime(TimetoWait);
            SpawnNote(note); // Notayý spawn et
        }
    }

    void SpawnNote(Note note)
    {
        int column = note.column; // Notanýn spawn edileceði sütun
        Transform spawnPoint = spawnPoints[column]; // Sütunun spawn noktasý
        GameObject noteObject = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity); // Notayý instantiate et
        noteObject.GetComponent<NoteMovement>().SetColor(note.color); // Notanýn rengini ayarla
    }
}
