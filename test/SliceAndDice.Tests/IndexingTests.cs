// Copyright (c) SciStack/NumSharp 2018
// Copyright (c) Henon 2019

using System.Linq;
using NUnit.Framework;

namespace SliceAndDice.Tests
{
    [TestFixture]
    public class IndexingTest 
    {
        [Test]
        public void GetValue_2D()
        {
            var a = ArraySlice<int>.Range(12).Reshape(3, 4);
            Assert.IsTrue(a.GetValue(1, 1) == 5);
            Assert.IsTrue(a.GetValue(2, 0) == 8);
        }

        [Test]
        public void GetValue_3D()
        {
            var a = ArraySlice<int>.Range(8).Reshape(2, 2, 2);
            Assert.AreEqual(a.GetValue(1, 1, 1), 7);
            Assert.AreEqual(a.GetValue(1, 1, 0) , 6);
            Assert.AreEqual(a.GetValue(1, 0, 0) , 4);
        }

        [Test]
        public void RowAccess()
        {
            var a = ArraySlice<int>.Range(4).Reshape(2, 2);
            var row1 = a["0"];
            Assert.AreEqual(row1[0], 0);
            Assert.AreEqual(row1[1], 1);
        }

        [Test]
        public void RowAccess_3D()
        {
            var a = ArraySlice<int>.Range(1, 18, 1).Reshape(3, 3, 2);
            var row1 = a["0"];
            Assert.AreEqual(row1[0, 0], 1);
            Assert.AreEqual(row1[0, 1], 2);
            Assert.AreEqual(row1[1, 0], 3);
            Assert.AreEqual(row1[1, 1], 4);
            Assert.AreEqual(row1[2, 0], 5);
            Assert.AreEqual(row1[2, 1], 6);
        }

        [Test]
        public void IndexAccessorSetter()
        {
            var a = ArraySlice<int>.Range(12).Reshape(3, 4);

            Assert.IsTrue(a.GetValue(0, 3) == 3);
            Assert.IsTrue(a.GetValue(1, 3) == 7);

            // set value
            a[ 0, 0]= 10;
            Assert.IsTrue(a.GetValue(0, 0) == 10);
            Assert.IsTrue(a.GetValue(1, 3) == 7);
        }

        //[Test]
        //public void BoolArray()
        //{
        //    var A = new double[] { 1, 2, 3 };

        //    var booleanArr = new bool[] { false, false, true };

        //    A[booleanArr.MakeGeneric<bool>()] = 1;

        //    Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(A.Data<double>(), new double[] { 1, 2, 1 }));

        //    A = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } };

        //    booleanArr = new bool[,] { { true, false, true }, { false, true, false } };

        //    A[booleanArr.MakeGeneric<bool>()] = -2;

        //    Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(A.Data<double>(), new double[] { -2, 2, -2, 4, -2, 6 }));

        //}

        //[Test]
        //public void Compare()
        //{
        //    NDArray A = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } };

        //    var boolArr = A < 3;
        //    Assert.IsTrue(Enumerable.SequenceEqual(boolArr.Data<bool>(), new[] { true, true, false, false, false, false }));

        //    A[A < 3] = -2;
        //    Assert.IsTrue(Enumerable.SequenceEqual(A.Data<double>(), new double[] { -2, -2, 3, 4, 5, 6 }));

        //    var a = A[A == -2 | A > 5];

        //    Assert.IsTrue(Enumerable.SequenceEqual(a.Data<double>(), new double[] { -2, -2, 6 }));
        //}

        //[Test]
        //public void NDArrayByNDArray()
        //{
        //    NDArray x = new double[] { 1, 2, 3, 4, 5, 6 };

        //    NDArray index = new int[] { 1, 3, 5 };

        //    NDArray selected = x[index];

        //    double[] a = (System.Array)selected as double[];
        //    double[] b = { 2, 4, 6 };

        //    Assert.IsTrue(Enumerable.SequenceEqual(a, b));
        //}

        //[Test]
        //public void Filter1D()
        //{
        //    var nd = np.array(new int[] { 3, 1, 1, 2, 3, 1 });
        //    var filter = np.array(new int[] { 0, 2, 5 });
        //    var result = nd[filter];

        //    Assert.IsTrue(Enumerable.SequenceEqual(new int[] { 3, 1, 1 }, result));
        //}

        //[Test]
        //public void Filter2D()
        //{
        //    var nd = np.array(new int[][]
        //    {
        //        new int[]{ 3, 1, 1, 2},
        //        new int[]{ 1, 2, 2, 3},
        //        new int[]{ 2, 1, 1, 3},
        //    });
        //    var filter = np.array(new int[] { 0, 2 });
        //    var result = nd[filter];

        //    Assert.IsTrue(Enumerable.SequenceEqual(new int[] { 3, 1, 1, 2 }, (result[0] as NDArray)));
        //    Assert.IsTrue(Enumerable.SequenceEqual(new int[] { 2, 1, 1, 3 }, (result[1] as NDArray)));

        //    var x = nd[1];
        //    x.ravel();
        //}

        [Test]
        public void Slice1()
        {
            var x = ArraySlice<int>.Range(5);
            var y1 = x["1:3"];
            Assert.AreEqual(y1, new int[] { 1, 2 });

            var y2 = x["3:"];
            Assert.AreEqual(y2, new int[] { 3, 4 });
            y2[0] = 8;
            y2[1] = 9;
            Assert.AreEqual((int)y2[0], 8);
        }


        [Test]
        public void Slice2()
        {
            //>>> x = ArraySlice<int>.Range(5)
            //        >>> x
            //array([0, 1, 2, 3, 4])
            //    >>> y = x[0:5]
            //    >>> y
            //array([0, 1, 2, 3, 4])
            var x = ArraySlice<int>.Range(5);
            var y1 = x["0:5"];
            Assert.AreEqual(y1, new int[] { 0, 1, 2, 3, 4 });
            y1 = x["1:4"];
            Assert.AreEqual(y1, new int[] { 1, 2, 3 });
            //    >>> z = x[:]
            //    >>> z
            //array([0, 1, 2, 3, 4])
            var y2 = x[":"];
            Assert.AreEqual(y2, new int[] { 0, 1, 2, 3, 4 });

            // out of bounds access is handled gracefully by numpy
            //    >>> y = x[0:77]
            //    >>> y
            //array([0, 1, 2, 3, 4])
            var y3 = x["0:77"];
            Assert.AreEqual(y3, new int[] { 0, 1, 2, 3, 4 });

            //    >>> y = x[-77:]
            //    >>> y
            //array([0, 1, 2, 3, 4])
            var y4 = x["-77:"];
            Assert.AreEqual(y4, new int[] { 0, 1, 2, 3, 4 });
            var y = x["-77:77"];
            Assert.AreEqual(y, new int[] { 0, 1, 2, 3, 4 });
        }

        [Test]
        public void Slice3()
        {
            //>>> x = ArraySlice<int>.Range(6)
            //>>> x
            //array([0, 1, 2, 3, 4, 5])
            //>>> y = x[1:5]
            //>>> y
            //array([1, 2, 3, 4])
            //>>> z = y[:3]
            //>>> z
            //array([1, 2, 3])
            //>>> z[0] = 99
            //>>> y
            //array([99, 2, 3, 4])
            //>>> x
            //array([0, 99, 2, 3, 4, 5])
            //>>>
            var x = ArraySlice<int>.Range(6);
            var y = x["1:5"];
            Assert.AreEqual(new int[] { 1, 2, 3, 4, }, y);
            var z = y[":3"];
            Assert.AreEqual(new int[] { 1, 2, 3 }, z);
            z[0] = 99;
            Assert.AreEqual(new int[] { 99, 2, 3, 4 }, y);
            Assert.AreEqual(new int[] { 0, 99, 2, 3, 4, 5 }, x);
        }

        [Test]
        public void Slice4()
        {
            //>>> x = ArraySlice<int>.Range(5)
            //>>> x
            //array([0, 1, 2, 3, 4])
            var x = ArraySlice<int>.Range(5);
            //>>> y = x[2:4]
            //>>> y
            //array([2,3])
            var y = x["2:4"];
            Assert.AreEqual(2, (int)y[0]);
            Assert.AreEqual(3, (int)y[1]);
            y[0] = 77;
            y[1] = 99;
            Assert.AreEqual(77, (int)x[2]);
            Assert.AreEqual(99, (int)x[3]);
        }

        [Test]
        public void Slice_Step()
        {
            //>>> x = ArraySlice<int>.Range(5)
            //>>> x
            //array([0, 1, 2, 3, 4])
            var x = ArraySlice<int>.Range(5);
            //>>> y = x[::-1]
            //>>> y
            //array([4, 3, 2, 1, 0])
            var y = x["::-1"];
            Assert.AreEqual(y, new int[] { 4, 3, 2, 1, 0 });

            //>>> y = x[::2]
            //>>> y
            //array([0, 2, 4])
            y = x["::2"];
            Assert.AreEqual(y, new int[] { 0, 2, 4 });
        }

        [Test]
        public void Slice_Step1()
        {
            //>>> x = ArraySlice<int>.Range(6)
            //>>> x
            //array([0, 1, 2, 3, 4, 5])
            //>>> y = x[::- 1]
            //>>> y
            //array([5, 4, 3, 2, 1, 0])
            //>>> y[0] = 99
            //>>> x
            //array([0, 1, 2, 3, 4, 99])
            //>>> y
            //array([99, 4, 3, 2, 1, 0])
            //>>> y = x[::-1]
            //>>> y
            //array([5, 4, 3, 2, 1, 0])
            var x = ArraySlice<int>.Range(6);
            var y = x["::-1"];
            y[0] = 99;
            Assert.AreEqual(new int[] { 0, 1, 2, 3, 4, 99 }, x);
            Assert.AreEqual(new int[] { 99, 4, 3, 2, 1, 0 }, y);
            //>>> z = y[::2]
            //>>> z
            //array([99, 3, 1])
            //>>> z[1] = 111
            //>>> x
            //array([0, 1, 2, 111, 4, 99])
            //>>> y
            //array([99, 4, 111, 2, 1, 0])
            var z = y["::2"];
            Assert.AreEqual(new int[] { 99, 3, 1 }, z);
            z[1] = 111;
            Assert.AreEqual(new int[] { 99, 111, 1 }, z);
            Assert.AreEqual(new int[] { 0, 1, 2, 111, 4, 99 }, x);
            Assert.AreEqual(new int[] { 99, 4, 111, 2, 1, 0 }, y);
        }

     
        [Test]
        public void Slice_Step2()
        {
            //>>> x = ArraySlice<int>.Range(5)
            //>>> x
            //array([0, 1, 2, 3, 4])
            var x = ArraySlice<int>.Range(5);
            //>>> y = x[::2]
            //>>> y
            //array([0, 2, 4])
            var y = x["::2"];
            Assert.AreEqual(0, (int)y[0]);
            Assert.AreEqual(2, (int)y[1]);
            Assert.AreEqual(4, (int)y[2]);
        }

        [Test]
        public void Slice3x2x2()
        {
            //>>> x=np.arange(12).reshape(3,2,2)
            //>>> x
            //array([[[0, 1],
            //        [ 2,  3]],
            //
            //       [[ 4,  5],
            //        [ 6,  7]],
            //
            //       [[ 8,  9],
            //        [10, 11]]])
            //>>> y1 = x[1:]
            //>>> y1
            //array([[[ 4,  5],
            //        [ 6,  7]],
            //
            //       [[ 8,  9],
            //        [10, 11]]])
            var x = ArraySlice<int>.Range(12).Reshape(3, 2, 2);
            var y1 = x["1:"];
            Assert.AreEqual(new int[] { 2, 2, 2 }, y1.Shape.Dimensions);
            Assert.AreEqual(new int[] { 4, 5, 6, 7, 8, 9, 10, 11 }, y1);

            var y1_0 = y1["0"];
            Assert.AreEqual(new int[] { 2, 2 }, y1_0.Shape.Dimensions);
            Assert.AreEqual(new int[] { 4, 5, 6, 7 }, y1_0);

            // change view
            y1.SetValues(new int[]{0, 1}, new int[] { 100, 101 });
            Assert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5, 100, 101, 8, 9, 10, 11 }, x);
            Assert.AreEqual(new int[] { 4,5, 100, 101, 8, 9, 10, 11 }, y1);

            var y2 = x["2:"];
            Assert.AreEqual(new int[] { 1, 2, 2 }, y2.Shape.Dimensions);
            Assert.AreEqual(new int[] { 8, 9, 10, 11 }, y2);
        }



    }
}
