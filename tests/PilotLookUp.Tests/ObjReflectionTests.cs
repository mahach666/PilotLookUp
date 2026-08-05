using PilotLookUp.Objects.TypeHelpers;
using PilotTestHost.Sdk;
using System;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Безопасность рефлексии: снуп не должен мутировать объект
    public class ObjReflectionTests
    {
        private sealed class ReflectionProbe : IDisposable
        {
            public bool DisposeCalled;
            public bool SideEffectCalled;

            public int Number => 42;
            public string Throwing => throw new InvalidOperationException("boom");

            public string GetGreeting() => "hello";
            public void SideEffect() => SideEffectCalled = true;
            public void Dispose() => DisposeCalled = true;
        }

        [PilotFact]
        public void Reflection_Reads_Values_Without_Side_Effects()
        {
            var probe = new ReflectionProbe();
            var pairs = new OtherHelper(probe).Reflection.KeyValuePairs;

            Check.That(!probe.DisposeCalled, "Dispose НЕ должен вызываться при снупе (deny-list)");
            Check.That(!probe.SideEffectCalled, "void-методы НЕ должны вызываться при снупе");

            var number = pairs.First(p => p.Key.Name == "Number").Value;
            Check.Equal(42, (int)number, "значение свойства");

            var greeting = pairs.First(p => p.Key.Name == "GetGreeting").Value;
            Check.Equal("hello", (string)greeting, "результат метода без параметров");
        }

        [PilotFact]
        public void Throwing_Property_Becomes_Error_Row_Not_Crash()
        {
            var pairs = new OtherHelper(new ReflectionProbe()).Reflection.KeyValuePairs;

            var throwing = pairs.First(p => p.Key.Name == "Throwing").Value;
            Check.Contains(throwing?.ToString(), "Error:", "бросающее свойство дает строку с ошибкой");
            Check.Contains(throwing?.ToString(), "boom", "текст исходного исключения");
        }

        [PilotFact]
        public void Void_Methods_And_Dispose_Are_Not_Listed()
        {
            var pairs = new OtherHelper(new ReflectionProbe()).Reflection.KeyValuePairs;
            var names = pairs.Keys.Select(k => k.Name).ToList();

            Check.That(!names.Contains("SideEffect"), "void-метод не попадает в грид");
            Check.That(!names.Contains("Dispose"), "Dispose не попадает в грид");
        }
    }
}
