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
        public async Task WhenAddedGetAsync()
        {
            var random = new Random();

            int expectedNum = random.Next(10, 20);
            int randomPadding = random.Next(5);
            int upperNumber = expectedNum + randomPadding;

            var list = new WhenableList<int>();

            async Task addTenAsync()
            {
                await Task.Delay(100);
                for (int i = 0; i < 10; i++)
                {
                    list.Add(i);
                    Task.Delay(10).Wait();
                }
            };

            Task addTenTask = addTenAsync();

            Task<int> whenAddedTask = list.WhenAdded(i => i == 9).GetAsync();

            await Task.WhenAll(addTenTask, whenAddedTask);

            Assert.IsTrue(whenAddedTask.IsCompletedSuccessfully);
            Assert.AreEqual(9, whenAddedTask.Result);
        }

        [TestMethod]
        public async Task WhenRemovedGetAsync()
        {
            var random = new Random();
            
            int expectedNum = random.Next(10, 20);
            int randomPadding = random.Next(5);
            int upperNumber = expectedNum + randomPadding;

            var list = new WhenableList<int>();

            for (int i = 0; i < upperNumber; i++)
                list.Add(i);

            async Task addRemoveAsync()
            {
                await Task.Delay(100);

                for (int i = expectedNum; i > 0; i--)
                {
                    list.Remove(i);
                }
            }

            Task addRemoveTask = addRemoveAsync();

            Task<int> whenRemovedTask = list.WhenRemoved(i => i == expectedNum).GetAsync();

            await Task.WhenAll(addRemoveTask, whenRemovedTask);

            Assert.IsTrue(whenRemovedTask.IsCompletedSuccessfully);
            Assert.AreEqual(expectedNum, whenRemovedTask.Result);
        }
    }
}
