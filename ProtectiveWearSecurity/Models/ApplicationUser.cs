﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtectiveWearSecurity.Models
{
    /// <summary>
    /// Clase encargada de tomar los registros del usuario a identificar.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Primer nombre del usuario.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Segundo nombre del usuario.
        /// </summary>
        public string LastName { get; set; }
    }
}
