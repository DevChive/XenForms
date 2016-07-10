﻿using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;
using XenForms.Core.Platform.Reflection;
using XenForms.Core.Reflection;
using XenForms.Core.Widgets;

namespace XenForms.Designer.Tests.Reflection
{
    [TestFixture]
    public class TestXenConstructorMethods
    {
        [Test]
        public void Should_find_structure_constructors()
        {
            var ctors = XenConstructorMethods.GetConstructors(typeof (GridLength));

            var c1 = ctors[0];
            var c2 = ctors[1];

            Assert.AreEqual(2, ctors.Length);
            Assert.AreEqual(typeof(GridLength).AssemblyQualifiedName, c1.TypeName);

            Assert.AreEqual("value", c2.Parameters[0].ParameterName);
            Assert.AreEqual("type", c2.Parameters[1].ParameterName);
            Assert.AreEqual(typeof(GridUnitType).FullName, c2.Parameters[1].TypeName);
        }


        [Test]
        public void Construct_GridLength_single_numeric_parameter_ctor()
        {
            var tf = new TypeFinder();

            var ctor = new XenConstructor
            {
                TypeName = typeof (GridLength).AssemblyQualifiedName,
                Parameters = new[]
                {
                    new XenParameter
                    {
                        TypeName = "System.Double",
                        ParameterName = "value",
                        Position = 0,
                        Value = 5
                    }
                }
            };

            var obj = XenConstructorMethods.Construct(tf, ctor);
            var casted = (GridLength) obj;

            Assert.AreEqual(5, casted.Value);
            Assert.AreEqual(GridUnitType.Absolute, casted.GridUnitType);
        }


        [Test]
        public void Construct_GridLength_two_parameter_ctors_one_isenum()
        {
            var tf = new TypeFinder();

            var ctor = new XenConstructor
            {
                TypeName = typeof(GridLength).AssemblyQualifiedName,
                Parameters = new[]
                {
                    new XenParameter
                    {
                        TypeName = "System.Double",
                        ParameterName = "value",
                        Position = 0,
                        Value = 7
                    },
                    new XenParameter
                    {
                        TypeName = typeof(GridUnitType).FullName,
                        ParameterName = "type",
                        Position = 1,
                        Value = GridUnitType.Star
                    }
                }
            };

            var obj = XenConstructorMethods.Construct(tf, ctor);
            var casted = (GridLength)obj;

            Assert.AreEqual(7, casted.Value);
            Assert.AreEqual(GridUnitType.Star, casted.GridUnitType);
        }


        [Test]
        public void No_typename_should_return_null_displayname()
        {
            var ctor = new XenConstructor();
            Assert.IsNull(ctor.DisplayName);
        }


        [Test]
        public void Empty_params_should_return_default_ctor()
        {
            var ctor = new XenConstructor
            {
                TypeName = typeof(Rectangle).FullName
            };

            Assert.AreEqual("Rectangle()", ctor.DisplayName);
        }


        [Test]
        public void Rectangle_constructors()
        {
            var ctors = XenConstructorMethods.GetConstructors(typeof(Rectangle));
            var c1 = ctors.Any(p => p.DisplayName == "Rectangle(Double x, Double y, Double width, Double height)");
            var c2 = ctors.Any(p => p.DisplayName == "Rectangle(Point loc, Size sz)");

            Assert.IsTrue(c1, "c1");
            Assert.IsTrue(c2, "c2");
        }
    }
}
