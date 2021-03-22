using UnityEngine;

public class AzureConnection : MonoBehaviour
{
    // Connection string for your IoT Hub

    // az iot hub show-connection-string --hub-name {your iot hub name} --policy-name service

    public static string IotConnectionString = "HostName=MohseenHub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=lXC2t+EC5ZFHK+4ZouNwamc93gcZwQ5UsxLBXOFzZKI=";



    // Connection string for your Storage Account

    //az storage account show-connection-string --resource-group {your resource group name} --name {your storage account name}



    public static string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=mohseenhubblob;AccountKey=SSMMWgXQnBk0oZr7YBbmmC4ZY5YYY/TypNX9CVXiDqFQTEHqSZ9IYl9JPUS+HXbI2y5HBsijkEdrcOTT5B4UrA==;EndpointSuffix=core.windows.net";



}