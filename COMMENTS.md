# Tecnologias utilizadas
- .NET 8
- Entity Framework Core
- MySQL
- Vue
- Vuetify

# Decisão da Arquitetura utilizada
_Optei por separar as decisões para cada camada da aplicação, as bibliotecas uitilizadas também estão nos arquivos._

## [Backend](COMMENTS_BACKEND.md)
## [Frontend](COMMENTS_FRONTEND.md)

# O que você melhoraria se tivesse mais tempo
- Adicionaria o tratamento de ativo/inativo para os Alunos.
- Adicionaria controle de horários para saber se aquele aluno pode ou não se matrícular na matéria específica.
- Adicionaria o controle para recuperação de senha.
- Mais personalidade ao Frontend.

# Quais requisitos obrigatórios que não foram entregues
- Acredito ter entregue todos os requisitos.

# Demonstração
[Demonstração](https://github.com/Nogs0/orbita-challenge-full-stack-web/wiki/Gerenciamento-de-Alunos)

# Instruções para rodar a aplicação

## Docker
O projeto possui a opção de rodar utilizando docker.
- Ajuste a ConnectionString no arquivo docker-compose.yml:
```bash
ConnectionStrings__DefaultConnection=Server=host.docker.internal;Port=3306;Database=turmamaisadb;Uid=root;Pwd=master; 
```
- Basta rodar o seguinte comando

``` bash
docker compose up --build
```

- As migrations já serão aplicadas ao rodar a imagem

## Tradicional

### No projeto do Backend:
- É necessário definir a string de conexão do banco de dados, no arquivo appsettings.json.
- Rodar as migrations no console do Gerenciador de Pacotes:
``` bash
    update-database
```
ou pelo cmd:
``` bash
    dotnet ef database update
```

### No projeto do Frontend:
- Vá até a raíz do projeto e rode no console:
``` bash
    npm install
```

- Após instalar as dependências:
``` bash
    npm run dev
```

# A aplicação estará disponível em:

Frontend:
- http://localhost:5173

Backend (swagger): 
- http://localhost:7047/swagger/index.html
