using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whenables;

namespace WhenablesTests
{
    [TestClass]
    public class WhenableTests
    {
        [TestMethod]
        public async Task SetValueAsync()
        {
            var v = new Whenable<int>();

            Task setTask = Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 10; i > 0; i--)
                {
                    v.Value = i;
                }
            });

            Task<int> valueTask = v.WhenAsync(i => i == 1);

            await Task.WhenAll(setTask, valueTask);

            Assert.IsTrue(valueTask.IsCompletedSuccessfully);
            Assert.AreEqual(1, valueTask.Result);
        }
    }
}
