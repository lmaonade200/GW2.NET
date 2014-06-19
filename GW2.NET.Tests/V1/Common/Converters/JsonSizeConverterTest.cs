﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSizeConverterTest.cs" company="">
//   
// </copyright>
// <summary>
//   The json size converter test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2DotNET.V1.Common.Converters
{
    using System.Drawing;

    using Newtonsoft.Json;

    using NUnit.Framework;

    /// <summary>The json size converter test.</summary>
    [TestFixture]
    public class JsonSizeConverterTest
    {
        /// <summary>The json size converter_ read both nil_ returns default.</summary>
        [Test]
        [Category("Converters")]
        public void Read_BothNil_ReturnsDefault()
        {
            const string input = "[0,0]";
            Size expected = default(Size);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read empty array_ returns default.</summary>
        [Test]
        [Category("Converters")]
        public void Read_EmptyArray_ReturnsDefault()
        {
            const string input = "[]";
            Size expected = default(Size);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read empty_ exception is thrown for value type.</summary>
        [Test]
        [Category("Converters")]
        [ExpectedException(typeof(JsonSerializationException))]
        public void Read_Empty_ExceptionIsThrownForValueType()
        {
            string input = string.Empty;
            JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());
        }

        /// <summary>The json size converter_ read empty_ returns null for nullable type.</summary>
        [Test]
        [Category("Converters")]
        public void Read_Empty_ReturnsNullForNullableType()
        {
            string input = string.Empty;
            var output = JsonConvert.DeserializeObject<Size?>(input, new JsonSizeConverter());

            Assert.IsNull(output);
        }

        /// <summary>The json size converter_ read more than two values_ converter throws json serialization exception.</summary>
        [Test]
        [Category("Converters")]
        [ExpectedException(typeof(JsonSerializationException))]
        public void Read_MoreThanTwoValues_ConverterThrowsJsonSerializationException()
        {
            const string input = "[1,2,3]";
            JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());
        }

        /// <summary>The json size converter_ read negative extremes_ size reflects input.</summary>
        [Test]
        [Category("Converters")]
        public void Read_NegativeExtremes_SizeReflectsInput()
        {
            const string input = "[-2147483648,-2147483648]";
            var expected = new Size(int.MinValue, int.MinValue);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read negative values_ size refects input.</summary>
        [Test]
        [Category("Converters")]
        public void Read_NegativeValues_SizeRefectsInput()
        {
            const string input = "[-1,-2]";
            var expected = new Size(-1, -2);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read null_ returns default for value type.</summary>
        [Test]
        [Category("Converters")]
        public void Read_Null_ReturnsDefaultForValueType()
        {
            const string input = "null";
            Size expected = default(Size);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read null_ returns null for nullable type.</summary>
        [Test]
        [Category("Converters")]
        public void Read_Null_ReturnsNullForNullableType()
        {
            const string input = "null";
            var output = JsonConvert.DeserializeObject<Size?>(input, new JsonSizeConverter());

            Assert.IsNull(output);
        }

        /// <summary>The json size converter_ read positive extremes_ size reflects input.</summary>
        [Test]
        [Category("Converters")]
        public void Read_PositiveExtremes_SizeReflectsInput()
        {
            const string input = "[2147483647,2147483647]";
            var expected = new Size(int.MaxValue, int.MaxValue);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read positive values_ size refects input.</summary>
        [Test]
        [Category("Converters")]
        public void Read_PositiveValues_SizeRefectsInput()
        {
            const string input = "[1,2]";
            var expected = new Size(1, 2);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read single nil_ returns default.</summary>
        [Test]
        [Category("Converters")]
        public void Read_SingleNil_ReturnsDefault()
        {
            const string input = "[0]";
            Size expected = default(Size);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ read single value_ converter assumes x equals y.</summary>
        [Test]
        [Category("Converters")]
        public void Read_SingleValue_ConverterAssumesXEqualsY()
        {
            const string input = "[1]";
            var expected = new Size(1, 1);
            var actual = JsonConvert.DeserializeObject<Size>(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ write default size_ json reflects input.</summary>
        [Test]
        [Category("Converters")]
        public void Write_DefaultSize_JsonReflectsInput()
        {
            const string expected = "[0,0]";
            Size input = default(Size);
            string actual = JsonConvert.SerializeObject(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ write negative extremes_ json reflects input.</summary>
        [Test]
        [Category("Converters")]
        public void Write_NegativeExtremes_JsonReflectsInput()
        {
            const string expected = "[-2147483648,-2147483648]";
            var input = new Size(int.MinValue, int.MinValue);
            string actual = JsonConvert.SerializeObject(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ write negative values_ json reflects input.</summary>
        [Test]
        [Category("Converters")]
        public void Write_NegativeValues_JsonReflectsInput()
        {
            const string expected = "[-1,-2]";
            var input = new Size(-1, -2);
            string actual = JsonConvert.SerializeObject(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ write positive extremes_ json reflects input.</summary>
        [Test]
        [Category("Converters")]
        public void Write_PositiveExtremes_JsonReflectsInput()
        {
            const string expected = "[2147483647,2147483647]";
            var input = new Size(int.MaxValue, int.MaxValue);
            string actual = JsonConvert.SerializeObject(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>The json size converter_ write positive values_ json reflects input.</summary>
        [Test]
        [Category("Converters")]
        public void Write_PositiveValues_JsonReflectsInput()
        {
            const string expected = "[1,2]";
            var input = new Size(1, 2);
            string actual = JsonConvert.SerializeObject(input, new JsonSizeConverter());

            Assert.AreEqual(expected, actual);
        }
    }
}