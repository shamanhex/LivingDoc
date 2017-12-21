# LivingDoc

Проект по автоматизации работы с документации. 

Функционал:
[X] Проверка синтаксиса Markdown
[X] Проверка орфографии MD файлов 
[ ] Построение изображений диаграмм из текстового описания (.uml.txt => *.uml.png)
[ ] 



## Линтинг MD файлов документации (LivingDoc.MDLinting)

Проект объединяет несколько внешних утилит и даёт вспомогательные скрипты для проверки синтаксиса Markdown, а так же контроля орфографии. 
 
Внешние зависимости:

	- [NodeJS](https://nodejs.org/en/download/current/)
	- NodeJS: [markdownlint-cli] (https://www.npmjs.com/package/markdownlint-cli) (```npm i markdownlint-cli -g```)
	- NodeJS: [markdown-spellcheck](https://www.npmjs.com/package/markdown-spellcheck) (```npm i markdown-spellcheck -g```)

### Пример проверки документации в проекте
``` PowerShell
.\LivingDoc.MDLint -Path .\Examples\Ex01_DocumentsLinting -Dics .\Examples\Dics -MDLintConf .\Examples\markdownlint.cfg.json


```

Система зайдёт в указанную папку (Path) и найдёт все файлы *.md. Затем, каждый файл будет проверен на соответствие 
стилистике Markdown (по правилам указанным в .\Examples\markdownlint.cfg.json), а так же будет проверена его орфография
(система возьмёт все словари из папки .\Examples\Dics).



### Пример посроения свох словарей



## Объединение MD файлов (LivingDoc.MDMerge)

Утилита позволяет объединить несколько MD файлов в один исходя из структуры заголовков. 

Сценарий использования:

1. Есть какой-то шаблонный документ, который разрабатывается человеком. Есть так же часть файла, которая генерируется. Нужно объединить 2 файла, чтобы получить 1 целостный документ с расширенной информацией. Мы такой подход применяем к Спецификациям.

Как использовать утилиту:
1. Пример простого объединения:
``` PowerShell
	>.\LivingDoc.MDMerge.exe result.md text1.md text2.md
```