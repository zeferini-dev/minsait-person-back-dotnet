#!/bin/bash
# Atualiza a branch feature com develop e garante push seguro para manter a PR aberta

FEATURE_BRANCH=${1:-$(git rev-parse --abbrev-ref HEAD)}
BASE_BRANCH=develop

# Garante que está na branch feature
if [ "$FEATURE_BRANCH" = "$BASE_BRANCH" ]; then
  echo "Você está na branch develop. Troque para sua feature antes de rodar este script."
  exit 1
fi

echo "Atualizando $FEATURE_BRANCH com $BASE_BRANCH..."
git fetch origin

git checkout $FEATURE_BRANCH || exit 1
git pull origin $FEATURE_BRANCH || exit 1

echo "Fazendo merge de $BASE_BRANCH em $FEATURE_BRANCH..."
git merge origin/$BASE_BRANCH

if [ $? -eq 0 ]; then
  echo "Merge realizado com sucesso. Fazendo push..."
  git push origin $FEATURE_BRANCH
  echo "Pronto! Sua branch está atualizada e a PR permanecerá aberta."
else
  echo "Conflitos detectados. Resolva os conflitos, faça commit e rode o script novamente."
  exit 1
fi
