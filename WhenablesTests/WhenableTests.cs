using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whenables;

namespace WhenablesTests
{
    [TestClass]
    public class WhenableTests
    {
        [TestMethod]
        public void SetValueGet()
        {
            var v = new Whenable<int>();

            Task.Delay(100).ContinueWith(t =>
            {
                for (int i = 10; i > 0; i--)
                {
                    v.Value = i;
                }
            });

            int value = v.When(i => i == 1).Get();

            Assert.AreEqual(1, value);
        }
    }
}
