using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace SearchStringHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Handle the file.
            int countTentatives = 0;
            int maxTries = 3;
            int countQuery = 1;
            string option = "";
            string fileName = "";
            string fileDirectory = "";
            string fileDirectoryValidation = "";
            string fileExists = "";
            int choosenOption = 0;
            string choosenFile = "";
            string chooseTxtFile = "";
            string searchStringInput = "Desenvolvimento or aplicação";

            // ---------------------------------------------------------------------------------------------------

            do
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
                    } while (choosenFile == "");

                    string cleanedSearchStrings = SearchStringUtils.NormalizeAndCleanText(searchStringInput);

                    //---------------------------- SEARCH STRING VALIDATION ------------------------------------
                    try
                    {
                        SearchStringUtils.ValidateStringExceptions(cleanedSearchStrings);
                    }
                    catch (System.Exception exception)
                    {
                        Console.Error.WriteLine("\n" + exception.ToString());
                        // TODO: Handle in a better way in the future.
                        Environment.Exit(0);
                        //if (++countTentatives == maxTries) throw exception;
                    }

                    //---------------------------- EXECUTE PROGRAM ---------------------------------------------

                    ExecuteProgram(searchStringInput: searchStringInput, cleanedSearchStrings: cleanedSearchStrings, fileDirectory: fileDirectory, fileName: fileName, countQuery: countQuery);
                    countQuery++;

                    //---------------------------- EXECUTE THE GENERATED REPORT ---------------------------------------------

                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo($@"{Directory.GetCurrentDirectory()}\generatedReport\generatedReport.pdf")
                    {
                        UseShellExecute = true
                    };
                    process.Start();


                }


                // TODO: Implement a .txt file reader that validades all the string inside the file.
                /*  else if (option == "3")
                 {
                     //TODO: A validação se o TXT contém algo será no método de leitura, certo?
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

                else
                {
                    Console.Clear();
                }

            } while (option != "5");

            // ---------------------------------------------------------------------------------------------------


        }

        #region ShowMenu
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
        public static void ExecuteProgram(string searchStringInput, string cleanedSearchStrings, string fileDirectory, string fileName, int countQuery)
        {

            StringBuilder pdfText = new StringBuilder();

            if (fileDirectory == "")
            {
                fileDirectory = "pdfs";
            }
            if (fileName == "")
            {
                fileName = "Projeto inicial - enunciado.pdf";
            }

            pdfText.Append(SearchStringUtils.ReadTextInPdf(fileDirectory: fileDirectory, fileName: fileName));

            SearchStringUtils.PrintOutputs<string>(outputName: "fileNameInserted", outputPrimitive: fileName);
            SearchStringUtils.PrintOutputs<string>(outputName: "fileDirectoryInserted", outputPrimitive: fileDirectory);

            SearchStringUtils.PrintOutputs<string>(outputName: "searchStringInserted", outputPrimitive: searchStringInput);
            SearchStringUtils.PrintOutputs<string>(outputName: "normalizedSearchString", outputPrimitive: cleanedSearchStrings);

            List<string> tokenizedSearchStrings = SearchStringUtils.TokenizeSearchString(cleanedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "tokenizedSearchStrings", outputList: tokenizedSearchStrings);

            List<List<string>> separatedExpressions = SearchStringUtils.SeparateExpressions(tokenizedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "separatedExpressions", outputListOfLists: separatedExpressions);

            List<Tuple<string, string>> VerifiedExpressions = SearchStringUtils.VerifyExpressions(separatedExpressions, pdfText.ToString());

            StringBuilder listOfTuples = new StringBuilder();

            foreach (var expressions in VerifiedExpressions)
            {
                listOfTuples.Append("[" + expressions.Item1 + ", " + expressions.Item2 + "] ");
            }

            Console.WriteLine($"\nverifiedExpressions ({VerifiedExpressions.GetType().Name})\t\t-->\t{listOfTuples.ToString().TrimEnd()}");

            Dictionary<string, int> foundedTokensInPdf = SearchStringUtils.FindExpressionsInPdf(VerifiedExpressions, pdfText.ToString());

            SearchStringUtils.PrintOutputs<Dictionary<string, int>>(outputName: "foundedTokensInPdf", outputDictionary: foundedTokensInPdf);

            SearchStringUtils.GenerateReport(countQuery: countQuery, fileName: fileName, searchString: searchStringInput, foundedTokensInPdf);

            // TODO: Testar o and e or sozinho na frase depois, colocar eles com aspas.

            // TODO: Fazer a leitura do PDF

            // TODO: verificar string (teste and desenvolvimento) or programação --> or está sendo pego junto.



        }
        #endregion
    }
}