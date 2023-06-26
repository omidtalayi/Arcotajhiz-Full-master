Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ReportSampleTypeEnum
        <EnumMember> CompleteAndHasScore = 0
        <EnumMember> WithoutNegetiveScore = 1
        <EnumMember> WithoutFinantialData = 2
        <EnumMember> JustHasNationalCode = 3
    End Enum
End Namespace
