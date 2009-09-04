using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pysor;

namespace Tests
{
    [TestFixture]
    public class CommonTypeToolTests
    {
        private CommonTypeTool _theTool;

        [SetUp]
        public void SetUp()
        {
            
            _theTool = new CommonTypeTool();
        }

        [Test]
        public void CanFindTypeOfTwoObjectsOfSameType()
        {
            var ret = _theTool.GetCommonType(new object[] {1, 2, 5});
            Assert.AreEqual(typeof(int), ret);
        }

        [Test]
        public void CanFindTypeOfTwoDescendantObjectsOfSameType()
        {
            var type = _theTool.GetCommonType(new object[] {new C(), new B(), new C() });
            Assert.AreEqual(typeof(B), type);
        }
        
        [Test]
        public void CanFindHierarchyRoot()
        {
            var ret = _theTool.GetCommonType(new object[] {new C(), new B(), new E() });
            Assert.AreEqual(typeof(A), ret);
        }

        [Test]
        public void CanDetectObjectAsHierarchyRoot()
        {
            var ret = _theTool.GetCommonType(new object[] {new C(), new B(), new E(), "d" });
            Assert.AreEqual(typeof(object), ret);
        }
    }

    public class A
    {}
    public class B : A
    {}
    public class C : B
    {}
    public class E : A
    {}
}
