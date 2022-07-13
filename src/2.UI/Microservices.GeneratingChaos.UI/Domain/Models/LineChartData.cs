using System;

namespace Microservices.GeneratingChaos.UI.Domain.Models
{
    public class LineChartData
    {
        public DateTime xValue { get; set; }
        public double temperatureValue { get; set; }
        public double humidityValue { get; set; }
        public double pressureValue { get; set; }
    }
}
