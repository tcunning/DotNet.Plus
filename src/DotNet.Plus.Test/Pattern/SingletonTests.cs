using System;
using System.Threading.Tasks;
using DotNet.Plus.Pattern;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Pattern
{
    [TestClass]
    public class SingletonTests
    {
        class ProperSingleton : Singleton<ProperSingleton>
        {
            private ProperSingleton() { }
        }

        class FailedConstructorSingleton : Singleton<FailedConstructorSingleton>
        {
            private FailedConstructorSingleton()
            {
                throw new Exception($"Testing failure in private constructor.");
            }
        }

        class NoPrivateConstructorSingleton : Singleton<NoPrivateConstructorSingleton>
        {
            public NoPrivateConstructorSingleton() { }
        }

        class NoPrivateDefaultConstructorSingleton : Singleton<NoPrivateDefaultConstructorSingleton>
        {
            private NoPrivateDefaultConstructorSingleton(int test) { }
        }

        class HasPublicConstructorSingleton : Singleton<HasPublicConstructorSingleton>
        {
            private HasPublicConstructorSingleton() { }

            public HasPublicConstructorSingleton(int value) : this() { }
        }
        
        class TaskRaceSingleton : Singleton<TaskRaceSingleton>
        {
            private TaskRaceSingleton() { }
        }

        [TestMethod]
        public async Task HasPublicConstructorTest()
        {
            var properSingleton = ProperSingleton.Instance;
            properSingleton.ShouldNotBeNull();
            properSingleton.ShouldBeOfType<ProperSingleton>();
            properSingleton.ShouldBeSameAs(ProperSingleton.Instance);
             
            Should.Throw<ConstructorException>(() => FailedConstructorSingleton.Instance);
            Should.Throw<ConstructorException>(() => NoPrivateConstructorSingleton.Instance);
            Should.Throw<ConstructorException>(() => NoPrivateDefaultConstructorSingleton.Instance);
            Should.Throw<ConstructorException>(() => HasPublicConstructorSingleton.Instance);

            (new NoPrivateConstructorSingleton()).ShouldNotBeNull();
            (new HasPublicConstructorSingleton(5)).ShouldNotBeNull();
            
            // Try to get two instances at the same time.
            TaskRaceSingleton taskRace1 = null;
            TaskRaceSingleton taskRace2 = null;

            var task1 = Task.Run(() => taskRace1 = TaskRaceSingleton.Instance);
            var task2 = Task.Run(() => taskRace2 = TaskRaceSingleton.Instance);
            await Task.WhenAll(task1, task2);

            taskRace1.ShouldNotBeNull();
            taskRace2.ShouldNotBeNull();
            taskRace1.ShouldBeSameAs(taskRace2);
        }
    }
}
