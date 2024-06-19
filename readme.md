# Microserviço de Gerenciamento de Pessoas 👤

![.NET 8 Badge](https://img.shields.io/badge/.NET-8-blue?logo=.net)
![Kafka Badge](https://img.shields.io/badge/Kafka-Message%20Broker-red?logo=apache%20kafka)
![Redis Badge](https://img.shields.io/badge/Redis-In--Memory%20Datastore-red?logo=redis)
![PostgreSQL Badge](https://img.shields.io/badge/PostgreSQL-Relational%20Database-blue?logo=postgresql)

## Funcionalidades Principais 🚀

- **Processamento de Pessoas**: O microserviço processa informações de pessoas recebidas através de mensagens Kafka.
- **Cacheamento com Redis**: Utiliza o Redis para armazenar em cache informações de pessoas, otimizando o acesso e melhorando o desempenho.
- **Persistência em PostgreSQL**: As informações processadas são persistidas no PostgreSQL para garantir a durabilidade dos dados.

## Arquitetura e Tecnologias Utilizadas 🛠️

### Tecnologias Principais:

- **.NET 8**: Framework utilizado para o desenvolvimento do microserviço.
- **Kafka**: Sistema de mensageria distribuída utilizado para receber mensagens sobre informações de pessoas.
- **Redis**: Banco de dados em memória utilizado para o cache das informações de pessoas, reduzindo o tempo de acesso a dados frequentemente acessados.
- **PostgreSQL**: Banco de dados relacional utilizado para persistência das informações processadas.

### Componentes do Projeto:

- **PessoaService**: Classe responsável por processar informações de pessoas. Utiliza injeção de dependência para acessar o repositório de dados e o cache Redis.
- **RedisCache**: Classe que encapsula a lógica de acesso e manipulação do Redis para armazenamento em cache de informações de pessoas.
- **IPessoaRepository**: Interface que define métodos para acesso e persistência de informações de pessoas no banco de dados PostgreSQL.

## Funcionamento do Microserviço 🔄

1. **Recepção de Mensagens Kafka**: O microserviço ouve um tópico Kafka onde são postadas mensagens contendo informações de pessoas.
2. **Processamento e Normalização**: As informações recebidas são processadas, normalizando o CPF e realizando outras validações necessárias.
3. **Cacheamento com Redis**: O microserviço verifica se a pessoa já está em cache no Redis antes de prosseguir com operações adicionais.
4. **Persistência em PostgreSQL**: Caso necessário, as informações são persistidas no PostgreSQL utilizando operações de adição ou atualização.
5. **Logging e Tratamento de Erros**: Utiliza logging para registrar informações relevantes do processo de processamento e persistência de pessoas, além de capturar e tratar exceções.
