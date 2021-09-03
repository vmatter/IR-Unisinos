using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SearchStringHandler
{
    /*
     * Class responsible for creating a menu executing the SearchStringUtils functions
    */
    public class Program
    {
        public static void Main(string[] args)
        {
            //* Variables that are used in the menu and in the ExecuteProgram function.
            string searchStringInput = "and";
            int countQuery = 1;
            string option = "";
            string fileName = "";
            string fileDirectory = "";
            string fileDirectoryValidation = "";
            int choosenOption = 0;
            string choosenFile = "";

            string fileExists = "";         //! Will be used in the next version of the code.
            int countTentatives = 0;        //! Will be used in the next version of the code.
            int maxTries = 3;               //! Will be used in the next version of the code.
            string chooseTxtFile = "";      //! Will be used in the next version of the code.


            //-------------------------------- SHOW MENU ------------------------------------

            /* do
            {
                ShowMenu();

                while (option != "1" && option != "5")
                {
                    Console.Write("ENTER A VALID OPTION: ");
                    option = Console.ReadLine();
                }

                if (option == "1")
                {
                    do
                    {
                        Console.Write("\n♦ Input a search string: ");
                        searchStringInput = Console.ReadLine();

                        if (searchStringInput == "")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\n♦ Write at least one word!!!\n");
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                    } while (searchStringInput == "");

                    do
                    {
                        Console.Write("\n♦ Input a directory name inside CurrentDirectory() that contains PDF files (or use \"pdfs\" as default): ");
                        Console.Write("\n♦ The current path is: ");
                        Console.Write(Directory.GetCurrentDirectory() + @"\");
                        fileDirectory = Console.ReadLine();
                        fileDirectoryValidation = Directory.GetCurrentDirectory() + @"\" + fileDirectory;
                        if (Directory.Exists(fileDirectoryValidation) is false)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\n♦ Directory doesn't exists, write a valid directory!!\n");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    } while (Directory.Exists(fileDirectoryValidation) is false);

                    do
                    {
                        Console.WriteLine("Input the name of your file: ");
                        choosenFile = Console.ReadLine();
                        if (File.Exists(choosenFile) is false)
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\n♦ This directory doesn't contain this file, please choose a valid option below\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            string[] arquivos = Directory.GetFiles(fileDirectoryValidation);
                            int cont = 0;
                            Console.WriteLine("Arquivos: ");
                            foreach (string arq in arquivos)
                            {
                                Console.Write("Opção: " + cont + ": ");
                                int fileNameIndex = arq.LastIndexOf(@"\") + 1;
                                string fileNames = arq.Substring(fileNameIndex, (arq.Length - fileNameIndex));
                                Console.WriteLine(fileNames);
                                cont++;
                            }

                            Console.WriteLine("Digite o número da opção desejada: ");
                            choosenOption = Convert.ToInt16(Console.ReadLine());
                            if (choosenOption >= 0 && choosenOption < arquivos.Length)
                            {
                                int fileNameIndex = arquivos[choosenOption].LastIndexOf(@"\") + 1;
                                choosenFile = arquivos[choosenOption].Substring(fileNameIndex, (arquivos[choosenOption].Length - fileNameIndex));
                                Console.WriteLine("Arquivo selecionardo: " + choosenFile);
                            }
                            else
                            {
                                choosenFile = "";
                            }
                        }
                    } while (choosenFile == ""); */

            //* ---------------------------- SEARCH STRING VALIDATION -----------------------------------

            string cleanedSearchStrings = SearchStringUtils.NormalizeAndCleanText(searchStringInput);

            try
            {
                SearchStringUtils.ValidateStringExceptions(cleanedSearchStrings);
            }
            catch (System.Exception exception)
            {
                Console.Error.WriteLine("\n" + exception.ToString());
                // TODO: Handle in a better way in the next version.
                Environment.Exit(0);
                // if (++countTentatives == maxTries) throw exception; //! Will be used in the next version of the code.
            }

            //* --------------------------------- EXECUTE PROGRAM ----------------------------------------

            ExecuteProgram(searchStringInput: searchStringInput, cleanedSearchStrings: cleanedSearchStrings, fileDirectory: fileDirectory, fileName: fileName, countQuery: countQuery);
            countQuery++;

            //* ------------------------------ OPEN GENERATED REPORT -------------------------------------

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo($@"{Directory.GetCurrentDirectory()}\generatedReport\generatedReport.pdf")
            {
                UseShellExecute = true
            };
            process.Start();
        }

        // TODO: Implement the .txt file reader {TestSearchStrings()} function that validades all search strings inside a test file.
        // TODO: Validate if the .txt is not empty.
        /*  else if (option == "3")
         {
             do
             {
                 Console.Write("\n♦ Input a directory name inside CurrentDirectory() that contains ypur TXT file (or use \"txtbusca\" as default): ");
                 Console.Write("\n♦ The current path is: ");
                 Console.Write(Directory.GetCurrentDirectory());
                 fileDirectory = Console.ReadLine();
                 fileDirectoryValidation = Directory.GetCurrentDirectory() + fileDirectory;
                 if (Directory.Exists(fileDirectoryValidation) is false)
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.Write("\n♦ Directory doesn't exists, write a valid directory!!\n");
                     Console.ForegroundColor = ConsoleColor.White;
                 }
             } while (Directory.Exists(fileDirectoryValidation) is false);

             do
             {
                 Console.WriteLine("Input the name of your TXT file: ");
                 chooseTxtFile = Console.ReadLine();
                 if (File.Exists(chooseTxtFile) is false)
                 {

                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.Write("\n♦ This directory doesn't contain this file, please choose a valid option below\n");
                     Console.ForegroundColor = ConsoleColor.White;
                     string[] arquivos = Directory.GetFiles(fileDirectoryValidation);
                     int cont = 0;
                     Console.WriteLine("Arquivos: ");
                     foreach (string arq in arquivos)
                     {
                         Console.Write("Opção: " + cont + ": ");
                         int fileNameIndex = arq.LastIndexOf(@"\") + 1;
                         string fileNames = arq.Substring(fileNameIndex, (arq.Length - fileNameIndex));
                         Console.WriteLine(fileNames);
                         cont++;
                     }

                     Console.WriteLine("Digite o número da opção desejada: ");
                     choosenOption = Convert.ToInt16(Console.ReadLine());
                     if (choosenOption >= 0 && choosenOption < arquivos.Length)
                     {
                         int fileNameIndex = arquivos[choosenOption].LastIndexOf(@"\") + 1;
                         chooseTxtFile = arquivos[choosenOption].Substring(fileNameIndex, (arquivos[choosenOption].Length - fileNameIndex));
                         Console.WriteLine("Arquivo selecionardo: " + chooseTxtFile);
                     }
                     else
                     {
                         chooseTxtFile = "";
                     }
                 }
             } while (chooseTxtFile == "");


             do
             {
                 Console.Write("\n♦ Input a directory name inside CurrentDirectory() that contains PDF files (or use \"pdfs\" as default): ");
                 Console.Write("\n♦ The current path is: ");
                 Console.Write(Directory.GetCurrentDirectory());
                 fileDirectory = Console.ReadLine();
                 fileDirectoryValidation = Directory.GetCurrentDirectory() + fileDirectory;
                 if (Directory.Exists(fileDirectoryValidation) is false)
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.Write("\n♦ Directory doesn't exists, write a valid directory!!\n");
                     Console.ForegroundColor = ConsoleColor.White;
                 }
             } while (Directory.Exists(fileDirectoryValidation) is false);


             do
             {
                 Console.WriteLine("Input the name of your file: ");
                 choosenFile = Console.ReadLine();
                 if (File.Exists(choosenFile) is false)
                 {

                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.Write("\n♦ This directory doesn't contain this file, please choose a valid option below\n");
                     Console.ForegroundColor = ConsoleColor.White;
                     string[] arquivos = Directory.GetFiles(fileDirectoryValidation);
                     int cont = 0;
                     Console.WriteLine("Arquivos: ");
                     foreach (string arq in arquivos)
                     {
                         Console.Write("Opção: " + cont + ": ");
                         int fileNameIndex = arq.LastIndexOf(@"\") + 1;
                         string fileNames = arq.Substring(fileNameIndex, (arq.Length - fileNameIndex));
                         Console.WriteLine(fileNames);
                         cont++;
                     }

                     Console.WriteLine("Digite o número da opção desejada: ");
                     choosenOption = Convert.ToInt16(Console.ReadLine());
                     if (choosenOption >= 0 && choosenOption < arquivos.Length)
                     {
                         int fileNameIndex = arquivos[choosenOption].LastIndexOf(@"\") + 1;
                         choosenFile = arquivos[choosenOption].Substring(fileNameIndex, (arquivos[choosenOption].Length - fileNameIndex));
                         Console.WriteLine("Arquivo selecionardo: " + choosenFile);
                     }
                     else
                     {
                         choosenFile = "";
                     }
                 }
             } while (choosenFile == "");
         } */

        /* else
        {
            Console.Clear();
        }

    } while (option != "5");

    Console.Clear(); 

}*/

        #region ShowMenu
        /*
         * Function that shows the custom menu.
        */
        public static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("\n▒▒▒▒▒▒▒▒▒▒▒▒▒ SEARCH STRING ENGINE ▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n\n");

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("╔═════════════════ OPTION MENU ═════════════════╗    ");

            Console.WriteLine("║ 1 - MANUAL SEARCH                             ║    ");

            Console.WriteLine("║                                               ║    ");

            Console.WriteLine("║ 3 - SEARCH USING .TXT FILE (TRABALHO FUTURO)  ║    ");

            Console.WriteLine("║                                               ║    ");

            Console.WriteLine("║ 5 - EXIT                                      ║    ");

            Console.WriteLine("╚═══════════════════════════════════════════════╝    ");

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("");
        }
        #endregion

        #region ExecuteProgram
        /*
         * Function responsible for executing the functions in the SearchStringUtils class.
        */
        public static void ExecuteProgram(string searchStringInput, string cleanedSearchStrings, string fileDirectory, string fileName, int countQuery)
        {

            StringBuilder pdfText = new StringBuilder();

            //* If the user does not write the directory or the file name the program will use default values for the tests.
            if (fileDirectory == "")
            {
                fileDirectory = "pdfs";
            }
            if (fileName == "")
            {
                fileName = "Projeto inicial - enunciado.pdf";
            }

            //* Appends the text of the .pdf file.
            pdfText.Append(SearchStringUtils.ReadTextFromPdf(fileDirectory: fileDirectory, fileName: fileName));

            //* Prints the user`s inputs and the search string normalized.
            SearchStringUtils.PrintOutputs<string>(outputName: "fileNameInserted", outputPrimitive: fileName);
            SearchStringUtils.PrintOutputs<string>(outputName: "fileDirectoryInserted", outputPrimitive: fileDirectory);
            SearchStringUtils.PrintOutputs<string>(outputName: "searchStringInserted", outputPrimitive: searchStringInput);
            SearchStringUtils.PrintOutputs<string>(outputName: "normalizedSearchString", outputPrimitive: cleanedSearchStrings);

            //* Tokenizes the search string and prints the result.
            List<string> tokenizedSearchStrings = SearchStringUtils.TokenizeSearchString(cleanedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "tokenizedSearchStrings", outputList: tokenizedSearchStrings);

            //* Separates the expressions from parentheses and prints the result.
            List<List<string>> separatedExpressions = SearchStringUtils.SeparateExpressionsFromParentheses(tokenizedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "separatedExpressions", outputListOfLists: separatedExpressions);

            //* Verifies if the expressions are valid or not and prints the result.
            List<Tuple<string, string>> VerifiedExpressions = SearchStringUtils.VerifyExpressions(separatedExpressions, pdfText.ToString());
            StringBuilder listOfTuples = new StringBuilder();

            foreach (var expressions in VerifiedExpressions)
            {
                listOfTuples.Append("[" + expressions.Item1 + ", " + expressions.Item2 + "] ");
            }
            Console.WriteLine($"\nverifiedExpressions ({VerifiedExpressions.GetType().Name})\t\t-->\t{listOfTuples.ToString().TrimEnd()}");

            //* Counts the search tokens in the .pdf file and prints the result. 
            Dictionary<string, int> countedTokensInPdf = SearchStringUtils.CountSearchTokensInPdf(VerifiedExpressions, pdfText.ToString());
            SearchStringUtils.PrintOutputs<Dictionary<string, int>>(outputName: "countedTokensInPdf", outputDictionary: countedTokensInPdf);

            //* Generates a report with the search string results and prints them.
            string report = SearchStringUtils.GenerateReport(countQuery: countQuery, fileName: fileName, searchString: searchStringInput, countedTokensInPdf);
            Console.WriteLine("\n" + report);
        }
        #endregion
    }
    // TODO: Test "and" and "or" with quotation and validate if is working.
}