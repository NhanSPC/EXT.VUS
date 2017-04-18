
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace EXT.VUS

    <Serializable()> _
    Public Class CLHistoryInfo
        Inherits Csla.ReadOnlyBase(Of CLHistoryInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _classHistoryId As Integer
        Public ReadOnly Property ClassHistoryId() As String
            Get
                Return _classHistoryId
            End Get
        End Property

        Private _changedDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ChangedDate() As String
            Get
                Return _changedDate.DateViewFormat
            End Get
        End Property

        Private _changedTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property ChangedTime() As String
            Get
                Return _changedTime.Text
            End Get
        End Property

        Private _classId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property ClassId() As String
            Get
                Return _classId.Text
            End Get
        End Property

        Private _classCode As String = String.Empty
        Public ReadOnly Property ClassCode() As String
            Get
                Return _classCode
            End Get
        End Property

        Private _openDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property OpenDate() As String
            Get
                Return _openDate.Text
            End Get
        End Property

        Private _closeDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property CloseDate() As String
            Get
                Return _closeDate.Text
            End Get
        End Property

        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _classHistoryId
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pClassHistoryId As Integer = ID.Trim
            If _classHistoryId < pClassHistoryId Then Return -1
            If _classHistoryId > pClassHistoryId Then Return 1
            Return 0
        End Function

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _classHistoryId
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _classHistoryId
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _classHistoryId
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            CLHistoryInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetCLHistoryInfo(ByVal dr As SafeDataReader) As CLHistoryInfo
            Return New CLHistoryInfo(dr)
        End Function

        Friend Shared Function EmptyCLHistoryInfo(Optional ByVal pClassHistoryId As String = "") As CLHistoryInfo
            Dim info As CLHistoryInfo = New CLHistoryInfo
            With info
                ._classHistoryId = pClassHistoryId.ToInteger

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _classHistoryId = dr.GetInt32("CLASS_HISTORY_ID")
            _changedDate.Text = dr.GetDateTime("CHANGED_DATE")
            _classId.Text = dr.GetInt32("CLASS_ID")
            _classCode = dr.GetString("CLASS_CODE").TrimEnd
            _openDate.Text = dr.GetDateTime("OPEN_DATE")
            _closeDate.Text = dr.GetDateTime("CLOSE_DATE")

        End Sub

        Private Sub New()
        End Sub


#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _classHistoryId)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace