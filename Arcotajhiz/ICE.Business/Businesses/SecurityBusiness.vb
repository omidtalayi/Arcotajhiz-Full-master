Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
'Imports TLSharp.Core.MTProto
Imports ARCO.Entities.Enums
Imports ARCO.Business.Models
Imports System.ComponentModel

Namespace Businesses
    Public Class SecurityBusiness
        Public Shared Function CreateSalt(size As Integer) As String
            Try
                Dim rng = New RNGCryptoServiceProvider()
                Dim buff = Array.CreateInstance(GetType(Byte), size)
                rng.GetBytes(buff)
                Return Convert.ToBase64String(buff)
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Shared Function CreatePasswordHash(password As String, salt As String) As String
            Try
                Dim passAndSalt = String.Concat(password, salt)
                Dim passBytes = (New UnicodeEncoding).GetBytes(passAndSalt)
                Dim passHash = (New SHA256Managed).ComputeHash(passBytes)
                Return Convert.ToBase64String(passHash)
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Shared Function CreatePasswordHash(password As String) As String
            Try
                Dim passBytes = (New UnicodeEncoding).GetBytes(password)
                Dim passHash = (New SHA256Managed).ComputeHash(passBytes)
                Return Convert.ToBase64String(passHash)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Shared Function checkPassword(password As String) As PasswordPolicyEnum
            Try
                If password.Length = 0 Then Return PasswordPolicyEnum.BLANK
                If password.Length < 6 Then Return PasswordPolicyEnum.TOOSHORT
                'If Not Regex.Match(password, "[\d]+", RegexOptions.ECMAScript).Success Then Return PasswordPolicyEnum.NODIGITS
                'If Not Regex.Match(password, "[A-Za-z]+", RegexOptions.ECMAScript).Success Then Return PasswordPolicyEnum.NOLETTERCHARACTER
                'If Not Regex.Match(password, "[a-z]+", RegexOptions.ECMAScript).Success Then Return PasswordPolicyEnum.NOLOWERCASECHARACTER
                'If Not Regex.Match(password, "[A-Z]+", RegexOptions.ECMAScript).Success Then Return PasswordPolicyEnum.NOUPPERCASECHARACTER
                'If Not Regex.Match(password, "[ .!@#$%^&*?_~-£()\[\]|+=\\\/]+", RegexOptions.ECMAScript).Success Then Return PasswordPolicyEnum.NOSPECIALCHARACTER
                Return PasswordPolicyEnum.OK
            Catch ex As Exception
                Throw ex
            End Try
        End Function
    End Class
End Namespace