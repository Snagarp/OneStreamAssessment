using System;
using System.Collections.Generic;
using System.Linq;
using Identity.Security.Enums;

namespace Identity.Security.Models
{
    /// <summary>
    /// Holds user attributes that are specific to currently executing context
    /// </summary>
    public class JwtUserSetting
    {
        private IList<PermissionsEnum.Permissions> _permissions;

        /// <summary>Gets or sets the type of the account.</summary>
        /// <value>The type of the account.</value>

        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        /// <summary>Gets or sets the user key.</summary>
        /// <value>The user key.</value>
        [Obsolete("Obsolete in NTier. Use User ID.")]
        public string UserKey { get; set; }

        /// <summary>
        /// Gets the list of permission IDs from the current permissions.
        /// </summary>
        /// <returns>A list of permission IDs as integers.</returns>
        public List<int> GetPid() => Permissions.Select(p => (int)p).ToList();

        /// <summary>
        /// Sets the permissions based on the provided list of permission IDs.
        /// </summary>
        /// <param name="value">A list of permission IDs as integers.</param>
        public void SetPid(List<int> value) => Permissions = value.Select(v => (PermissionsEnum.Permissions)v).ToList();



        /// <summary>Gets or sets the user permissions.</summary>
        /// <value>The permissions.</value>
        public IList<PermissionsEnum.Permissions> Permissions
        {
            get => _permissions ??= new List<PermissionsEnum.Permissions>();
            set => _permissions = value ?? throw new ArgumentNullException(nameof(value));
        }


        
    }
}
