# NotificationService Project

## Overview

**NotificationService** is an email delivery service that integrates with **AWS SES** (Simple Email Service) to send messages to users. It handles the CRUD operations for email message templates and processes messages from a **RabbitMQ** queue to send emails efficiently.

Key features include:

- **Email Template Management**: CRUD operations on email templates, accessible only to **SuperAdmin** and **Admin** roles.
- **RabbitMQ Queue**: Processes incoming email messages and sends them via **AWS SES**.
- **Configurable Delays**: Fine-grained control over message send delay to avoid throttling issues.
- **Concurrency Management**: Adjustable number of worker threads for sending emails, ensuring optimal performance based on **AWS SES** rate limits.

---

## Features

### Email Template Management

- **CRUD Operations**: SuperAdmins and Admins can create, read, update, and delete email templates used in messages.
- **Template Format**: Supports dynamic content within templates, enabling personalized email content for users.

### Email Sending

- **Message Queue**: Messages are published to a RabbitMQ queue. **NotificationService** picks them up, processes them, and sends the emails via **AWS SES**.
- **Customizable Delays**: A delay between email sends can be configured to avoid hitting AWS SES sending limits.
- **Concurrency Control**: Number of email worker threads is configurable. Thread count must be carefully managed based on the mode of **AWS SES** (production or sandbox), as there are different limits on message sending rates.

---

## Security

### Environment Variables

Before running **NotificationService**, ensure the following environment variables are set:

- `JWT_KEY`: Secret key for JWT token verification (if needed for API access).
- `RABBITMQ_USER`: RabbitMQ username for connecting to the message queue.
- `RABBITMQ_PASS`: RabbitMQ password for connecting to the message queue.
- `AWS_ACCESS_KEY_ID`: AWS access key for accessing **AWS SES**.
- `AWS_SECRET_ACCESS_KEY`: AWS secret key for accessing **AWS SES**.

---

## Modes of Operation

### Development Mode

- Uses a self-signed SSL certificate.
- Suitable for local development and testing.

### Production Mode

- Operates over HTTP, with traffic securely proxied by a reverse proxy (such as **NGINX** or **Traefik**).
- Ensure proper AWS SES configuration and rate limits are adhered to.

---

## Notifications

Notifications are managed through the **NotificationService**, which processes messages from the RabbitMQ queue and sends them via **AWS SES**.

> **Important**: Ensure that **NotificationService** is connected to the same **RabbitMQ** instance used by other services to enable smooth message delivery.

---

## Technology Stack

- **Email Sending**: AWS SES (Simple Email Service)
- **Messaging**: RabbitMQ
- **Concurrency**: Configurable worker threads for email processing
- **SSL**: Self-signed SSL for development, HTTP for production
- **Environment Management**: Docker for environment consistency

---

## IdentityCore Project Deployment with Docker Compose

### Overview

This document outlines the deployment of the **IdentityCore** project using Docker Compose, specifically configured for
local development with **Traefik** as a reverse proxy.

### Docker Compose Configuration

Below is the Docker Compose configuration for the **NotificationService** service:

```yaml
version: '3.8'

services:
   notification_service:
      image: isukaza/notification_service:latest  # Use this image for testing
      container_name: notification_service
      networks:
         - soundify
      restart: unless-stopped
      environment:
         ASPNETCORE_ENVIRONMENT: "Production"
         ASPNETCORE_URLS: "http://+:80"
         JWT_KEY: ${JWT_KEY}
         RABBITMQ_USER: ${RABBITMQ_USER}
         RABBITMQ_PASS: ${RABBITMQ_PASS}
         AWS_ACCESS_KEY_ID: ${AWS_ACCESS_KEY_ID}
         AWS_SECRET_ACCESS_KEY: ${AWS_SECRET_ACCESS_KEY}
      volumes:
         - "{local-path-to-settings}:/app/appsettings.json"  # Replace with actual path
      labels:
         traefik.enable: "true"
         traefik.http.routers.notification_service.rule: "Host(`localhost`) && PathPrefix(`/ntf`)"
         traefik.http.routers.notification_service.entrypoints: "websecure"
         traefik.http.routers.notification_service.tls.certresolver: "myresolver"
         traefik.http.services.notification_service.loadbalancer.server.port: "80"
         traefik.http.middlewares.notification_service-strip.stripprefix.prefixes: "/ntf"
         traefik.http.routers.notification_service.middlewares: "notification-strip"

networks:
  soundify:
    driver: bridge
```

### Building the Docker Image

To build the Docker image for **NotificationService**, use the following command:

```bash
  docker build -t notification_service:latest -f NotificationService/Dockerfile .     
```

---

## Deployment and Environment Setup

### Docker-Oriented Design

**NotificationService** is designed to operate seamlessly within a Docker environment. It supports both **Development** and **Production** configurations.

1. **Development**:
    - Operates with a self-signed SSL certificate for local testing and development.

2. **Production**:
    - Operates over HTTP with secure, proxied traffic (e.g., via NGINX or Traefik).
    - Ensure that AWS SES is set to production mode for email sending.

### Prerequisites

Before starting, ensure the following dependencies are installed and configured:

1. **RabbitMQ**: For message queueing.
2. **AWS SES**: For sending emails.

### Configuration

For **Production** mode, define the following environment variables in your `.env` file:

- `JWT_KEY`: Secret key used to sign JWT tokens.
- `RABBITMQ_USER`: RabbitMQ username.
- `RABBITMQ_PASS`: RabbitMQ password.
- `AWS_ACCESS_KEY_ID`: AWS access key for accessing SES.
- `AWS_SECRET_ACCESS_KEY`: AWS secret key for accessing SES.

**Important Note**: Ensure that all special characters in the values are URL encoded to prevent configuration errors.

For **Development** mode, you can use the default `appsettings.Development.json` file. In **Production** mode, it's recommended to mount the `appsettings.json` file as a volume to easily manage and configure the settings of the container without modifying the Docker image directly.

---

### Running the Service

1. Start the service in Docker using the appropriate configuration for your environment (Development or Production).
2. Verify connectivity with:
    - RabbitMQ for message queue operations.
    - AWS SES for email sending.
3. Test the key workflows:
    - Email template creation and management.
    - Email sending via RabbitMQ messages.
    - Email send delays and concurrency handling.

---

## Swagger Documentation in Development Mode

In **Development mode**, **Swagger** is available for easier API exploration and testing. It provides:

- Examples of expected request and response data.
- XML documentation for each **endpoint** of the service.
- An interactive interface for sending API requests directly.

You can access the Swagger documentation at the following URL:

[https://localhost:9433/api/index.html](https://localhost:9433/api/index.html)

---

## Future Enhancements

For information on the project's future plans and enhancements, please refer to the [Roadmap](https://github.com/Isukaza/NotificationService/blob/develop/ROADMAP.md).

---

**NotificationService** is continuously evolving to meet modern email delivery and management needs. Planned enhancements include improved concurrency control, additional message formats, and better integration with other messaging services.
