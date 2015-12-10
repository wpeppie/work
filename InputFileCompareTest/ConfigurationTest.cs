using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Xml.Linq;
using System.Reflection;
using NSubstitute;
using FileSystemAssembly.Interfaces;
using InputFileComparer;

namespace InputFileCompareTest
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void ConfigurationMustHaveColumnSeparator()
        {
            CheckForEmptyValueMessage("ColumnsSeparator");
        }

        [TestMethod]
        public void ConfigurationMustHaveKeyColumns()
        {
            CheckForEmptyCollectionMessage("KeyColumns");
        }

        [TestMethod]
        public void ConfigurationMustHaveComparisonColumns()
        {
            CheckForEmptyCollectionMessage("ComparisonColumns");
        }

        [TestMethod]
        public void ConfigurationMustHaveFirstFile()
        {
            CheckForEmptyValueMessage("FirstFile");
        }

        [TestMethod]
        public void ConfigurationFirstFileMustExist()
        {
            CheckForNonExistingFile("FirstFile");
        }

        [TestMethod]
        public void ConfigurationMustHaveSecondFile()
        {
            CheckForEmptyValueMessage("SecondFile");
        }

        [TestMethod]
        public void ConfigurationSecondFileMustExist()
        {
            CheckForNonExistingFile("SecondFile");
        }

        [TestMethod]
        public void ConfigurationMustHaveDateColumnName()
        {
            CheckForEmptyValueMessage("DateColumnName");
        }

        [TestMethod]
        public void FirstFileHeaderLineIsUsedIfSecondFileHeaderLineIsNotProvided()
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.SecondFileHeaderLine = null;
            new ConfigurationValidator(FileSystemCreator.GetFileSystem()).ValidateConfiguration(config);
            config.SecondFileHeaderLine.Should().Be(config.FirstFileHeaderLine);
        }

        [TestMethod]
        public void IfSecondFileHeaderLineIsProvidedItIsNotOverwritten()
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.SecondFileHeaderLine = 1;
            new InputFileComparer.ConfigurationValidator(FileSystemCreator.GetFileSystem()).ValidateConfiguration(config);
            config.SecondFileHeaderLine.Should().Be(1);
        }

        [TestMethod]
        public void ConfigurationMustHaveOutputFileName()
        {
            CheckForEmptyValueMessage( "OutputFileName");
        }

        [TestMethod]
        public void OutputFileFolderMustExist()
        {
            CheckForNonExistingFolder("OutputFileName");
        }

        private void CheckForEmptyValueMessage(string propertyName)
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.GetType().GetProperty(propertyName).SetValue(config, null, null);
            ValidateConfiguration(config).ShouldThrow<ArgumentException>()
                .WithMessage("Configuration " + propertyName + " must be specified");
        }

        private void CheckForEmptyCollectionMessage(string propertyName)
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.GetType().GetProperty(propertyName).SetValue(config, null, null);
            ValidateConfiguration(config).ShouldThrow<ArgumentException>()
                .WithMessage("Configuration " + propertyName + " must contain at least 1 string element");
        }

        private void CheckForNonExistingFile(string propertyName)
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.GetType().GetProperty(propertyName).SetValue(config, "FileDoesNotExist", null);
            ValidateConfiguration(config).ShouldThrow<ArgumentException>()
                .WithMessage("Configuration " + propertyName + " does not exist");
        }

        private void CheckForNonExistingFolder(string propertyName)
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.GetType().GetProperty(propertyName).SetValue(config, @"C:\Temp\DoesNotExist\File.txt", null);
            ValidateConfiguration(config).ShouldThrow<ArgumentException>()
                .WithMessage("Configuration " + propertyName + " does not exist");
        }

        private Action ValidateConfiguration(Config config)
        {
            return () => new InputFileComparer.ConfigurationValidator
                    (FileSystemCreator.GetFileSystem()).ValidateConfiguration(config);
        }







    }
}
