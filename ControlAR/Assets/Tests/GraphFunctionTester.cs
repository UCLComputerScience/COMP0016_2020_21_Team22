using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class GraphFunctionTester
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ThresholdClickTest()
        {
            var ThresholdTester = new GameObject().AddComponent<LinePlotter>();
            ThresholdTester.scaleControl = new GameObject().AddComponent<Slider>();
            ThresholdTester.thresholdButton = new GameObject().AddComponent<Button>();
            ThresholdTester.X = new GameObject().AddComponent<InputField>();
            ThresholdTester.Y = new GameObject().AddComponent<InputField>();
            ThresholdTester.Z = new GameObject().AddComponent<InputField>();
            ThresholdTester.changeGraphType = new GameObject().AddComponent<Button>();
            ThresholdTester.lastData = new GameObject().AddComponent<Button>();
            ThresholdTester.nextData = new GameObject().AddComponent<Button>();
            ThresholdTester.xAxis = new GameObject().AddComponent<Dropdown>();
            ThresholdTester.yAxis = new GameObject().AddComponent<Dropdown>();
            ThresholdTester.zAxis = new GameObject().AddComponent<Dropdown>();
            ThresholdTester.thresholdCanvas = new GameObject().AddComponent<Canvas>();

            ThresholdTester.onThresholdClick();
            Assert.IsFalse(ThresholdTester.thresholdCanvas.gameObject.activeSelf);
            GameObject.Destroy(ThresholdTester);

        }

        [Test]
        public void thresholdUpdateTest()
        {
            var ThresholdTester = new GameObject().AddComponent<LinePlotter>();
            ThresholdTester.scaleControl = new GameObject().AddComponent<Slider>();
            ThresholdTester.thresholdButton = new GameObject().AddComponent<Button>();
            ThresholdTester.X = new GameObject().AddComponent<InputField>();
            ThresholdTester.Y = new GameObject().AddComponent<InputField>();
            ThresholdTester.Z = new GameObject().AddComponent<InputField>();
            ThresholdTester.changeGraphType = new GameObject().AddComponent<Button>();
            ThresholdTester.lastData = new GameObject().AddComponent<Button>();
            ThresholdTester.nextData = new GameObject().AddComponent<Button>();
            ThresholdTester.xAxis = new GameObject().AddComponent<Dropdown>();
            ThresholdTester.yAxis = new GameObject().AddComponent<Dropdown>();
            ThresholdTester.zAxis = new GameObject().AddComponent<Dropdown>();

            ThresholdTester.ThresholdUpdate();
            Assert.IsTrue(GameObject.Find("not ben") != null);
            GameObject.Destroy(ThresholdTester);

        }

        [Test]
        public void scaleUpdateTest()
        {
            var scaleUpdateTester = new GameObject().AddComponent<LinePlotter>();
            scaleUpdateTester.scaleControl = new GameObject().AddComponent<Slider>();
            scaleUpdateTester.thresholdButton = new GameObject().AddComponent<Button>();
            scaleUpdateTester.X = new GameObject().AddComponent<InputField>();
            scaleUpdateTester.Y = new GameObject().AddComponent<InputField>();
            scaleUpdateTester.Z = new GameObject().AddComponent<InputField>();
            scaleUpdateTester.changeGraphType = new GameObject().AddComponent<Button>();
            scaleUpdateTester.lastData = new GameObject().AddComponent<Button>();
            scaleUpdateTester.nextData = new GameObject().AddComponent<Button>();
            scaleUpdateTester.xAxis = new GameObject().AddComponent<Dropdown>();
            scaleUpdateTester.yAxis = new GameObject().AddComponent<Dropdown>();
            scaleUpdateTester.zAxis = new GameObject().AddComponent<Dropdown>();

            scaleUpdateTester.ValueChangeCheck();
            Assert.AreEqual(scaleUpdateTester.plotScale, scaleUpdateTester.scaleControl.value);
            GameObject.Destroy(scaleUpdateTester);

        }


        [Test]
        public void graphTypeChange()
        {
            var typeChangeCheck = new GameObject().AddComponent<LinePlotter>();
            typeChangeCheck.scaleControl = new GameObject().AddComponent<Slider>();
            typeChangeCheck.thresholdButton = new GameObject().AddComponent<Button>();
            typeChangeCheck.X = new GameObject().AddComponent<InputField>();
            typeChangeCheck.Y = new GameObject().AddComponent<InputField>();
            typeChangeCheck.Z = new GameObject().AddComponent<InputField>();
            typeChangeCheck.changeGraphType = new GameObject().AddComponent<Button>();
            typeChangeCheck.lastData = new GameObject().AddComponent<Button>();
            typeChangeCheck.nextData = new GameObject().AddComponent<Button>();
            typeChangeCheck.xAxis = new GameObject().AddComponent<Dropdown>();
            typeChangeCheck.yAxis = new GameObject().AddComponent<Dropdown>();
            typeChangeCheck.zAxis = new GameObject().AddComponent<Dropdown>();

            bool before = typeChangeCheck.line;
            typeChangeCheck.onChangeGraphClicked();
            Assert.AreNotEqual(before, typeChangeCheck.line);
            GameObject.Destroy(typeChangeCheck);

        }


        [Test]
        public void DrawLIneTest()
        {
            var DrawLineTester = new GameObject().AddComponent<LinePlotter>();
            DrawLineTester.scaleControl = new GameObject().AddComponent<Slider>();
            DrawLineTester.thresholdButton = new GameObject().AddComponent<Button>();
            DrawLineTester.X = new GameObject().AddComponent<InputField>();
            DrawLineTester.Y = new GameObject().AddComponent<InputField>();
            DrawLineTester.Z = new GameObject().AddComponent<InputField>();
            DrawLineTester.changeGraphType = new GameObject().AddComponent<Button>();
            DrawLineTester.lastData = new GameObject().AddComponent<Button>();
            DrawLineTester.nextData = new GameObject().AddComponent<Button>();
            DrawLineTester.xAxis = new GameObject().AddComponent<Dropdown>();
            DrawLineTester.yAxis = new GameObject().AddComponent<Dropdown>();
            DrawLineTester.zAxis = new GameObject().AddComponent<Dropdown>();
            DrawLineTester.smallPointPrefab = (GameObject)Resources.Load("smallDataBall");
            DrawLineTester.line = false;

            DrawLineTester.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.red, Color.red, "ben");


            Assert.IsTrue(GameObject.Find("ben") != null);
            GameObject.Destroy(DrawLineTester);

        }

        [Test]
        public void NoDivideZeroTest1()
        {
            var DivideZeroTester = new GameObject().AddComponent<LinePlotter>();
            DivideZeroTester.scaleControl = new GameObject().AddComponent<Slider>();
            DivideZeroTester.thresholdButton = new GameObject().AddComponent<Button>();
            DivideZeroTester.X = new GameObject().AddComponent<InputField>();
            DivideZeroTester.Y = new GameObject().AddComponent<InputField>();
            DivideZeroTester.Z = new GameObject().AddComponent<InputField>();
            DivideZeroTester.changeGraphType = new GameObject().AddComponent<Button>();
            DivideZeroTester.lastData = new GameObject().AddComponent<Button>();
            DivideZeroTester.nextData = new GameObject().AddComponent<Button>();
            DivideZeroTester.xAxis = new GameObject().AddComponent<Dropdown>();
            DivideZeroTester.yAxis = new GameObject().AddComponent<Dropdown>();
            DivideZeroTester.zAxis = new GameObject().AddComponent<Dropdown>();

            float x = DivideZeroTester.NoDivideZero(10, 10, 10);


            Assert.AreEqual(x, 0);
            GameObject.Destroy(DivideZeroTester);

        }

        [Test]
        public void NoDivideZeroTest2()
        {
            var DivideZeroTester = new GameObject().AddComponent<LinePlotter>();
            DivideZeroTester.scaleControl = new GameObject().AddComponent<Slider>();
            DivideZeroTester.thresholdButton = new GameObject().AddComponent<Button>();
            DivideZeroTester.X = new GameObject().AddComponent<InputField>();
            DivideZeroTester.Y = new GameObject().AddComponent<InputField>();
            DivideZeroTester.Z = new GameObject().AddComponent<InputField>();
            DivideZeroTester.changeGraphType = new GameObject().AddComponent<Button>();
            DivideZeroTester.lastData = new GameObject().AddComponent<Button>();
            DivideZeroTester.nextData = new GameObject().AddComponent<Button>();
            DivideZeroTester.xAxis = new GameObject().AddComponent<Dropdown>();
            DivideZeroTester.yAxis = new GameObject().AddComponent<Dropdown>();
            DivideZeroTester.zAxis = new GameObject().AddComponent<Dropdown>();

            float x = DivideZeroTester.NoDivideZero(10, 0, 10);


            Assert.AreEqual(x, 1);
            GameObject.Destroy(DivideZeroTester);
        }

            // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
            // `yield return null;` to skip a frame.
            [UnityTest]
        public IEnumerator GraphFunctionTesterWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
