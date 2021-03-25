using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

namespace Tests
{
    public class ARFunctionTester
    {
        // A Test behaves as an ordinary method
        [Test]
        public void cleanserSimplePasses()
        {
            var cleanseBeforeObject = new GameObject().AddComponent<PlacingObjects>();
            cleanseBeforeObject.stopEditButton = new GameObject().AddComponent<Button>();
            cleanseBeforeObject.selectMachine = new GameObject().AddComponent<Button>();
            cleanseBeforeObject.stopPlacingButton = new GameObject().AddComponent<Button>();
            cleanseBeforeObject.showDataButton = new GameObject().AddComponent<Button>();
            cleanseBeforeObject.showFunctionButton = new GameObject().AddComponent<Button>(); 
            var objectToDestroy = new GameObject("name");

            cleanseBeforeObject.cleanseBefore("name");
            Assert.IsFalse(GameObject.Find("name") == null);
        }

        [Test]
        public void TaskonClickSimplePasses()
        {
            var onClickTest = new GameObject().AddComponent<PlacingObjects>();
            onClickTest.stopEditButton = new GameObject().AddComponent<Button>();
            onClickTest.selectMachine = new GameObject().AddComponent<Button>();
            onClickTest.stopPlacingButton = new GameObject().AddComponent<Button>();
            Text textObject = new GameObject().AddComponent<Text>();
            textObject.transform.SetParent(onClickTest.stopPlacingButton.transform);
            onClickTest.showDataButton = new GameObject().AddComponent<Button>();
            onClickTest.showFunctionButton = new GameObject().AddComponent<Button>(); 

            onClickTest.TaskOnClick();
            Assert.AreEqual(textObject.text, "size and\nrotation");
        }
        
        [Test]
        public void MachineSelectTest()
        {
            var machineSelectTest = new GameObject().AddComponent<PlacingObjects>();
            machineSelectTest.stopEditButton = new GameObject().AddComponent<Button>();
            machineSelectTest.selectMachine = new GameObject().AddComponent<Button>();
            machineSelectTest.stopPlacingButton = new GameObject().AddComponent<Button>();
            machineSelectTest.showDataButton = new GameObject().AddComponent<Button>();
            machineSelectTest.showFunctionButton = new GameObject().AddComponent<Button>();
            machineSelectTest.stateMessage = new TextMeshProUGUI();

            machineSelectTest.onMachineSelected("name");
            Assert.IsTrue(machineSelectTest.stopPlacingButton.IsActive());
        }

        [Test]
        public void getDifferenceTest()
        {
            var getDifferenceTest = new GameObject().AddComponent<PlacingObjects>();
            getDifferenceTest.stopEditButton = new GameObject().AddComponent<Button>();
            getDifferenceTest.selectMachine = new GameObject().AddComponent<Button>();
            getDifferenceTest.stopPlacingButton = new GameObject().AddComponent<Button>();
            getDifferenceTest.showDataButton = new GameObject().AddComponent<Button>();
            getDifferenceTest.showFunctionButton = new GameObject().AddComponent<Button>();

            string x = getDifferenceTest.getDifferenceAtEnd("abcdefghijk", "abcd");
            Assert.AreEqual(x, "efghijk");
        }

        [Test]
        public void deselectTest()
        {
            var deselectTest = new GameObject().AddComponent<PlacingObjects>();
            deselectTest.stopEditButton = new GameObject().AddComponent<Button>();
            deselectTest.selectMachine = new GameObject().AddComponent<Button>();
            deselectTest.stopPlacingButton = new GameObject().AddComponent<Button>();
            deselectTest.showDataButton = new GameObject().AddComponent<Button>();
            deselectTest.showFunctionButton = new GameObject().AddComponent<Button>();
            deselectTest.stateMessage = new TextMeshProUGUI();

            deselectTest.deselected();
            Assert.IsFalse(deselectTest.stopPlacingButton.IsActive());
        }
        
        [Test]
        public void functionTest()
        {
            var functionListTestObject = new GameObject().AddComponent<PlacingObjects>();
            functionListTestObject.stopEditButton = new GameObject().AddComponent<Button>();
            functionListTestObject.selectMachine = new GameObject().AddComponent<Button>();
            functionListTestObject.stopPlacingButton = new GameObject().AddComponent<Button>();
            functionListTestObject.showDataButton = new GameObject().AddComponent<Button>();
            functionListTestObject.showFunctionButton = new GameObject().AddComponent<Button>();
            GameObject testObject = new GameObject();
            functionListTestObject.functionList = testObject;

            functionListTestObject.onShowFunctionClick();
            Assert.IsFalse(testObject.activeSelf);
        }

        [Test]
        public void dataListTest()
        {
            var dataListTest = new GameObject().AddComponent<PlacingObjects>();
            dataListTest.stopEditButton = new GameObject().AddComponent<Button>();
            dataListTest.selectMachine = new GameObject().AddComponent<Button>();
            dataListTest.stopPlacingButton = new GameObject().AddComponent<Button>();
            dataListTest.showDataButton = new GameObject().AddComponent<Button>();
            dataListTest.showFunctionButton = new GameObject().AddComponent<Button>();
            GameObject testObject = new GameObject();
            dataListTest.dataList = testObject;

            dataListTest.onShowDataClicked();
            Assert.IsFalse(testObject.activeSelf);
        }


        [Test]
        public void displayTest()
        {
            var displayTestObject = new GameObject().AddComponent<PlacingObjects>();
            displayTestObject.stopEditButton = new GameObject().AddComponent<Button>();
            displayTestObject.selectMachine = new GameObject().AddComponent<Button>();
            displayTestObject.stopPlacingButton = new GameObject().AddComponent<Button>();
            displayTestObject.showDataButton = new GameObject().AddComponent<Button>();
            displayTestObject.showFunctionButton = new GameObject().AddComponent<Button>();
            displayTestObject.stateMessage = new TextMeshProUGUI();
            displayTestObject.selectedName = "test";
            displayTestObject.displayData();

            Assert.IsTrue(displayTestObject.stateMessage.text.Contains("test"));
        }

        [Test]
        public void addTextTest()
        {
            var addTextObject = new GameObject().AddComponent<PlacingObjects>();
            addTextObject.stopEditButton = new GameObject().AddComponent<Button>();
            addTextObject.selectMachine = new GameObject().AddComponent<Button>();
            addTextObject.stopPlacingButton = new GameObject().AddComponent<Button>();
            addTextObject.showDataButton = new GameObject().AddComponent<Button>();
            addTextObject.showFunctionButton = new GameObject().AddComponent<Button>();
            addTextObject.dataTextToInstantiate = (GameObject)Resources.Load("Text");
            addTextObject.dataHolder = new GameObject();

            addTextObject.addTextToScreen("hello", "test", 0);
            Assert.IsTrue(GameObject.Find("test") != null);
        }
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ARFunctionTesterWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
