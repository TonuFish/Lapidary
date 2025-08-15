namespace Lapidary.GemBuilder.Definitions;

/// <summary>
/// Flags for GciLoginEx
/// </summary>
/// <remarks>
/// gci.ht
/// </remarks>
[Flags]
public enum LoginFlags : uint
{
    /// <summary>
    /// The gemstonePassword is encrypted.
    /// </summary>
    GCI_LOGIN_PW_ENCRYPTED = 1,

    /// <summary>
    /// Session created by this login
    /// is a child of the current session and will be terminated if the
    /// current session terminates. Intended for GciInterface .
    /// ignored in linked session login
    /// </summary>
    GCI_LOGIN_IS_SUBORDINATE = 2,

    /// <summary>
    /// support for full compression
    /// </summary>
    GCI_LOGIN_FULL_COMPRESSION_ENABLED = 4,

    /// <summary>
    /// oop fields of GciErrSType
    /// added to ReferencedSet instead of PureExportSet
    /// </summary>
    GCI_LOGIN_ERRS_USE_REF_SET = 8,

    GCI_LOGIN_QUIET = 0x10,

    /// <summary>
    /// a GCI_LOGIN_ flag ,
    /// Non-zero value ignored unless STN_ALLOW_NO_SESSION_INIT=TRUE in stone config.
    /// Non-zero means GCI application
    /// will do
    /// GciPerform(OOP_CLASS_GSCURRENT_SESSION, "initialize", NULL, 0);
    /// after a successful GciLogin.  Zero means that the VM
    /// is supposed to do that message send itself as part of GciLogin
    /// </summary>
    GCI_CLIENT_DOES_SESSION_INIT = 0x20,

    /// <summary>
    /// a GCI_LOGIN_ flag, 
    /// bit is controlled by libgcirpc and libgcits
    /// and bit in flags arg passed to GciLoginEx and GciTsLogin is ignored
    /// </summary>
    GCI_TS_CLIENT = 0x40,

    /// <summary>
    /// authenticate using Kerberos / GSSAPI ,
    /// ignored by GciX509Login
    /// </summary>
    GCI_PASSWORDLESS_LOGIN = 0x80,

    /// <summary>
    /// also in image/gsparams.gs
    /// </summary>
    GCI_LOGIN_SOLO = 0x100,

    /// <summary>
    /// controlled by libgci code
    /// </summary>
    GCI_WINDOWS_CLIENT = 0x200,

    /// <summary>
    /// password is a one-time security token
    /// </summary>
    GCI_ONETIME_PASSWORD = 0x400,

    GCI_LOGIN_ALL_FLAGS = 0x7FF
}
