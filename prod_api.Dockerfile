FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app 

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ./API/API.csproj ./API/API.csproj
COPY ./Application/Application.csproj ./Application/Application.csproj
COPY ./Domain/Domain.csproj ./Domain/Domain.csproj
COPY ./Persistence/Persistence.csproj ./Persistence/Persistence.csproj 

#install dependencies
RUN dotnet restore 

# copy everything else and build app
COPY ./API/. ./API/.
COPY ./Application/. ./Application/.
COPY ./Domain/. ./Domain/.
COPY ./Persistence/. ./Persistence/. 

WORKDIR /app/API
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app 

COPY --from=build /app/API/out ./
ENTRYPOINT ["dotnet", "API.dll"]