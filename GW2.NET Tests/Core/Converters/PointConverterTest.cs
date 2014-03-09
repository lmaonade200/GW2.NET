﻿using System.Drawing;
using Newtonsoft.Json;
using NUnit.Framework;
using PointConverter = GW2DotNET.V1.Core.Converters.PointConverter;

namespace GW2DotNET.Core.Converters
{
    [TestFixture]
    public class PointConverterTest
    {
        [Test]
        [Category("Converters")]
        public void PointConverter_ReadBothNil_ReturnsDefault()
        {
            const string input = "[0,0]";
            Point expected = default(Point);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadEmptyArray_ReturnsDefault()
        {
            const string input = "[]";
            Point expected = default(Point);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        [ExpectedException(typeof(JsonSerializationException))]
        public void PointConverter_ReadEmpty_ExceptionIsThrownForValueType()
        {
            string input = string.Empty;
            JsonConvert.DeserializeObject<Point>(input, new PointConverter());
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadEmpty_ReturnsNullForNullableType()
        {
            string input = string.Empty;
            var output = JsonConvert.DeserializeObject<Point?>(input, new PointConverter());

            Assert.IsNull(output);
        }

        [Test]
        [Category("Converters")]
        [ExpectedException(typeof(JsonSerializationException))]
        public void PointConverter_ReadMoreThanTwoValues_ConverterThrowsJsonSerializationException()
        {
            const string input = "[1,2,3]";
            JsonConvert.DeserializeObject<Point>(input, new PointConverter());
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadNegativeExtremes_PointReflectsInput()
        {
            const string input = "[-2147483648,-2147483648]";
            var expected = new Point(int.MinValue, int.MinValue);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadNegativeValues_PointRefectsInput()
        {
            const string input = "[-1,-2]";
            var expected = new Point(-1, -2);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Category("Converters")]
        public void PointConverter_ReadNull_ReturnsDefaultForValueType()
        {
            const string input = "null";
            Point expected = default(Point);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadNull_ReturnsNullForNullableType()
        {
            const string input = "null";
            var output = JsonConvert.DeserializeObject<Point?>(input, new PointConverter());

            Assert.IsNull(output);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadPositiveExtremes_PointReflectsInput()
        {
            const string input = "[2147483647,2147483647]";
            var expected = new Point(int.MaxValue, int.MaxValue);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadPositiveValues_PointRefectsInput()
        {
            const string input = "[1,2]";
            var expected = new Point(1, 2);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadSingleNil_ReturnsDefault()
        {
            const string input = "[0]";
            Point expected = default(Point);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_ReadSingleValue_ConverterAssumesValueIsX()
        {
            const string input = "[1]";
            var expected = new Point(1, 0);
            var actual = JsonConvert.DeserializeObject<Point>(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_WriteDefaultPoint_JsonReflectsInput()
        {
            const string expected = "[0,0]";
            Point input = default(Point);
            string actual = JsonConvert.SerializeObject(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_WriteNegativeExtremes_JsonReflectsInput()
        {
            const string expected = "[-2147483648,-2147483648]";
            var input = new Point(int.MinValue, int.MinValue);
            string actual = JsonConvert.SerializeObject(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_WriteNegativeValues_JsonReflectsInput()
        {
            const string expected = "[-1,-2]";
            var input = new Point(-1, -2);
            string actual = JsonConvert.SerializeObject(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Category("Converters")]
        public void PointConverter_WritePositiveExtremes_JsonReflectsInput()
        {
            const string expected = "[2147483647,2147483647]";
            var input = new Point(int.MaxValue, int.MaxValue);
            string actual = JsonConvert.SerializeObject(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Converters")]
        public void PointConverter_WritePositiveValues_JsonReflectsInput()
        {
            const string expected = "[1,2]";
            var input = new Point(1, 2);
            string actual = JsonConvert.SerializeObject(input, new PointConverter());

            Assert.AreEqual(expected, actual);
        }
    }
}