# Build stage
FROM node:18-alpine as build-stage

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy project files
COPY . .

# Build the project
RUN npm run build

# Production stage
FROM nginx:stable-alpine as production-stage

# Copy Nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

# Copy build files to Nginx
COPY --from=build-stage /app/dist /usr/share/nginx/html

# Expose port 4389
EXPOSE 4389

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]