// Copyright (c) 2019, Henon <meinrad.recheis@gmal.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SliceAndDice.Tests
{
    [TestFixture]
    public class PrettyPrintingTests
    {
        [Test]
        public void FlatTests()
        {
            var a = new ArraySlice<int>(0,1,2,3,4,5,6,7,8,9);            Console.WriteLine(a.ToString(flat: true));
            Assert.AreEqual("[0, 1, 2, 3, 4, 5, 6, 7, 8, 9]", a.ToString(flat:true));            a = ArraySlice<int>.Range(9).Reshape(3, 3);            Console.WriteLine(a.ToString(flat: true));            Assert.AreEqual("[[0, 1, 2], [3, 4, 5], [6, 7, 8]]", a.ToString(flat: true));
            a = ArraySlice<int>.Range(8).Reshape(2, 2, 2);            Console.WriteLine(a.ToString(flat: true));            Assert.AreEqual("[[[0, 1], [2, 3]], [[4, 5], [6, 7]]]", a.ToString(flat: true));
            a = ArraySlice<int>.Range(24).Reshape(2, 3, 4);
            Console.WriteLine(a.ToString(flat: true));
            Assert.AreEqual("[[[0, 1, 2, 3], [4, 5, 6, 7], [8, 9, 10, 11]], [[12, 13, 14, 15], [16, 17, 18, 19], [20, 21, 22, 23]]]", a.ToString(flat: true));
            a = ArraySlice<int>.Range(24).Reshape(4, 3, 2);
            Console.WriteLine(a.ToString(flat: true));
            Assert.AreEqual("[[[0, 1], [2, 3], [4, 5]], [[6, 7], [8, 9], [10, 11]], [[12, 13], [14, 15], [16, 17]], [[18, 19], [20, 21], [22, 23]]]", a.ToString(flat: true));        }

        [Test]
        public void NonFlatTests()
        {
            var a = new ArraySlice<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);            Console.WriteLine(a);
            Assert.AreEqual("[0, 1, 2, 3, 4, 5, 6, 7, 8, 9]", a.ToString(flat: false));            a = ArraySlice<int>.Range(9).Reshape(3, 3);            Console.WriteLine(a);            Assert.AreEqual("[[0, 1, 2], \r\n" +                                    "[3, 4, 5], \r\n" +                                    "[6, 7, 8]]", a.ToString(flat: false));
            a = ArraySlice<int>.Range(8).Reshape(2, 2, 2);
            Console.WriteLine(a);
            Assert.AreEqual("[[[0, 1], \r\n" +                                    "[2, 3]], \r\n" +                                    "[[4, 5], \r\n" +                                    "[6, 7]]]", a.ToString(flat: false));
        }
    }
}
