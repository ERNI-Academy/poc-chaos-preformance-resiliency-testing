using System;

namespace Microservices.GeneratingChaos.UI.Domain.Models
{
    public class WheatherForecastDto
    {
        public DateTime Date { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public double Pressure { get; set; }

        public string Icon { get; set; }
    }
}
