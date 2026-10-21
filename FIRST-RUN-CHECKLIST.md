# Primeira instalação / First installation

Este aplicativo instala **o mod de idioma**, não o jogo.

## Antes do mod

1. Instale o cliente **Gran Saga RU** pelo **4game**.
2. Deixe o 4game concluir as atualizações.
3. Abra o jogo uma vez até a tela inicial para baixar os dados iniciais.
4. Feche o jogo.
5. Extraia o ZIP completo do mod em uma pasta. Não execute somente o EXE de dentro do ZIP.

O instalador precisa de Windows x64 e **.NET Framework 4.8 ou superior**.
Esse componente costuma estar disponível em Windows 10/11 atualizados.
Não é necessário Python, compilador, modelo de tradução nem ferramenta de PAK.
Se houver dúvida sobre o .NET, use `Iniciar-Launcher.cmd`; ele verifica o requisito
e informa o endereço oficial da Microsoft sem instalar nada automaticamente.

## Aplicar

1. Execute `GranSagaLanguages.exe`.
2. Selecione a pasta que o 4game usa, contendo `GranSaga.exe` e `ProjectR`.
3. Clique em **Verificar / Check**.
4. Escolha o idioma e clique em **Aplicar / Apply**.
5. Aguarde a confirmação e abra pelo **4game**.

Não é necessário selecionar outro idioma dentro do jogo ou copiar arquivos extras.
O mod usa os recursos da região russa. Não mude a região/cultura para `en-US`
ou `pt-BR` para tentar ativá-lo.

O arquivo alterado é `ProjectR\PersistentDownloadDir\ndb\ProjectR-WindowsNoEditor.pak`.
A assinatura `.sig`, executáveis, vozes e configurações do cliente são preservados.
O backup fica em `GranSagaLanguageBackup`, ao lado do pacote.

## Mensagens comuns

- **Dados de localização ausentes:** abra o cliente RU pelo 4game, complete os downloads e feche antes de aplicar.
- **Versão desconhecida:** o jogo mudou. Não force a instalação nem edite hashes; será preciso um patch compatível.
- **Assinatura ausente/incompatível:** verifique os arquivos pelo 4game. Não substitua o `.sig` pelo mod.
- **Sem permissão de escrita:** reabra o instalador como administrador se a pasta do jogo exigir.
- **Patch/manifesto ausente:** extraia o ZIP inteiro, mantendo `patches/`, `manifest.json` e o EXE juntos.
- **Algum texto ainda russo:** há textos em tabelas internas separadas, descritas em `KNOWN-LIMITATIONS.md`.

Reserve espaço para o pacote temporário, backup e versões anteriores. Reaplicar
após uma verificação do 4game pode ser necessário, desde que a versão seja compatível.

## O que foi confirmado

A instalação numa pasta de teste sem INIs e sem traduções soltas foi verificada,
incluindo backup, troca e restauração. As cenas e menus das versões anteriores
foram confirmados por capturas do usuário. A revisão visual destes menus ampliados
e a abertura em um perfil de jogo totalmente novo ainda precisam de teste no jogo.

## English

Install and update **Gran Saga RU through 4game first**, start it once up to the
title screen, then close it. Extract the full mod ZIP, run `GranSagaLanguages.exe`,
select the game folder, check compatibility, choose a language and apply.
Start the game through 4game. No in-game language switch or manual INI edits are needed.
Requires .NET Framework 4.8+. Python and translation models are not required.
Unsupported builds and invalid original signatures are rejected.
