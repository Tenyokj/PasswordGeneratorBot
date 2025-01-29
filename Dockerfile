# Указываем базовый образ с .NET SDK 8.0 для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем файлы проекта в контейнер
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и собираем приложение
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Переходим к минимальному образу для запуска (используем тот же .NET 8.0)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем результаты сборки из предыдущего этапа
COPY --from=build /app/out .

# Указываем команду запуска
ENTRYPOINT ["dotnet", "TenyokjBot.dll"]
