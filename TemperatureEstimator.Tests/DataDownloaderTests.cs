using FluentAssertions;
using lib12.DependencyInjection;
using System;
using TemperatureEstimator.Logic;
using Xunit;

namespace TemperatureEstimator.Tests
{
    public class DataDownloaderTests
    {
        [Fact]
        void download_and_parse()
        {
            var downloader = Instances.Get<DataDownloader>();
            var result = downloader.DownloadTemperature("EPKK", new DateTime(2014, 3, 12));

            result.Should().NotBeEmpty();
            result[0].Date.Should().Be(new DateTime(2014, 3, 1));
            result[0].Value.Should().Be(12);
        }
    }
}