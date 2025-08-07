#!/bin/bash

# Script para configurar SSL no Volatus
# Execute após configurar o DNS: sudo bash setup-ssl.sh

echo "🔒 Configurando SSL para volatus.erikneves.com.br..."

# Verificar se o domínio está resolvendo
echo "📡 Verificando DNS..."
if ! nslookup volatus.erikneves.com.br > /dev/null 2>&1; then
    echo "❌ Erro: volatus.erikneves.com.br não está resolvendo!"
    echo "Configure o DNS primeiro: volatus.erikneves.com.br -> $(curl -s ifconfig.me)"
    exit 1
fi

echo "✅ DNS configurado corretamente!"

# Configurar Nginx temporariamente para HTTP
cp nginx.conf /etc/nginx/sites-available/volatus
ln -s /etc/nginx/sites-available/volatus /etc/nginx/sites-enabled/
rm -f /etc/nginx/sites-enabled/default

# Testar configuração
nginx -t

# Reiniciar Nginx
systemctl restart nginx

# Obter certificado SSL
echo "🔐 Obtendo certificado SSL..."
certbot --nginx -d volatus.erikneves.com.br --non-interactive --agree-tos --email erik@erikneves.com.br

# Verificar renovação automática
echo "🔄 Configurando renovação automática..."
(crontab -l 2>/dev/null; echo "0 12 * * * /usr/bin/certbot renew --quiet") | crontab -

echo "✅ SSL configurado com sucesso!"
echo "🌐 Acesse: https://volatus.erikneves.com.br"
echo "📚 API Docs: https://volatus.erikneves.com.br/swagger"
