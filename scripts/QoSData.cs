using System;
using System.Collections.Generic;

namespace QosOptimizer
{
    public class QoSMetric
    {
        public int ProviderId { get; set; }
        public double Latency { get; set; }       
        public double Cost { get; set; }          
        public double Availability { get; set; }  
    }

    public class SlaRequirements
    {
        public double MaxLatency { get; set; }
        public double MaxCost { get; set; }
        public double MinAvailability { get; set; }
        public double WeightLatency { get; set; }
        public double WeightCost { get; set; }
        public double WeightAvailability { get; set; }
        public double PenaltyFactor { get; set; }
    }

    public static class QoSDataLoader
    {
        public static List <QoSMetric> LoadSimulatedData(int NumProviders)
        {
            var data = new List <QoSMetric>();
            var random = new Random();

            for(int i = 0; i < NumProviders; i++)
            {
                data.Add(new QoSMetric
                {
                    ProviderId = i,
                    Latency = 40.0 + random.NextDouble() * 60.0,
                    Cost = 0.01 + random.NextDouble() * 0.04,
                    Availability = 0.95 + random.NextDouble() * 0.0499
                });
            }
            return data;
        }
    }
}