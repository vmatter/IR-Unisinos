using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SearchStringHandler
{
    public class Program
    {
        public static string searchString = "";
        public static void Main(string[] args)
        {
            //TODO: Implementar metodo de leitura.
            //string searchString = SearchStringUtils.InputSearchString();

            // Handle the file.
            string filePath = @"E:\vitor_desktop\iCybersec\C#\Trabalho I\Busca-por-strings-em-documentos\testes\Enunciado - Projeto 1.pdf";
            int count = 0;
            int maxTries = 3;

            //? Testar strings
            //string text = "Em um mundo onde a informação tornou-se um dos recursos abundantes mais relevantes para a sociedade, é imprescindível que além da extração segura dos dados, realizar uma classificação significativa dos dados adquiridos também deve ser possível, visto que estes podem conter informações sensíveis de entidades. Uma das formas mais utilizadas de extração de informação é através de textos, portanto técnicas de Processamento de Linguagem Natural (PLN) vêm sendo vastamente exploradas. Levando isso em consideração, o objetivo deste trabalho foi encontrar arquiteturas sistêmicas capazes de aplicar classificação em textos e extrair com sucesso informações relevantes. Uma revisão sistemática da literatura (RSL) foi conduzida para analisar artigos acadêmicos publicados de 2010 até o início de janeiro de 2021. O processo de triagem resultou em uma população final de 21 estudos de um total de 234 analisados. A filtragem inclui a remoção de artigos não relacionados a uma classificação de texto ou arquitetura sistêmica de classificação de informações. Neste artigo, propostas e resultados que contribuem para os desafios de classificação de texto são apresentados considerando quatro questões de pesquisa. A conclusão do estudo atestou que não existe uma arquitetura sistêmica ou algoritmo de classificação específico capaz de ser considerado o estado da arte no campo da classificação de texto.";
            //string searchString = "@@#((\"Classificação''' :::;';'';'~~de ;'';'~~Texto\"!@#$%$%@ OR##@! !@#!@#Classificação ;'';'~~de ;'';'~~Informação);'';'~~ AND PLN)";
            string searchString = "(texto or info) and (a or b)";
            //string searchString = "desenvolvimento AND aplicação";
            //string searchString = "(\"texto info\" banana and opcao)";
            //string searchString = "\"(\"texto info\" banana and opcao)\"";
            //string searchString = "\"teste\"";

            SearchStringUtils.PrintLogs<string>(name:"searchString", searchString);

            //? Testar com o While depois.
            /* while (true)
            { */
            //Console.WriteLine("Digite sua String de Busca");
            //searchString = Console.ReadLine();

            string searchStringCleaned = SearchStringUtils.CleanSearchString(searchString);
            SearchStringUtils.PrintLogs<string>("searchStringCleaned", searchStringCleaned);

            // While
            try
            {
                SearchStringUtils.ValidateStringExceptions(searchStringCleaned);
            }
            catch (System.Exception exception)
            {
                Console.Error.WriteLine("\n" + exception.ToString());
                if (++count == maxTries) throw exception;
            }

            List<string> searchStringTokens = SearchStringUtils.TokenizeSearchString(searchStringCleaned);

            SearchStringUtils.PrintLogs("searchStringTokens", stringList: searchStringTokens);

            List<List<string>> tokenizedValidation = SearchStringUtils.SeparateExpressions(searchStringTokens);

            SearchStringUtils.PrintLogs("tokenizedValidation", stringListOfLists: tokenizedValidation);

            Dictionary<string, int> searchTokensdictionary = SearchStringUtils.FindExpressionsInPdf(tokenizedValidation, filePath);

            //SearchStringUtils.GenerateReport(++contQuery, filePath, searchString, searchTokensdictionary);
            /* } */

            // End While

            // }

        }
    }
}