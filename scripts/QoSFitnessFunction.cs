using AForge.Genetic;
using System;
using System.Collections.Generic;

namespace QosOptimizer
{
    public class QosFitnessFunction : IFitnessFunction
    {
        private readonly List<QoSMetric> _providerData;
        private readonly SlaRequirements _sla;

        public QosFitnessFunction(List<QoSMetric> providerData, SlaRequirements sla)
        {
            _providerData = providerData;
            _sla = sla;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var qosChromosome = (QoSSelectionChromosome)chromosome;
            double totalLatency = 0;
            double totalCost = 0;
            double totalAvailability = 1.0;

            foreach (int providerId in qosChromosome.Genes)
            {
                var provider = _providerData[providerId];
                totalLatency += provider.Latency;
                totalCost += provider.Cost;
                totalAvailability *= provider.Availability;
            }

            double penalty = 0;

            if (totalLatency > _sla.MaxLatency)
            {
                penalty += (totalLatency - _sla.MaxLatency) / _sla.MaxLatency;
            }

            if (totalCost > _sla.MaxCost)
            {
                penalty += (totalCost - _sla.MaxCost) / _sla.MaxCost;
            }

            if (totalAvailability < _sla.MinAvailability)
            {
                penalty += (_sla.MinAvailability - totalAvailability) / _sla.MinAvailability;
            }

            double totalPenaltyValue = penalty * _sla.PenaltyFactor;
            double normLatency = (_sla.MaxLatency - totalLatency) / _sla.MaxLatency; 
            double normCost = (_sla.MaxCost - totalCost) / _sla.MaxCost;
            double normAvailability = totalAvailability; 
            
            double utility = (_sla.WeightLatency * normLatency) +
                             (_sla.WeightCost * normCost) +
                             (_sla.WeightAvailability * normAvailability);

            double fitness = utility - totalPenaltyValue;

            return fitness;
        }
    }
}