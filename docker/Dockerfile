FROM microsoft/aspnetcore-build:2.0.0-jessie

RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && \
    apt-get install nodejs -y --no-install-recommends

WORKDIR /app
EXPOSE 80

COPY ./AdsSystem.sln .
RUN dotnet restore ./AdsSystem.sln
COPY . .

WORKDIR /app/AdsSystem/public
RUN npm i -q

WORKDIR /app

RUN dotnet build ./AdsSystem.sln -o ./build

WORKDIR /app/AdsSystem

CMD dotnet run --server.urls=http://0.0.0.0:80

