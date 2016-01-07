using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whenables;

namespace WhenablesTests
{
    [TestClass]
    public class WhenableDictionaryTests
    {
        [TestMethod]
        public void WhenAddedGet()
        {
            const int expectedKey = 9;

            var dict = new WhenableDictionary<int, string>();
            
            Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 0; i < expectedKey+1; i++)
                {
                    dict.Add(i, i.ToString());
                }
            });

            string result = dict.WhenAdded((k, v) => k == expectedKey).GetValue();

            Assert.AreEqual(expectedKey.ToString(), result);
        }

        [TestMethod]
        public void WhenRemovedGet()
        {
            const int expectedKey = 9;

            var dict = new WhenableDictionary<int, string>();

            Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 0; i < expectedKey + 1; i++)
                {
                    dict.Add(i, i.ToString());
                }
                for (int i = expectedKey; i > 0; i--)
                {
                    dict.Remove(i);
                }
            });

            string result = dict.WhenRemoved((k, v) => k == expectedKey).GetValue();

            Assert.AreEqual(expectedKey.ToString(), result);
        }

        [TestMethod]
        public async Task WhenAddedAsync()
        {
            const int expectedKey = 9;

            var dict = new WhenableDictionary<int, string>();

            Task task = Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 0; i < expectedKey + 1; i++)
                {
                    dict.Add(i, i.ToString());
                }
            });

            string result = await dict.WhenAdded((k, v) => k == expectedKey).GetValueAsync();

            Assert.AreEqual(expectedKey.ToString(), result);
        }
    }
}
