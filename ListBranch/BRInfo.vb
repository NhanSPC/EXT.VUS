
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

'Namespace EXT.VUS

<Serializable()> _
Public Class BRInfo
    Inherits Csla.ReadOnlyBase(Of BRInfo)
    Implements IComparable
    Implements IInfo
    Implements IDocLink
    'Implements IInfoStatus


#Region " Business Properties and Methods "


    Private _branchid As Integer
    Public ReadOnly Property Branchid() As String
        Get
            Return _branchid
        End Get
    End Property

    Private _branchcode As String = String.Empty
    Public ReadOnly Property Branchcode() As String
        Get
            Return _branchcode
        End Get
    End Property

    Private _idxorder As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public ReadOnly Property Idxorder() As String
        Get
            Return _idxorder.Text
        End Get
    End Property

    'Get ID
    Protected Overrides Function GetIdValue() As Object
        Return _branchid
    End Function

    'IComparable
    Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
        Dim ID = IDObject.ToString
        Dim pBranchid As Integer = ID.Trim.ToInteger
        If _branchid < pBranchid Then Return -1
        If _branchid > pBranchid Then Return 1
        Return 0
    End Function

    Public ReadOnly Property Code As String Implements IInfo.Code
        Get
            Return _branchid
        End Get
    End Property

    Public ReadOnly Property LookUp As String Implements IInfo.LookUp
        Get
            Return _branchid
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IInfo.Description
        Get
            Return _branchid
        End Get
    End Property


    Public Sub InvalidateCache() Implements IInfo.InvalidateCache
        BRInfoList.InvalidateCache()
    End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

    Friend Shared Function GetBRInfo(ByVal dr As SafeDataReader) As BRInfo
        Return New BRInfo(dr)
    End Function

    Friend Shared Function EmptyBRInfo(Optional ByVal pBranchid As String = "") As BRInfo
        Dim info As BRInfo = New BRInfo
        With info
            ._branchid = pBranchid

        End With
        Return info
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        _branchid = dr.GetInt32("BranchID")
        _branchcode = dr.GetString("BranchCode").TrimEnd
        _idxorder.Text = dr.GetInt32("IdxOrder")

    End Sub

    Private Sub New()
    End Sub


#End Region ' Factory Methods

#Region "IDoclink"
    Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
        Return String.Format("{0}#{1}", Get_TransType, _branchid)
    End Function

    Public Function Get_TransType() As String Implements IDocLink.Get_TransType
        Return Me.GetType.ToClassSchemaName.Leaf
    End Function
#End Region

End Class

'End Namespace