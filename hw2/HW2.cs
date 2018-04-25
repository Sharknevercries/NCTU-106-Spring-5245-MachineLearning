using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.DataType;

namespace HW2
{
    public class HW2
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
            }
            else
            {
                string part = "";
                foreach (var arg in args)
                {
                    if (arg.StartsWith("--part="))
                    {
                        part = arg.Substring(7);
                    }
                }

                if (part == "mnist")
                {
                    int mode = default;
                    string trainDataPath = default;
                    string trainLabelPath = default;
                    string testDataPath = default;
                    string testLabelPath = default;
                    string output = "result";

                    foreach (var arg in args)
                    {
                        if (arg.StartsWith("--train="))
                        {
                            trainDataPath = arg.Substring(8);
                        }
                        else if (arg.StartsWith("--train_label="))
                        {
                            trainLabelPath = arg.Substring(14);
                        }
                        else if (arg.StartsWith("--test="))
                        {
                            testDataPath = arg.Substring(7);
                        }
                        else if (arg.StartsWith("--test_label="))
                        {
                            testLabelPath = arg.Substring(13);
                        }
                        else if (arg.StartsWith("--output="))
                        {
                            output = arg.Substring(9);
                        }
                        else if (arg.StartsWith("--mode="))
                        {
                            mode = int.Parse(arg.Substring(7));
                        }
                    }

                    FileStream fs = new FileStream(output, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    TextWriter stdout = Console.Out;
                    Console.SetOut(sw);

                    
                    var train = Core.Datasets.MNIST.GetDataset(trainDataPath, trainLabelPath);
                    var test = Core.Datasets.MNIST.GetDataset(testDataPath, testLabelPath);
                    MNISTTrainner trainer = default;

                    if (mode == 0)
                    {
                        trainer = new FrequencyMNISTTrainer(train);
                    }
                    else if (mode == 1)
                    {
                        trainer = new GaussianMNISTTrainer(train);
                    }
                    else
                    {
                        throw new ArgumentException();
                    }

                    int counter = 0;
                    int error = 0;
                    foreach (var testData in test)
                    {
                        Console.Write($"{ ++counter }\t");
                        trainer.Predict(testData, out var posterior);
                        foreach (var v in posterior)
                        {
                            Console.Write($"{ v.ToString("0.####") }\t");
                        }

                        int prediction = posterior.ToList().IndexOf(posterior.Max());
                        Console.Write(prediction);

                        if (prediction != testData.Label)
                            error++;
                        Console.WriteLine();

                        if (counter % 1000 == 1 && counter != 1)
                        {
                            Console.SetOut(stdout);
                            Console.WriteLine(counter - 1);
                            Console.SetOut(sw);
                        }                        
                    }

                    Console.WriteLine($"Error = { (double)error / counter }");

                    sw.Close();
                }
                else if (part == "beta")
                {
                    int alpha = default;
                    int beta = default;
                    string inputPath = default;

                    foreach (var arg in args)
                    {
                        if (arg.StartsWith("--alpha="))
                        {
                            alpha = int.Parse(arg.Substring(8));
                        }
                        else if (arg.StartsWith("--beta="))
                        {
                            beta = int.Parse(arg.Substring(7));
                        }
                        else if (arg.StartsWith("--input="))
                        {
                            inputPath = arg.Substring(8);
                        }
                    }

                    var data = GetTossDataset(inputPath);
                    var dist = new BetaDistribution(alpha, beta);

                    int counter = 1;
                    foreach (var line in data)
                    {
                        Console.WriteLine($"{ counter } line [Before]");
                        dist.Summary();

                        int success = 0, fail = 0;
                        foreach (var ch in line)
                        {
                            switch (ch)
                            {
                                case '0':
                                    fail++;
                                    break;
                                case '1':
                                    success++;
                                    break;
                                default:
                                    // Ignore
                                    break;
                            }
                        }

                        dist = new BetaDistribution(dist.Alpha + success, dist.Beta + fail);
                        Console.WriteLine($"{ counter } line [After]");
                        dist.Summary();

                        Console.WriteLine("====================================");
                        counter++;
                    }

                }
                else
                {
                    PrintHelp();
                }

            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage: hw2 [arguments]");
            Console.WriteLine("arguments:");
            Console.WriteLine("\t--mode=MODE              \tmnist or beta distribution");
            Console.WriteLine("\t--train=FILENAME         \tMNIST trainning data");
            Console.WriteLine("\t--train_label=FILENAME   \tMNIST trainning data label");
            Console.WriteLine("\t--test=FILENAME          \tMNIST test data");
            Console.WriteLine("\t--test_label=FILENAME    \tMNIST test data label");
        }

        public static IEnumerable<string> GetTossDataset(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
