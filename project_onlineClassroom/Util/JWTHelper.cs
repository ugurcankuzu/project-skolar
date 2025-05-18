using System.IdentityModel.Tokens.Jwt;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Util
{
    public class JWTHelper
    {
        private readonly string _secretKey;

        public JWTHelper(IConfiguration configuration)
        {
            _secretKey = configuration["AUTH_SECRET"] ?? throw new ArgumentNullException("AUTH_SECRET not found in configuration");
        }
        //Mock Payload Object. TODO: Create a real DTO for the payload

        public string GenerateToken(User user)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            // create a token
            var payload = new Dictionary<string, object>()
            {
                { "id", user.Id },
                { "email", user.Email },
                { "firstName", user.FirstName },
                { "lastName", user.LastName },
                { "exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds() }
        }; // Token expiration time
            string token = encoder.Encode(payload, _secretKey);
            return token;
        }
        public string? DecodeToken(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider dateTimeProvider = new UtcDateTimeProvider();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtValidator validator = new JwtValidator(serializer, dateTimeProvider);
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                //decode the token
                var json = decoder.Decode(token, _secretKey, verify: true);
                return json;
            }
            catch (TokenNotYetValidException ex)
            {
                Console.WriteLine("Token not yet valid");
                return null;
            }
            catch (TokenExpiredException ex)
            {
                Console.WriteLine("Token expired");
                return null;
            }
            catch (SignatureVerificationException ex)
            {
                Console.WriteLine("Token signature verification failed");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token decoding failed");
                return null;
            }
        }

    }
}
