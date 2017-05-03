
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

'Namespace EXT.VUS

<Serializable()> _
Public Class CLInfo
    Inherits Csla.ReadOnlyBase(Of CLInfo)
    Implements IComparable
    Implements IInfo
    Implements IDocLink
    'Implements IInfoStatus


#Region " Business Properties and Methods "


    Private _classid As Integer
    Public ReadOnly Property Classid() As Integer
        Get
            Return _classid
        End Get
    End Property

    Private _classcode As String = String.Empty
    Public ReadOnly Property Classcode() As String
        Get
            Return _classcode
        End Get
    End Property

    Private _branchid As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public ReadOnly Property Branchid() As String
        Get
            Return _branchid.Text
        End Get
    End Property

    Private _opendate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
    Public ReadOnly Property Opendate() As String
        Get
            Return _opendate.DateViewFormat
        End Get
    End Property

    Private _closedate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
    Public ReadOnly Property Closedate() As String
        Get
            Return _closedate.DateViewFormat
        End Get
    End Property

    Private _createddate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
    Public ReadOnly Property Createddate() As String
        Get
            Return _createddate.DateViewFormat
        End Get
    End Property

    Private _coursetypeid As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public ReadOnly Property Coursetypeid() As String
        Get
            Return _coursetypeid.Text
        End Get
    End Property

    'Get ID
    Protected Overrides Function GetIdValue() As Object
        Return _classid
    End Function

    'IComparable
    Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
        Dim ID = IDObject.ToString
        Dim pClassid As Integer = ID.Trim.ToInteger
        If _classid < pClassid Then Return -1
        If _classid > pClassid Then Return 1
        Return 0
    End Function

    Public ReadOnly Property Code As String Implements IInfo.Code
        Get
            Return _classid
        End Get
    End Property

    Public ReadOnly Property LookUp As String Implements IInfo.LookUp
        Get
            Return _classid
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IInfo.Description
        Get
            Return _classid
        End Get
    End Property


    Public Sub InvalidateCache() Implements IInfo.InvalidateCache
        CLInfoList.InvalidateCache()
    End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

    Friend Shared Function GetCLInfo(ByVal dr As SafeDataReader) As CLInfo
        Return New CLInfo(dr)
    End Function

    Friend Shared Function EmptyCLInfo(Optional ByVal pClassid As String = "") As CLInfo
        Dim info As CLInfo = New CLInfo
        With info
            ._classid = pClassid.ToInteger

        End With
        Return info
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        _classid = dr.GetInt32("ClassID")
        _classcode = dr.GetString("ClassCode").TrimEnd
        _branchid.Text = dr.GetInt32("BranchID")
        _opendate.Date = dr.GetDateTime("OpenDate")
        _closedate.Date = dr.GetDateTime("CloseDate")
        _createddate.Date = dr.GetDateTime("CreatedDate")
        _coursetypeid.Text = dr.GetInt32("CourseTypeID")

    End Sub

    Private Sub New()
    End Sub


#End Region ' Factory Methods

#Region "IDoclink"
    Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
        Return String.Format("{0}#{1}", Get_TransType, _classid)
    End Function

    Public Function Get_TransType() As String Implements IDocLink.Get_TransType
        Return Me.GetType.ToClassSchemaName.Leaf
    End Function
#End Region

End Class

'End Namespace