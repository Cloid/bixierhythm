using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChartLoader.NET.Utils;
using ChartLoader.NET.Framework;

public class ChartLoaderEx : MonoBehaviour
{
    public static ChartReader chartReader;

    public float speed = 1f;

    public Transform[] notePrefabs;

    public Transform[] chordPrefabs;

    // Documents list of chord notes in a song
    // Records all chord notes in a song
    private List<Note> chordNotes = new List<Note>();

    // Records all bottom chord notes in a pair
    private List<Note> botChordNotes = new List<Note>();

    // Records all top chord notes in a pair
    private List<Note> topChordNotes = new List<Note>();

    // Records all leftover bottom chord notes during SpawnChord()
    private List<Note> leftoverBotChordNotes = new List<Note>();

    // Start is called before the first frame update
    void Start()
    {
        chartReader = new ChartReader();
        //Chart yoshiChart = chartReader.ReadChartFile("Assets/ChartLoader/ChartLoader/Songs/Yoshi/Yoshi.chart");
        //Chart yoshiChart = chartReader.ReadChartFile(Application.streamingAssetsPath + "/Bixie_Track_1.chart");
        Chart yoshiChart = chartReader.ReadChartFile(Application.streamingAssetsPath + "/Yoshi.chart");

        Note[] expertGuitarNotes = yoshiChart.GetNotes("ExpertSingle");
        SpawnNotes(expertGuitarNotes);
    }

    /// <summary>
    /// Spawn all notes from the chart
    /// </summary>
    public void SpawnNotes(Note[] notes)
    {
        foreach (Note note in notes)
        {
            SpawnNote(note);
        }

        SpawnChords();
    }

    // Original Function
    /// <summary>
    /// Spawn all chords from known chord notes
    /// </summary>
    public void SpawnChords()
    {
        //Debug.Log("Spawning chords");
        // Sets botChordIndex to the start of the chordNotes list (which was already generated in SpawnNotes) while topChordIndex is empty
        int botChordIndex = chordNotes[0].Index;
        int topChordIndex = 0;
        for (int i = 0; i < chordNotes.Count; i++)
        {
            // -- Bottom Chord --
            // If index is the same as botChordIndex then add chord to bot chord list (a separate list for bottom chords)
            if (chordNotes[i].Index == botChordIndex)
            {
                //Debug.Log("Adding chord to bottom chord list");
                botChordNotes.Add(chordNotes[i]);
            }

            // -- Top Chord --
            // If index is the same as topChordIndex then add chord to top chord list (a separate list for top chords)
            if (chordNotes[i].Index == topChordIndex && topChordIndex != 0)
            {
                //Debug.Log("Adding chord to top chord list");
                topChordNotes.Add(chordNotes[i]);
            }

            // -- Cases --
            // If index is not the same as botChordIndex and topChordIndex is 0 then assume that we're at the top chord of the chord pair
            // Add the top chord to the top chord list
            if (chordNotes[i].Index != botChordIndex && topChordIndex == 0)
            {
                //Debug.Log("Setting top chord index");
                topChordIndex = chordNotes[i].Index;
                //Debug.Log("Adding chord to top chord list");
                topChordNotes.Add(chordNotes[i]);
            }
            // If index is not the same as botChordIndex and topChordIndex (which is nonzero) then assume it is the beginning of a new chord pair
            // Spawn notes
            else if (chordNotes[i].Index != botChordIndex && chordNotes[i].Index != topChordIndex)
            {
                //Debug.Log("Mismatch detected -> Beginning new chord pair");
                SpawnChord(botChordNotes, topChordNotes);
                botChordNotes.Clear();
                topChordNotes.Clear();
                botChordIndex = chordNotes[i].Index;
                botChordNotes.Add(chordNotes[i]);
                topChordIndex = 0;
            }

            // If LeftoverBotChordNotes is greater then 0 (which is from SpawnChord), 
            // then attempt to pair it with botChordNotes (which we are assuming as the top pair), then clear it
            if (leftoverBotChordNotes.Count > 0) SpawnChord(leftoverBotChordNotes, botChordNotes); leftoverBotChordNotes.Clear();
        }
    }

    /// <summary>
    /// Spawns individual notes, based on button index and type
    /// </summary>
    public void SpawnNote(Note note)
    {
        Vector3 point;
        for (int i = 0; i < note.ButtonIndexes.Length; i++)
        {
            if (note.ButtonIndexes[i])
            {
                point = new Vector3(i - 2f, 0f, note.Seconds * speed);
                string noteTag = "Note";
                // If note is a chord, then tag it as a chord and add it to the chordNotes list
                if (note.IsChord)
                {
                    //Debug.Log(note);
                    point = new Vector3(i - 2f, 0f, note.Seconds * speed);
                    // Add chord note to list
                    noteTag = "ChordNote";
                    chordNotes.Add(note);
                    SpawnPrefab(notePrefabs[i], noteTag, point, new Vector3(1, 1, 1));
                }
                else if (note.IsHammerOn)
                {
                    // Debug.Log("Is Hammer");
                    // noteTag = "Hammer";
                }
                SpawnPrefab(notePrefabs[i], noteTag, point, new Vector3 (1, 1, 1));
            }
        }
    }

    // Original Function
    /// <summary>
    /// Spawns chord if preceding note is a chord
    /// </summary>
    public void SpawnChord(List<Note> botChords, List<Note> topChords)
    {
        //Debug.Log("Spawn Chord");
        Note topNote;
        Note botNote;
        // Goes through the topChord list 
        for (int i = 0; i < topChords.Count; i++)
        {
            topNote = topChords[i];
            if(i < botChords.Count)
            {
               botNote = botChords[i];
            } else
            {
                botNote = botChords[botChords.Count - 1];
            }

            //Debug.Log(topNote.Index);
            //Debug.Log(botNote.Index);
            // If i is even then spawn the chords
            if (i % 2 == 0)
            {
                for (int j = 0; j < topNote.ButtonIndexes.Length; j++)
                {
                    // If topNote and botNote match indexes, then spawn note
                    //Debug.Log(topNote.ButtonIndexes[j] + " " + botNote.ButtonIndexes[j]);
                    if (topNote.ButtonIndexes[j] && botNote.ButtonIndexes[j])
                    {
                        //Debug.Log("Chord being created at " + j);
                        Vector3 chordPoint = new Vector3(j - 2f, 0f, ((botNote.Seconds + topNote.Seconds) / 2) * speed);
                        //Debug.Log(botNote.Seconds);
                        SpawnPrefab(chordPrefabs[j], "Chord", chordPoint, new Vector3(0.2f, Mathf.Abs(botNote.Seconds-topNote.Seconds) * 1.5f, 1));
                    }
                    // If topNote is true but botNote is not true, then there is a mismatch and we must treat it as a leftover botNote
                    else if (topNote.ButtonIndexes[j] && !botNote.ButtonIndexes[j])
                    {
                        leftoverBotChordNotes.Add(topNote);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Spawns prefab
    /// </summary>
    public void SpawnPrefab(Transform prefab, string tag, Vector3 point, Vector3 scale)
    {
        Transform tmp = Instantiate(prefab);
        tmp.SetParent(transform);
        tmp.position = point;
        tmp.tag = tag;
        tmp.localScale = scale;
    }
}
