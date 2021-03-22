using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ARTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ARTestSimplePasses()
        {
            // Use the Assert class to test conditions
            var objectToDestroy = new GameObject("name");
            var cleanseBeforeObject = new GameObject().AddComponent<PlacingObjects>();
            cleanseBeforeObject.cleanseBefore("name");
            Assert.IsFalse(GameObject.Find("name"));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ARTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
