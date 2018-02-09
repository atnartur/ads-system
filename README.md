# Рекламная система

Технологии: .NET Core 2, ASP.NET Core, Entity Framework Core, Handlebars, Docker

## Команды

- `docker-compose up` - запуск тестовой базы данных (в качестве базы данных используется mysql)
- `dotnet ef migrations add` - создание новых миграций (нужно выполнять в папке AdsSystem)
- `docker build -t atnartur/ads-system:latest -f docker/Dockerfile .` - сборка Docker-образа

## Переменные окружения

- `DB_HOST` - хост базы данных
- `DB_USER` - пользователь базы данных
- `DB_PASS` - пароль базы данных
- `DB_NAME` - название базы данных

## Авторы

&copy; 2017-2018 [Артур Атнагулов](http://i.atnartur.ru) & [Данила Бутин](https://vk.com/id176956071)
