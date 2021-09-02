using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace SearchStringHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Handle the file.
            int countTentatives = 0;
            int maxTries = 3;
            int countQuery = 0;
            string option = "";
            string fileName = "";
            string fileDirectory = "";
            string fileDirectoryValidation = "";
            string fileExists = "";
            int choosenOption = 0;
            string choosenFile = "";
            string chooseTxtFile = "";
            //string searchStringInput = "(teste and desenvolvimento) or programação";

            // ---------------------------------------------------------------------------------------------------

            /* do
            {
                ShowMenu();

                while (option != "1" && option != "3" && option != "5")
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
                }
                else if (option == "3")
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
                }
                else
                {
                    Console.Clear();
                }

            } while (option != "5"); */

            // ---------------------------------------------------------------------------------------------------

            //? Testar strings
            //string text = "Em um mundo onde a informação tornou-se um dos recursos abundantes mais relevantes para a sociedade, é imprescindível que além da extração segura dos dados, realizar uma classificação significativa dos dados adquiridos também deve ser possível, visto que estes podem conter informações sensíveis de entidades. Uma das formas mais utilizadas de extração de informação é através de textos, portanto técnicas de Processamento de Linguagem Natural (PLN) vêm sendo vastamente exploradas. Levando isso em consideração, o objetivo deste trabalho foi encontrar arquiteturas sistêmicas capazes de aplicar classificação em textos e extrair com sucesso informações relevantes. Uma revisão sistemática da literatura (RSL) foi conduzida para analisar artigos acadêmicos publicados de 2010 até o início de janeiro de 2021. O processo de triagem resultou em uma população final de 21 estudos de um total de 234 analisados. A filtragem inclui a remoção de artigos não relacionados a uma classificação de texto ou arquitetura sistêmica de classificação de informações. Neste artigo, propostas e resultados que contribuem para os desafios de classificação de texto são apresentados considerando quatro questões de pesquisa. A conclusão do estudo atestou que não existe uma arquitetura sistêmica ou algoritmo de classificação específico capaz de ser considerado o estado da arte no campo da classificação de texto.";
            string searchStringInput = "(texto and info) or (a and b)";
            //string searchStringInput = "(texto and info)(a and b)"; // ! Revisar questão
            //string searchStringInput = "(texto and info) (a and b)"; //! REvisar bug no contains "info"
            //string searchStringInput = "info";
            //string searchStringInput = "desenvolvimento or teste or coragem or verificar";
            //string searchStringInput = "Desenvolvimento or aplicação";
            //string searchStringInput = "\"Desenvolvimento de Aplicações\" AND \"Mineração de Texto\"";
            //string searchStringInput = "desenvolvimento or teste or coragem and verificar";
            //string searchStringInput = "teste and (promoção or testando)"; // ! Revisar questão do and(.
            //string searchStringInput = "teste and (olhando ";
            //string searchStringInput = "teste and olhando)";
            //string searchStringInput = "teste andolhando";
            //string searchStringInput = "teste and(verificação de linguagens)"; // ! Bug de junto do parênteses
            //string searchStringInput = "teste and ((mand de) linguagens)";
            //string searchStringInput = "and teste or verificação"; // ! Revisar questão do and.
            //string searchStringInput = "verificando and teste @"; // ! Revisar questão do and.
            //string searchStringInput = "(olhando and observando) or verificação -"; // ! Revisar questão do and.
            //string searchStringInput = "a and b or c and d or e";
            //string searchStringInput = "desenvolvimento AND aplicação";
            //string searchStringInput = "(\"texto info\" banana and opcao)";
            //string searchStringInput = "\"(\"texto info\" banana and opcao)\"";
            //string searchStringInput = "\"teste\"";
            //string searchStringInput = "@@#((\"Classificação''' :::;';'';'~~de ;'';'~~Texto\"!@#$%$%@ OR##@! !@#!@#Classificação ;'';'~~de ;'';'~~Informação);'';'~~ AND PLN)";


            string cleanedSearchStrings = SearchStringUtils.NormalizeAndCleanText(searchStringInput);

            try
            {
                SearchStringUtils.ValidateStringExceptions(cleanedSearchStrings);
            }
            catch (System.Exception exception)
            {
                Console.Error.WriteLine("\n" + exception.ToString());
                // TODO: Tratar depois.
                Environment.Exit(0);
                //if (++countTentatives == maxTries) throw exception;
            }
            ExecuteProgram(searchStringInput: searchStringInput, cleanedSearchStrings: cleanedSearchStrings, fileDirectory: fileDirectory, fileName: fileName, countQuery: countQuery);

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

            Console.WriteLine("║ 3 - SEARCH USING .TXT FILE                    ║    ");

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

            SearchStringUtils.GenerateReport(countQuery: ++countQuery, fileName: fileName, searchString: searchStringInput, foundedTokensInPdf);

            // TODO: Testar o and e or sozinho na frase depois, colocar eles com aspas.

            // TODO: Fazer a leitura do PDF

            // TODO: verificar string (teste and desenvolvimento) or programação --> or está sendo pego junto.



        }
        #endregion
    }
}