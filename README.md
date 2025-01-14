# Fictitious Thamco  Products API container

##  Table of contents
- [Overview](#overview)
- [Features](#features)
- [Start](#start)
- [Installations](#installations)
- [Api endpoints](#Endpoints)
- [Configurations](#Configurations)
- [Deployment](#Deployment)
- [Contributions](#Contributions)
- [license](#licence)

## Overview
#### This Products API is a microservice that provides endpoints for managing product data in my application. It enables operations such as reading and adding products fetched from third party products suppliers  to the products database. This containerized API is built using .NET and deployed to azure using github actions.  The yaml file for automatic CI/CD pipeline can be found at the root of the project. 

## Features
#### Get and post operations for products, RESTful API design, Github actions for easy deployment, Configurable environment variables, Integrated with a backend database, Numerous interfaces for  other container.

## Getting Started
#### Follow the steps below to set up and run the containerized Products API locally or in a production environment.
### Prerequisites
#### .NET Runtime (for local development, optional)

## Installation
#### Clone this repository, ensure the build.yaml file is configured correctly and update to the latest version if required. Ensure all Entity framework core libraries are installed and authentication and authorization libraries are downloaded.

## API Endpoints
#### Below is a summary of available endpoints.
### Products
#### GET /api/products: Get all products.
#### GET /api/products/{id}: Get a single product by ID.
### POST /api/products: Create a new product (This is an http call to a third party)

## Deployment
#### This project has being deployed on azure, all resource groups and app service plans would be deleted once submitted

## Configuration
#### You can configure the container by using an .env file or environment variables directly. Key variables like database connection string can be found in appsettings.json.
#### Appsettings.json: Set for Production.
#### Appsettings.Development.json: set for development environment
#### PORT: Port number on which the application runs.

### Contribution
#### To contribute to this project: Contributions are not allowed as this is a university assignment
