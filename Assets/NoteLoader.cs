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
    public string jsonFilePath = "Assets/song_notes.json"; // JSON dosyas�n�n yolu
    public List<Note> notes; // JSON'dan y�klenecek notalar

    public GameObject notePrefab; // Nota i�in prefab
    public Transform[] spawnPoints; // Her s�tun i�in spawn noktalar�
    public AudioSource audioSource; // Ana m�zik i�in AudioSource
    public AudioSource audioSource2;

    public float beforeTime = 0;

    private float startTime; // M�zi�in ba�lama zaman�

    void Start()
    {
         // Notalar� y�kle

       
         // Notalar� spawn etmek i�in coroutine ba�lat
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

        // Notlar� renklerine g�re s�tunlara ata
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
        startTime = (float)AudioSettings.dspTime; // M�zi�in ba�lama zaman�n� kaydet
        audioSource.PlayScheduled(startTime); // M�zi�i ba�lat (planlanm�� zamanla)
        audioSource2.PlayScheduled(startTime);

    }
    IEnumerator SpawnNotes()
    {
        
        foreach (Note note in notes)
        {
            // M�zi�in ba�lad��� zaman ile notan�n spawn zaman�n� senkronize et
            float TimetoWait = note.time - beforeTime;
            beforeTime = note.time;
            yield return new WaitForSecondsRealtime(TimetoWait);
            SpawnNote(note); // Notay� spawn et
        }
    }

    void SpawnNote(Note note)
    {
        int column = note.column; // Notan�n spawn edilece�i s�tun
        Transform spawnPoint = spawnPoints[column]; // S�tunun spawn noktas�
        GameObject noteObject = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity); // Notay� instantiate et
        noteObject.GetComponent<NoteMovement>().SetColor(note.color); // Notan�n rengini ayarla
    }
}
