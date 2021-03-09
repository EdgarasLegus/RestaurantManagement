using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestaurantManagement.IdentityServer
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1001",
                    Username = "edgaras",
                    Password = "visma123",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Edgaras"),
                        new Claim(JwtClaimTypes.FamilyName, "Legus"),
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("RestaurantManagementAPI.read"),
                new ApiScope("RestaurantManagementAPI.write")
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("RestaurantManagementAPI")
                {
                    Scopes = new List<string> { "RestaurantManagementAPI.read", "RestaurantManagementAPI.write" },
                    ApiSecrets = new List<Secret> { new Secret("QwXzc3VUz2".Sha256()) }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "restaurant",
                    ClientName = "Restaurant Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("clientsecret".Sha256())},
                    AllowedScopes = { "RestaurantManagementAPI.read" }
                }
            };
        }
                 
    }
}
