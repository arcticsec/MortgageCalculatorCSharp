using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageCalculatorMain
{
    public class Customer
    {   
        // Customer name
        public string Name { get; set; }

        // Total loan amount
        public double TotalLoan { get; set; }

        // Annual interest rate
        public double InterestRate { get; set; }

        // Period of the loan in years
        public int TotalYears { get; set; }

        // Constant used in the monthly payment calculation
        private const int MONTHS_IN_A_YEAR = 12;

        /// <summary>
        /// Calculation to find out the monthly payment for the loan
        /// </summary>
        /// <returns>Monthly payment</returns>
        public double CalculateMonthlyPayment()
        {
            double monthlyInterestRate = InterestRate / MONTHS_IN_A_YEAR / 100;
            int numberOfPayments = TotalYears * 12;
            double pow = 1;
            for (int i = 0; i < numberOfPayments; i++)
            {
                pow *= (1 + monthlyInterestRate);
            }
            double monthlyPayment = TotalLoan * monthlyInterestRate / (1 - (1 / pow));
            return monthlyPayment;
        }
    }
    class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// The program that handles the mortgage plan and validates the input from a file
    /// </summary>
    class Program
    {  
        /// <summary>
        /// The main method that handles calculation and reading the input from file
        /// </summary>
        static void Main(string[] args)
        {
            string inputFile = "../../prospects.txt";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            try
            {
                using(StreamReader reader = new StreamReader(inputFile))
                {
                    int lineNumber = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lineNumber++;

                        if (lineNumber == 1) continue;
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        

                        Customer customer = ValidateCustomer(line);
                        double monthlyPayment = customer.CalculateMonthlyPayment();

                        // Formatting the output string we see in the console
                        string result = string.Format("Prospect {0}: {1} wants to borrow {2:#.00} € for a period of {3} years and pay {4:#.00} € each month",
                                lineNumber - 1,
                                customer.Name,
                                customer.TotalLoan,
                                customer.TotalYears,
                                monthlyPayment);
                        Console.WriteLine(result);
                        
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + e);
                Console.ReadLine();
                return;
            }
            catch (InvalidInputException)
            {
                Console.WriteLine("Invalid input");
                Console.ReadLine();
                return;
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Method that handles the validation of the customer data from input file
        /// </summary>
        /// <param name="line">One line of data from the input file</param>
        /// <returns></returns>
        static Customer ValidateCustomer(string line)
        {
            string[] values = line.Split(',');

            // Check that the line has enough parameters
            if (values.Length < 4)
            {
                throw new InvalidInputException("Format invalid: " + line);
            }
            string customer = values[0].Trim();
            // Check if the name has a comma in it so we can remove it
            int offset = 0;
            if (values.Length > 4)
            {
                offset = 1;
                customer = values[0] + " " + values[1];
            }
            else
            {
                customer = values[0];
            }
            customer = customer.Replace("\"", "");
            

            // Check that the input is valid data for our calculation
            if (double.TryParse(values[offset + 1].Replace('.', ','), out double totalLoan) &&
                double.TryParse(values[offset + 2].Replace('.', ','), out double interestRate) &&
                int.TryParse(values[offset + 3], out int totalYears))
            {
                Customer Prospect = new Customer
                {
                    Name = customer,
                    TotalLoan = totalLoan,
                    InterestRate = interestRate,
                    TotalYears = totalYears
                };
                return Prospect;
            }
            else
            {
                throw new Exception("Format invalid: " + line);
            }
            
        }
    }
}
