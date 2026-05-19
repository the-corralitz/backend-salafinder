# Ingeniería Web: Proyecto Salafinder

Elaborado por:
- Samuel Corrales Salazar
- Diego Collazos Bermúdez
---

## SalaFinder API
Backend REST API para el sistema de reserva de espacios universitarios.

---

## Instalación

```bash
git clone https://github.com/the-corralitz/backend-salafinder.git
cd backend-salafinder
dotnet restore
dotnet ef database update
dotnet run
```

Scalar disponible en:
https://localhost:{puerto}/scalar/v1

---

## Roles del sistema

| Rol | Descripción |
|-----|-------------|
| Student | Reserva espacios y gestiona sus propias reservas |
| Staff | Registra no-shows de estudiantes |
| Admin | Acceso completo al sistema |

> El rol **Student** se asigna automáticamente al registrarse.
> El rol **Staff** lo asigna el Admin desde el endpoint `/usuarioperfil/rol/cambiar`.

---
