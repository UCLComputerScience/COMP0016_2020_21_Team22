# ControlAR

An application that allows you to visualise data coming from IoT devices in AR and also control IoT devices in AR. 

## Deployment Manual

### Prerequisites

Prerequisites:

•	Unity 2019+ (ideally  2019.4.18(LTS))

•	Azure account

•	Mobile Device – specifically Android device (will compile for iOS but application was not designed for iOS) 

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

Replace the value of the CONNECTION_STRING variable with the device connection string you made a note of earlier. Then save your changes to SimulatedDevice.py.

Create a storage account in Azure:

More info on this here:
https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-cli

create a blob container called: “iotoutput”

Create a stream analytics job in azure to store the data from the Raspberry Pi:

More info on this here:
https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-live-data-visualization-in-power-bi

Make sure to set the input as the IoT Hub and the output as the “iotoutput” container you made earlier. It is also very important that you set the “event serialization format” for the output as CSV.

## Unity App

Input the connection strings for storage and IoT hub in to the "AzureConnnection" script in Unity.


Select the target platform as android and then build and run on your android phone!

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone whose code was used
* Inspiration
* etc

