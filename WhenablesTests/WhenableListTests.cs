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

            Task<int> whenAddedTask = list.WhenAddedAsync(i => i == 9);

            await Task.WhenAll(addTenTask, whenAddedTask);

            Assert.IsTrue(whenAddedTask.IsCompletedSuccessfully);
            Assert.AreEqual(9, whenAddedTask.Result);
        }

        [TestMethod]
        public async Task MultipleWhenAddedAsync()
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
                    await Task.Delay(10);
                }
            }

            Task addTenTask = addTenAsync();

            Task<int> whenAddedTask = list.WhenAddedAsync(i => i == 9);
            Task<int> whenAdded2Task = list.WhenAddedAsync(IsThree);
            Task<int> whenAdded3Task = list.WhenAddedAsync(IsThree);

            await Task.WhenAll(addTenTask, whenAddedTask, whenAdded2Task, whenAdded3Task);

            Assert.IsTrue(whenAddedTask.IsCompletedSuccessfully);
            Assert.AreEqual(9, whenAddedTask.Result);

            Assert.IsTrue(whenAdded2Task.IsCompletedSuccessfully);
            Assert.AreEqual(3, whenAdded2Task.Result);

            Assert.IsTrue(whenAdded3Task.IsCompletedSuccessfully);
            Assert.AreEqual(3, whenAdded2Task.Result);

            static bool IsThree(int i)
            {
                return i == 3;
            }
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

            Task<int> whenRemovedTask = list.WhenRemovedAsync(i => i == expectedNum);

            await Task.WhenAll(addRemoveTask, whenRemovedTask);

            Assert.IsTrue(whenRemovedTask.IsCompletedSuccessfully);
            Assert.AreEqual(expectedNum, whenRemovedTask.Result);
        }
    }
}
