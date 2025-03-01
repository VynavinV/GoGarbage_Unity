using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class MapLoader : MonoBehaviour
{
    [Header("Map Settings")]
    public double latitude = 0.0, longitude = 0.0; // Latitude and longitude for the map center
    public int zoom = 15; // Zoom level for the map

    [Header("Grid Settings")]
    public int gridSize = 8; // Number of tiles in the grid (gridSize x gridSize)
    public float tileSize = 2f; // Size of each tile

    [Header("UI")]
    public TextMeshProUGUI errorText; // UI element to display errors

    [Header("Marker Settings")]
    public GameObject markerPrefab; // Prefab for the markers

    // We'll now store full report objects that include the Coordinates and ImageURL.
    private List<Report> markerReports = new List<Report>();
    public double markerProximityThreshold = 1e-10; // Proximity threshold to determine if a marker is nearby

    private GameObject[,] tileObjects; // 2D array to store tile GameObjects
    private List<GameObject> activeMarkers = new List<GameObject>(); // List to store active marker GameObjects

    void Start()
    {
        CreateGrid(); // Create the grid of tiles
        StartCoroutine(FetchMarkerCoordinates()); // Fetch marker coordinates (and ImageURLs) from the server
        StartCoroutine(UpdateMapPeriodically()); // Periodically update the map

        // Enable location services
        Input.location.Start();
    }
    public void UpdateMap(double lat, double lon)
    {
        latitude = lat;
        longitude = lon;
    }
    void CreateGrid()
    {
        tileObjects = new GameObject[gridSize, gridSize]; // Initialize the tile array
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                // Create a new tile
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tile.name = $"Tile_{row}_{col}"; // Name the tile
                tile.transform.parent = transform; // Set the parent of the tile
                tile.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // Rotate the tile to lie flat
                tile.transform.localScale = Vector3.one * tileSize; // Scale the tile
                tileObjects[row, col] = tile; // Store the tile in the array
            }
        }
    }

    IEnumerator UpdateMapPeriodically()
    {
        while (true)
        {
            // Get the device's real-time location
            if (Input.location.status == LocationServiceStatus.Running)
            {
                latitude = Input.location.lastData.latitude;
                longitude = Input.location.lastData.longitude;
            }
            else
            {
                errorText?.SetText("Unable to determine device location.");
            }

            errorText?.SetText(""); // Clear any previous error messages
            UpdateAllTiles(); // Update all the tiles and markers
            yield return new WaitForSeconds(9f); // Wait for 9 seconds before updating again
        }
    }

    void UpdateAllTiles()
    {
        // Calculate the center tile coordinates based on the current latitude and longitude
        double centerTileX = LonToTileX(longitude, zoom);
        double centerTileY = LatToTileY(latitude, zoom);
        int startTileX = (int)Math.Floor(centerTileX) - gridSize / 2;
        int startTileY = (int)Math.Floor(centerTileY) - gridSize / 2;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                int tileX = startTileX + col, tileY = startTileY + row;
                // Position the tile based on its coordinates
                tileObjects[row, col].transform.localPosition = new Vector3(
                    (float)((tileX + 0.5 - centerTileX) * tileSize),
                    0,
                    (float)(-(tileY + 0.5 - centerTileY) * tileSize)
                );
                // Load the tile texture
                StartCoroutine(LoadTile(tileX, tileY, tileObjects[row, col]));
            }
        }
        UpdateMarkers(centerTileX, centerTileY); // Update the markers on the map
    }

    IEnumerator LoadTile(int tileX, int tileY, GameObject tileObj)
    {
        // URL to fetch the tile texture
        string url = $"https://tile.openstreetmap.org/{zoom}/{tileX}/{tileY}.png";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Apply the texture to the tile
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            tileObj.GetComponent<Renderer>().material = new Material(Shader.Find("Universal Render Pipeline/Unlit"))
            {
                mainTexture = texture
            };
        }
        else
        {
            // Display an error message if the tile could not be loaded
            errorText?.SetText($"Error downloading tile {tileX}/{tileY}: {www.error}\n");
        }
    }

    IEnumerator FetchMarkerCoordinates()
    {
        // URL to fetch marker data (coordinates and ImageURL)
        string url = "https://urdpxgmczkokojlhtynd.supabase.co/rest/v1/Reports";
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("apikey", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVyZHB4Z21jemtva29qbGh0eW5kIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTczOTQwODc0MSwiZXhwIjoyMDU0OTg0NzQxfQ.0Vmn0GL1M36pM1VmwtYva5GRAK-vGt0UnJhBMuX-4EE");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response and extract reports (which include Coordinates and ImageURL)
            string jsonResponse = www.downloadHandler.text;
            // Wrap the response in an object so that JsonUtility can deserialize it.
            var reports = JsonUtility.FromJson<ReportsResponse>($"{{\"reports\": {jsonResponse}}}");
            markerReports = reports.reports;
        }
        else
        {
            // Display an error message if the markers could not be fetched
            errorText?.SetText($"Error fetching markers: {www.error}\n");
        }
    }

    // ...existing code...
    void UpdateMarkers(double centerTileX, double centerTileY)
    {
        // Remove all active markers that are no longer in the markerReports or are marked as cleaned or hazardous
        activeMarkers.RemoveAll(marker =>
        {
            var markerComponent = marker.GetComponent<Marker>();
            var report = markerReports.FirstOrDefault(r => r.ImageURL == markerComponent.imageurl);
            if (report == null || report.Cleaned || report.Hazards)
            {
                Destroy(marker);
                return true;
            }
            return false;
        });

        foreach (Report report in markerReports)
        {
            // Skip markers that are marked as cleaned or hazardous
            if (report.Cleaned || report.Hazards)
            {
                continue;
            }

            string[] parts = report.Coordinates.Split(',');
            if (parts.Length == 2 &&
                double.TryParse(parts[0], out double markerLat) &&
                double.TryParse(parts[1], out double markerLon) &&
                IsNearby(markerLat, markerLon, latitude, longitude, markerProximityThreshold))
            {
                // Check if the marker already exists
                if (!activeMarkers.Any(m => m.GetComponent<Marker>().imageurl == report.ImageURL))
                {
                    // Place a marker if it is nearby and print the ImageURL
                    PlaceMarker(markerLat, markerLon, centerTileX, centerTileY, report.ImageURL);
                }
            }
        }
    }

    // ...existing code...

    void PlaceMarker(double markerLat, double markerLon, double centerTileX, double centerTileY, string imageUrl)
    {
        // Calculate the position of the marker
        float posX = (float)((LonToTileX(markerLon, zoom) - centerTileX) * tileSize);
        float posZ = (float)(-(LatToTileY(markerLat, zoom) - centerTileY) * tileSize);
        // Instantiate the marker at the calculated position
        GameObject marker = Instantiate(markerPrefab, new Vector3(posX, 0.1f, posZ), Quaternion.identity, transform);
        activeMarkers.Add(marker); // Add the marker to the list of active markers
        string completeurl = "http://gogarbage.zapto.org" + imageUrl;
        marker.GetComponent<Marker>().SetImage(completeurl);
        marker.GetComponent<Marker>().imageurl = imageUrl; // Store the ImageURL in the marker component

        // Print the ImageURL for this marker
        Debug.Log($"Marker spawned with ImageURL: {imageUrl}");
    }

    // Check if two coordinates are within a certain threshold
    bool IsNearby(double lat1, double lon1, double lat2, double lon2, double threshold)
        => Math.Abs(lat1 - lat2) < threshold && Math.Abs(lon1 - lon2) < threshold;

    // Convert longitude to tile X coordinate
    double LonToTileX(double lon, int zoom) => (lon + 180.0) / 360.0 * Math.Pow(2, zoom);

    // Convert latitude to tile Y coordinate
    double LatToTileY(double lat, int zoom) =>
        (1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) + 1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * Math.Pow(2, zoom);

    [Serializable]
    public class Report
    {
        public string Coordinates;
        public string ImageURL;
        public bool Cleaned;
        public bool Hazards;
        // Other fields (e.g., id, Hazards, Cleaned) can be added here if needed.
    }

    [Serializable]
    public class ReportsResponse
    {
        public List<Report> reports;
    }
}
