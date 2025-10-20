FROM node:22-alpine AS build-env
WORKDIR /app

# Copia os arquivos de dependências e instala
COPY package*.json ./
RUN npm install

COPY . .

# Roda o script de build do Vue, que gera os arquivos na pasta /dist
RUN npm run build

FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Remove o conteúdo padrão do Nginx
RUN rm -rf ./*

# Copia os arquivos compilados do estágio de build para a pasta pública do Nginx
COPY --from=build-env /app/dist .
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Expõe a porta 80
EXPOSE 80

# Inicia o Nginx em modo 'daemon off' para manter o container rodando
CMD ["nginx", "-g", "daemon off;"]