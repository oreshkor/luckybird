using System;
using NUnit.Framework;

namespace luckybird
{
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