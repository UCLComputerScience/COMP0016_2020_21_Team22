using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using System;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public class IoTControl : MonoBehaviour
{
    public Button lightTrigger;
    private static ServiceClient s_serviceClient;

    // Connection string for your IoT Hub
    // az iot hub show-connection-string --hub-name {your iot hub name} --policy-name service
    private static string IotConnectionString = AzureConnection.IotConnectionString;
    private string storageConnectionString = AzureConnection.storageConnectionString;

    // Start is called before the first frame update
    void Start()
    {
        lightTrigger.onClick.AddListener(onTriggerClick);
    }

    async void onTriggerClick()
    {
        await IoTInvoke();
        updateLog(storageConnectionString);
    }

    private static async Task IoTInvoke()
    {
        ValidateConnectionString();

        // Create a ServiceClient to communicate with service-facing endpoint on your hub.
        s_serviceClient = ServiceClient.CreateFromConnectionString(IotConnectionString);

        await InvokeMethodAsync();

        s_serviceClient.Dispose();
    }

    public void updateLog(string cxnstr)
    {
        //Debug.Log(cxnstr);


        CloudStorageAccount act = CloudStorageAccount.Parse(cxnstr);
        CloudBlobClient client = act.CreateCloudBlobClient();

        var container = client.GetContainerReference("iotlogs");
        container.CreateIfNotExistsAsync().Wait();

        CloudBlockBlob blob = container.GetBlockBlobReference("log.txt");
        //blob.UploadTextAsync("Unity upload").Wait();
        appendText(blob, "Unity log: " + "Toggled light - "  + System.DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss"));

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
    private static async Task InvokeMethodAsync()
    {
        var methodInvocation = new CloudToDeviceMethod("ToggleLight")
        {
            ResponseTimeout = TimeSpan.FromSeconds(30),
        };
        //methodInvocation.SetPayloadJson("10");

        // Invoke the direct method asynchronously and get the response from the simulated device.
        var response = await s_serviceClient.InvokeDeviceMethodAsync("MyPythonDevice", methodInvocation);

        //Console.WriteLine($"\nResponse status: {response.Status}, payload:\n\t{response.GetPayloadAsJson()}");
    }

    private static void ValidateConnectionString()
    {
        try
        {
            _ = IotHubConnectionStringBuilder.Create(IotConnectionString);
        }
        catch (Exception)
        {
            //Console.WriteLine("This sample needs a device connection string to run. Program.cs can be edited to specify it, or it can be included on the command-line as the only parameter.");
            Environment.Exit(1);
        }
    }
}
