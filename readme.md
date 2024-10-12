# DotNet App Service Health Monitor

## Overview
This .NET 8 application monitors specified services and sends alerts to a Telegram chat if any services are down. Additionally, it pings a health checks URL to ensure system health.

## Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Environment Variables** (add these to an `.env` file in `/opt/service-health-monitor/.env` or your shell configuration file):
  - `BOT_TOKEN`: Token for your Telegram bot.
  - `CHAT_ID`: Telegram chat ID for receiving alerts.
  - `HEALTHCHECKS_URL`: URL endpoint for health checks.

## Monitored Services
- `your-app`
- `error-log-monitor`

## Setup

1. **Clone the Repository**:
    ```bash
    git clone <repository_url>
    cd <repository_directory>
    ```

2. **Set Environment Variables**:
    Ensure that `BOT_TOKEN`, `CHAT_ID`, and `HEALTHCHECKS_URL` are properly configured in an `.env` file or shell configuration.

3. **Create systemd Service**:
    Configure the systemd service to manage the application:
    ```bash
    sudo cp service-health-monitor.service.template /etc/systemd/system/service-health-monitor.service
    sudo systemctl daemon-reload
    sudo systemctl enable --now service-health-monitor.service
    ```

## Usage

To manage the application as a systemd service, use the following commands:
- **Start Service**: `sudo systemctl start service-health-monitor.service`
- **Stop Service**: `sudo systemctl stop service-health-monitor.service`
- **Check Service Status**: `sudo systemctl status service-health-monitor.service`

## Code Explanation

The main functionalities are:
- **CheckServices**: Verifies if specified services are active via `systemctl is-active --quiet`.
- **SendTelegramMessage**: Sends an alert message to the configured Telegram chat.
- **PingHealthchecks**: Pings the health check URL to monitor overall system health.

## Contributing
Feel free to submit issues or pull requests for improvements or bug fixes.
