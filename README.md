# Humberto Costa | Interpretador de Pseudo-Código

Licenciamento: Old 4-clause BSD
Requer a 'publicidade' da origem do código fonte, conforme a licença, o que impede que se misture código GPL com este.

Foi incorporado o Projeto ACE (https://github.com/ajaxorg/ace-builds) na interface Web do Projeto HCADWEB, este é licenciado em 3-clause BSD

## Como usar

### Projeto HCADWEB - Ambiente de Desenvolvimento Web

Em Construção

### Projeto HCIPC - Interpretador de Pseudo-Código

Atualmente o Interpretador sempre olhará o arquivo teste.hcp que deve estar na mesma pasta que o executável.
Bastando alterar o conteúdo do arquivo e executar novamente.

## Exemplo de código compatível

```portugol

algoritmo "teste"

var
    n1, n2, soma: inteiro

funcao multiplicaPor2 (valor:inteiro)
var
    teste1:inteiro
inicio
    teste1 <- 2
    teste1 <- valor * teste1
fimfuncao
 
inicio
    escreva("Digite o primeiro número..: ")
    leia(n1)

    escreva("Digite o segundo número...: ")
    leia(n2)

    soma <- (n1 + n2);

    escreval("Número 1..................: ", n1)
    escreval("Número 2..................: ", n2)
    escreval("Soma......................: ", soma)
    escreval("Subtração.................: ", n1-n2)
    escreval("Multiplicação.............: ", n1*n2)
    escreval("Multiplica por 2..........: ", multiplicaPor2(n1))
    se (n2 > 0) entao
        escreval("Divisão...................: ", n1/n2)
        escreval("Módulo....................: ", n1 mod n2)
    senao
        escreval("Divisão...................: FALHA")
        escreval("Módulo....................: FALHA")
    fimse
    escreval("Igual.....................: ", n1=n2)
    escreval("Maior que.................: ", n1>n2)
    escreval("Menor que.................: ", n1<n2)

    escreva("Contar até 10.............:")
    n1 <- 0

    enquanto (n1 < 5) faca
        escreva(" ", n1 + 1)
        n1 <- n1 + 1
    fimenquanto

    repita
        escreva(" ", n1 + 1)
        n1 <- n1 + 1
    ate (n1 = 10)

    escreval("")

    escreva("Contagem regressiva.......:")

    n1 <- n2
    
    enquanto (n1 >= 0) faca
        escreva(" ", n1)
        n1 <- n1 - 1
    fimenquanto

    escreval("")
 
fimalgoritmo

```

## Projeto

Este projeto tem como objetivo de criar uma versão independente de plataforma de um interpretador de Pseudo-Código, compatível com o VisuAlg.

Projeto inspirado nas aulas da Professora Débora, que deveria estar dando aula na USP

## Funcionamento

- Lê o código em Pseudo-Código
- Extrai os Tokens/Trechos(Palavras, sinais, números, strings, etc) do código fonte
- Converte esses tokens/trechos em Nós de uma Arvore de Dados, onde se armazena as características de cada parte do código
- Processa esses nós, para que eles sejam executáveis
- Executa a partir do nó inicial, que sequencialmente guiará aos próximos
- Se houver um erro armazena os dados disponíveis e exibe ao usuário mostrando a linha de comando que deu erro

## Objetivos Iniciais (Focado diretamente na execução do Interpretador)

- [x] Estrutura Base
- [x] Extração dos Arvore de Tokens apartir do Código Fonte
- [x] Criação dos Nós basicos para declaração de variáveis e execução de contas matemáticas simples
- [x] Criação do comando Escreva/Escreval
- [x] Criação do comando Leia
- [x] Criação dos comandos SE e Expressões Condicionais
- [x] Criação dos comandos de repetição (REPITA, ENQUANTO)
- [ ] Criação dos comandos de repetição (PARA)
- [ ] Diferenciar conteúdo do bloco VAR e do bloco INICIO, bloqueando a declaração de variáveis no código fonte
- [ ] Implementar comandos matemáticos avançados (RAIZQ, etc)
- [x] Implementar Procedimentos e Funções

## Objetivos Auxiliares

- [x] Montar Ambiente de Desenvolvimento para Execução do Código Fonte
- [ ] Melhorar Ambiente de Desenvolvimento
- [ ] Montar documentação da Linguagem e Funcionamento da ferramenta
- [ ] Montar rotinas de testes
