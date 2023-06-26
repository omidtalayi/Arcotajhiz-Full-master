Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports ARCO.Entities.Enums
Imports ARCO.Business.Models
Imports System.Text.RegularExpressions
Imports System.ComponentModel

Public Class CryptographyBusiness
    Private enc As System.Text.UTF8Encoding
    Private encryptor As ICryptoTransform
    Private decryptor As ICryptoTransform
    Private symmetricKey As RijndaelManaged
    Private Const Inputkey = "D216E540-7266-49DB-A1BD-0ACDA18CDD41"
    Public Sub New()
        symmetricKey = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Me.enc = New System.Text.UTF8Encoding
        'Dim KEY_128 As Byte() = {42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23}
        'Dim IV_128 As Byte() = {234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 78}
    End Sub
    Public Function Encrypt(ByVal sPlainText As String, Optional keyType As EncryptKeyEnum = EncryptKeyEnum.NONE, Optional salt As String = Nothing) As String
        Dim result As String
        Dim KEY_128 As Byte() = Nothing
        Dim IV_128 As Byte() = Nothing
        If String.IsNullOrEmpty(salt) Then
            KEY_128 = KEY(keyType:=keyType)
            IV_128 = IV(keyType:=keyType)
        Else
            InitSalt(salt:=salt, KEY:=KEY_128, IV:=IV_128)
        End If
        Try
            If Not String.IsNullOrEmpty(sPlainText) Then
                Me.encryptor = symmetricKey.CreateEncryptor(KEY_128, IV_128)
                Dim memoryStream As MemoryStream = New MemoryStream()
                Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, Me.encryptor, CryptoStreamMode.Write)
                cryptoStream.Write(Me.enc.GetBytes(sPlainText), 0, sPlainText.Length)
                cryptoStream.FlushFinalBlock()
                result = Convert.ToBase64String(memoryStream.ToArray())
                memoryStream.Close()
                cryptoStream.Close()
            End If
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub InitSalt(salt As String, ByRef KEY As Byte(), ByRef IV As Byte())
        Try
            salt = salt.PadLeft(10, "0")
            Dim saltBytes = Me.enc.GetBytes(salt)
            Dim k = New Rfc2898DeriveBytes(Inputkey, saltBytes)
            Dim aesAlg = New RijndaelManaged()
            aesAlg.Key = k.GetBytes(aesAlg.KeySize / 8)
            aesAlg.IV = k.GetBytes(aesAlg.BlockSize / 8)
            KEY = aesAlg.Key
            IV = aesAlg.IV
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Public Function Decrypt(ByVal encryptedText As String, Optional keyType As EncryptKeyEnum = EncryptKeyEnum.NONE, Optional ByVal salt As String = Nothing) As String
        Dim result As String
        Dim KEY_128 As Byte() = Nothing
        Dim IV_128 As Byte() = Nothing
        If String.IsNullOrEmpty(salt) Then
            KEY_128 = KEY(keyType:=keyType)
            IV_128 = IV(keyType:=keyType)
        Else
            InitSalt(salt:=salt, KEY:=KEY_128, IV:=IV_128)
        End If
        Try
            Me.decryptor = symmetricKey.CreateDecryptor(KEY_128, IV_128)
            Dim cypherTextBytes As Byte() = Convert.FromBase64String(encryptedText)
            Dim memoryStream As MemoryStream = New MemoryStream(cypherTextBytes)
            Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, Me.decryptor, CryptoStreamMode.Read)
            Dim plainTextBytes(cypherTextBytes.Length) As Byte
            Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
            memoryStream.Close()
            cryptoStream.Close()
            result = Me.enc.GetString(plainTextBytes, 0, decryptedByteCount)
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function KEY(keyType As EncryptKeyEnum) As Byte()
        Try
            Select Case keyType
                Case EncryptKeyEnum.Key1
                    Return New Byte() {42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23}
            End Select
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function IV(keyType As EncryptKeyEnum) As Byte()
        Try
            Select Case keyType
                Case EncryptKeyEnum.Key1
                    Return New Byte() {234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 78}
            End Select
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function DecryptStringFromBytes(ByVal cipherText() As Byte, ByVal key() As Byte, ByVal iv() As Byte) As String
        Try
            ' Check arguments.  
            If ((cipherText Is Nothing) _
                        OrElse (cipherText.Length <= 0)) Then
                Throw New ArgumentNullException("cipherText")
            End If

            If ((key Is Nothing) _
                        OrElse (key.Length <= 0)) Then
                Throw New ArgumentNullException("key")
            End If

            If ((iv Is Nothing) _
                        OrElse (iv.Length <= 0)) Then
                Throw New ArgumentNullException("key")
            End If

            ' Declare the string used to hold  
            ' the decrypted text.  
            Dim plaintext As String = Nothing
            ' Create an RijndaelManaged object  
            ' with the specified key and IV.  
            Dim rijAlg = New RijndaelManaged
            'Settings  
            rijAlg.Mode = CipherMode.CBC
            rijAlg.Padding = PaddingMode.PKCS7
            rijAlg.FeedbackSize = 128
            rijAlg.Key = key
            rijAlg.IV = iv
            ' Create a decrytor to perform the stream transform.  
            Dim decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV)
            Try
                ' Create the streams used for decryption.  
                Dim msDecrypt = New MemoryStream(cipherText)
                Dim csDecrypt = New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
                Dim srDecrypt = New StreamReader(csDecrypt)
                ' Read the decrypted bytes from the decrypting stream  
                ' and place them in a string.  
                plaintext = srDecrypt.ReadToEnd
            Catch ex As System.Exception
                plaintext = "keyError"
            End Try

            Return plaintext
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class