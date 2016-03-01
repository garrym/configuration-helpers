using System.Collections.Specialized;
using System.Configuration;
using Moq;
using NUnit.Framework;

namespace BaseFour.ConfigurationHelpers.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        private Mock<IConfigurationManagerWrapper> GetMockWrapperWithAppSettings(NameValueCollection appSettings)
        {
            var mockConfigurationManagerWrapper = new Mock<IConfigurationManagerWrapper>();
            mockConfigurationManagerWrapper.SetupGet(x => x.AppSettings).Returns(appSettings);
            return mockConfigurationManagerWrapper;
        }

        private Mock<IConfigurationManagerWrapper> GetMockWrapperWithConnectionStrings(ConnectionStringSettingsCollection connectionStrings)
        {
            var mockConfigurationManagerWrapper = new Mock<IConfigurationManagerWrapper>();
            mockConfigurationManagerWrapper.SetupGet(x => x.ConnectionStrings).Returns(connectionStrings);
            return mockConfigurationManagerWrapper;
        }

        [Test]
        public void Ensure_GetAppSettingOrDefaultAsT_Returns_Value_If_Present()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "1" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingOrDefaultAs("Test", 0);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Ensure_GetAppSettingOrDefaultAsT_Returns_DefaultT_If_Missing()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection());
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingOrDefaultAs("Test", 5);

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Ensure_GetAppSettingOrDefaultAsT_Returns_DefaultT_If_Empty(string value)
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", value } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingOrDefaultAs("Test", 5);

            // Assert
            Assert.AreEqual(5, result);
        }

        [Test]
        public void Ensure_GetAppSettingOrDefaultAsT_Returns_DefaultT_If_Present_But_Fails_Conversion()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "abc" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingOrDefaultAs("Test", 5);

            // Assert
            Assert.AreEqual(5, result);
        }

        [Test]
        public void Ensure_GetAppSettingAsT_Returns_Value_If_Present_And_Required_Is_True()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "5" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingAs<int>("Test", true);

            // Assert
            Assert.AreEqual(5, result);
        }

        [Test]
        public void Ensure_GetAppSettingAsT_Returns_Value_If_Present_And_Required_Is_False()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "6" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingAs<int>("Test", false);

            // Assert
            Assert.AreEqual(6, result);
        }

        [Test]
        public void Ensure_GetAppSettingAsT_Returns_DefaultT_If_Missing_And_Required_Is_False()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection());
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingAs<int>("Test", false);

            // Assert
            Assert.AreEqual(default(int), result);
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Ensure_GetAppSettingAsT_Returns_DefaultT_If_Empty_And_Required_Is_False(string value)
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", value } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingAs<int>("Test", false);

            // Assert
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void Ensure_GetAppSettingAsT_Throws_Exception_If_Missing_And_Required_Is_True()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection());
            var configuration = new Configuration(mockWrapper.Object);

            // Act + Assert
            Assert.Throws<ConfigurationErrorsException>(() => configuration.GetAppSettingAs<int>("Test", true));
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Ensure_GetAppSettingAsT_Throws_Exception_If_Empty_And_Required_Is_True(string value)
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection() { { "Test", value } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act + Assert
            Assert.Throws<ConfigurationErrorsException>(() => configuration.GetAppSettingAs<int>("Test", true));
        }

        [Test]
        public void Ensure_GetAppSettingAsT_Throws_Exception_If_Present_But_Fails_Conversion_When_Required_Is_True()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "abc" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act + Assert
            Assert.Throws<ConfigurationErrorsException>(() => configuration.GetAppSettingAs<int>("Test", true));
        }

        [Test]
        public void Ensure_GetAppSettingAsT_Returns_DefaultT_If_Present_But_Fails_Conversion_When_Required_Is_False()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "abc" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingAs<int>("Test", false);

            // Assert
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void Ensure_GetAppSetting_Returns_Value_If_Present_And_Required_Is_True()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "abc" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSetting("Test", true);

            // Assert
            Assert.AreEqual("abc", result);
        }

        [Test]
        public void Ensure_GetAppSetting_Returns_Value_If_Present_And_Required_Is_False()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "abc" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSetting("Test", false);

            // Assert
            Assert.AreEqual("abc", result);
        }

        [Test]
        public void Ensure_GetAppSetting_Throws_Exception_If_Missing_And_Required_Is_True()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection());
            var configuration = new Configuration(mockWrapper.Object);

            // Act + Assert
            Assert.Throws<ConfigurationErrorsException>(() => configuration.GetAppSetting("Test", true));
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Ensure_GetAppSetting_Throws_Exception_If_Empty_And_Required_Is_True(string value)
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", value } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act + Assert
            Assert.Throws<ConfigurationErrorsException>(() => configuration.GetAppSetting("Test", true));
        }

        [Test]
        public void Ensure_GetAppSetting_Returns_Empty_String_If_Missing_And_Required_Is_False()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection());
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSetting("Test", false);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Ensure_GetAppSetting_Returns_Empty_String_If_Empty_And_Required_Is_False(string value)
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", value } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSetting("Test", false);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Ensure_GetAppSettingOrDefault_Returns_Value_If_Present()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", "abc" } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingOrDefault("Test", "def");

            // Assert
            Assert.AreEqual("abc", result);
        }


        [Test]
        public void Ensure_GetAppSettingOrDefault_Returns_Default_Value_If_Missing()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection());
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingOrDefault("Test", "abc");

            // Assert
            Assert.AreEqual("abc", result);
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Ensure_GetAppSettingOrDefault_Returns_Default_Value_If_Empty(string value)
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithAppSettings(new NameValueCollection { { "Test", value } });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetAppSettingOrDefault("Test", "abc");

            // Assert
            Assert.AreEqual("abc", result);
        }

        [Test]
        public void Ensure_GetConnectionString_Throws_Exception_If_Missing()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithConnectionStrings(new ConnectionStringSettingsCollection());
            var configuration = new Configuration(mockWrapper.Object);

            // Act + Assert
            Assert.Throws<ConfigurationErrorsException>(() => configuration.GetConnectionString("ConnectionString1"));
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Ensure_GetConnectionString_Throws_Exception_If_Empty(string value)
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithConnectionStrings(new ConnectionStringSettingsCollection { new ConnectionStringSettings("ConnectionString1", value) });
            var configuration = new Configuration(mockWrapper.Object);

            // Act + Assert
            Assert.Throws<ConfigurationErrorsException>(() => configuration.GetConnectionString("ConnectionString1"));
        }

        [Test]
        public void Ensure_GetConnectionString_Returns_ConnectionString_If_Present()
        {
            // Arrange
            var mockWrapper = GetMockWrapperWithConnectionStrings(new ConnectionStringSettingsCollection { new ConnectionStringSettings("ConnectionString1", "TestConnectionString", "TestProviderName") });
            var configuration = new Configuration(mockWrapper.Object);

            // Act
            var result = configuration.GetConnectionString("ConnectionString1");

            // Assert
            Assert.AreEqual("ConnectionString1", result.Name);
            Assert.AreEqual("TestConnectionString", result.ConnectionString);
            Assert.AreEqual("TestProviderName", result.ProviderName);
        }
    }
}
