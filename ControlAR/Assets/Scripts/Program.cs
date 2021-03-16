using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Microsoft.Azure.Devices;
using System;
using UnityEngine.Scripting;
using System.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;

public class Program : MonoBehaviour
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
        //string connection_String = "DefaultEndpointsProtocol=https;AccountName=mohseenhubblob;AccountKey=SSMMWgXQnBk0oZr7YBbmmC4ZY5YYY/TypNX9CVXiDqFQTEHqSZ9IYl9JPUS+HXbI2y5HBsijkEdrcOTT5B4UrA==;EndpointSuffix=core.windows.net";
        //Debug.Log("Script runs when cube is rendered.");
        //GameObject ifieldobj = GameObject.Find("Canvas/param");
        //InputField ifield = ifieldobj.GetComponent<InputField>();
        //ifield.onEndEdit.AddListener(ParameterInput);
        
    }
    void Update()
    {
        update += Time.deltaTime;
        if (update > 3.5f)
        {
            update = 0;
            if(path != null)
            {
                GetIoTData(path);
                //try
                //{
                //    var csv = File.ReadLines(Application.temporaryCachePath + "/{0}" + path).Select((line, index) => index == 0 ? line + ",MessageNumber" : line + "," + index.ToString()).ToList();
                //    Debug.Log(String.Join("\n", csv));
                //    File.WriteAllLines(Application.temporaryCachePath + "/{0}" + path, csv);
                //}
                //catch (IOException)
                //{

                //}
            }
        }
    }
    public static async Task GetIoTData(string pathname)
    {
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=mohseenhubblob;AccountKey=SSMMWgXQnBk0oZr7YBbmmC4ZY5YYY/TypNX9CVXiDqFQTEHqSZ9IYl9JPUS+HXbI2y5HBsijkEdrcOTT5B4UrA==;EndpointSuffix=core.windows.net";

        BlobContainerClient container = new BlobContainerClient(connectionString, "iotoutput");

        var fileName = container.GetBlobs().OrderByDescending(m => m.Properties.LastModified).ToList().First().Name;

        BlobClient blobClient = container.GetBlobClient(fileName);

        BlobDownloadInfo download = await blobClient.DownloadAsync();

        string downloadFilePath = string.Format(Application.temporaryCachePath + "/{0}", pathname + ".csv");
        //try
        //{
            File.Delete(downloadFilePath);
        //}
        //catch(Exception e)
        //{
        //    Debug.Log("error");
        //}
        using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
        {
            await download.Content.CopyToAsync(downloadFileStream);
            downloadFileStream.Close();
        }

    }
    //User enters connection string and hits Return key.
    //public void ParameterInput(string cxnstr)
    //{
    //    //Debug.Log(cxnstr);


    //    CloudStorageAccount act = CloudStorageAccount.Parse(cxnstr);
    //    CloudBlobClient client = act.CreateCloudBlobClient();

    //    var container = client.GetContainerReference("iotoutput");
    //    container.CreateIfNotExistsAsync().Wait();

    //    CloudBlockBlob blob = container.GetBlockBlobReference("log.txt");
    //    //blob.UploadTextAsync("Unity upload").Wait();
    //    appendText(blob, "Unity log: " + System.DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss"));

    //    downloadDemo(cxnstr);
    //}

    //public async Task downloadDemo(string cxnstr)
    //{
    //BlobModel bm = new BlobModel("cat.obj", "iotoutput", cxnstr);
    //if (await bm.exists())
    //{
    //    await bm.download("catmodel.obj"); //you MUST await this, otherwise file can't be imported since it may not be downloaded yet
    //    Debug.Log("Downloaded.");

    //    Mesh meshHold = new Mesh();
    //    ObjImporter newMesh = new ObjImporter();
    //    meshHold = newMesh.ImportFile("./Assets/Resources/catmodel.obj");//"./Assets/BlobServerModels/catmodel.obj"); VS ./Assets/Scenes/catmodel.obj
    //    Debug.Log("Imported");

    //    GameObject myCat = new GameObject();
    //    MeshRenderer meshRenderer = myCat.AddComponent<MeshRenderer>();
    //    MeshFilter filter = myCat.AddComponent<MeshFilter>();
    //    filter.mesh = meshHold;
    //    //./Assets/Resources/metal01.mat
    //    Material catMaterial = Resources.Load("metal01", typeof(Material)) as Material;
    //    myCat.GetComponent<MeshRenderer>().material = catMaterial;

    //    Instantiate(myCat);
    //    myCat.transform.position = new Vector3(47, -365, -59);

    //    Debug.Log("Done");
    //}

    //Debug.Log("path is " + Application.temporaryCachePath);
    //        BlobModel bmtwo = new BlobModel("1184906515_2bf7a63f2ff44de2b45d366dff28dd5a_1.csv", "iotoutput", cxnstr);
    //        if (await bmtwo.exists())
    //        {
    //            await bmtwo.download(path); //you MUST await this, otherwise file can't be imported since it may not be downloaded yet
    //            //Debug.Log("Downloaded.");
    //        }
    //}


    //    public static async Task appendText(CloudBlockBlob blob, string v)
    //{
    //    var upload = v;

    //    if (await blob.ExistsAsync())
    //    {
    //        //append. here we test retrieval & read...
    //        var content = await blob.DownloadTextAsync();

    //        upload = content + "\n" + v;
    //    }

    //    blob.UploadTextAsync(upload).Wait();
    //}
}
