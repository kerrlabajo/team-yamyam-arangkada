FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD="YourPassword"

USER root

RUN apt-get -y update && \
    apt-get install -y dos2unix

RUN mkdir -p /usr/arangkada-db/sql

COPY . /usr/arangkada-db/sql

WORKDIR /usr/arangkada-db/sql

RUN dos2unix import-arangkada-db.sh

RUN chmod +x import-arangkada-db.sh

EXPOSE 1433

USER mssql

ENTRYPOINT /bin/bash ./arangkada-db-entrypoint.sh
