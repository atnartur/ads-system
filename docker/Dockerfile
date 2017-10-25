FROM microsoft/aspnetcore-build:2.0.0-jessie

WORKDIR /app
EXPOSE 8081

COPY . .

RUN dotnet restore ./AdsSystem.sln && \
	dotnet build ./AdsSystem.sln -o ./build # -c Release
	# dotnet publish ./AdsSystem.sln -c Release -o ./build

WORKDIR /app/AdsSystem

CMD dotnet run

