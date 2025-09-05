# 🛒 Hypesoft Products Manager

Sistema completo de **gestão de produtos e categorias**, desenvolvido como desafio técnico da Hypesoft.  
Inclui **backend em .NET** com autenticação Keycloak e **frontend em React** com Tailwind + Shadcn UI.

---

## 🚀 Tecnologias

### Backend (Pasta Products_Manager)
- .NET 7 / ASP.NET Core
- MongoDB (via Docker)
- Keycloak (OAuth2/OpenID Connect)
- CQRS + MediatR + Clean Architecture
- Serilog, AutoMapper, FluentValidation

### Frontend (Pasta sgp_f)
- React 18 + Vite + TypeScript
- Tailwind CSS + Shadcn UI
- React Query / TanStack Query
- React Hook Form + Zod
- Recharts (dashboards)

---

## 📂 Estrutura do Projeto

- backend/ -> API em ASP.NET Core
- frontend/ -> Aplicação web em React/Vite
- docker/ -> Configurações de Docker Compose


---

## ⚙️ Pré-requisitos

- [Docker](https://www.docker.com/)
- [Node.js 18+](https://nodejs.org/)
- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- Git

---

## 📊 Funcionalidades

- Produtos: CRUD, busca por nome, associação a categorias

- Categorias: CRUD simples

- Estoque: controle de quantidades, alerta de baixo estoque (<10)

- Dashboard: total de produtos, valor total em estoque, produtos por categoria

- Autenticação: login via Keycloak, proteção de rotas e roles

## 📌 Critérios Atendidos

- Clean Architecture + DDD

- Integração com Keycloak

- Frontend moderno e responsivo

- Testes unitários e integração

- Docker Compose funcional

- Swagger documentado

---
Para mais informações acessar README.md da API e Front-End
