using System;
using System.Collections.Generic;

namespace SearchStringHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //? Testar strings
            //string text = "Em um mundo onde a informação tornou-se um dos recursos abundantes mais relevantes para a sociedade, é imprescindível que além da extração segura dos dados, realizar uma classificação significativa dos dados adquiridos também deve ser possível, visto que estes podem conter informações sensíveis de entidades. Uma das formas mais utilizadas de extração de informação é através de textos, portanto técnicas de Processamento de Linguagem Natural (PLN) vêm sendo vastamente exploradas. Levando isso em consideração, o objetivo deste trabalho foi encontrar arquiteturas sistêmicas capazes de aplicar classificação em textos e extrair com sucesso informações relevantes. Uma revisão sistemática da literatura (RSL) foi conduzida para analisar artigos acadêmicos publicados de 2010 até o início de janeiro de 2021. O processo de triagem resultou em uma população final de 21 estudos de um total de 234 analisados. A filtragem inclui a remoção de artigos não relacionados a uma classificação de texto ou arquitetura sistêmica de classificação de informações. Neste artigo, propostas e resultados que contribuem para os desafios de classificação de texto são apresentados considerando quatro questões de pesquisa. A conclusão do estudo atestou que não existe uma arquitetura sistêmica ou algoritmo de classificação específico capaz de ser considerado o estado da arte no campo da classificação de texto.";

            string searchString = "@@#((\"Classificação''' :::;';'';'~~de ;'';'~~Texto\"!@#$%$%@ OR##@! !@#!@#Classificação ;'';'~~de ;'';'~~Informação);'';'~~ AND PLN)";
            //searchStringCleaned = "(texto or info) or (a and b)";
            //searchStringCleaned = "(\"texto info\" banana and opcao)";
            //searchStringCleaned = "\"(\"texto info\" banana and opcao)\"";
            //string searchString = "\"teste\"";

            //? Testar com o While depois.
            // while (true)
            // {

            int count = 0;
            int maxTries = 3;
            bool areValidParentheses = false;
            //Console.WriteLine("Digite sua String de Busca"); 

            //string searchString = Console.ReadLine();
            //string searchString = "(texto or info) or (a and b)";
            string searchStringCleaned = SearchStringUtils.CleanSearchString(searchString);

            // TODO: Pedir para o Marcio se tem como voltar ao início do programa estando dentro da classe.

            // While
            try
            {
                areValidParentheses = SearchStringUtils.ValidateStringConditions(searchStringCleaned);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine("String de busca irregular, verificar novamente.");
                Console.Error.WriteLine(exception.ToString());
                if (++count == maxTries) throw exception;
            }

            // End While

            Stack<string> searchStringTokens = SearchStringUtils.TokenizeSearchString(searchStringCleaned, areValidParentheses);

            //Console.WriteLine("\nsearchStringTokens --> [" + string.Join(", ", searchStringTokens) + "]");

            // Handle the file.
            string filePath = @"E:\vitor_desktop\iCybersec\C#\Trabalho I\Busca-por-strings-em-documentos\testes\Enunciado - Projeto 1.pdf";

            int contQuery = 0;

            SearchStringUtils.SearchTokensInPDF(filePath, searchStringCleaned, ++contQuery);

            // }

        }
    }
}