# Estágio 1: Build da aplicação Angular
FROM node:18 as build
WORKDIR /usr/src/app

COPY package*.json ./
RUN npm install --legacy-peer-deps
COPY . .
RUN npm run build

# Estágio 2: Servir a aplicação com Nginx
FROM nginx:alpine
COPY --from=build /usr/src/app/dist/Volatus /usr/share/nginx/html
EXPOSE 80