// Note: the Shape tests has been taken from SciSharp/NumSharp
// Copyright (c) SciSharp
// Copyright (c) 2019, Henon <meinrad.recheis@gmal.com>

using System;
using System.Linq;
using NUnit.Framework;

namespace SliceAndDice.Tests
{
    [TestFixture]
    public class ShapeTests
    {

        [Test]
        public void Index()
        {
            var shape0 = new Shape(4,3);

            int idx0 = shape0.GetIndex(2,1);
        
            Assert.AreEqual(7, idx0);
        }

        [Test]
        public void CheckIndexing()
        {
            var shape0 = new Shape(4,3,2);

            int[] strgDimSize = shape0.Strides;

            int index = shape0.GetIndex(1,2,1);

            Assert.IsTrue(Enumerable.SequenceEqual(shape0.GetCoords(index),new int[]{1,2,1}));

            var rnd = new Random();
            var randomIndex = new int[]{rnd.Next(0,3),rnd.Next(0,2),rnd.Next(0,1)};

            int index1 = shape0.GetIndex(randomIndex);
            Assert.IsTrue(Enumerable.SequenceEqual(shape0.GetCoords(index1),randomIndex));

            var shape1 = new Shape(2,3,4);

            index = shape1.GetIndex(1,2,1);
            Assert.IsTrue(Enumerable.SequenceEqual(shape1.GetCoords(index),new int[]{1,2,1}));

            randomIndex = new int[]{rnd.Next(0,1),rnd.Next(0,2),rnd.Next(0,3)};
            index = shape1.GetIndex(randomIndex);
            Assert.IsTrue(Enumerable.SequenceEqual(shape1.GetCoords(index),randomIndex));

            randomIndex = new int[]{rnd.Next(1,10),rnd.Next(1,10),rnd.Next(1,10)};            

            var shape2 = new Shape(randomIndex);

            randomIndex = new int[]{rnd.Next(0,shape2.Dimensions[0]),rnd.Next(0,shape2.Dimensions[1]),rnd.Next(0,shape2.Dimensions[2])};

            index = shape2.GetIndex(randomIndex);
            Assert.IsTrue(Enumerable.SequenceEqual(shape2.GetCoords(index),randomIndex));
        }

        //[Test]
        //public void CheckColRowSwitch()
        //{
        //    var shape1 = new Shape(5);
        //    Assert.IsTrue(Enumerable.SequenceEqual(shape1.Strides, new int[] { 1 }));

        //    shape1.ChangeTensorLayout();
        //    Assert.IsTrue(Enumerable.SequenceEqual(shape1.Strides, new int[] { 1 }));

        //    var shape2 = new Shape(4, 3);
        //    Assert.IsTrue(Enumerable.SequenceEqual(shape2.Strides, new int[] { 1, 4 }));

        //    shape2.ChangeTensorLayout();
        //    Assert.IsTrue(Enumerable.SequenceEqual(shape2.Strides, new int[] { 3, 1 }));

        //    var shape3 = new Shape(2, 3, 4);
        //    Assert.IsTrue(Enumerable.SequenceEqual(shape3.Strides, new int[] { 1, 2, 6 }));

        //    shape3.ChangeTensorLayout();
        //    Assert.IsTrue(Enumerable.SequenceEqual(shape3.Strides, new int[] { 12, 4, 1 }));

        //}
    }
}
