using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DotNet.Essentials.BasicType.Tests;
using DotNet.Plus.BasicType;
using Shouldly;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class EnumTests
    {
        [DefaultValue(Color.Red)]
        enum Color : int
        {
            Unknown = 0,
            Red = 1,
            Green = 2,
            Blue = 3
        }

        [DefaultValue(2)]
        enum CarBrand
        {
            Unknown = 0,
            Tesla = 1,
            Ford = 2,
            GeneralMotors = 3
        }

        [DefaultValue("ModelS")]
        enum CarMake
        {
            Unknown = 0,
            ModelS = 1,
            ModelX = 2,
            Model3 = 3,
        }

        [DefaultValue(5)]  // 5 not in the enum!
        enum CarClass
        {
            Unknown = 0,
            Compact = 1,
            Truck = 2,
            Suv = 3,
            Van = 4
        }

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
        public class CustomAttribute : Attribute
        {
        }

        enum NoDefault
        {
            [System.ComponentModel.Description("Letter A")]
            A,
            
            [System.ComponentModel.Description("Letter B")]
            [Custom]
            B,

            [System.ComponentModel.Description("Letter C")]
            [Custom]
            [Custom]
            C,

            D
        }

        struct NotAnEnum
        {
        }

        [TestMethod]
        public void EnumDefaultTest()
        {
            //var test = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //Trace.Listeners.RemoveAt(0);
            //System.Diagnostics.Trace.Listeners.Clear();

            default(Color).ShouldBe(Color.Unknown);
            Enum<Color>.DefaultValue.ShouldBe(Color.Red);
            Enum<CarBrand>.DefaultValue.ShouldBe(CarBrand.Ford);
            Enum<CarMake>.DefaultValue.ShouldBe(CarMake.ModelS);

            using(Trace.Listeners.AssertSuspend()) {
                Enum<CarClass>.DefaultValue.ShouldBe(CarClass.Unknown);  
            }

            Enum<CarClass>.DefaultValue.ShouldBe(default(CarClass));
            Enum<CarClass>.TryConvert(new NotAnEnum(), out var result).ShouldBe(false);
            result.ShouldBe(default(CarClass));

            Enum<NoDefault>.DefaultValue.ShouldBe(default(NoDefault));
        }

        [TestMethod]
        public void EnumConvertTest()
        {
            Enum<Color>.TryConvert(Color.Red).ShouldBe(Color.Red);
            Enum<Color>.TryConvert(2).ShouldBe(Color.Green);
            Enum<Color>.TryConvert(2ul).ShouldBe(Color.Green);
            Enum<Color>.TryConvert(2f).ShouldBe(Color.Green);
            Enum<Color>.TryConvert("Red").ShouldBe(Color.Red);

            Enum<Color>.TryConvert("GREEN").ShouldBe(Enum<Color>.DefaultValue);
            Enum<Color>.TryConvert(100).ShouldBe(Enum<Color>.DefaultValue);

            Enum<Color>.TryConvert(new object()).ShouldBe(Enum<Color>.DefaultValue);

            Color.Green.ToValue<int>().ShouldBe(2);
            Color.Green.ToValue<float>().ShouldBe(2f);
            Color.Green.ToValue<string>().ShouldBe("Green");

            Should.Throw<InvalidOperationException>(() => Enum<Color>.Convert(new object(), out var color, enableDefaultValue: false));  // System.InvalidOperationException: Unknown enum type
            Should.Throw<ConvertObjectToEnumException>(() => Enum<Color>.Convert(CarClass.Van, out var color, enableDefaultValue: false));

            Enum<Color>.Convert(CarClass.Van, out var color, enableDefaultValue: true).ShouldBe(false);
            color.ShouldBe(Enum<Color>.DefaultValue);
        }

        [TestMethod]
        public void EnumAttributesTest()
        {
            NoDefault.A.Description().ShouldBe("Letter A");
            NoDefault.B.Description().ShouldBe("Letter B");
            NoDefault.C.Description().ShouldBe("Letter C");
            NoDefault.D.Description().ShouldBe("D");
            
            NoDefault.D.TryGetAttributes<DescriptionAttribute>().Any().ShouldBe(false);
        }
    }
}