FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt install tzdata
ENV TZ=Europe/Moscow
ENV LANG=ru_RU.UTF-8
ENV LC_ALL=ru_RU.UTF-8

ARG PUB_FOLDER=publish

WORKDIR /var/services/app
COPY ${PUB_FOLDER} .

EXPOSE 5050

ENTRYPOINT ["dotnet", "ChatApp.dll"]
