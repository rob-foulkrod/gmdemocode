# Java MSAL demo
This is a sample Java application that uses MSAL to sign in users and call Microsoft Graph.

## Prerequisites
- Java Development Kit (JDK) version 10 or later
- Maven

## Running the sample
`mvn exec:java -Dexec.mainClass=org.example.Main "-Dexec.args=<client_id> <tenant_id>"`

Make sure to replace <client_id> and <tenant_id> with the appropriate values for your application.
