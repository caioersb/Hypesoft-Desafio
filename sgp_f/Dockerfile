# FROM node:alpine as build
FROM node:20.11-alpine as build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . /app

RUN npm run build


FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]