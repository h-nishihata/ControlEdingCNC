using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CSVデータの読込.
/// https://note.mu/macgyverthink/n/n83943f3bad60
/// </summary>
public class CSVReader : MonoBehaviour
{
    private TextAsset csvFile; // CSVファイル.
    public string fileName = "archiveData";
    private StringReader reader;
    private List<string> csvData = new List<string>(); // CSVファイルの中身を入れるリスト.

    void Awake()
    {
        csvFile = Resources.Load(fileName) as TextAsset; // Resouces下のCSV読み込み.
        reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1) // reader.Peekが0になるまで繰り返す.
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み.
            csvData.Add(line);   //，リストに追加.
        }
        // [行][列]を指定して値を自由に取り出せる.
        // Debug.Log(csvData[0][1]);

        StartCoroutine(this.CSV2OSC());
    }

    private IEnumerator CSV2OSC()
    {
        var cachedWait = new WaitForSeconds(1f);
        for (int i = 0; i < csvData.Count; i++)
        {
            Debug.Log(csvData[i]);
            yield return cachedWait;
        }
    }
}