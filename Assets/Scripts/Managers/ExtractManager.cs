using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractManager : Manager<ExtractManager>
{
    public List<ExtractPoints> ExtractPoints = new List<ExtractPoints>();
    public List<ExtractPoints> activeExtractPoints = new List<ExtractPoints>();
    public ExtractPoints extractONE;
    public ExtractPoints extractTWO;


    public void ChooseExtractPoints()
    {
        extractONE = ExtractPoints[Random.Range(0, ExtractPoints.Count)];
        do
        {
            extractTWO = ExtractPoints[Random.Range(0, ExtractPoints.Count)];
        }while (extractONE == extractTWO);
        activeExtractPoints.Add(extractONE);
        activeExtractPoints.Add(extractTWO);
        foreach(ExtractPoints point in activeExtractPoints) { point.IsActive = true; }
    }

    public void ClearExtracts()
    {
        ExtractPoints.Clear();
        activeExtractPoints.Clear();
    }
}
