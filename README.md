# Grok-CLI

## Overview
Grok CLI is a command-line interface (CLI) tool built with C# and .NET Core, designed to interact with the Grok API (provided by xAI) for various tasks such as sending messages, uploading files, retrieving rate limits, and managing conversations. This tool enables developers and users to leverage Grok's AI capabilities efficiently from the command line, with features like logging, decompression of API responses, and configurable themes for console output.

## Features
- **Grok Interaction**: Send messages to Grok and receive responses in real-time.
- **File Upload**: Upload files (e.g., JSON, Markdown, images) to the Grok API for processing.
- **Rate Limit Management**: Check and manage API rate limits to ensure smooth operation.
- **Logging**: Configurable logging with support for console and file output, including color themes (light and dark).
- **Decompression**: Handles compressed API responses (e.g., Brotli, gzip, deflate) transparently.
- **Extensible Commands**: Type-based command routing for easy addition of new commands via an `ICommand` interface.

## Authentication

### Step 1: Generate SSH Key Pair
Users need an SSH key pair. They can generate one using:
```bash
ssh-keygen -t rsa -b 4096 -C "user@example.com"
```

