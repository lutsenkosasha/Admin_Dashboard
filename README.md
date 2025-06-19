# Admin Dashboard

**Admin Dashboard** — это веб-приложение для управления клиентами, курсом токена и просмотра истории платежей.

---

## 📦 Функционал

- Авторизация по email и паролю
- Защищенная админ-панель (`/dashboard`)
- Просмотр списка клиентов
- Просмотр и редактирование текущего курса токена
- История последних платежей
- Защита маршрутов через JWT
- Статус загрузки при получении данных

---

## 🛠️ Технологии

### Backend:
- ASP.NET Core (.NET 8)
- PostgreSQL
- Entity Framework Core
- Docker
- JWT (аутентификация)
- Swagger

### Frontend:
- React + Vite
- React Router DOM
- Axios
- CSS

---

## ⚙️ Как запустить

1. Установите зависимости:
```bash
cd backend
dotnet restore

cd ../frontend
npm install
```

2. Запуск PostgreSQL и бэкенда:
```bash
cd backend
docker-compose up -d
dotnet run
```

3. Запуск фронтенда:
```bash
cd frontend
npm run dev
```

### Бекенд доступен на порту :5000, фронтенд - на порту :5173

## 🔐 Данные для авторизации
Email: admin@example.com   
Password: admin123

## 🔗 Эндпоинты (Backend)
| Метод | URL                        | Описание                                      | Требуется JWT |
|-------|----------------------------|-----------------------------------------------|--------------|
| POST  | `/api/auth/login`          | Авторизация, возвращает JWT                   | ❌ Нет       |
| GET   | `/api/clients`             | Получить список всех клиентов                 | ✅ Да        |
| GET   | `/api/clients/{id}`        | Получить клиента по ID                        | ✅ Да        |
| POST  | `/api/clients`             | Добавить нового клиента                       | ✅ Да        |
| PUT   | `/api/clients/{id}`        | Обновить данные клиента                       | ✅ Да        |
| DELETE| `/api/clients/{id}`        | Удалить клиента по ID                         | ✅ Да        |
| GET   | `/api/payments?take=N`     | Получить последние N платежей (по умолчанию 5)| ✅ Да        |
| GET   | `/api/rate`                | Получить текущий курс токена                  | ✅ Да        |
| POST  | `/api/rate`                | Обновить курс токена                          | ✅ Да        |

> Для всех защищённых маршрутов требуется передавать JWT в заголовке:
```bash
Authorization: Bearer <token>
```
📘 Также доступна Swagger-документация по адресу: [http://localhost:5000/Swagger](http://localhost:5000/Swagger)

## 📬 Примеры запросов (Postman)
1. Авторизация   
POST http://localhost:5000/api/auth/login   
Body (JSON):   
```bash
{
  "email": "admin@example.com",
  "password": "admin123"
}
```

Response (200 OK):
```bash
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI..."
}
```

2. Получить клиентов   
GET http://localhost:5000/api/clients   
Headers:   
```bash
Authorization: Bearer {token}
```

Response (200 OK):
```bash
[
    {
        "id": 2,
        "clientId": 2,
        "client": {
            "id": 2,
            "name": "Bob",
            "email": "bob@mail.com",
            "balanceT": 50
        },
        "amount": 20,
        "timestamp": "2025-06-18T22:59:01.838648Z"
    },
    {
        "id": 3,
        "clientId": 3,
        "client": {
            "id": 3,
            "name": "Charlie",
            "email": "charlie@mail.com",
            "balanceT": 75
        },
        "amount": 30,
        "timestamp": "2025-06-18T22:59:01.838648Z"
    },
    {
        "id": 1,
        "clientId": 1,
        "client": {
            "id": 1,
            "name": "Alice",
            "email": "alice@mail.com",
            "balanceT": 100
        },
        "amount": 10,
        "timestamp": "2025-06-18T22:59:01.838624Z"
    }
]
```
