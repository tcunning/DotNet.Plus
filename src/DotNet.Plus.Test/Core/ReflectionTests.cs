using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Core.Tests
{
    [TestClass()]
    public class ReflectionTests
    {
        public int TestField;

        public int TestProperty { get; set; }

        public int AddOne(int test) => test + 1;

        [TestMethod()]
        public void GetPropertyTest()
        {
            AddOne(1).ShouldBe(2);
            typeof(ReflectionTests).TryGetMethod(nameof(AddOne), BindingFlags.Instance | BindingFlags.Public, typeof(int)).ShouldNotBeNull();

            this.SetProperty<int>(nameof(TestProperty), 100);
            this.GetProperty<int>(nameof(TestProperty)).ShouldBe(100);
            Should.Throw<ArgumentException>(() => this.SetProperty<int>("MissingProperty", 10));
            Should.Throw<ArgumentException>(() => this.GetProperty<int>("MissingProperty"));

            this.SetField<int>(nameof(TestField), 100);
            this.GetField<int>(nameof(TestField)).ShouldBe(100);
            Should.Throw<ArgumentException>(() => this.SetField<int>("MissingProperty", 10));
            Should.Throw<ArgumentException>(() => this.GetField<int>("MissingProperty"));


        }
    }
}