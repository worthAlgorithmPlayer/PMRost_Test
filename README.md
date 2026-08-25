# PMRost_Test

Перед запуском требуется добавить в appsettings.json строку подключения к базе данных, для быстрого использования -
{
"ConnectionStrings": {
"PmRostTestDatabaseMongo": "mongodb://mongo-db:27017/PMRostTest"
},
"MongoDbOptions": {
"DatabaseName": "PmRostDatabase"
}
}

Для запуска можно использовать docker-compose up --build
