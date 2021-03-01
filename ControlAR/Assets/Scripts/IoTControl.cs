using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using System;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class IoTControl : MonoBehaviour
{
    public Button lightTrigger;
    private static ServiceClient s_serviceClient;
        
        // Connection string for your IoT Hub
        // az iot hub show-connection-string --hub-name {your iot hub name} --policy-name service
    private static string s_connectionString = "HostName=MohseenHub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=lXC2t+EC5ZFHK+4ZouNwamc93gcZwQ5UsxLBXOFzZKI=";
    
    Material SphereMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        lightTrigger.onClick.AddListener(onTriggerClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTriggerClick(){
        IoTInvoke();
        //SphereMaterial = Resources.Load<Material>("GreenMat");
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.material = SphereMaterial;
    }

    private static async Task IoTInvoke()
    {
        ValidateConnectionString();

        // Create a ServiceClient to communicate with service-facing endpoint on your hub.
        s_serviceClient = ServiceClient.CreateFromConnectionString(s_connectionString);

        await InvokeMethodAsync();

        s_serviceClient.Dispose();
    }


    private static async Task InvokeMethodAsync()
    {
        var methodInvocation = new CloudToDeviceMethod("SetTelemetryInterval")
        {
            ResponseTimeout = TimeSpan.FromSeconds(30),
        };
        methodInvocation.SetPayloadJson("10");

        // Invoke the direct method asynchronously and get the response from the simulated device.
        var response = await s_serviceClient.InvokeDeviceMethodAsync("MyPythonDevice", methodInvocation);

        //Console.WriteLine($"\nResponse status: {response.Status}, payload:\n\t{response.GetPayloadAsJson()}");
    }

    private static void ValidateConnectionString()
    {
        try
        {
            _ = IotHubConnectionStringBuilder.Create(s_connectionString);
        }
        catch (Exception)
        {
            //Console.WriteLine("This sample needs a device connection string to run. Program.cs can be edited to specify it, or it can be included on the command-line as the only parameter.");
            Environment.Exit(1);
        }
    }
}
