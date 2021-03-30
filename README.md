# ControlAR

A mobile application that allows you to visualise data coming from IoT devices in AR and also control IoT devices in AR. 

## Deployment Manual

NOTE: It is recommended that you download this repo as a zip file rather than clone it, as it uses Git LFS, so it may not clone completely if the GitHub LFS data quota has been used up for the month. 

### Prerequisites

Prerequisites:

•	Unity 2019+ (ideally  2019.4.18(LTS))

•	Azure account

•	Android Device

•	Raspberry Pi 

### Azure account set up:

Create an IoT hub:

Either use the azure CLI which can be done through following the instructions in the following link:

https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-create-using-cli

or use the azure portal web app:

https://docs.microsoft.com/en-us/azure/iot-hub/quickstart-send-telemetry-python#create-an-iot-hub

To add a device/machine to the IoT hub run the following in Azure CLI:

```
az iot hub device-identity create --hub-name {YourIoTHubName} --device-id MyPythonDevice
```

YourIoTHubName: Replace this placeholder below with the name you chose for your IoT hub.

Run the following command in Azure CLI to get the device connection string for the device you registered:

```
az iot hub device-identity connection-string show --hub-name {YourIoTHubName} --device-id MyPythonDevice --output table
```

### Set Up the Raspberry Pi

Replace the value of the CONNECTION_STRING variable with the device connection string you made a note of earlier. Then save your changes to SimulatedDevice2.py.

Run the script on Raspberry Pi to start sending data to the IoT Hub

### Create a storage account in Azure:

More info on this here:
https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-cli

create a blob container called: “iotoutput”

### Create a stream analytics job in Azure to store the data from the Raspberry Pi into a CSV:

More info on this here:
https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-live-data-visualization-in-power-bi

Make sure to set the input as the name of the IoT Hub you made earlier and the output as the “iotoutput” container you made earlier. It is also very important that you set the “event serialization format” for the output as CSV.

<img width="1551" alt="Screenshot 2021-03-30 at 07 24 03" src="https://user-images.githubusercontent.com/56094705/112943106-f4665a80-9128-11eb-807d-b6588fa3d2ec.png">

## Unity App

Open the ControlAR folder in Unity

Input your Azure account connection strings for the storage and IoT Hub in to the "AzureConnnection.cs" script in Unity.

To obtain the IoT Hub connection string you can run the following in the Azure CLI:

```
az iot hub show-connection-string --hub-name {your iot hub name} --policy-name service
```

To obtain the Storage Account connection string you can run the following command in the Azure CLI:

```
az storage account show-connection-string --resource-group {your resource group name} --name {your storage account name}
```

### Building the app:

Select the target platform as Android

Before building make sure in the player settings:

•	Only Graphics API's should be OpenGLES3 

•	Multithreaded rendering should be turned off 

•	Minimum API level should be set too Android 8.0 'Oreo' (API level 26)

<img width="765" alt="Screenshot 2021-03-30 at 07 01 35" src="https://user-images.githubusercontent.com/56094705/112940986-d3503a80-9125-11eb-8731-a4614ccb5606.png">

