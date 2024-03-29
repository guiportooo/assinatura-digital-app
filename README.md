# assinatura_digital

App assinatura_digital desenvolvido com Xamarin Forms

## Setup

- Ferramentas necessárias: Visual Studio ou Visual Studio for Mac, Git, Xamarin
- Para executar os projetos localmente, configurar emulador do Android e simulador do iOS

## Políticas de Código

- Código escrito em inglês, exceto nomes exclusivos da Língua Portuguesa (Ex.: CPF, CNH, RG, etc.)
- Regras de escrita do código utilizarão arquivo .editorconfig
- Dependências serão atualizadas conforme o time sentir a necessidade

## Políticas de branchs

- Branches principais: **master** e **develop**
- Novas features serão desenvolvidas a partir da branch **develop** com padrão de nome _feature/[nome_da_feature]_. Ex.: feature/tela-cadastro
- Trabalhos pontuais serão desenvolvidos a partir da branch **develop** com o padrão de nome _[iniciais_do_desenvolvedor]/[nome_do_trabalho]_. Ex.: am/alterar-cor-botoes
- Hotfixes serão desenvolvidos a partir da branch **master** com o padrão de nome _hotfix/[nome_do_hotfix]_
- Todo commit na **develop** deverá ser realizado via **pull request**
- Novas features serão levadas da branch **develop** para a **master** via **merge**
- Commits de hotfixes na **master** serão realizados via **pull request**

## Políticas de Pull Request

- Títulos dos Pull Requests deverão estar na Língua Portugues e se possível com o tempo verbal no infinitivo. Ex.: Adicionar tela de token para primeiro acesso
- Critérios para aceite do Pull Request: ao menos um aprovador (exceto quem abriu o PR), build passando
- Ao completar um Pull Request deverá ser realizado **squash**
- Os Pull Requests devem ser analisados em menos de um dia útil

## Políticas de Code Review

- Deverão ser avaliados code standards (de acordo com .editorconfig), qualidade de código (falta de testes automatizados), melhorias de design e implementação

## Políticas de Testes Automatizados

- Todas as ViewModels e Services deverão possuir testes de unidade
- Frameworks utilizados: NUnit, FluentAssertions

## Política de Release

- Todo commit nas branches principais deverá iniciar build no AppCenter
- Toda build da branch **develop** finalizada com sucesso deverá iniciar release para o AppCenter
- Releases para o AppCenter deverão ser aprovadas por pelo menos um desenvolvedor
