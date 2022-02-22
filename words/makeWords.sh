#!/usr/bin/bash

DIR=. # todo
if [ "$1" == "pt-BR" ]; then
  echo Running por Brazilian portuguese!
else
  >&2 echo "Unsupported language, send a PR!"
  exit 1
fi
cat $DIR/termo/conjugações $DIR/termo/dicio $DIR/termo/palavras $DIR/termo/verbos | awk '{print tolower($0)}' | iconv -t ASCII//TRANSLIT | sort -u | tr -d '\015' | awk 'length==5' > $DIR/ptBR-termo.txt
cat $DIR/libreoffice/pt_BR/pt_BR.dic | sed 's/\/.*//' | sed '/.*[-|\.].*/d' | awk '{print tolower($0)}' | iconv -t ASCII//TRANSLIT | sort -u | tr -d '\015' | awk 'length==5' > $DIR/ptBR-libreoffice.txt
cat ptBR-*.txt | sort -u > ptBR-all.txt
echo Done.
