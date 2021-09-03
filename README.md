# Busca por String em documentos

# Contribuidores
| <img src="https://avatars.githubusercontent.com/u/22084856?v=4" width=150px height=150px><br> [Paulo Backes](https://github.com/JrBackes)| <img src="https://avatars1.githubusercontent.com/u/43481916?s=400&u=2683d479631afcd710a45ec6cae3e82ba1a846bf&v=4" width=150px height=150px><br> [Vitor Matter](https://github.com/vmatter) |
|---|---|

# Dados da aplicação
Projeto tem como premissa, utilizar uma String de busca informada pelo usuário, para buscar as palavras em um arquivo em formato PDF.

# Regras do negócio
- As palavras que deverão ser localizadas no texto serão informadas em letras minúsculas.
- No caso de múltiplas palavras deverá ser informado o operador lógico entre elas. Os operadores lógicos aceitos serão **AND** e **OR**.
- Os operadores deverão ser escritos em letras maiúsculas, enquanto as palavras consultadas em letras minúsculas.
- A aplicação deverá retornar os seguintes resultados, com base na consulta realizada.
- Caso o operador OR seja utilizado, a aplicação deverá retornar o número de ocorrências de cada uma das palavras consultadas.
- Caso o operador AND seja utilizado, a aplicação deverá retornar se as duas palavras foram encontradas no texto, juntamente com o número de ocorrências de cada uma delas. 
É importante frisar que a as palavras poderão constar no documento em mais de um formato. Por exemplo, a palavra aplicação pode constar também como Aplicação, APLICAÇÃO, aplicacao, etc.

# Métodos

## Create files and folders

    string  TestSearchStrings(fileDirectory: string , fileName: string)
    
    string ReadTextInPdf(fileDirectory: string,fileName: string)



