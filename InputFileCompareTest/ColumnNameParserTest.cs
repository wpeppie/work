using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputFileComparer;
using FluentAssertions;

namespace InputFileCompareTest
{
    [TestClass]
    public class ColumnNameParserTest
    {
        [TestMethod]
        public void FindingSingleColumnInLineWorksCorrectly()
        {
            ColumnNameParser.GetSingleColumnNumberFromHeaderLine(new[] { "#BBUNIQUE" }, "#bbUnique")
                .Should().Be(0);
        }

        [TestMethod]
        public void FindingSingleColumnInMultipleColumnsWorksCorrectly()
        {
            ColumnNameParser.GetSingleColumnNumberFromHeaderLine(new[] { "#BBNONUNIQUE", "#BBUNIQUE" }, "#bbUnique")
                .Should().Be(1);
        }

        [TestMethod]
        public void FindingCollectionOfSingleColumnInMultipleColumnsWorksCorrectly()
        {
            var columns = ColumnNameParser.GetColumnNumbersFromHeaderLine(
                new[] { "#BBNONUNIQUE", "#BBUNIQUE" }, new[] { "#bbUnique" });
            columns[0].Should().Be(1);
            columns.Length.Should().Be(1);
        }
        [TestMethod]
        public void FindingCollectionOfMultipleColumnsInMultipleColumnsWorksCorrectly()
        {
            var columns = ColumnNameParser.GetColumnNumbersFromHeaderLine(
                new[] { "#BBNONUNIQUE", "#BBUNIQUE" }, new[] { "#bbUnique", "#BBNONUNIQUE" });
            columns[0].Should().Be(1);
            columns[1].Should().Be(0);
            columns.Length.Should().Be(2);
        }

        [TestMethod]
        public void FindingMultipleColumnsWithOneNoneExistingThrowsError()
        {
            Action action = () => ColumnNameParser.GetColumnNumbersFromHeaderLine(
                new[] { "#BBNONUNIQUE", "#BBUNIQUE" }, new[] { "#bbUnique", "NOTFOUND" });
            action.ShouldThrow<InvalidOperationException>().WithMessage(
                "The column NOTFOUND was not found in the columnnames #BBNONUNIQUE - #BBUNIQUE"
                );
        }

        [TestMethod]
        public void FindingSingleNoneExistingColumnThrowsError()
        {
            Action action = () => ColumnNameParser.GetSingleColumnNumberFromHeaderLine(
                new[] { "#BBNONUNIQUE", "#BBUNIQUE" }, "NOTFOUND");
                action.ShouldThrow<InvalidOperationException>().WithMessage(
                "The column NOTFOUND was not found in the columnnames #BBNONUNIQUE - #BBUNIQUE"
                );
        }
    }
}
