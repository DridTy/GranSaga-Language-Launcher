[![Português BR](assets/pt.svg)](README.pt-BR.md) [![English](assets/en.svg)](README.md)

# Gran Saga Language Launcher

Instalador comunitário de idiomas para o **cliente russo do Gran Saga no Windows x64**.
Escolha Português brasileiro, English ou restaure o pacote russo original.

## Progresso das traduções

| Idioma | Conteúdo | Estado |
|---|---|---|
| Português brasileiro v0.4 | Menus e 5.467 registros de falas; prólogo, MAQ001 e parte de MAQ002 | Parcial |
| English progression v0.2 | 258 textos distintos de interface e 1.984 registros de falas; abertura, prólogo e primeiros 140 registros de MAQ001 | Parcial; ampliação aguardando revisão no jogo |
| Russo | Pacote original compatível exato | Opção de restauração |

As contagens incluem falas repetidas. Nenhuma tradução cobre o jogo inteiro.
O inglês usa o cliente russo; não ativa o antigo serviço global.

## Baixar e instalar

[Baixar nas releases](https://github.com/DridTy/GranSaga-Language-Launcher/releases)

1. Baixe o ZIP da release e extraia **todos** os arquivos.
2. Feche o jogo e execute `GranSagaLanguages.exe`.
3. Selecione a pasta do jogo, por exemplo `C:\Games\GranSaga`.
4. Escolha o idioma e clique em **Aplicar / Apply**.
5. Abra o jogo pelo **4game**.

Use **Restaurar russo / Restore RU** para voltar ao pacote original.
É possível atualizar diretamente sobre os mods anteriores compatíveis.
Mantenha juntos o executável, o manifesto e os patches da mesma release.
Versões desconhecidas do jogo são recusadas.

O instalador confere backups e hashes, preserva a assinatura original e os
executáveis do jogo e não solicita dados de login. Tutoriais e outros recursos
não traduzidos ainda podem aparecer em russo.

## Compilar e contribuir

Execute `Build.ps1` no Windows para compilar com .NET Framework 4.x.
O código fica em `src/`; as memórias de tradução ficam em `translations/`.
Ao relatar problemas, informe a versão do jogo, o idioma e uma captura de tela.
Não inclua senhas, códigos de acesso nem registros de autenticação.

Projeto de fãs, sem vínculo oficial com Gran Saga ou 4game.
