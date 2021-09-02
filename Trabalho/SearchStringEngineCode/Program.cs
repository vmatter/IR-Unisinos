using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            string searchStringInput = "(teste and desenvolvimento) or programação";

            // ---------------------------------------------------------------------------------------------------

            do
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

                    //TODO: Validar se o diretório existe. if(dirExists() else asks again)
                    Console.Write("\n♦ Input a directory name inside CurrentDirectory() that contains PDF files (or use \"pdfs\" as default): ");
                    fileDirectory = Console.ReadLine();

                    //TODO: Validar se o arquivo existe. if(fileExists() else asks again)
                    Console.Write("\n♦ Input a PDF file name that is inside the directory chosen (or use \"Projeto inicial - enunciado.pdf\" as default): ");
                    fileName = Console.ReadLine();

                }
                else if (option == "3")
                {

                }
                else
                {
                    Console.Clear();
                }

            } while (option != "5");

            // ---------------------------------------------------------------------------------------------------

            Environment.Exit(0);

            SearchStringUtils.PrintOutputs<string>(outputName: "fileNameInput", outputPrimitive: fileName);
            SearchStringUtils.PrintOutputs<string>(outputName: "fileDirectoryInput", outputPrimitive: fileDirectory);
            SearchStringUtils.PrintOutputs<string>(outputName: "searchStringInput", outputPrimitive: searchStringInput);
            string cleanedSearchStrings = SearchStringUtils.NormalizeAndCleanText(searchStringInput);
            SearchStringUtils.PrintOutputs<string>(outputName: "normalizedSearchString", outputPrimitive: cleanedSearchStrings);

            //? Testar strings
            //string text = "Em um mundo onde a informação tornou-se um dos recursos abundantes mais relevantes para a sociedade, é imprescindível que além da extração segura dos dados, realizar uma classificação significativa dos dados adquiridos também deve ser possível, visto que estes podem conter informações sensíveis de entidades. Uma das formas mais utilizadas de extração de informação é através de textos, portanto técnicas de Processamento de Linguagem Natural (PLN) vêm sendo vastamente exploradas. Levando isso em consideração, o objetivo deste trabalho foi encontrar arquiteturas sistêmicas capazes de aplicar classificação em textos e extrair com sucesso informações relevantes. Uma revisão sistemática da literatura (RSL) foi conduzida para analisar artigos acadêmicos publicados de 2010 até o início de janeiro de 2021. O processo de triagem resultou em uma população final de 21 estudos de um total de 234 analisados. A filtragem inclui a remoção de artigos não relacionados a uma classificação de texto ou arquitetura sistêmica de classificação de informações. Neste artigo, propostas e resultados que contribuem para os desafios de classificação de texto são apresentados considerando quatro questões de pesquisa. A conclusão do estudo atestou que não existe uma arquitetura sistêmica ou algoritmo de classificação específico capaz de ser considerado o estado da arte no campo da classificação de texto.";
            //string searchStringInput = "(texto and info) or (a and b)";
            //string searchStringInput = "desenvolvimento and teste and coragem and verificar";
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

            List<string> tokenizedSearchStrings = SearchStringUtils.TokenizeSearchString(cleanedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "tokenizedSearchStrings", outputList: tokenizedSearchStrings);

            List<List<string>> separatedExpressions = SearchStringUtils.SeparateExpressions(tokenizedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "separatedExpressions", outputListOfLists: separatedExpressions);

            string pdfText = SearchStringUtils.ReadTextInPdf(fileDirectory: fileDirectory, fileName: fileName);

            List<Tuple<string, string>> VerifiedExpressions = SearchStringUtils.VerifyExpressions(separatedExpressions, pdfText);

            StringBuilder listOfTuples = new StringBuilder();

            foreach (var expressions in VerifiedExpressions)
            {
                listOfTuples.Append("[" + expressions.Item1 + ", " + expressions.Item2 + "] ");
            }

            Console.WriteLine($"\nVerifiedExpressions ({VerifiedExpressions.GetType().Name})\t\t-->\t{listOfTuples.ToString().TrimEnd()}");

            //Dictionary<string, int> searchTokensdictionary = SearchStringUtils.FindExpressionsInPdf(separatedExpressions, filePath);

            /* string pdfText = "";
            List<Tuple<string, string>> VerifiedExpressions = SearchStringUtils.VerifyExpressions(separatedExpressions, pdfText); */

            //SearchStringUtils.GenerateReport(++countQuery, filePath, searchStringInput, searchTokensdictionary);

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
        public static void ExecuteProgram()
        {

        }
        #endregion
    }
}