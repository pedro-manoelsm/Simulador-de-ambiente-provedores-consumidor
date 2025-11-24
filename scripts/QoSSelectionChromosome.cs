using AForge.Genetic;
using System;
using System.Linq;

namespace QosOptimizer
{
    public class QoSSelectionChromosome : ChromosomeBase
    {
        public int[] Genes { get; private set; }

        private readonly int _numberOfTasks;
        private readonly int _numberOfProviders;

        public QoSSelectionChromosome(int numberOfTasks, int numberOfProviders)
        {
            _numberOfTasks = numberOfTasks;
            _numberOfProviders = numberOfProviders;
            Genes = new int[numberOfTasks];
            
            Generate();
        }

        private QoSSelectionChromosome(int numberOfTasks, int numberOfProviders, int[] genes)
        {
            _numberOfTasks = numberOfTasks;
            _numberOfProviders = numberOfProviders;
            Genes = genes;
        }

        public override void Generate()
        {
            var rand = new Random();
            for (int i = 0; i < _numberOfTasks; i++)
            {
                Genes[i] = rand.Next(0, _numberOfProviders);
            }
        }

        public override IChromosome CreateNew()
        {
            return new QoSSelectionChromosome(_numberOfTasks, _numberOfProviders);
        }

        public override IChromosome Clone()
        {
            return new QoSSelectionChromosome(_numberOfTasks, _numberOfProviders, (int[])Genes.Clone());
        }

        public override void Mutate()
        {
            var rand = new Random();
            int index = rand.Next(0, _numberOfTasks);
            Genes[index] = rand.Next(0, _numberOfProviders);
        }

        public override void Crossover(IChromosome pair)
        {
            var other = (QoSSelectionChromosome)pair;
            var rand = new Random();

            int crossPoint = rand.Next(0, _numberOfTasks);

            for (int i = crossPoint; i < _numberOfTasks; i++)
            {
                int temp = Genes[i];
                Genes[i] = other.Genes[i];
                other.Genes[i] = temp;
            }
        }
    }
}