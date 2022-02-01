FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY ./TO-DO-ENTİTY/*.csproj ./TO-DO-ENTİTY/
COPY ./TO-DO.SERVİCE/*.csproj ./TO-DO.SERVİCE/
COPY ./TO-DO-UTEST.API/*.csproj ./TO-DO-UTEST.API/
COPY ./TO-DO.API/*.csproj ./TO-DO.API/
COPY *.sln .
RUN dotnet restore
COPY . .
RUN dotnet test ./TO-DO-UTEST.API/*.csproj
RUN dotnet publish ./TO-DO.API/*.csproj -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet" ,"/app/TO-DO.API.dll"]