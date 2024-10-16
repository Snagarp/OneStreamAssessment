using System.Runtime.Serialization;

namespace Identity.Security.Enums
{
    /// <summary>
    /// Contains the <see cref="Permissions"/> enum, which defines specific permissions 
    /// used within the identity and security system.
    /// </summary>
    [DataContract]
    public class PermissionsEnum
    {
        /// <summary>
        /// Represents the available permissions within the system.
        /// </summary>
        public enum Permissions
        {
            /// <summary>
            /// Permission for performing administrative tasks in OneStream.
            /// </summary>
            [EnumMember]
            OneStreamAdmin = 1
        }
    }
}