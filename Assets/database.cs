using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[System.Serializable]
public class Report
{
    public int id;
    public string Coordinates;
    public bool Hazards;
    public bool Cleaned;
    public string ImageURL;
}

[System.Serializable]
public class ReportsList
{
    public Report[] reports;
}

public class database : MonoBehaviour
{
    private string supabaseUrl = "https://urdpxgmczkokojlhtynd.supabase.co/rest/v1/Reports";
    private string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVyZHB4Z21jemtva29qbGh0eW5kIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzk0MDg3NDEsImV4cCI6MjA1NDk4NDc0MX0.l0C2lLSrvGRJZEmjgqk3FDj3i17Cmby_buI5FLzsrUg";

    void Start()
    {
        StartCoroutine(FetchReports());
    }

    IEnumerator FetchReports()
    {
        UnityWebRequest request = UnityWebRequest.Get(supabaseUrl);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = "{\"reports\":" + request.downloadHandler.text + "}";
            ReportsList reportsList = JsonUtility.FromJson<ReportsList>(jsonResponse);

            foreach (Report report in reportsList.reports)
            {
                Debug.Log($"ID: {report.id}, Coordinates: {report.Coordinates}, Hazards: {report.Hazards}, Cleaned: {report.Cleaned}, ImageURL: {report.ImageURL}");
            }
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }
}
