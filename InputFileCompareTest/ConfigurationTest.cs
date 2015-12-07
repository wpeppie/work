using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Xml.Linq;


namespace InputFileCompareTest
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void ConfigurationMustHaveKeyColumns()
        {
            var config = CreateCorrectConfig();
            config.KeyColumns = null;
            Action action = () => new InputFileComparer.ConfigurationValidator().ValidateConfiguration(config);
            action.ShouldThrow<ArgumentException>().WithMessage("Configuration KeyColumns must contain at least 1 string element");
        }

        [TestMethod]
        public void ConfigurationMustHaveFirstFile()
        {
            var config = CreateCorrectConfig();
            config.FirstFile = null;
            Action action = () => new InputFileComparer.ConfigurationValidator().ValidateConfiguration(config);
            action.ShouldThrow<ArgumentException>().WithMessage("Configuration FirstFile must be specified");
        }

        [TestMethod]
        public void ConfigurationMustHaveSecondFile()
        {
            var config = CreateCorrectConfig();
            config.SecondFile = null;
            Action action = () => new InputFileComparer.ConfigurationValidator().ValidateConfiguration(config);
            action.ShouldThrow<ArgumentException>().WithMessage("Configuration SecondFile must be specified");
        }

        [TestMethod]
        public void ConfigurationMustHaveDateColumName()
        {
            var config = CreateCorrectConfig();
            config.DateColumnName = null;
            Action action = () => new InputFileComparer.ConfigurationValidator().ValidateConfiguration(config);
            action.ShouldThrow<ArgumentException>().WithMessage("Configuration DateColumnName must be specified");
        }
        [TestMethod]
        public void ConfigurationMustHaveFirstFileHeaderLine()
        {
            var config = CreateCorrectConfig();
            config.FirstFileHeaderLine = null;
            Action action = () => new InputFileComparer.ConfigurationValidator().ValidateConfiguration(config);
            action.ShouldThrow<ArgumentException>().WithMessage("Configuration FirstFileHeaderLine must be specified");
        }

        [TestMethod]
        public void FirstFileHeaderLineIsUsedIfSecondFileHeaderLineIsNotProvided()
        {
            var config = CreateCorrectConfig();
            config.SecondFileHeaderLine = null;
            new InputFileComparer.ConfigurationValidator().ValidateConfiguration(config);
            config.SecondFileHeaderLine.Should().Be(config.FirstFileHeaderLine);
        }

        [TestMethod]
        public void ConfigurationMustHaveOutputFileName()
        {
            var config = CreateCorrectConfig();
            config.OutputFileName = null;
            Action action = () => new InputFileComparer.ConfigurationValidator().ValidateConfiguration(config);
            action.ShouldThrow<ArgumentException>().WithMessage("Configuration OutputFileName must be specified");
        }
        
        private InputFileComparer.Config CreateCorrectConfig()
        {
            var config = new InputFileComparer.Config();
            config.KeyColumns = new[] { "BB_UNIQUE" };
            config.FirstFile = @"C:\Temp\File1.txt";
            config.SecondFile = @"C:\Temp\File2.txt";
            config.DateColumnName = "DATE";
            config.OutputFileName = @"C:\Temp\File3.txt";
            config.FirstFileHeaderLine = 1;
            config.SecondFileHeaderLine = 2;
            return config;
        }
    }
}
