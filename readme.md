# DotNet Service Health Monitor

A .NET 8 application that monitors specified services, sends alerts to a Telegram chat if any services are down, and pings a health checks URL to ensure system health.

## Features
- **Service Monitoring**: Verifies if specified services are active using `systemctl`.
- **Alert Notifications**: Sends alerts to a configured Telegram chat.
- **Health Checks**: Pings a health checks URL to monitor overall system health.

## Setup

### Prerequisites
- .NET 8 SDK (only needed if building the binary yourself)
- `systemctl` for service management

### Steps

1. **Clone the Repository**:
    ```bash
    git clone <repository_url>
    cd <repository_directory>
    ```

2. **Set Environment Variables**:
    Ensure that `BOT_TOKEN`, `CHAT_ID`, `HEALTHCHECKS_URL`, and `SERVICES` are properly configured in an `.env` file or shell configuration.

3. **Build the Binary**:
    You can either build the binary yourself or download it from the releases.

    **To build the binary yourself**:
    ```bash
    dotnet publish --configuration Release --output ./publish
    sudo mkdir -p /opt/service-health-monitor
    sudo cp -r ./publish/* /opt/service-health-monitor/
    ```

    **To download the binary from the releases**:
    - Go to the [Releases](https://github.com/<your-repo>/releases) page.
    - Download the latest release zip file.
    - Unpack it to `/opt/service-health-monitor/`:
    ```bash
    sudo mkdir -p /opt/service-health-monitor
    sudo unzip path/to/downloaded/release.zip -d /opt/service-health-monitor/
    ```

4. **Create systemd Service**:
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

## Contributing
Feel free to submit issues or pull requests for improvements or bug fixes.
