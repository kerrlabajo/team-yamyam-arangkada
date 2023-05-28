FROM node

WORKDIR /app

COPY package.json .

COPY config-overrides.js .

COPY tsconfig.json .

RUN npm install

COPY . .

EXPOSE 3000

CMD [ "npm", "start" ]