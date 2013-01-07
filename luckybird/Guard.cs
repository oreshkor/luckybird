using NUnit.Framework;
using System;

namespace luckybird
{
    public static class Guard
    {
        public static void Against(bool condition, string paramName = null, string message = null)
        {
            if (condition)
                throw new ArgumentException(paramName, message);
        }
    }

    [TestFixture]
    public class Guard_should
    {

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void throw_an_exception_if_condition_is_true()
        {

            Guard.Against(true, "a", "b");
            Assert.Fail();
        }

        [Test]
        public void not_throw_an_exception_if_condiiton_is_false()
        {
            Guard.Against(false, "a", "b");
        }
    }
}
