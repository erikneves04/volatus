#!/bin/bash

# Script de configuração do servidor Volatus
# Execute como root: sudo bash setup-server.sh

echo "🚀 Configurando servidor Volatus..."

# Atualizar sistema
apt update && apt upgrade -y

# Instalar dependências
apt install -y curl wget git unzip software-properties-common apt-transport-https ca-certificates gnupg lsb-release

# Instalar Docker
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | tee /etc/apt/sources.list.d/docker.list > /dev/null
apt update
apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

# Instalar Docker Compose
curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose

# Instalar Nginx
apt install -y nginx

# Instalar Certbot para SSL
apt install -y certbot python3-certbot-nginx

# Configurar firewall
ufw allow 22/tcp
ufw allow 80/tcp
ufw allow 443/tcp
ufw --force enable

# Criar diretório do projeto
mkdir -p /opt/volatus
cd /opt/volatus

# Clonar repositório
echo "📥 Clonando repositório..."
git clone https://github.com/erikneves04/volatus.git .

echo "✅ Servidor configurado!"
echo "📝 Próximos passos:"
echo "1. Configure o DNS: volatus.erikneves.com.br -> $(curl -s ifconfig.me)"
echo "2. Execute: sudo bash setup-ssl.sh"
