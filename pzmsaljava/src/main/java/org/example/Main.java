package org.example;

import com.azure.identity.InteractiveBrowserCredentialBuilder;
import com.microsoft.aad.msal4j.InteractiveRequestParameters;
import com.microsoft.aad.msal4j.PublicClientApplication;
import com.microsoft.graph.authentication.TokenCredentialAuthProvider;
import com.microsoft.graph.requests.GraphServiceClient;

import java.io.File;
import java.net.URI;
import java.nio.file.Files;
import java.util.Collections;

public class Main {
    public static void main(String[] args) throws Exception {
        if (args.length != 2) {
            System.out.println("Usage: org.example.Main <client_id> <tenant_id>");

            return;
        }

        System.out.println("Starting MSAL demo...");

        var clientId = args[0];
        var tenantId = args[1];

        System.out.printf("client_id: %s", clientId);
        System.out.printf("tenant_id: %s", tenantId);

        var app = PublicClientApplication.builder(clientId)
                .authority("https://login.microsoftonline.com/" + tenantId)
                .build();

        var scopes = Collections.singleton("User.Read");

        var parameters = InteractiveRequestParameters.builder(URI.create("http://localhost"))
                .scopes(scopes)
                .build();

        var result = app.acquireToken(parameters).get();

        var token = result.accessToken();

        var jwt = token.substring(0, token.length() - 10);

        System.out.printf("JWT: %s", jwt);

        var browserCredential = new InteractiveBrowserCredentialBuilder()
                .clientId(clientId)
                .tenantId(tenantId)
                .redirectUrl("http://localhost")
                .build();

        var tokenAuthProvider = new TokenCredentialAuthProvider(scopes.stream().toList(), browserCredential);

        var graphClient = new GraphServiceClient.Builder()
                .authenticationProvider(tokenAuthProvider)
                .buildClient();

        var photoResponse = graphClient.me().photo().content().buildRequest().get();

        var file = new File("photo.jpg");

        Files.copy(photoResponse, file.toPath());

        System.out.println("Done.");
    }
}