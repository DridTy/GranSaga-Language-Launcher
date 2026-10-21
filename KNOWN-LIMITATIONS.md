# Cobertura e revisão

O recurso principal `Localization/Game/ru-RU/Game.locres` tem 3.814 registros
de texto usados em 8.728 referências de interface. Seus **3.611 registros com
russo foram traduzidos nos dois idiomas**, sem cirílico residual nesse recurso.

| Métrica | Português | English |
|---|---:|---:|
| Registros de texto alterados | 3.635 | 3.615 |
| Referências que usam esses textos | 8.338 | 8.309 |
| Registros anteriores ou revisados nesta rodada | 2.656 | 579 |
| Registros com tradução automática ainda sem revisão completa | 979 | 3.036 |

Os totais incluem poucos textos originalmente chineses ou ingleses, além dos russos.
Registros não equivalem necessariamente a frases distintas; há repetições e textos
de desenvolvimento no recurso. A cobertura é deste arquivo, não de todos os menus
ou de todo o jogo. Rascunhos automáticos precisam de revisão contextual e visual.

## Ainda fora desta tradução

Há textos de outras tabelas internas que podem continuar em russo. Exemplos já
observados são o corpo da confirmação de saída, o título do número da conta,
alguns botões de suporte/conta, tutoriais de combate e nomes de monstros.
O formato NDB interno dessas tabelas ainda não foi decodificado. Não foi alterado.

## História preservada

Esta atualização amplia a interface. Mantém exatamente a tabela de história
do PT-BR v0.4 (5.467 registros de falas) e English progression v0.2
(1.984 registros de falas). Ambas as histórias continuam parciais e incluem
frases repetidas; os nomes têm contagem separada.

## Tradução automática

Os textos novos tiveram uma base local feita com modelos do Argos Translate,
seguida de revisão dos termos principais. Modelos e bibliotecas ficam só no
ambiente de preparação, não no instalador. Não foram usadas credenciais de
tradução nem um serviço pago. Recursos do projeto:

- https://github.com/argosopentech/argos-translate
- https://github.com/argosopentech/argospm-index

Os parâmetros entre chaves, comandos de cor/tamanho, nomes de chaves, namespaces,
hashes de origem e referências do recurso foram preservados e verificados.
As outras 410 entradas de cada pacote foram extraídas e comparadas sem alteração.

## English

All 3,611 Russian text records in the main Game.locres resource have translations.
This is **not** full-game or all-menu coverage. Other internal tables remain
unresolved. English includes 3,036 machine-draft records awaiting full review;
Portuguese includes 979. Existing story translations are unchanged. Expanded
menus still require in-game visual testing.


## In-game feedback after v0.3

Confirmed in English screenshots: main-menu labels overlap; Gran Weapon names, skill descriptions and attributes, some inventory names, tutorials, quest titles/categories and skip-story summaries remain in Russian. Some automatic English wording needs contextual editing. These are tracked for the next version; the current preview does not fix them. The same screens must be checked in PT-BR.
