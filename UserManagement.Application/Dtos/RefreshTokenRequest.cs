﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Dtos
{
    public class RefreshTokenRequest
    { 
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
