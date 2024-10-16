using System;
using System.Collections.Generic;
using Identity.Security.Enums;

namespace Identity.Security
{
    /// <summary>
    /// Defines permissions required for method execution
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute
    {
        private readonly int[] _permissions;

        /// <summary>Gets the permissions.</summary>
        /// <value>The permissions.</value>
        public IReadOnlyList<int> Permissions => _permissions;

        /// <summary>Initializes a new instance of the <see cref="PermissionAttribute"/> class.</summary>
        /// <param name="permissions">The permissions.</param>
        public PermissionAttribute(params PermissionsEnum.Permissions[] permissions)
        {
            switch (permissions)
            {
                case null:
                    throw new ArgumentNullException(nameof(permissions));
                default:
                    _permissions = new int[permissions.Length];
                    permissions.CopyTo(_permissions, 0);
                    break;
            }
        }
    }
}
