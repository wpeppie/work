using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using InputFileComparer;
using NSubstitute;
using FileSystemAssembly.Interfaces;
using System.IO;

namespace InputFileCompareTest
{

    [TestClass]
    public class InputComparerTest
    {
        [TestMethod]
        public void CallingProcessWithEmptyArgumentsThrowsError()
        {
            Action action = () => new InputComparer(Substitute.For<IFileSystem>()).Process(null);
            action.ShouldThrow<ArgumentException>().WithMessage(
                "The InputFileComparer must be called with the full name to a configurationfile as the argument.");
        }

        [TestMethod]
        public void CallingProcessWithMoreThanOneArgumentThrowsError()
        {
            Action action = () => new InputComparer(Substitute.For<IFileSystem>()).Process(new [] {"1", "2"});
            action.ShouldThrow<ArgumentException>().WithMessage(
                "The InputFileComparer must be called with the full name to a configurationfile as the argument.");
        }

        [TestMethod]
        public void CallingProcessWithNoneExistingFileThrowsError()
        {
            Action action = () => new InputComparer(FileSystemCreator.GetFileSystem()).Process(new[] { @"FileDoesNotExist"});
            action.ShouldThrow<FileNotFoundException>().WithMessage(
                "The file 'FileDoesNotExist' was not found");
        }

        [TestMethod]
        public void CallingProcessWithExistingProducesCorrectOutput()
        {
            var fileSystem = FileSystemCreator.GetFileSystem();
            new InputComparer(fileSystem).Process(new[] { @"FileDoesExist" });
            var lines = new List<string>();
            lines.Add("#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5");
            lines.Add("ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID7|DATE1|ValueSame|ValueOther|ValueSame|ValueSame|ValueOther");
            lines.Add("ID8|DATE1|ValueOther|ValueOther|ValueOther|ValueOther|ValueOther");
            lines.Add("ID8|DATE2|ValueOther|ValueOther|ValueOther|ValueOther|ValueOther");
            lines.Add("ID8|DATE3|ValueOther|ValueOther|ValueOther|ValueOther|ValueOther");
            var outputLines = lines.ToArray();
            fileSystem.Received().WriteFile(Arg.Any<string>(), Arg.Is<string[]>(output => outputLines.SequenceEqual(output)));
        }

    }
}
