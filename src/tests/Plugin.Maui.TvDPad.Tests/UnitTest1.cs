using Xunit;
using Plugin.Maui.TvDPad;

namespace Plugin.Maui.TvDPad.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void DPadKeyEnum_HasValues()
        {
            // Simple test to ensure enum exists
            var values = System.Enum.GetValues(typeof(DPadKey));
            Assert.True(values.Length > 0);
        }
    }
}
