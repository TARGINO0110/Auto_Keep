# Auto Keep
Projeto destinado ao gerenciamento de um estacionamento com pagamento em dinheiro automático

![image_auto_keep](https://github.com/TARGINO0110/Auto_Keep/assets/40901408/2c1080fd-0f19-4401-9635-bad0e1b0e8f7)

## Etapas para seguir:

Prepare a instalação do seu docker local via CLI ou Windows:

- CLI
[Install docker Debian](https://docs.docker.com/engine/install/debian/)

- WINDOWS
[Docker-Desktop](https://codesandbox.io/docs/learn/introduction/overview](https://docs.docker.com/desktop/install/windows-install/)https://docs.docker.com/desktop/install/windows-install/)

Vamos agora baixar imagem do banco de dados PostgresSQL
```bash
docker pull postgres
```
Após Baixar o PostgresSQL Acesse a Pasta do repositório `DataBase`

![explorer_QnBoV9VPqj](https://github.com/TARGINO0110/Auto_Keep/assets/40901408/90af9f3c-77a6-401d-88a6-acb84cf75fce)

- Visualizando o arquivo docker-compose.yml será nessa estrutura a montagem do .yml:

``````
version: '3.1'

services:

  db:
    image: postgres
    restart: always
    container_name: postgres_sql
    environment:
      POSTGRES_PASSWORD: "8617f!!6"
      POSTGRES_DB: "auto_keep"
    ports:
      - "5432:5432"

    volumes:
      - /var/lib/docker/postgresql/data
      
  adminer:
    image: adminer
    restart: always
    ports:
      - 8080:8080
    depends_on:
      - db
``````

- Nessa estrutura compose você ira montar as imagens e os containers (adminier - Gerenciador Postgres e Banco de dados PostgresSQL)
- Alterar o caminho do volume de acordo com seu modelo docker instalado
- Ex: CLI => `-/var/lib/docker/postgresql/data` ou Docker-Desktop com WSL => `- <caminho do repositorio até pasta database>:/var/lib/docker/postgresql/data`
- Após salvar as alterações do volume execute o seguinte comando para subir o docker:
  
  ```bash
  docker-compose -f docker-compose.yml up
  ````

- Verifique que agora está alocado a porta `8080` para adminer e a porta `5432` para o postgresSQL

# Abra a solução com Visual Studio

- Acesse o arquivo `appsettings.Development.json` e altere a ConnectionStrings para seguinte forma:
  
  ```json
  "DefaultConnection": "Host=localhost;Port=5432;Database=auto_keep;Username=postgres;Password=8617f!!6;Pooling=false;"
  ```
- Verifique que na pasta `Migrations` já possui as migrações criadas para atualizar na base de dados `auto_keep`, execulte o seguinte comando do EF:
  
  ```
  update-database
  ```

- Feito esse processo verifique que agora as tabelas estarão presentes na base auto_keep:
  `EstoqueMonetario`
  `HistoricoVeiculos`
  `Precos`
  `TiposVeiculos`

# Regras dos modelos e Autenticação JWT
- Nessa aplicação foi definido 2 tipos de Credênciais `Administrador` e `Cliente`, de acordo com a regra da aplicação será permitido o uso de alguns endpoints para 
  Cliente e geral permitido para Administrador Ex:
  
- Administrador acessa todos endpoints da API
- Cliente acessa os endpoints `HistoricoVeiculos`:
  
- `[get]`
  - /api/v1/HistoricoVeiculos/ListarHistoricosVeiculosGeral
  - /api/v1/HistoricoVeiculos/ListarHistoricosVeiculosPlacas/{placaVeiculo}
  - /api/v1/HistoricoVeiculos/FiltrarRegistroId/{id_HistVeiculo}
  - /api/v1/HistoricoVeiculos/ObterValorEstadiaAtual/{id_HistVeiculo}
- `[post]`
  - /api/v1/HistoricoVeiculos/RegistrarHistoricoVeiculo
  
  ```json
  {
    "placaVeiculo": "string",
    "id_TiposVeiculos": 0
  }
  ```
- `[put]`
  - /api/v1/HistoricoVeiculos/AtualizarSaidaVeiculo/{id_HistoricoVeiculo}
    
  ```json
  {
    "placaVeiculo": "string",
    "notas_Moedas": [
      0
    ],
    "id_TiposVeiculos": 0
  }
  ```
- Cliente acessa os endpoints `Precos`:
    
- `[get]`
  - /api/v1/Precos/ListarPrecos
  - /api/v1/Precos/ListarPrecosTipoVeiculos/{id_Veiculos}
  - /api/v1/Precos/FiltrarPrecoPorId/{id_Preco}
 
- Cliente acessa os endpoints `TiposVeiculos`:

- `[get]`
  - /api/v1/TiposVeiculos/ListarTipos
  - /api/v1/TiposVeiculos/FiltrarTiposVeiculosPorId/{id_TipoVeiculo}

- As credênciais de autenticação no `JWT` se encontra na pasta `Services/ServicesAuthorization/CredentialsRepository.cs`:

- `[post]`
  ```json
  {
    "id": 0,
    "user": "string",
    "password": "string",
    "role": "string"
  }
  ```
  - Cada token tem um tempo de expiração de 30 min

**Vamos a prática agora!** 🎉
