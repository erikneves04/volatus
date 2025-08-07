# 🚁 Volatus - Sistema de Simulação de Entregas por Drone

## 📋 Visão Geral

O **Volatus** é um sistema completo de simulação de entregas por drone que implementa todas as funcionalidades avançadas solicitadas no desafio técnico. O projeto utiliza uma arquitetura moderna com backend em **.NET 9** e frontend em **Angular 19**, oferecendo uma experiência completa de gerenciamento de entregas com simulação em tempo real.

## 🏗️ Arquitetura do Sistema

### Backend (.NET 9)
- **Volatus.Api**: Controllers RESTful e configuração da aplicação
- **Volatus.Domain**: Entidades, serviços e interfaces do domínio
- **Volatus.Data**: Repositórios e contexto do Entity Framework
- **Volatus.Tests**: Testes unitários abrangentes

### Frontend (Angular 19)
- **Dashboard Interativo**: Visualização em tempo real dos drones e entregas
- **Mapa de Posicionamento**: Interface gráfica para acompanhar movimentação
- **Componentes Modulares**: Arquitetura componentizada com Material Design

### Infraestrutura
- **PostgreSQL**: Banco de dados principal
- **Docker Compose**: Orquestração completa dos serviços
- **Entity Framework**: ORM para persistência de dados

## 🚀 Funcionalidades Implementadas

### 1. **Simulação de Bateria do Drone**
- **Consumo Dinâmico**: Bateria diminui baseada na distância percorrida
- **Fórmula de Consumo**: `1% por unidade de distância`
- **Recarga Automática**: Drones retornam à base quando terminam uma entrega para carregamento
- **Taxa de Recarga**: `5% por intervalo de tempo` quando na base

### 2. **Cálculo de Tempo Total de Entrega**
- **Algoritmo de Distância**: Fórmula euclidiana `√((x₂-x₁)² + (y₂-y₁)²)`
- **Velocidade dos Drones**: `1.0 unidades por intervalo de tempo`

### 3. **Seleção Inteligente de Entregas**
- **Sistema de Prioridades**: Alta → Média → Baixa
- **Algoritmo de Alocação**: Otimização baseada em:
  - Distância do drone
  - Capacidade de carga
  - Nível de bateria
  - Prioridade da entrega

### 4. **Otimização Inteligente**
- **Algoritmo do Vizinho Mais Próximo**: Para roteamento otimizado
- **Maximização de Carga**: Combinação de pacotes por viagem
- **Eficiência de Bateria**: Seleção do drone mais adequado

## 🔌 APIs RESTful Bem Definidas

### Endpoints Principais:

#### Gestão de Entregas
```http
POST   /api/delivery                    # Criar nova entrega
GET    /api/delivery                    # Listar entregas
GET    /api/delivery/{id}               # Obter entrega específica
PUT    /api/delivery/{id}               # Atualizar entrega
DELETE /api/delivery/{id}               # Excluir entrega
POST   /api/delivery/assign-drone       # Atribuir drone à entrega
```

#### Gestão de Drones
```http
GET    /api/drone                      # Listar drones
GET    /api/drone/{id}                 # Obter drone específico
POST   /api/drone                      # Criar novo drone
PUT    /api/drone/{id}                 # Atualizar drone
```

#### Sistema de Trabalho (Worker)
```http
POST   /api/worker/process-deliveries  # Processar sistema de entregas
POST   /api/worker/allocate-deliveries # Alocar entregas
POST   /api/worker/update-drone-positions # Atualizar posições
POST   /api/worker/calculate-route     # Calcular rota otimizada
```

#### Dashboard e Métricas
```http
GET    /api/dashboard/metrics          # Métricas gerais
GET    /api/dashboard/drones/status    # Status dos drones
GET    /api/dashboard/deliveries/recent # Entregas recentes
GET    /api/dashboard/events/recent    # Eventos recentes
```

## ⚙️ Configurações do Sistema

### **Velocidade e Movimento**
- **Velocidade Padrão dos Drones**: `1.0 unidades por intervalo`
- **Taxa de Consumo de Bateria**: `1% por unidade de distância`
- **Taxa de Recarga**: `5% por intervalo na base`
- **Bateria Mínima para Operação**: `30%`
- **Bateria Máxima**: `100%`

### **Coordenadas e Posicionamento**
- **Base Central**: `(0, 0)` - Ponto de partida e retorno
- **Formato de Endereço**: `"(x, y)"` - Coordenadas cartesianas
- **Precisão de Chegada**: `0.1 unidades` - Tolerância para considerar chegada

### **Capacidades dos Drones**
- **Peso Máximo**: Configurável por drone (kg)
- **Capacidade de Bateria**: Configurável por drone
- **Posição Inicial**: Sempre na base `(0, 0)`

## 🤖 Comportamento dos Drones

### **Estados de Operação**
1. **Disponível**: Drone na base, pronto para missão
2. **Em Uso**: Executando entrega
3. **Retornando à base**: Voltando após entrega
4. **Manutenção**: Indisponível para operação
5. **Offline**: Drone desativado

### **Lógica de Movimento**
```csharp
// Algoritmo de movimento implementado
if (distanceToTarget <= drone.Speed) {
    // Chegou ao destino
    drone.CurrentX = drone.TargetX;
    drone.CurrentY = drone.TargetY;
} else {
    // Move em direção ao alvo
    var directionX = (drone.TargetX - drone.CurrentX) / distanceToTarget;
    var directionY = (drone.TargetY - drone.CurrentY) / distanceToTarget;
    
    drone.CurrentX += directionX * drone.Speed;
    drone.CurrentY += directionY * drone.Speed;
}
```

### **Sistema de Recarga**
- **Detecção Automática**: Quando presente na base
- **Retorno à Base**: Drone volta automaticamente
- **Recarga Progressiva**: 5% por intervalo
- **Retorno ao Serviço**: Sob demanda

## 📊 Dashboard e Métricas

### **Métricas Principais**
- **Total de Entregas**: Contador geral
- **Entregas Concluídas**: Sucessos
- **Tempo Médio por Entrega**: Eficiência
- **Taxa de Sucesso**: Percentual de sucesso

### **Visualizações em Tempo Real**
- **Mapa de Posicionamento**: Localização atual dos drones
- **Status dos Drones**: Bateria, posição, estado
- **Timeline de Atividades**: Histórico de eventos
- **Gráficos de Performance**: Métricas visuais

## 🧪 Testes Automatizados

### **Cobertura de Testes**
- **Testes de Entidades**: Validação de modelos
- **Testes de Serviços**: Lógica de negócio
- **Testes de Algoritmos**: Cálculos de distância e roteamento

### **Principais Cenários Testados**
- Cálculo de distâncias com coordenadas válidas e inválidas
- Algoritmo do vizinho mais próximo
- Validação de bateria para entregas
- Processamento de sistema de entregas
- Alocação de drones

## 🚀 Como Executar o Projeto

### **Pré-requisitos**
- Docker e Docker Compose
- .NET 9 SDK (para desenvolvimento local)
- Node.js 18+ (para desenvolvimento frontend)

### **Execução com Docker (Recomendado)**

1. **Clone o repositório**
```bash
git clone https://github.com/erikneves04/volatus
cd volatus
```

2. **Execute com Docker Compose**
```bash
docker-compose up --build
```

3. **Acesse as aplicações**
- **Frontend**: http://localhost:8080
- **Backend API**: http://localhost:8081
- **Banco de Dados**: localhost:5432

### **Execução Local (Desenvolvimento)**

#### **Backend**
```bash
cd backend
dotnet restore
dotnet build
dotnet run --project Volatus.Api
```

#### **Frontend**
```bash
cd frontend
npm install
ng serve
```

## 📁 Estrutura do Projeto

```
volatus/
├── backend/
│   ├── Volatus.Api/           # Controllers e configuração
│   ├── Volatus.Domain/        # Entidades e serviços
│   ├── Volatus.Data/          # Repositórios
│   ├── Volatus.Tests/         # Testes unitários
│   └── Dockerfile
├── frontend/
│   ├── src/app/
│   │   ├── components/        # Componentes Angular
│   │   ├── pages/             # Páginas da aplicação
│   │   ├── services/          # Serviços HTTP
│   │   └── models/            # Interfaces TypeScript
│   └── Dockerfile
├── docker-compose.yaml        # Orquestração dos serviços
└── README.md
```

## 🔧 Configurações Avançadas

### **Variáveis de Ambiente**
```env
# Banco de Dados
POSTGRES_USER=Volatus_user
POSTGRES_PASSWORD=Volatus_pwd
POSTGRES_DB=Volatus

# API
ASPNETCORE_URLS=http://+:8080
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Host=volatus_db;Port=5432;Database=Volatus;Username=Volatus_user;Password=Volatus_pwd
```

### **Portas Utilizadas**
- **Frontend**: 8080
- **Backend API**: 8081
- **PostgreSQL**: 5432

## 🎨 Diferenciais Implementados

### **1. Tratamento de Erros Robusto**
- Validação de entrada de dados
- Mensagens de erro claras e informativas

### **2. Dashboard Completo**
- Visualização em tempo real
- Métricas de performance
- Mapa interativo de posicionamento
- Timeline de atividades

### **3. Funcionalidades Criativas**
- **Recarga Automática**: Drones voltam à base quando bateria baixa
- **Feedback em Tempo Real**: Status atualizado constantemente
- **Otimização Inteligente**: Algoritmos de roteamento

### **4. Simulação Orientada a Eventos**
- Estados bem definidos para drones
- Transições automáticas entre estados
- Logging detalhado de eventos

## 📈 Performance e Escalabilidade

### **Otimizações Implementadas**
- **Algoritmo do Vizinho Mais Próximo**: Para roteamento eficiente
- **Alocação Inteligente**: Seleção otimizada de drones
- **Processamento Assíncrono**: Operações não-bloqueantes

### **Monitoramento**
- Logs detalhados de operações
- Métricas de performance
- Dashboard de monitoramento em tempo real

## 🛠️ Ferramentas de Desenvolvimento

### **Cursor AI - Aceleração de Desenvolvimento**

Este projeto foi desenvolvido com o auxílio do **Cursor AI**, uma ferramenta de desenvolvimento inteligente que acelerou significativamente o processo de criação e correção de código. O Cursor foi utilizado para:

#### **Aceleração de Desenvolvimento**
- **Geração de Código Inteligente**: Criação rápida de controllers, serviços e componentes
- **Refatoração Automática**: Melhoria contínua da estrutura do código
- **Debugging Inteligente**: Identificação e correção rápida de bugs

#### **Correções e Melhorias**
- **Análise de Código**: Detecção de problemas de performance e segurança
- **Otimização de Algoritmos**: Melhoria dos algoritmos de roteamento e alocação
- **Testes Automatizados**: Geração de testes unitários abrangentes

#### **Benefícios Alcançados**
- **Redução no tempo de desenvolvimento**
- **Implementação mais rápida de funcionalidades complexas**

O uso do Cursor AI é uma ferramenta promissora para acelerar o desenvolvimento de código, mas é importante guiar e validar os resultados gerados.

## 📄 Licença

Este projeto foi desenvolvido como parte de um desafio técnico e está disponível para fins educacionais e de demonstração.

---

**Desenvolvido usando .NET 9, Angular 19, Docker e Cursor AI**