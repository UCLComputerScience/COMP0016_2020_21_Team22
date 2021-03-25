using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using TMPro;

public class CSVDownloader : MonoBehaviour
{
    // Start is called before the first frame update
    private float update;
    public string fileName;
    public string path;
    private void Awake()
    {
        update = 2;
    }
    void Start()
    {

    }
    void Update()
    {
        update += Time.deltaTime;
        if (update > 3.5f)
        {
            update = 0;
            if (path != null)
            {
                GetIoTData(path);

            }
        }
    }
    public static async Task GetIoTData(string pathname)
    {
        string connectionString = AzureConnection.storageConnectionString;

        BlobContainerClient container = new BlobContainerClient(connectionString, "iotoutput");

        container.CreateIfNotExistsAsync().Wait();

        if (existsFile(container))
        {
            var fileName = container.GetBlobs().OrderByDescending(m => m.Properties.LastModified).ToList().First().Name;

            BlobClient blobClient = container.GetBlobClient(fileName);

            BlobDownloadInfo download = await blobClient.DownloadAsync();

            string downloadFilePath = string.Format(Application.temporaryCachePath + "/{0}", pathname + ".csv");

            File.Delete(downloadFilePath);

            using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
            {
                await download.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }
        }
        else
        {
            try
            {
                GameObject stateMessage =  GameObject.Find("State text");
                stateMessage.GetComponent<TextMeshProUGUI>().text = "No files found";
            }
            catch (NullReferenceException)
            {

            }
        }

    }

    public static bool existsFile(BlobContainerClient container)
    {
        var findfile = container.GetBlobs().OrderByDescending(m => m.Properties.LastModified).ToList();

        if (findfile.Count > 0)
        {
            return true;
        }
        else
        {
            Debug.Log("file not exist");
            return false;
        }
    }
}