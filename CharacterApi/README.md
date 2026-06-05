# Character API - Інструкція з розгортання

## Структура проєкту

```
CharacterApi/
├── CharacterLib/
│   ├── Character.cs          <- оригінальний клас
│   └── CharacterLib.csproj
├── CharacterApi/
│   ├── Program.cs            <- REST API обгортка
│   └── CharacterApi.csproj
├── Dockerfile                <- для деплою
├── .gitignore
└── README.md
```

## Endpoints

| Метод | URL | Опис |
|-------|-----|------|
| GET | /health | Health check |
| GET | /swagger | Swagger UI |
| POST | /character | Створити персонажа |
| POST | /character/take-damage | Отримати пошкодження |
| POST | /character/upgrade | Підвищити рівень |
| POST | /character/use-energy | Витратити енергію |
| POST | /character/gain-experience | Отримати досвід |
| POST | /character/profile | Отримати профіль |

---

## КРОК 1 - Перевірка локально (Windows)

### 1.1 Встановити .NET 10 SDK
Завантажити з https://dotnet.microsoft.com/download/dotnet/10.0
Вибрати "SDK 10.x.x" -> Windows x64 Installer

### 1.2 Перевірити встановлення
Відкрити PowerShell або CMD:
```
dotnet --version
```
Має показати: 10.0.xxx

### 1.3 Перейти в папку проєкту і зібрати
```
cd CharacterApi
dotnet build CharacterApi/CharacterApi.csproj
```
Результат: Build succeeded.

### 1.4 Запустити локально
```
dotnet run --project CharacterApi/CharacterApi.csproj
```
API запуститься на http://localhost:8080

### 1.5 Перевірити в браузері
- Відкрити http://localhost:8080/health -> має повернути JSON зі status: "ok"
- Відкрити http://localhost:8080/swagger -> Swagger UI з усіма endpoints

### 1.6 Перевірити через curl (PowerShell)
```powershell
# Health check
curl http://localhost:8080/health

# Створити персонажа
curl -X POST http://localhost:8080/character `
  -H "Content-Type: application/json" `
  -d '{"level":1,"health":100,"energy":50,"experience":0}'

# Нанести пошкодження
curl -X POST http://localhost:8080/character/take-damage `
  -H "Content-Type: application/json" `
  -d '{"level":1,"health":100,"energy":50,"experience":0,"damage":30}'

# Підвищити рівень (потрібно 100+ досвіду)
curl -X POST http://localhost:8080/character/upgrade `
  -H "Content-Type: application/json" `
  -d '{"level":1,"health":50,"energy":20,"experience":100}'
```

---

## КРОК 2 - Завантажити на GitHub

### 2.1 Створити репозиторій
1. Відкрити https://github.com -> New repository
2. Назвати: `character-api`
3. Зробити Public
4. НЕ додавати README (він вже є)
5. Натиснути Create repository

### 2.2 Завантажити код
В PowerShell всередині папки CharacterApi:
```
git init
git add .
git commit -m "Initial commit - Character REST API"
git branch -M main
git remote add origin https://github.com/ВАШ_ЛОГІН/character-api.git
git push -u origin main
```
Замінити ВАШ_ЛОГІН на ваш GitHub username.

---

## КРОК 3 - Деплой на Railway (безкоштовно)

Railway - найпростіша платформа для .NET, підтримує Docker.

### 3.1 Реєстрація
1. Перейти на https://railway.app
2. Натиснути "Login" -> "Login with GitHub"
3. Авторизувати Railway доступ до GitHub

### 3.2 Створити новий проєкт
1. На дашборді натиснути "+ New Project"
2. Вибрати "Deploy from GitHub repo"
3. Вибрати репозиторій `character-api`
4. Railway автоматично знайде Dockerfile і почне білд

### 3.3 Налаштувати публічний URL
1. Після успішного деплою перейти в розділ "Settings" -> "Networking"
2. Натиснути "Generate Domain"
3. Отримаєте URL вигляду: `https://character-api-production-xxxx.up.railway.app`

### 3.4 Перевірити
Відкрити в браузері:
- `https://ВАШ-URL.up.railway.app/health`
- `https://ВАШ-URL.up.railway.app/swagger`

---

## КРОК 4 - Верифікація через curl

Замінити `YOUR_URL` на ваш Railway URL:

```bash
# Health check
curl https://YOUR_URL.up.railway.app/health

# Основний endpoint - створити персонажа
curl -X POST https://YOUR_URL.up.railway.app/character \
  -H "Content-Type: application/json" \
  -d '{"level":1,"health":100,"energy":50,"experience":0}'

# Нанести пошкодження 40
curl -X POST https://YOUR_URL.up.railway.app/character/take-damage \
  -H "Content-Type: application/json" \
  -d '{"level":1,"health":100,"energy":50,"experience":0,"damage":40}'

# Персонаж з 100 досвідом - upgrade
curl -X POST https://YOUR_URL.up.railway.app/character/upgrade \
  -H "Content-Type: application/json" \
  -d '{"level":1,"health":70,"energy":30,"experience":100}'

# Витратити 20 енергії
curl -X POST https://YOUR_URL.up.railway.app/character/use-energy \
  -H "Content-Type: application/json" \
  -d '{"level":1,"health":100,"energy":50,"experience":0,"amount":20}'

# Отримати 75 досвіду
curl -X POST https://YOUR_URL.up.railway.app/character/gain-experience \
  -H "Content-Type: application/json" \
  -d '{"level":1,"health":100,"energy":50,"experience":0,"amount":75}'

# Отримати профіль
curl -X POST https://YOUR_URL.up.railway.app/character/profile \
  -H "Content-Type: application/json" \
  -d '{"level":3,"health":80,"energy":40,"experience":50}'
```

---

## КРОК 5 - Що зафіксувати для звіту

1. **URL** - публічне посилання на Railway (https://...up.railway.app)
2. **Скріншот Railway Dashboard** - розділ Deployments, де видно статус "Active"
3. **Скріншот /health** - відповідь браузера або curl з `"status": "ok"`
4. **Скріншот /swagger** - відображення Swagger UI з усіма endpoints
5. **Скріншот curl-запиту** - результат одного з POST запитів вище

---

## Очікувані відповіді API

### GET /health
```json
{
  "status": "ok",
  "service": "Character API",
  "version": "1.0.0",
  "timestamp": "2025-06-05T12:00:00Z"
}
```

### POST /character (успіх)
```json
{
  "level": 1,
  "health": 100,
  "energy": 50,
  "experience": 0,
  "isAlive": true,
  "maxHealth": 100,
  "maxEnergy": 50,
  "experiencePerLevel": 100,
  "profile": "Рівень: 1 | ХП: 100/100 | Енергія: 50/50 | Досвід: 0 | Статус: Живий"
}
```

### POST /character/upgrade (з experience >= 100)
```json
{
  "upgraded": true,
  "character": {
    "level": 2,
    "health": 100,
    "energy": 50,
    "experience": 0,
    ...
  }
}
```

### Помилка (наприклад, від'ємне пошкодження)
```json
{
  "error": "Пошкодження не може бути від'ємним. (Parameter 'damage')"
}
```
