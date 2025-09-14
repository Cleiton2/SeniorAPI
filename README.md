# SeniorAPI

Uma API RESTful desenvolvida em **C#**, com objetivo de gerenciar operações de cadastro de pessoas. Este projeto é voltado para fins seletivos e demonstra boas práticas de arquitetura, incluindo camadas de serviço, validação, middlewares e controllers.

---

## Tecnologias Utilizadas

- **Linguagem:** C# (.NET 8)
- **Arquitetura:** API RESTful
- **Ferramenta de Teste:** Swagger
- **Estrutura do Projeto:**
SeniorAPI  
├─ Controllers  
├─ Enumerador  
├─ Helpers  
├─ Middleware  
├─ Model  
├─ Service  
├─ Validacao  
├─ appsettings.json  
├─ Program.cs  
└─ SeniorAPI.http  

---

## Como Usar

1. **Executar a API**  
 Abra o projeto no Visual Studio e execute (`F5` ou `Ctrl + F5`). O Swagger será aberto automaticamente em seu navegador.

2. **Login**  
 Para autenticar, utilize as credenciais padrão:  
 - **Usuário:** `admin`  
 - **Senha:** `ADM1`  

3. **Gerar Token**  
 - Realize o login via endpoint no Swagger.  
 - Copie o token JWT retornado.

4. **Authorize no Swagger**  
 - Clique no botão **Authorize** no Swagger.  
 - Cole o token JWT no campo e confirme.

5. **Consumir Endpoints**  
 - Após a autorização, você poderá acessar os demais endpoints da API diretamente pelo Swagger.

---

## Endpoints Principais

A API possui endpoints organizados por controller. Alguns exemplos:
- `/api/Autenticacao/login`- Realizar login, para obter token.
- `/api/Pessoa/adicionarPessoa` – Adiciona uma nova pessoa.
- `/api/Pessoa/Consultar` – Retorna todas as pessoas cadastradas.
- `/api/Pessoa/ConsultarPorCodigo/{codigo}` – Consulta pessoa pelo código.
- `/api/Pessoa/ConsultarPorCodigo/{codigo}` – Edita Pessoa já cadastrada.
- `/api/Pessoa/ConsultarPorUF/{UF}` – Consultar pessoa por UF cadastrada.
- `/api/Pessoa/Deletar/{codigo}` – Apaga cadastro de pessoa..

> Todos os endpoints exigem autorização via token JWT, exceto o endpoint de login.

---

## Testes

O projeto já inclui testes unitários na pasta `SeniorAPI.Testes`.  
Para executar os testes:

```bash
dotnet test SeniorAPI.Testes ou executar pela interface visual studio

````

## Sql Proposto(parte 2 desafio)

 - Como foram apresentadas duas tabelas, que possuem mesma estrutura(nomes de tabela), então de fato é somente necessário utilizar as colunas, com o UNION ALL, e criar uma coluna, que vai servir como indice de referência, sobre qual das tabelas o item pertence.
----
SELECT  
    COALESCE(cp.Numero, cap.Numero) AS NumeroProcesso,  
    p.Nome AS NomeFornecedor,  
    COALESCE(cp.DataVencimento, cap.DataVencimento) AS DataVencimento,  
    cp.DataPagamento AS DataPagamento,  
    CASE  
        WHEN cp.Numero IS NOT NULL THEN   
            (cp.Valor + cp.Acrescimento - cp.Desconto)  
        ELSE   
            (cap.Valor + cap.Acrescimento - cap.Desconto)  
    END AS ValorLiquido,  
    CASE  
        WHEN cp.Numero IS NOT NULL THEN 'Paga'  
        ELSE 'A Pagar'  
    END AS StatusConta  
FROM  
    ContasAPagar cap  
LEFT JOIN  
    ContasPagas cp ON cap.Numero = cp.Numero AND cap.CodigoFornecedor = cp.CodigoFornecedor  
LEFT JOIN  
    Pessoas p ON cap.CodigoFornecedor = p.Codigo  
