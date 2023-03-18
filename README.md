# ServerlessMicroservices
This repository contains a microservices solution for managing financial transactions and customer information for a bank. The solution is built using .NET 6 and follows a serverless architecture using Azure Functions, Cosmos DB, and Service Bus Queue.

![Serverless Architecture](https://user-images.githubusercontent.com/30439829/226095814-b2f4352e-1a1d-4fc0-9806-e1695ece0ec6.png)

## Microservices
The solution consists of three microservices, each responsible for a specific aspect of the bank's operations:
- Account service: responsible for managing customer account information.
- Movements service: responsible for processing deposit and withdrawal transactions.
- Notification service: responsible for sending "email" notifications to customers regarding their transactions.
