Weather Api Application
This application provides access to getting the *weather forecast* from 3rd party Api - Visual Crossing (https://www.visualcrossing.com/weather-api/)
# How to get started?
At first you should **register** on the Visual Crossing platform to get a **secret Api key** - https://www.visualcrossing.com/weather-api/
Further, you should init a **secrets** in .NET project and set your secret key.
```bash
cd ./Weather\ API/
dotnet user-secrets init
dotnet user-secrets set "ExternalApi:WeatherApi:Key" "[YOUR KEY]"
```
Also you should up a docker compose.
```bash
cd Docker
docker compose up -d
```
**Now you can launch the API and use it!**
https://roadmap.sh/projects/weather-api-wrapper-service
