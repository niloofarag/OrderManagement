# Order Management Service

A small order management REST API built with .NET 9, FastEndpoints, EF Core, and PostgreSQL.

## Why this architecture

The solution has four projects, each referencing only inward:

```
OrderManagement.Domain          (no dependencies)
OrderManagement.Application     -> Domain
OrderManagement.Infrastructure  -> Domain, Application
OrderManagement.Api             -> Domain, Application, Infrastructure
```

- **Vertical slices, not technical layers** — each project is organized by feature (`orders/`, `products/`, `users/`), and each feature by action (`Create/`, `Detail/`, `Search/`, ...). Everything needed to understand one capability (request, response, validator, endpoint) sits in one folder instead of being spread across `Controllers/`, `DTOs/`, `Validators/`.
- **Repository/Service interfaces live in Domain** (`IOrderRepository`, `IOrderService`, ...) because they describe behavior the domain cares about, independent of any framework. Their implementations live in Application/Infrastructure. Auth-related interfaces (`IAuthService`, `IJwtTokenGenerator`, `IPasswordHasher`) live entirely in Application since authentication isn't a domain rule.
- **FastEndpoints** instead of controllers — one class per action, mapping directly onto the vertical-slice folders, with built-in role-based authorization.
- **Plain repositories + services** — no CQRS, no MediatR; kept intentionally simple.
- **No `if`/`switch` for order status transitions** — [`OrderStatusExtensions.GetNextStatus()`](OrderManagement.Domain/orders/OrderStatusExtensions.cs) uses a `Dictionary<OrderStatus, OrderStatus>` lookup table instead, so the next valid state is data, not branching logic.

Other notable decisions: a `SemaphoreSlim` guard against concurrent stock overselling, order creation wrapped in a single DB transaction, soft delete via an EF Core global query filter, `AsNoTracking()` on read queries, and auto-migrate + idempotent seeding on startup.

## Running locally

**Prerequisites**: .NET 9 SDK, a reachable PostgreSQL instance.

1. Set the connection string in `OrderManagement.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "Default": "User ID=admin;Password=admin;Host=localhost;Port=5432;Database=orders;Pooling=true;"
     }
   }
   ```
2. Run the API — migrations and seed data are applied automatically on startup (safe to re-run, the seeder is a no-op once `Users` has rows):
   ```bash
   dotnet run --project OrderManagement.Api
   ```
3. Open Swagger at `http://localhost:5000/swagger`, or get a JWT for the seeded admin account:
   ```bash
   curl -X POST http://localhost:5000/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"phoneNumber":"09000000000","password":"123456"}'
   ```
   Use the returned token as `Authorization: Bearer <jwt>`. All 50 seeded regular users also use password `123456`.

## Good to know

- **Endpoints**: `POST /api/auth/login`, `POST/GET /api/orders`, `GET /api/orders/{id}`, `POST /api/orders/{id}/next-status`, `DELETE /api/orders/{id}` (Admin only), `POST /api/orders/bulk`.
- **Domain**: `User` covers both login accounts and customers (`Role = Admin/User`); `Order` has snapshotted `OrderItem.UnitPrice` (not a live product price reference).
- **Not included by design**: unit tests, logging, CQRS/MediatR, registration/refresh tokens, extra entities beyond `User`/`Product`/`Order`/`OrderItem`.

## Status

- Order/Product/User system — done
- Login auth (JWT) — done
- Seeder & migrations — done
- Layered architecture — done
- Logging (Serilog) — not implemented
- Tests — not implemented
