using UnityEngine;

public class CsvUsage : MonoBehaviour
{
    [SerializeField] private TextAsset csv;

    private void Start()
    {
        Test();
    }

    private void Test()
    {
        var csvReader = new CsvReader();
        var csvData = csvReader.ReadCsv(csv.text);

// Access the CSV data
        foreach (var line in csvData)
        foreach (var field in line)
            // Do something with each field
            Debug.Log(field);
    }
}