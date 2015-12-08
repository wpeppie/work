using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputFileComparer;
using System.Collections.Generic;
using FluentAssertions;

namespace InputFileCompareTest
{
    [TestClass]
    public class FileLineCollectionTest
    {
        private static FileLineCollection GetFileLineCollection(bool saveOriginalData)
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            var fileSystem = FileSystemCreator.GetFileSystem();
            var fileLineCollection = new FileLineCollection(config, fileSystem, saveOriginalData);
            return fileLineCollection;
        }

        private static FileLineCollection GetFileLineCollection(Config config, bool saveOriginalData)
        {
            var fileSystem = FileSystemCreator.GetFileSystem();
            var fileLineCollection = new FileLineCollection(config, fileSystem, saveOriginalData);
            return fileLineCollection;
        }

        [TestMethod]
        public void AddingOneLineToCollectionWithMissingKeyColumnsThrowsError()
        {
            var fileLineCollection = GetFileLineCollection(false);
            var lines = new List<string>{
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame"
            };
            fileLineCollection.ParseLines(lines, 0);
            fileLineCollection.NumberOfComparableLines.Should().Be(1);

        }

        [TestMethod]
        public void AddingOneLineToCollectionCreatesCorrectCollection()
        {
            var fileLineCollection = GetFileLineCollection(false);
            var lines = new List<string>{
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame"
            };
            fileLineCollection.ParseLines(lines, 0);
            fileLineCollection.NumberOfComparableLines.Should().Be(1);
        }

        [TestMethod]
        public void AddingSameLineToCollectionCreatesEmptyCollection()
        {
            var fileLineCollection = GetFileLineCollection(false);
            var lines = new List<string> { 
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" 
            };
            fileLineCollection.ParseLines(lines, 0);
            fileLineCollection.NumberOfComparableLines.Should().Be(0);
        }

        [TestMethod]
        public void AddingSameLineWithDifferentDateToCollectionCreatesEmptyCollection()
        {
            var fileLineCollection = GetFileLineCollection(false);
            var lines = new List<string> { 
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
                "ID1|DATE2|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" 
            };
            fileLineCollection.ParseLines(lines, 0);
            fileLineCollection.NumberOfComparableLines.Should().Be(0);
        }

        [TestMethod]
        public void AddingSameLineWithDifferentDateAsPartOfTheKeyToCollectionCreatesCorrectCollection()
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.KeyColumns = new[] { "#BBUNIQUE", "DATE" };
            var fileLineCollection = GetFileLineCollection(config, false);
            var lines = new List<string> { 
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
                "ID1|DATE2|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" 
            };
            fileLineCollection.ParseLines(lines, 0);
            fileLineCollection.NumberOfComparableLines.Should().Be(2);
        }

        [TestMethod]
        public void AddingDifferentLinesToCollectionCreatesCorrectCollection()
        {
            var fileLineCollection = GetFileLineCollection(false);
            var lines = new List<string> {
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
                "ID2|DATE2|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" 
            };
            fileLineCollection.ParseLines(lines, 0);
            fileLineCollection.NumberOfComparableLines.Should().Be(2);
        }

        [TestMethod]
        public void FilteringFileLineCollectionWithIdenticalRowsClearsCollection()
        {
            var fileLineCollectionSource = GetFileLineCollection(false);
            var lines = new List<string> { 
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
            };
            fileLineCollectionSource.ParseLines(lines,0);
            var fileLineCollectionResult = GetFileLineCollection(false);
            lines = new List<string> {
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
            };
            fileLineCollectionResult.ParseLines(lines, 0);
            fileLineCollectionResult.FilterComparableLines(fileLineCollectionSource);
            fileLineCollectionResult.NumberOfComparableLines.Should().Be(0);
        }

        [TestMethod]
        public void FilteringFileLineCollectionWithDifferentRowsReturnsCorrectCollection()
        {
            var fileLineCollectionSource = GetFileLineCollection(false);
            var lines = new List<string> {
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame" ,
            };
            fileLineCollectionSource.ParseLines(lines, 0);
            var fileLineCollectionResult = GetFileLineCollection(true);
            lines = new List<string> {
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
            };
            fileLineCollectionResult.ParseLines(lines, 0);
            fileLineCollectionResult.FilterComparableLines(fileLineCollectionSource);
            var outputLines = fileLineCollectionResult.GetOutputLines();
            outputLines[0].Should().Be("#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5");
            outputLines[1].Should().Be("ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame");
        }

        [TestMethod]
        public void FilteringFileLineCollectionWithSameValueInComparisonColumnsClearsCollection()
        {
            var config = ConfigurationCreator.CreateCorrectConfig();
            config.ComparisonColumns = new[] { "COL2" };
            var fileLineCollectionSource = GetFileLineCollection(config, false);
            var lines = new List<string> {
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame" ,
            };
            fileLineCollectionSource.ParseLines(lines, 0);
            var fileLineCollectionResult = GetFileLineCollection(config, false);
            lines = new List<string> {
                "#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5",
                "ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame" ,
            };
            fileLineCollectionResult.ParseLines(lines, 0);
            fileLineCollectionResult.FilterComparableLines(fileLineCollectionSource);
            fileLineCollectionResult.NumberOfComparableLines.Should().Be(0);
        }



    }
}
