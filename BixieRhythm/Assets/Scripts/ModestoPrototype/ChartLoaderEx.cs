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

    // Start is called before the first frame update
    void Start()
    {
        chartReader = new ChartReader();
        Chart yoshiChart = chartReader.ReadChartFile("Assets/ChartLoader/ChartLoader/Songs/Yoshi/Yoshi.chart");

        Note[] expertGuitarNotes = yoshiChart.GetNotes("ExpertSingle");
        SpawnNotes(expertGuitarNotes);
    }

    //spawn all notes
    public void SpawnNotes(Note[] notes)
    {
        foreach(Note note in notes)
        {
            SpawnNote(note);
        }
    }

    public void SpawnNote(Note note)
    {
        Vector3 point;
        for(int i = 0; i < note.ButtonIndexes.Length; i++)
        {
            if (note.ButtonIndexes[i])
            {
                point = new Vector3(i-2f, 0f, note.Seconds * speed);
                SpawnPrefab(notePrefabs[i], point);
            }
        }
    }

    public void SpawnPrefab(Transform prefab, Vector3 point)
    {
        Transform tmp = Instantiate(prefab);
        tmp.SetParent(transform);
        tmp.position = point;
    }
}
