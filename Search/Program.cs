using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Search
{
    class Program
    {

        #region CleanSearchString
        /*
         * Method responsible for removing unnecessary characters from the search string.
         * @see https://stackoverflow.com/questions/11395775/clean-the-string-is-there-any-better-way-of-doing-it
        */
        private static string CleanSearchString(string searchString)
        {
            string normalizedSearchString = searchString.ToLower().Normalize(NormalizationForm.FormD);

            string textCleaned = "";

            foreach (char c in normalizedSearchString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    /* if (c == '\"' || c == '(' || c == ')' || Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c))
                    {
                        textCleaned += c;
                    } */

                    if (Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c))
                    {
                        textCleaned += c;
                    }

                }

            }

            return textCleaned.Normalize(NormalizationForm.FormC);
        }
        #endregion 

        #region TokenizeSearchString

        // @see https://stackoverflow.com/questions/16265247/printing-all-contents-of-array-in-c-sharp
        //Console.WriteLine("\n[" + string.Join(", ", splits) + "]\n");
        private static List<string> TokenizeSearchString(string searchStringCleaned)
        {

            Dictionary<string, int> ocurrences = new Dictionary<string, int>();

            List<string> listOfAND = new List<string>();
            List<string> listOfOR = new List<string>();
            List<string> listAll = new List<string>();

            Stack<string> stackTest = new Stack<string>();

            int countOpenParentheses = 0;
            int countClosedParentheses = 0;
            bool areValidParentheses = false;

            searchStringCleaned = "(\"texto info\" banana and opcao)";

            if (searchStringCleaned.Length > 0)
            {
                // Count how many parentheses exist.
                countOpenParentheses = searchStringCleaned.Length - searchStringCleaned.Replace("(", "").Length;
                countClosedParentheses = searchStringCleaned.Length - searchStringCleaned.Replace(")", "").Length;
                //Console.WriteLine("countOpenParentheses '(' --> " + countOpenParentheses);
                //Console.WriteLine("countOpenParentheses ')' --> " + countClosedParentheses);
                //Environment.Exit(0);
            }
            else
            {

                Console.Error.WriteLine("Operação inválida searchStringCleaned.Lengt");
                Environment.Exit(0);
            }

            if ((countOpenParentheses - countClosedParentheses) != 0)
            {
                Console.Error.WriteLine("Operação inválida countClosedParentheses");
                Environment.Exit(0);
            }
            else
            {
                areValidParentheses = true;
            }

            //searchStringCleaned = "(texto or info) or (a and b)";
            searchStringCleaned = "(\"texto info\" banana and opcao)";
            Console.WriteLine("\n" + searchStringCleaned);

            string[] stringValidator = searchStringCleaned.Split(" ");

            List<string> andListTest = new List<string>();

            int qtdParentheses = 0;
            bool hasQuotation = false;
            bool closeParentheses = false;
            string quotationString = "";
            string stringBeforeParentheses = "";
            string searchWord = "";

            // TODO: Verificar o hasQuotation para os parênteses

            foreach (string word in stringValidator)
            {
                searchWord = word;
                if (areValidParentheses)
                {
                    if (searchWord[0] == '(')
                    {
                        closeParentheses = false;
                        qtdParentheses++;
                        stackTest.Push("(");
                        //resultString += word.Substring(1);
                        searchWord = word.Substring(1);
                        //stackTest.Push(word.Substring(1));
                    }
                    else if (searchWord[searchWord.Length - 1] == ')')
                    {
                        closeParentheses = true;
                        stringBeforeParentheses = searchWord.Remove(searchWord.Length - 1);
                        //stackTest.Push(searchWord.Remove(searchWord.Length - 1));
                        //stackTest.Push(")");
                        //qtdParentheses--;
                        //continue;
                    }
                }

                if (searchWord[0] == '\"')
                {
                    hasQuotation = true;
                    quotationString = searchWord;
                    continue;
                }
                else if (hasQuotation)
                {
                    if (searchWord[searchWord.Length - 1] == '\"')
                    {

                        quotationString += " " + searchWord;
                    }
                    else
                    {
                        quotationString += searchWord;
                        continue;
                    }
                }

                if ((searchWord != "and" && searchWord != "or") && (stackTest.Peek() != "and" && stackTest.Peek() != "or") && !hasQuotation)
                {
                    stackTest.Push("and");
                    stackTest.Push(searchWord);
                }
                else
                {
                    if (hasQuotation)
                    {
                        stackTest.Push(quotationString);
                        hasQuotation = false;
                        quotationString = "";
                    }
                    else if (closeParentheses)
                    {
                        stackTest.Push(stringBeforeParentheses);
                        stackTest.Push(")");
                    }
                    else
                    {
                        stackTest.Push(searchWord);
                    }
                }
            }

            Console.WriteLine("\nstackTest --> [" + string.Join(", ", stackTest) + "]");

            Stack<string> stackPrint = new Stack<string>();

            foreach (string word in stackTest)
            {
                stackPrint.Push(word);
            }

            Console.WriteLine("\nstackPrint --> [" + string.Join(", ", stackPrint) + "]");
            Environment.Exit(0);

            Console.WriteLine("\nSearch String --> " + searchStringCleaned);

            //Environment.Exit(0);
            //Console.WriteLine("\nstackTest --> [" + string.Join(", ", stackTest) + "]\n");

            //Environment.Exit(0);

            // [(, desenvolvimento, and, aplicacao, ), and, (, teste, or, validacao, or vacina, )]

            // ["(, "texto and info" ,), "or" , "(", "a and b", ")"] 

            //string text = "Um texto contendo desenvolvimento, validacao";
            bool valid = false;
            //["desenvolvimento and aplicacao and teste or valor or verdade"]

            if (searchStringCleaned.Contains(" and "))
            {

                listAll = searchStringCleaned.Split(" and ").ToList();
                listOfAND = listAll.ToList();

                //Console.WriteLine("\ntext --> " + text);
                //Console.WriteLine("\nlistAll --> [" + string.Join(", ", listAll) + "]\n");
                foreach (string item in listAll)
                {
                    if (item.Contains("or"))
                    {
                        listOfOR.AddRange(item.Split(" or "));
                        foreach (string val in listOfOR)
                        {
                            // OR condition
                            /* if (text.Contains(val))
                            {
                                valid = true;
                            } */

                            // AND condition
                            /* if (text.Contains(val))
                            {
                                valid = true;
                            } else 
                            {
                                valid = false;
                            }  */
                            //Console.WriteLine(val);
                        }
                        //listOfAND.Remove(item);
                        //listOfAND.Remove(item);
                        //Console.WriteLine("isValid --> " + valid);
                    }
                    //Console.WriteLine("\nlistOfOR --> [" + string.Join(", ", listOfOR) + "]\n");
                }

                // (desenvolvimento or aplicacao) and (teste or validacao or vacina)
                // busca primeira lista de or
                // busca segunda lista de or
                // compara o and

                /* if (listOfAND.Contains("or"))
                {
                    listOfAND.Remove
                } */


            }

            /* if (searchStringCleaned.Contains("or")) 
            {
                listOfOR = (searchStringCleaned.Split(" or ").ToList());
            } */
            // (palavraA AND palavraB) AND (palavraC AND palavraD) OR (palavraE AND palavraF)
            // {[and]["palavraA", "palavraB"], [and]["palavraC", "palavraD"], [and]["palavraE", "palavraF"]}
            // AND --> Contains ("palavraA") &&  Contains ("palavraB")
            // OR  --> Contains ("palavraA") ||  Contains ("palavraB")

            // lista de palavras
            // dicionário com a palavra + qtdDePalavras
            // 1

            // (banana AND maca AND pera) --> "banana, maca, pera"
            // (desenvolvimento OR teste) --> "desenvolvimento, teste"
            // (desenvolvimento AND teste) OR (banana AND maca AND pera) --> "desenvolvimento, teste", "banana, maca, pera"
            // (a OR b) AND (c OR d) --> "a, b" e "c"

            // lista de dicionarios {[and]["banana, maca, pera"], }
            // [and]["banana, maca, pera"]
            // [or]["desenvolvimento, teste"]

            //"{desenvolvimento, aplicacao, teste}"

            //Console.WriteLine("\n" + searchStringCleaned + "\n");

            //string[] splits = searchStringCleaned.Split(new string[] { " or ", " and " }, StringSplitOptions.TrimEntries);

            //Console.WriteLine("\n-------------------------------------------------------");
            Console.WriteLine("\nlistAll --> [" + string.Join(", ", listAll) + "]\n");
            Console.WriteLine("\nlistOfAND --> [" + string.Join(", ", listOfAND) + "]\n");
            Console.WriteLine("\nlistOfOR --> [" + string.Join(", ", listOfOR) + "]\n");

            // [stringBusca][stringType]
            // [key][value]

            // dicionario 
            // pos 1 --> ["desenvolvimento, teste"], ["and"]
            // pos 2 --> "banana, maca, pera", ["and"]

            // if contains pos1 || contains pos2


            return listOfAND;
        }

        #endregion


        /*  void PrintStack(Stack<string> s)
 {
     // If stack is empty then return
     if ()
         return;


     int x = s.top();

     // Pop the top element of the stack
     s.pop();

     // Recursively call the function PrintStack
     PrintStack(s);

     // Print the stack element starting
     // from the bottom
     Console.WriteLine(x + " ");

     // Push the same element onto the stack
     // to preserve the order
     s.push(x);
 } */

        #region VerifySearchStringInPDF

        public static void VerifySearchStringInPDF(string filePath, string searchString, int contQuery)
        {
            string searchStringCleaned = CleanSearchString(searchString);
            TokenizeSearchString(searchStringCleaned);

            /* using (PdfReader reader = new PdfReader(@filePath))
            {
                var texto = new System.Text.StringBuilder();

                // [0] == desenvolvimento , [1] = aplicacao

                // projeto,projeto não

                using StreamWriter file = new(@"E:\vitor_desktop\iCybersec\C#\Trabalho I\Busca-por-strings-em-documentos\testes\text55.txt");
                {
                    file.WriteLine($"*****************************************");
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string aux = PdfTextExtractor.GetTextFromPage(reader, i);
                        string[] linhas = aux.Split('\n');
                        foreach (string linha in linhas)
                        {

                            foreach (string word in searchStringCleaned)
                            {
                                if (linha.Contains(@"" + word))
                                {
                                    texto.Append($"{word}{"\n"}");
                                    file.WriteLine(word);

                                }
                            }
                        }
                    }
                    file.WriteLine($"*****************************************");
                    Console.WriteLine(texto);
                }

            } */
            //contQuery++;
            //ShowHistoryReport(contQuery, filePath, searchString, ocurrences);
        }

        public static void ShowHistoryReport(int contQuery, string filePath, string searchString, Dictionary<string, int> occurrences)
        {
            int fileNameIndex = filePath.LastIndexOf(@"\") + 1;

            string fileName = filePath.Substring(fileNameIndex, (filePath.Length - fileNameIndex));

            Console.WriteLine(fileName);


        }

        #endregion
        public static void Main(string[] args)
        {

            //string text = "Em um mundo onde a informação tornou-se um dos recursos abundantes mais relevantes para a sociedade, é imprescindível que além da extração segura dos dados, realizar uma classificação significativa dos dados adquiridos também deve ser possível, visto que estes podem conter informações sensíveis de entidades. Uma das formas mais utilizadas de extração de informação é através de textos, portanto técnicas de Processamento de Linguagem Natural (PLN) vêm sendo vastamente exploradas. Levando isso em consideração, o objetivo deste trabalho foi encontrar arquiteturas sistêmicas capazes de aplicar classificação em textos e extrair com sucesso informações relevantes. Uma revisão sistemática da literatura (RSL) foi conduzida para analisar artigos acadêmicos publicados de 2010 até o início de janeiro de 2021. O processo de triagem resultou em uma população final de 21 estudos de um total de 234 analisados. A filtragem inclui a remoção de artigos não relacionados a uma classificação de texto ou arquitetura sistêmica de classificação de informações. Neste artigo, propostas e resultados que contribuem para os desafios de classificação de texto são apresentados considerando quatro questões de pesquisa. A conclusão do estudo atestou que não existe uma arquitetura sistêmica ou algoritmo de classificação específico capaz de ser considerado o estado da arte no campo da classificação de texto.";

            //string searchString = "@@#((\"Classificação''' :::;';'';'~~de ;'';'~~Texto\"!@#$%$%@ OR##@! !@#!@#Classificação ;'';'~~de ;'';'~~Informação);'';'~~ AND PLN)";

            // TODO: Validate VerifySearchStringInPDF

            /* while (true)
            { */

            Console.WriteLine("Digite sua String de Busca");

            // Handle the searchString.
            //string searchString = Console.ReadLine();
            // TODO: FileName with Open in VS.
            // Console.WriteLine("Abrir arquivo de nome tal.");
            // ReadLine()

            string searchString = "desenvolvimento OR aplicação";

            // Handle the file.
            string filePath = @"E:\vitor_desktop\iCybersec\C#\Trabalho I\Busca-por-strings-em-documentos\testes\Enunciado - Projeto 1.pdf";

            int contQuery = 0;

            VerifySearchStringInPDF(filePath, searchString, ++contQuery);

            //List<Dictionary<string, string>> a = new List<Dictionary<string, string>>();

            // > 0    > 0
            //"teste, validacao";


            /* foreach(string word in searchStringWords)
            {
                if(text.Contains(word))
                {
                    //TODO: Counting logic for each word, perhaps an hashmap that will have all words and a second attribute for qtd?
                }
            } */


        }

    }
}
