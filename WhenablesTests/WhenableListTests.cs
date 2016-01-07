using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whenables;

namespace WhenablesTests
{
    [TestClass]
    public class WhenableListTests
    {
        [TestMethod]
        public void WhenAddedGet()
        {
            var list = new WhenableList<int>();

            Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 0; i < 10; i++)
                {
                    list.Add(i);
                    Task.Delay(10).Wait();
                }
            });

            int result = list.WhenAdded(i => i == 9).Get();

            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void WhenRemovedGet()
        {
            const int expectedNum = 9;

            var list = new WhenableList<int>();

            Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 0; i < expectedNum + 1; i++)
                {
                    list.Add(i);
                }
                for (int i = expectedNum; i > 0; i--)
                {
                    list.Remove(i);
                }
            });

            int result = list.WhenRemoved(i => i == expectedNum).Get();

            Assert.AreEqual(expectedNum, result);
        }
    }
}
