FROM microsoft/aspnetcore-build:2.0.0-jessie

WORKDIR /app
EXPOSE 8081

COPY ./AdsSystem.sln .
RUN dotnet restore ./AdsSystem.sln
COPY . .
RUN dotnet build ./AdsSystem.sln -o ./build

WORKDIR /app/AdsSystem

CMD dotnet run

