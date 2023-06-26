Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ReportTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> BASIC_INDIVIDUAL_REPORT = 1
        <EnumMember> STANDARD_INDIVIDUAL_REPORT = 2
        <EnumMember> SCORING_INDIVIDUAL_REPORT = 3
        <EnumMember> SCORING_REQUEST = 4
        <EnumMember> ADVANCED_INDIVIDUAL_REPORT = 5
        <EnumMember> HISTORY_INDIVIDUAL_REPORT = 6
        <EnumMember> FULL_INDIVIDUAL_REPORT = 7
    End Enum
End Namespace


