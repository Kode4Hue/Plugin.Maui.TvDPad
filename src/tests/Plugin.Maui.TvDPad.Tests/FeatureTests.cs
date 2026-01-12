using System;
using System.Linq;
using System.Reflection;
using Xunit;
using Plugin.Maui.TvDPad;

namespace Plugin.Maui.TvDPad.Tests
{
    public class FeatureTests
    {
        [Fact]
        public void IFeature_Interface_Exists_WithExpectedMembers()
        {
            var iface = typeof(IFeature);
            Assert.NotNull(iface);
            Assert.True(iface.IsInterface, "IFeature should be an interface");

            var members = iface.GetMembers();
            Assert.True(members.Length > 0, "IFeature should expose members");
            Assert.True(members.Any(m => m.Name.IndexOf("Enable", StringComparison.OrdinalIgnoreCase) >= 0) || members.Any(m => m.Name.IndexOf("Start", StringComparison.OrdinalIgnoreCase) >= 0) || members.Any(m => m.Name.IndexOf("IsSupported", StringComparison.OrdinalIgnoreCase) >= 0), "IFeature should have enable/start/isSupported-like member");
        }

        [Fact]
        public void Feature_Has_Static_Instance_Or_Getter()
        {
            // Use compile-time reference to Feature to ensure accurate type
            var featureType = typeof(Feature);
            Assert.NotNull(featureType);

            var instProp = featureType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)
                ?? featureType.GetProperty("Current", BindingFlags.Public | BindingFlags.Static)
                ?? featureType.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);

            var getter = featureType.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static)
                ?? featureType.GetMethod("Create", BindingFlags.Public | BindingFlags.Static)
                ?? featureType.GetMethod("Get", BindingFlags.Public | BindingFlags.Static);

            Assert.True(instProp != null || getter != null, "Feature type should expose a static instance property or a static getter method (Instance|Current|Default or GetInstance|Create|Get)");
        }
    }
}
