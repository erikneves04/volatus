# 🚀 Guia de Deploy Manual - Volatus

## 📋 Pré-requisitos
- VPS Ubuntu 20.04+ (Oracle Cloud)
- Acesso SSH root
- Domínio: `volatus.erikneves.com.br`

## 🛠️ Passo a Passo

### 1. **Configurar VPS Oracle Cloud**

```bash
# Conectar via SSH
ssh ubuntu@SEU_IP_SERVIDOR

# Executar script de configuração
wget https://raw.githubusercontent.com/erikneves04/volatus/main/setup-server.sh
chmod +x setup-server.sh
sudo bash setup-server.sh
```

### 2. **Configurar DNS**

No seu provedor de DNS (onde está registrado `erikneves.com.br`):

```
Tipo: A
Nome: volatus
Valor: SEU_IP_SERVIDOR
TTL: 300
```

### 3. **Configurar SSL**

```bash
# Aguardar propagação do DNS (5-10 minutos)
nslookup volatus.erikneves.com.br

# Configurar SSL
cd /opt/volatus
chmod +x setup-ssl.sh
sudo bash setup-ssl.sh
```

### 4. **Configurar Variáveis de Ambiente**

```bash
# Criar arquivo .env
cat > .env << EOF
POSTGRES_USER=volatus_user
POSTGRES_PASSWORD=SUA_SENHA_FORTE
POSTGRES_DB=volatus
EOF
```

### 5. **Fazer Deploy**

```bash
cd /opt/volatus
chmod +x deploy.sh
./deploy.sh
```

### 6. **Verificar Status**

```bash
# Ver containers
docker ps

# Ver logs
docker-compose -f docker-compose.prod.yaml logs -f

# Testar API
curl https://volatus.erikneves.com.br/api/swagger
```

## 🌐 URLs de Acesso

- **Frontend**: `https://volatus.erikneves.com.br`
- **API**: `https://volatus.erikneves.com.br/api`
- **Swagger**: `https://volatus.erikneves.com.br/swagger`
- **Health Check**: `https://volatus.erikneves.com.br/health`

## 🔧 Comandos Úteis

```bash
# Atualizar código
cd /opt/volatus
git pull origin main

# Parar serviços
docker-compose -f docker-compose.prod.yaml down

# Reiniciar serviços
docker-compose -f docker-compose.prod.yaml restart

# Ver logs
docker-compose -f docker-compose.prod.yaml logs -f api

# Backup do banco
docker exec volatus_db pg_dump -U volatus_user volatus > backup.sql

# Renovar SSL
sudo certbot renew

# Ver status do SSL
sudo certbot certificates
```

## 🚨 Troubleshooting

### Problema: DNS não resolve
```bash
# Verificar se o DNS está configurado
nslookup volatus.erikneves.com.br

# Verificar IP do servidor
curl -s ifconfig.me
```

### Problema: SSL não funciona
```bash
# Verificar certificado
sudo certbot certificates

# Renovar certificado
sudo certbot renew --force-renewal
```

### Problema: Containers não iniciam
```bash
# Verificar logs
docker-compose -f docker-compose.prod.yaml logs

# Verificar espaço em disco
df -h

# Verificar memória
free -h
```

### Problema: API não responde
```bash
# Verificar se API está rodando
curl https://volatus.erikneves.com.br/health

# Verificar logs da API
docker logs volatus_api
```

### Problema: Frontend não carrega
```bash
# Verificar se frontend está rodando
curl https://volatus.erikneves.com.br

# Verificar logs do frontend
docker logs volatus_web
```

## 📊 Monitoramento

```bash
# Ver uso de recursos
docker stats

# Ver logs em tempo real
docker-compose -f docker-compose.prod.yaml logs -f

# Ver status do Nginx
sudo systemctl status nginx

# Ver status do SSL
sudo certbot certificates
```

## 🔒 Segurança

- ✅ SSL/HTTPS configurado automaticamente
- ✅ Firewall configurado (portas 22, 80, 443)
- ✅ Headers de segurança configurados
- ✅ Renovação automática de SSL
- ⚠️ Altere as senhas padrão
- ⚠️ Configure backup automático
- ⚠️ Mantenha o sistema atualizado

## 📈 Performance

- **Nginx**: Proxy reverso otimizado
- **Docker**: Containers isolados
- **SSL**: HTTP/2 habilitado
- **Caching**: Headers de cache configurados
- **Compression**: Gzip habilitado

## 🔄 Atualizações

Para atualizar o projeto:

```bash
cd /opt/volatus
git pull origin main
./deploy.sh
```
