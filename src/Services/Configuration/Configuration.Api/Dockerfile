FROM streamone.azurecr.io/s1-integration-base-7:latest
ARG BuildConfiguration=Release
COPY bin/$BuildConfiguration/net7.0/. app
WORKDIR /app