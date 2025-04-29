using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProdBase.Domain.Interfaces;
using System.Text.Json;

namespace ProdBase.Infrastructure.Auth
{
    public class FirebaseAuth : IAuthService
    {
        private readonly FirebaseAdmin.Auth.FirebaseAuth _firebaseAuth;
        private readonly ILogger<FirebaseAuth> _logger;

        public FirebaseAuth(IConfiguration configuration, ILogger<FirebaseAuth> logger)
        {
            _logger = logger;

            // Initialize Firebase Admin SDK if not already initialized
            if (FirebaseApp.DefaultInstance == null)
            {
                try
                {
                    // Check if environment variables for service account are provided
                    var projectId = configuration["Firebase:ProjectId"];
                    var privateKeyId = configuration["Firebase:PrivateKeyId"];
                    var privateKey = configuration["Firebase:PrivateKey"]?.Replace("\\n", "\n");
                    var clientEmail = configuration["Firebase:ClientEmail"];
                    var clientId = configuration["Firebase:ClientId"];
                    var authUri = configuration["Firebase:AuthUri"];
                    var tokenUri = configuration["Firebase:TokenUri"];
                    var authProviderCertUrl = configuration["Firebase:AuthProviderX509CertUrl"];
                    var clientCertUrl = configuration["Firebase:ClientX509CertUrl"];

                    if (!string.IsNullOrEmpty(projectId))
                    {
                        // Create a service account credential from configuration
                        var serviceAccount = new
                        {
                            type = "service_account",
                            project_id = projectId,
                            private_key_id = privateKeyId,
                            private_key = privateKey,
                            client_email = clientEmail,
                            client_id = clientId,
                            auth_uri = authUri,
                            token_uri = tokenUri,
                            auth_provider_x509_cert_url = authProviderCertUrl,
                            client_x509_cert_url = clientCertUrl
                        };

                        var serviceAccountJson = JsonSerializer.Serialize(serviceAccount);
                        var credential = GoogleCredential.FromJson(serviceAccountJson);

                        FirebaseApp.Create(new AppOptions
                        {
                            Credential = credential
                        });

                        _logger.LogInformation("Firebase Admin SDK initialized with service account credentials");
                    }
                    else
                    {
                        // Initialize Firebase with application default credentials
                        FirebaseApp.Create(new AppOptions
                        {
                            Credential = GoogleCredential.GetApplicationDefault()
                        });

                        _logger.LogInformation("Firebase Admin SDK initialized with application default credentials");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to initialize Firebase Admin SDK: {ex.Message}");
                }
            }

            _firebaseAuth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
        }

        public async Task<Dictionary<string, object>> VerifyIdTokenAsync(string idToken)
        {
            if (_firebaseAuth == null)
            {
                throw new InvalidOperationException("Firebase Auth client is not initialized");
            }

            try
            {
                // Verify the token
                var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken);

                // Return the token claims
                return decodedToken.Claims.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error verifying ID token: {ex.Message}", ex);
            }
        }
    }
}
