# Image.Resize

Welcome to the Image.Resize repository! This project is designed to provide functionality for resizing and managing images using Azure Functions and Azure Blob Storage.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Features

- Automatically resize uploaded images.
- Save resized images to Azure Blob Storage.
- Retrieve resized images using Blob Storage URLs.

## Getting Started

### Prerequisites

- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- [.NET Core SDK](https://dotnet.microsoft.com/download)
- Azure Storage Account (for Blob Storage)

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/hiralpatel-cloud-evangelist/Image.Resize.git
2. Navigate to the project directory:
   ```sh
   cd Image.Resize

# Configuration

1. Create a `local.settings.json` file in the root directory with the following content:
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "YourAzureWebJobsStorageConnection",
    "StorageContainerthumbnail": "YourStorageContainerName"
  }
}


2. Replace YourAzureWebJobsStorageConnection with your Azure Functions Storage connection string.
3. Replace YourStorageContainerName with the name of your Blob Storage container for thumbnails.

# Usage

1. Build and run the project using Azure Functions Core Tools or your preferred development environment.
2. Upload an image to the configured Blob Storage container.
3. The Azure Function will automatically resize the image upon upload and save the resized image to the Blob Storage container.
4. Retrieve the resized image using the Blob Storage URL.
