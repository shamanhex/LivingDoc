# LivingDoc

Проект по автоматизации работы с документации. 

Функционал построения документации:
[X] Построение изображений диаграмм из текстового описания (.uml.txt => *.uml.png)
[X] Проверка орфографии Markdown файлов 
[X] Проверка синтаксиса Markdown
[X] Объединение нескольких Markdown файлов в 1
[X] Генерация HTML файлов
[ ] Генерация DOCX файлов
[ ] Генерация PDF файлов

Функционал публикации:
[ ] Публикация файлов в SharePoint Wiki
[ ] Публикация файлов в Shared Folder по заданной структуре


## Внешние зависимости

- [NodeJS](https://nodejs.org/en/download/current/)
- NodeJS: [markdownlint-cli] (https://www.npmjs.com/package/markdownlint-cli) (```npm i markdownlint-cli -g```)
- NodeJS: [markdown-spellcheck](https://www.npmjs.com/package/markdown-spellcheck) (```npm i markdown-spellcheck -g```)

## Линтинг MD файлов документации (LivingDoc.MDLinting)

Проект объединяет несколько внешних утилит и даёт вспомогательные скрипты для проверки синтаксиса Markdown, а так же контроля орфографии. 

### Пример проверки документации в проекте
``` PowerShell
PS> .\LivingDoc.MDLint.ps1 -Path ..\..\Examples\Ex01_СР\СР_Curator.md -DicPath ..\..\Examples\Dics\ru_RU -MDLintConfigFile ..\..\Examples\markdownlint.cfg.json
Found 1 files:
 - СР_Curator.md

SPELL CHECK: СР_Curator.md
    E:/src/LivingDoc/Examples/Ex01_СР/СР_Curator.md
        5 | я**: Ответственный за раздел (Curator)
       13 | Решение содержит веб-часть, отображающую информацию о ку
       14 | Веб-часть отображает только одного кура
       19 | отображения для широкого окна браузера
       22 | т отображения для узкого окна браузера
       25 | Рис 3. Параметры веб-части
       29 | ### Пакет «Curator.wsp»
       33 | ##### Веб-часть «Ответственный за раздел»
       37 | н в соответствующем параметре веб-части.
       38 | я в соответствующем параметре веб-части дополнительного списка раздел

>> 10 spelling errors found in 1 file

MARKDOWN LINTING: СР_Curator.md
PS> 
```

Система зайдёт в указанную папку (Path) и найдёт все файлы *.md. Затем, каждый файл будет проверен на соответствие 
стилистике Markdown (по правилам указанным в Examples\markdownlint.cfg.json), а так же будет проверена его орфография
(система возьмёт указанные словари из папки Examples\Dics).

### Пример посроения свох словарей

***Функционал в разработке***

## Объединение MD файлов (LivingDoc.MDMerge)

Утилита позволяет объединить несколько MD файлов в один исходя из структуры заголовков. 

Сценарий использования:

1. Есть какой-то шаблонный документ, который разрабатывается человеком. Есть так же часть файла, которая генерируется. Нужно объединить 2 файла, чтобы получить 1 целостный документ с расширенной информацией. Мы такой подход применяем к Спецификациям.

Как использовать утилиту:

1. Пример простого объединения:
``` PowerShell
PS> .\LivingDoc.MDMerge.exe .\СР_Curator_1.0.md .\СР_Curator.md .\СР_Curator.WspDetails.md
Read markdown files:
СР_Curator.md: Read lines...
СР_Curator.md: Parse markdown...
СР_Curator.WspDetails.md: Read lines...
СР_Curator.WspDetails.md: Parse markdown...
Merge files...
Write merged Markdown to СР_Curator_1.0.md
PS>
```