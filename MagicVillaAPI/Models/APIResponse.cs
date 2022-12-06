﻿using System.Net;

namespace MagicVillaAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucces { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public object? Result { get; set; }
    }
}
