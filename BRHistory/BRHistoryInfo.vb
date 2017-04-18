
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace EXT.VUS

    <Serializable()> _
    Public Class BRHistoryInfo
        Inherits Csla.ReadOnlyBase(Of BRHistoryInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _branchHistoryId As Integer
        Public ReadOnly Property BranchHistoryId() As Integer
            Get
                Return _branchHistoryId
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

        Private _branchIdStudy As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property BranchIdStudy() As String
            Get
                Return _branchIdStudy.Text
            End Get
        End Property

        Private _classId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property ClassId() As String
            Get
                Return _classId.Text
            End Get
        End Property

        Private _operationId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property OperationId() As String
            Get
                Return _operationId.Text
            End Get
        End Property

        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _branchHistoryId
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pBranchHistoryId As Integer = ID.Trim.ToInteger
            If _branchHistoryId < pBranchHistoryId Then Return -1
            If _branchHistoryId > pBranchHistoryId Then Return 1
            Return 0
        End Function

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _branchHistoryId
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _branchHistoryId
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _branchHistoryId
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            BRHistoryInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetBRHistoryInfo(ByVal dr As SafeDataReader) As BRHistoryInfo
            Return New BRHistoryInfo(dr)
        End Function

        Friend Shared Function EmptyBRHistoryInfo(Optional ByVal pBranchHistoryId As String = "") As BRHistoryInfo
            Dim info As BRHistoryInfo = New BRHistoryInfo
            With info
                ._branchHistoryId = pBranchHistoryId.ToInteger

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _branchHistoryId = dr.GetInt32("BRANCH_HISTORY_ID")
            _changedDate.Text = dr.GetDateTime("CHANGED_DATE")
            _changedTime.Text = dr.GetDateTime("CHANGED_DATE")
            _branchIdStudy.Text = dr.GetInt32("BRANCH_ID_STUDY")
            _classId.Text = dr.GetInt32("CLASS_ID")
            _operationId.Text = dr.GetInt32("OPERATION_ID")

        End Sub

        Private Sub New()
        End Sub


#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _branchHistoryId)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace