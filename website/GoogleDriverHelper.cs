using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Vanct.Dal.Entities;

namespace Vanct.Report
{
    public class GoogleDriverHelper
    {
        readonly string[] _scopes = {DriveService.Scope.DriveFile};

        private readonly Object _thisLock = new Object();

        public DriveService Service;

        public TokenResponse Token;

        public UserCredential Credential;

        public string AppName;

        public GoogleDriverHelper()
        {
            using (var db = new VanctEntities())
            {
                // get google config
                var googleConfigItems = db.OpenIdConfigs.Where(i => i.ExternalSystem.Equals("Google")).ToList();

                // set client secret object
                var clientId = googleConfigItems.Single(i => i.Name.Equals("ClientID")).Value;
                var clientSecret = googleConfigItems.Single(i => i.Name.Equals("ClientSecret")).Value;
                var secrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                };

                // set google flow-code object
                IAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = secrets,
                    Scopes = _scopes,
                });

                // set response-token object
                var accessToken = googleConfigItems.Single(i => i.Name.Equals("AccessToken")).Value;
                var refreshToken = googleConfigItems.Single(i => i.Name.Equals("RefreshToken")).Value;
                var tokenType = googleConfigItems.Single(i => i.Name.Equals("TokenType")).Value;
                Token = new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenType = tokenType,
                    ExpiresInSeconds = 3600,
                    Issued = DateTime.Now,
                    Scope = DriveService.Scope.DriveFile
                };

                // set credential object
                Credential = new UserCredential(flow, "me", Token);

                // build drive service
                AppName = googleConfigItems.Single(i => i.Name.Equals("ApplicationName")).Value;
                Service = new DriveService(new BaseClientService.Initializer
                {
                    ApplicationName = AppName,
                    HttpClientInitializer = Credential
                });
            }
        }

        /// <summary>
        /// Retrieve a list of File resources.
        /// </summary>
        /// <returns>List of File resources.</returns>
        public IList<File> GetFiles()
        {
            var empty = Service.Files.EmptyTrash();
            empty.Execute();

            var result = new List<File>();
            FilesResource.ListRequest request = Service.Files.List();

            do
            {
                try
                {

                    FileList files = request.Execute();

                    result.AddRange(files.Items);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    request.PageToken = null;
                }
            } while (!string.IsNullOrEmpty(request.PageToken));

            RefreshToken();

            return result.Where(i => !string.IsNullOrEmpty(i.OriginalFilename)).ToList();
        }

        /// <summary>
        /// Download a file
        /// Documentation: https://developers.google.com/drive/v2/reference/files/get
        /// </summary>
        /// <param name="fileResource">File resource of the file to download</param>
        /// <param name="saveTo">location of where to save the file including the file name to save it as.</param>
        /// <returns></returns>
        public Boolean DownloadFile(File fileResource, string saveTo)
        {
            if (String.IsNullOrEmpty(fileResource.DownloadUrl)) return false;

            try
            {
                var x = Service.HttpClient.GetByteArrayAsync(fileResource.DownloadUrl);
                byte[] arrBytes = x.Result;
                System.IO.File.WriteAllBytes(saveTo, arrBytes);
                RefreshToken();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Refreshes the token.
        /// </summary>
        public void RefreshToken()
        {
            lock (_thisLock)
            {
                using (var db = new VanctEntities())
                {
                    var success = Credential.RefreshTokenAsync(CancellationToken.None).Result;
                    if (!success) return;

                    Token = Credential.Token;
                    Service = new DriveService(new BaseClientService.Initializer
                    {
                        ApplicationName = AppName,
                        HttpClientInitializer = Credential
                    });

                    // update access-token to db
                    const string sql = "update [OpenIdConfig] set [Value]=N'{2}' where [Name]='{0}' and [ExternalSystem]='{1}';";
                    db.Database.ExecuteSqlCommand(string.Format(sql, "AccessToken", "Google", Token.AccessToken));
                    db.Database.ExecuteSqlCommand(string.Format(sql, "RefreshToken", "Google", Token.RefreshToken));
                    db.Database.ExecuteSqlCommand(string.Format(sql, "TokenType", "Google", Token.TokenType));
                }
            }
            
        }
    }
}