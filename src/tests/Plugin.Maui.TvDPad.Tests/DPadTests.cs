using System;
using System.Linq;
using Xunit;
using Plugin.Maui.TvDPad;

namespace Plugin.Maui.TvDPad.Tests
{
    public class DPadTests
    {
        [Fact]
        public void DPadKey_Enum_HasCommonValues()
        {
            var names = Enum.GetNames(typeof(DPadKey));
            Assert.NotEmpty(names);
            Assert.True(names.Any(n => string.Equals(n, "Up", StringComparison.OrdinalIgnoreCase)), "Enum should contain 'Up'");
            Assert.True(names.Any(n => string.Equals(n, "Down", StringComparison.OrdinalIgnoreCase)), "Enum should contain 'Down'");
            Assert.True(names.Any(n => string.Equals(n, "Left", StringComparison.OrdinalIgnoreCase)), "Enum should contain 'Left'");
            Assert.True(names.Any(n => string.Equals(n, "Right", StringComparison.OrdinalIgnoreCase)), "Enum should contain 'Right'");
        }

        [Fact]
        public void DPadKeyEventArgs_Has_Key_Property()
        {
            var type = typeof(DPadKeyEventArgs);
            Assert.NotNull(type);

            var prop = type.GetProperties().FirstOrDefault(p => p.Name.IndexOf("Key", StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.NotNull(prop);

            var keyType = typeof(DPadKey);
            Assert.True(prop.PropertyType == keyType || Nullable.GetUnderlyingType(prop.PropertyType) == keyType, "Key property should be of DPadKey enum type");
        }

        [Fact]
        public void FocusNavigationEventArgs_Has_Relevant_Properties()
        {
            var type = typeof(FocusNavigationEventArgs);
            Assert.NotNull(type);

            var props = type.GetProperties().Select(p => p.Name.ToLowerInvariant()).ToArray();
            Assert.True(props.Any(p => p.Contains("focus") || p.Contains("element") || p.Contains("direction") || p.Contains("focused")), "FocusNavigationEventArgs should expose focus-related property");
        }
    }
}
