using System;
using AForge.Genetic;
using System.Collections.Generic;

namespace QosOptimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Iniciando Otimização de QoS (AForge.Genetic) ===");
            int numberOfTasks = 10;       // número de passos do workflow
            int numberOfProviders = 100;  // número de provedores disponíveis
            int populationSize = 50;      // tamanho da população
            int generations = 100;        // número de gerações

            Console.WriteLine("Gerando dataset de provedores...");
            var providerData = QoSDataLoader.LoadSimulatedData(numberOfProviders);
            var sla = new SlaRequirements
            {
                MaxLatency = 800.0,       
                MaxCost = 0.40,           
                MinAvailability = 0.75,   

                // peso de acordo com as preferências do consumidor
                WeightLatency = 0.4,      
                WeightCost = 0.4,         
                WeightAvailability = 0.2, 
                PenaltyFactor = 100.0
            };
            
            var fitnessFunction = new QosFitnessFunction(providerData, sla);
            var ancestor = new QoSSelectionChromosome(numberOfTasks, numberOfProviders);
            var selectionMethod = new EliteSelection();
            var population = new Population(populationSize, ancestor, fitnessFunction, selectionMethod);

            Console.WriteLine("Iniciando evolução...");

            for (int i = 0; i < generations; i++)
            {
                population.RunEpoch(); 
                if (i % 10 == 0 || i == generations - 1)
                {
                    Console.WriteLine($"Geração {i}: Melhor Fitness = {population.BestChromosome.Fitness:F4}");
                }
            }

            
            var bestSolution = (QoSSelectionChromosome)population.BestChromosome;
            
            Console.WriteLine("\n=== Melhor Solução Encontrada ===");
            Console.WriteLine($"Fitness Final: {bestSolution.Fitness:F4}");
            Console.Write("Provedores Escolhidos (Genes): ");
            
            foreach (var gene in bestSolution.Genes)
            {
                Console.Write($"{gene} ");
            }
            Console.WriteLine("\n\nSimulação Concluída.");
            Console.WriteLine("\n=== Comparação com Seleção Aleatória ===");
            var randomSolution = new QoSSelectionChromosome(numberOfTasks, numberOfProviders);
            double randomFitness = fitnessFunction.Evaluate(randomSolution);

            Console.WriteLine($"Fitness do AG (Otimizado): {bestSolution.Fitness:F4}");
            Console.WriteLine($"Fitness Aleatório (Sem IA): {randomFitness:F4}");

            if (bestSolution.Fitness > randomFitness)
            {
                Console.WriteLine("CONCLUSÃO: O Algoritmo Genético superou a escolha aleatória!");
            }
        }
    }
}