## Sumário

- [Tecnologias](#tecnologias)
- [MER Banco](#mer-banco)
- [Scripts](#scripts)
- [Comandos para Build](#comandos-para-build)

---


## Tecnologias

- **.NET 8**
- **Dapper** (Micro ORM)
- **Entity Framework Core** (ORM)
- **SQL Server** (Banco de dados relacional)
- **NUnit** (Testes unitários)
- **Moq** (Mocking para testes)

---

## MER Banco

- **Script para gerar o MER:**  
  [ScriptDiagramMER.txt](https://github.com/joaoguthix/OmnibeesTest/blob/master/ApiOmnibess/ScriptDiagramMER.txt)

- **Ferramenta utilizada:**  
  [dbdiagram.io](https://dbdiagram.io/d)

![MER Image](https://github.com/user-attachments/assets/7afc92d9-c92f-4876-8964-d0269761ebb8)

---

## Scripts

- **Script para geração do banco de dados:**  
  [ScriptDb.txt](https://github.com/joaoguthix/OmnibeesTest/blob/master/ApiOmnibess/ScriptDb.txt)

---

## Comandos para Build

Execute na raiz do projeto:

```bash
dotnet restore
dotnet build
dotnet run --project ApiOmnibees
