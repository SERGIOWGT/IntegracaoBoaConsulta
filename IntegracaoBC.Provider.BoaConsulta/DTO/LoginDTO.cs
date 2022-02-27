﻿namespace IntegracaoBC.Provider.BoaConsulta.DTO
{
    public record LoginRequest
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string user_id { get; set; }
    }
    public record LoginResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string created_at { get; set; }
    }
}
