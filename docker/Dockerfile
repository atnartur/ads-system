FROM microsoft/aspnetcore-build:2.0.0-jessie

WORKDIR /app
EXPOSE 8081

COPY . .

RUN dotnet restore ./AdsSystem.sln && dotnet publish ./AdsSystem.sln -c Release -o ./build

WORKDIR /app/AdsSystem

CMD dotnet run
ENTRYPOINT dotnet run
