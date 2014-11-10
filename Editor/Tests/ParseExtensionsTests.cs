using NUnit.Framework;
using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityParseHelpers;

namespace UnityParseHelpersTests
{
    [TestFixture]
    public class ParseExtensionsTests
    {
        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void GetsSingleKey()
        {
            var father = new Father();
            Assert.AreEqual("name", father.GetKey(f => f.Name));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsExeptionTryingToGetKeyOfAPropertyWithNoFieldNameAttribute()
        {
            var father = new Father();
            father.GetKey(f => f.AProperty);
        }

        [Test]
        public void GetsSingleChain()
        {
            var father = new Father();
            Assert.AreEqual("son.name", father.GetKey(f => f.Son.Name));
        }

        [Test]
        public void GetsSingleChainWithInterface()
        {
            var father = new Father();
            Assert.AreEqual("daughter.name", father.GetKey(f => f.Daughter.Name));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsExeptionTryingToGetKeyOfAPropertyWithNoFieldNameAttributeInChain()
        {
            var father = new Father();
            father.GetKey(f => f.Son.Age);
        }

        [Test]
        public void GetsComplexChain()
        {
            var father = new Father();
            Assert.AreEqual("daughter.sister.sister.sister.name", father.GetKey(f => f.Daughter.Sister.Sister.Sister.Name));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsExeptionTryingToGetInterfacedPropertyWithoutAFieldType()
        {
            var father = new Child();
            father.GetKey(c => c.Brother.Name);
        }

        [Test]
        public void GetsListKey()
        {
            var father = new Father();
            Assert.AreEqual("children", father.GetKey(f => f.Children));
        }

        [Test]
        public void GetsChainedKeyFollowingList()
        {
            var father = new Father();
            Assert.AreEqual("children.sister.name", father.GetKey(f => f.Children[0].Sister.Name));
        }
    }
}
