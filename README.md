# Humberto Costa | Interpretador de Pseudo-Código

Licenciamento: Old 4-clause BSD

Escolhida com o objetivo de ser uma licença aberta porém incompatível com a GPL, assim perturbando a vida de gente chata =)
Requer a 'publicidade' da origem do código fonte, conforme a licença, o que impede que se misture código GPL com este.

## Como usar

Atualmente o Interpretador sempre olhará o arquivo teste.hcp que deve estar na mesma pasta que o executável.
Bastando alterar o conteúdo do arquivo e executar novamente.

## Exemplo de código compatível

```portugol

algoritmo "teste"

var
    n1, n2, soma: inteiro
 
inicio
 
    escreva("Digite o primeiro número..: ")
    leia(n1)

    escreva("Digite o segundo número...: ")
    leia(n2)

    soma <- n1 + n2;

    escreval("Número 1..................: ", n1)
    escreval("Número 2..................: ", n2)
    escreval("Soma......................: ", soma)
    escreval("Subtração.................: ", n1-n2)
    escreval("Multiplicação.............: ", n1*n2)
    escreval("Divisão...................: ", n1/n2)
    escreval("Módulo....................: ", n1 mod n2)
 
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
- [ ] Criação dos comandos SE e Expressões Condicionais
- [ ] Criação dos comandos de repetição (PARA, ENQUANTO)
- [ ] Diferenciar conteúdo do bloco VAR e do bloco INICIO, bloqueando a declaração de variáveis no código fonte
- [ ] Implementar comandos matemáticos avançados (RAIZQ, etc)
- [ ] Implementar Procedimentos e Funções

## Objetivos Auxiliares

- [ ] Montar Ambiente de Desenvolvimento para Execução do Código Fonte
- [ ] Montar documentação da Linguagem e Funcionamento da ferramenta
- [ ] Montar rotinas de testes
