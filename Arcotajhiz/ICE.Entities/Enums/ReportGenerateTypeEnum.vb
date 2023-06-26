Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ReportGenerateTypeEnum
        <EnumMember> DefaultReport = 0
        <EnumMember> FromICS24 = 1
        <EnumMember> AppICS24 = 2
    End Enum
End Namespace
