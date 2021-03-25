using NUnit.Framework;
using UnityEngine;



namespace Tests
{
    public class AzureConnectionTest
    {
        // tests if program can download CSV successfully
        [Test]
        async public void AzureConnectionDownloadTest()
        {

            // Use the Assert class to test conditions
            await CSVDownloader.GetIoTData("test");
            Assert.IsTrue(System.IO.File.Exists(string.Format(Application.temporaryCachePath + "/{0}", "test" + ".csv")));
        }
    }

}
