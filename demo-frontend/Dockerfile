# Build Angular app
FROM node:20.10.0 AS development
WORKDIR /app
COPY . .
RUN npm install -g npm@9.6.2
RUN npm install --force
RUN npm run build

# Nginx setup
FROM nginx:alpine AS production
COPY .nginx/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=development /app/dist/demo-frontend/browser /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
