# NICTS Probate Simple Queue Service PoC

![Probate SQS Diagram](ProbateSQS.jpg "Probate SQS Overview")

PoC to highlight how to integrate with Amazon SQS via GOV UK PaaS.

API communicates with Queue set up on AWS SQS (in Dev account), using the Amazon SDK. Queue is a standard type, which means it has unlimited transactions per second, is guranteed at least once delivery (which means there could be duplicates) and it has best effort ordering.

Application leverages SteelToe to pull in configuration that has been set on Cloud Foundry, specifically the AWS credentials for the user of SQS client. 

This is achieved by creating a user provided service on Cloud Foundry that is bound to this app. Credentials then become accessible to app via VCAP_SERVICES. These can be retrieved in Startup directly from Configuration object, or by injecting into services using the SteelToe Cloud Foundry Options pattern, see queue services for examples.
