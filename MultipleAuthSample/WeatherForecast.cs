namespace MultipleAuthSample
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}


//var tokenHandler = new JwtSecurityTokenHandler();
//var tokenDescriptor = new SecurityTokenDescriptor
//{
//    Subject = new ClaimsIdentity(new[] { new Claim("sub", Guid.NewGuid().ToString()) }),
//    Claims = new Dictionary<string, object>
//            {
//                { "Roles", "Admin" },
//            },
//    Issuer = config.GetValue<string>("IdentitySeverA:Issuer"),
//    Audience = config.GetValue<string>("IdentitySeverA:Audience"),
//    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeThingThatShouldNotBeHereA"))
//    , SecurityAlgorithms.HmacSha256Signature)
//};
//var token = tokenHandler.CreateToken(tokenDescriptor);
//var tokenff = tokenHandler.WriteToken(token);