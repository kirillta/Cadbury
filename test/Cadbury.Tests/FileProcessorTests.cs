using System.Collections.Generic;
using System.Linq;
using Floxdc.Cadbury;
using Moq;
using Xunit;

namespace Cadbury.Tests
{
    public class FileProcessorTests
    {
        /*[Fact]
        public void ShouldNotOpenScvByPath1()
        {
            var processorMock = new Mock<FileProcessor>();
            processorMock.Setup(p => p.ReadCsvAsync(It.IsAny<string>())).Throws<CsvHelperException>();
            var fileProcessor = processorMock.Object;

            Assert.Throws<CsvHelperException>(() => fileProcessor.ReadCsvAsync(string.Empty));
        }


        [Fact]
        public void ShouldNotOpenScvByPath()
        {
            var fileProcessor = new FileProcessor();
            var result = fileProcessor.ReadCsvAsync(string.Empty);

            Assert.IsType<IEnumerable<TargetUrl>>(result);
        }


        [Fact]
        public void ShouldNotOpenScvByPath2()
        {
            var fileProcessor = new FileProcessor();
            var result = fileProcessor.ReadCsvAsync(@"c:\Projects\Cadbury\images.csv");

            Assert.IsType<IEnumerable<TargetUrl>>(result);
        }*/
    }
}
