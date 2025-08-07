#!/bin/bash

# Script de deploy do Volatus
# Execute na pasta do projeto: bash deploy.sh

echo "🚀 Iniciando deploy do Volatus..."

# Atualizar código do repositório
echo "📥 Atualizando código..."
git pull origin main

# Parar containers existentes
docker-compose -f docker-compose.prod.yaml down

# Rebuild das imagens
echo "🔨 Rebuild das imagens..."
docker-compose -f docker-compose.prod.yaml build --no-cache

# Subir containers
echo "🚀 Subindo containers..."
docker-compose -f docker-compose.prod.yaml up -d

# Aguardar inicialização
echo "⏳ Aguardando inicialização dos serviços..."
sleep 30

# Verificar status
echo "📊 Status dos containers:"
docker-compose -f docker-compose.prod.yaml ps

# Verificar logs
echo "📋 Logs dos containers:"
docker-compose -f docker-compose.prod.yaml logs --tail=20

echo "✅ Deploy concluído!"
echo "🌐 Acesse: https://volatus.erikneves.com.br"
echo "📚 API Docs: https://volatus.erikneves.com.br/swagger"
