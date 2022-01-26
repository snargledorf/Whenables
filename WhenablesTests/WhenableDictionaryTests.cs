using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whenables;

namespace WhenablesTests
{
    [TestClass]
    public class WhenableDictionaryTests
    {
        [TestMethod]
        public async Task WhenAddedAsync()
        {
            const int expectedKey = 9;

            var dict = new WhenableDictionary<int, string>();
            
            Task addTask = Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 0; i < expectedKey+1; i++)
                {
                    dict.Add(i, i.ToString());
                }
            });

            Task<KeyValuePair<int, string>> resultTask = dict.WhenAddedAsync((k, v) => k == expectedKey);

            await Task.WhenAll(addTask, resultTask);

            Assert.IsTrue(resultTask.IsCompletedSuccessfully);
            Assert.AreEqual(expectedKey, resultTask.Result.Key);
        }

        [TestMethod]
        public async Task WhenRemovedAsync()
        {
            const int expectedKey = 9;

            var dict = new WhenableDictionary<int, string>();

            Task addRemoveTask = Task.Delay(100).ContinueWith(t =>
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

            Task<KeyValuePair<int, string>> resultTask = dict.WhenRemovedAsync((k, v) => k == expectedKey);

            await Task.WhenAll(addRemoveTask, resultTask);

            Assert.IsTrue(resultTask.IsCompletedSuccessfully);
            Assert.AreEqual(expectedKey, resultTask.Result.Key);
        }
    }
}
