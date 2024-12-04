# 1. For build React app
FROM node:20.10.0 AS development
WORKDIR /app
COPY . .
RUN npm install -g npm@9.6.2
RUN npm install --force
RUN npm run build

# 2. For Nginx setup
FROM nginx:alpine as production
COPY .nginx/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=development /app/build /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
