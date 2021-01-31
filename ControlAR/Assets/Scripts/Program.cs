using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public class Program : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string connection_String = "DefaultEndpointsProtocol=https;AccountName=mohseenhubblob;AccountKey=SSMMWgXQnBk0oZr7YBbmmC4ZY5YYY/TypNX9CVXiDqFQTEHqSZ9IYl9JPUS+HXbI2y5HBsijkEdrcOTT5B4UrA==;EndpointSuffix=core.windows.net";
        //Debug.Log("Script runs when cube is rendered.");
        //GameObject ifieldobj = GameObject.Find("Canvas/param");
        //InputField ifield = ifieldobj.GetComponent<InputField>();
        //ifield.onEndEdit.AddListener(ParameterInput);
        ParameterInput(connection_String);

    }

    //User enters connection string and hits Return key.
    public void ParameterInput(string cxnstr)
    {
        Debug.Log(cxnstr);


        CloudStorageAccount act = CloudStorageAccount.Parse(cxnstr);
        CloudBlobClient client = act.CreateCloudBlobClient();

        var container = client.GetContainerReference("iotoutput");
        container.CreateIfNotExistsAsync().Wait();

        CloudBlockBlob blob = container.GetBlockBlobReference("log.txt");
        //blob.UploadTextAsync("Unity upload").Wait();
        appendText(blob, "Unity log: " + System.DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss"));

        downloadDemo(cxnstr);
    }

    public async Task downloadDemo(string cxnstr)
    {
        BlobModel bm = new BlobModel("cat.obj", "iotoutput", cxnstr);
        if (await bm.exists())
        {
            await bm.download("catmodel.obj"); //you MUST await this, otherwise file can't be imported since it may not be downloaded yet
            Debug.Log("Downloaded.");

            Mesh meshHold = new Mesh();
            ObjImporter newMesh = new ObjImporter();
            meshHold = newMesh.ImportFile("./Assets/Resources/catmodel.obj");//"./Assets/BlobServerModels/catmodel.obj"); VS ./Assets/Scenes/catmodel.obj
            Debug.Log("Imported");

            GameObject myCat = new GameObject();
            MeshRenderer meshRenderer = myCat.AddComponent<MeshRenderer>();
            MeshFilter filter = myCat.AddComponent<MeshFilter>();
            filter.mesh = meshHold;
            //./Assets/Resources/metal01.mat
            Material catMaterial = Resources.Load("metal01", typeof(Material)) as Material;
            myCat.GetComponent<MeshRenderer>().material = catMaterial;

            Instantiate(myCat);
            myCat.transform.position = new Vector3(47, -365, -59);

            Debug.Log("Done");
        }

        BlobModel bmtwo = new BlobModel("test.csv", "iotoutput", cxnstr);
        if (await bmtwo.exists())
        {
            await bmtwo.download("test.csv"); //you MUST await this, otherwise file can't be imported since it may not be downloaded yet
            Debug.Log("Downloaded.");
        }
    }


        public static async Task appendText(CloudBlockBlob blob, string v)
    {
        var upload = v;

        if (await blob.ExistsAsync())
        {
            //append. here we test retrieval & read...
            var content = await blob.DownloadTextAsync();

            upload = content + "\n" + v;
        }

        blob.UploadTextAsync(upload).Wait();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
