# PMRost_Test

Перед запуском требуется добавить в appsettings.json строку подключения к базе данных, для быстрого использования -

```json
{
  "ConnectionStrings": {
    "PmRostTestDatabaseMongo": "mongodb://mongo-db:27017/PMRostTest"
  },
  "MongoDbOptions": {
    "DatabaseName": "PmRostDatabase"
  }
}
```

Для запуска можно использовать: 
```bash
docker-compose up --build
```
